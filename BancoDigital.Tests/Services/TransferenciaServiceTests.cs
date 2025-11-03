using BancoDigital.Application.Interface;
using BancoDigital.Application.Request;
using BancoDigital.Application.Response;
using BancoDigital.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;
using Xunit;

namespace BancoDigital.Tests.Services
{
    public class TransferenciaServiceTests
    {
        private readonly Mock<ILogger<TransferenciaService>> _loggerMock;
     private readonly Mock<ITransfereciaRepository> _repositoryMock;
        private readonly Mock<HttpMessageHandler> _httpHandlerMock;
   private readonly HttpClient _httpClient;
        private readonly TransferenciaService _service;

        public TransferenciaServiceTests()
        {
      _loggerMock = new Mock<ILogger<TransferenciaService>>();
        _repositoryMock = new Mock<ITransfereciaRepository>();
     _httpHandlerMock = new Mock<HttpMessageHandler>();
      _httpClient = new HttpClient(_httpHandlerMock.Object);
 
      // Using constructor injection to set the HttpClient
            var constructor = typeof(TransferenciaService).GetConstructors()[0];
     _service = (TransferenciaService)constructor.Invoke(new object[] 
       { 
  _loggerMock.Object, 
             _repositoryMock.Object,
  _httpClient 
          });
        }

        [Fact]
        public async Task Transferencia_WithValidData_Succeeds()
     {
    // Arrange
     var request = new transferenciaRequest
    {
            idContaCorrenteOrigem = 1,
            idContaCorrenteDestino = 2,
      valor = 100,
    dataMovimento = DateTime.Now
  };

  var expectedResponse = new transferenciaResponse
            {
  idContaCorrenteOrigem = request.idContaCorrenteOrigem,
     idContaCorrenteDestino = request.idContaCorrenteDestino,
      valor = request.valor,
dataMovimento = request.dataMovimento
            };

            _repositoryMock.Setup(x => x.TransferenciaContaCorrente(It.IsAny<transferenciaRequest>()))
  .ReturnsAsync("Success");

var responseMessage = new HttpResponseMessage
     {
      StatusCode = HttpStatusCode.OK,
          Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
            };

      _httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
          "SendAsync",
    ItExpr.IsAny<HttpRequestMessage>(),
          ItExpr.IsAny<CancellationToken>()
  )
    .ReturnsAsync(responseMessage);

            // Act
            var result = await _service.Transferencia(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.idContaCorrenteOrigem, result.idContaCorrenteOrigem);
            Assert.Equal(request.idContaCorrenteDestino, result.idContaCorrenteDestino);
       Assert.Equal(request.valor, result.valor);

      _repositoryMock.Verify(x => x.TransferenciaContaCorrente(
          It.Is<transferenciaRequest>(r =>
   r.idContaCorrenteOrigem == request.idContaCorrenteOrigem &&
     r.idContaCorrenteDestino == request.idContaCorrenteDestino &&
    r.valor == request.valor
         )), Times.Once);
        }

        [Fact]
  public async Task Transferencia_WithHttpFailure_ThrowsException()
      {
          // Arrange
         var request = new transferenciaRequest
        {
                idContaCorrenteOrigem = 1,
             idContaCorrenteDestino = 2,
       valor = 100,
      dataMovimento = DateTime.Now
            };

            var responseMessage = new HttpResponseMessage
          {
       StatusCode = HttpStatusCode.InternalServerError
            };

            _httpHandlerMock.Protected()
  .Setup<Task<HttpResponseMessage>>(
     "SendAsync",
   ItExpr.IsAny<HttpRequestMessage>(),
     ItExpr.IsAny<CancellationToken>()
    )
      .ReturnsAsync(responseMessage);

            // Act & Assert
     await Assert.ThrowsAsync<Exception>(() => _service.Transferencia(request));
 
            _loggerMock.Verify(
x => x.Log(
    LogLevel.Error,
        It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => true),
    It.IsAny<Exception>(),
           It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
     Times.Once);
 }

        [Fact]
      public async Task Transferencia_WithRepositoryFailure_ThrowsException()
        {
            // Arrange
         var request = new transferenciaRequest
   {
       idContaCorrenteOrigem = 1,
                idContaCorrenteDestino = 2,
         valor = 100,
   dataMovimento = DateTime.Now
        };

            var expectedResponse = new transferenciaResponse
       {
      idContaCorrenteOrigem = request.idContaCorrenteOrigem,
                idContaCorrenteDestino = request.idContaCorrenteDestino,
        valor = request.valor,
         dataMovimento = request.dataMovimento
   };

    _repositoryMock.Setup(x => x.TransferenciaContaCorrente(It.IsAny<transferenciaRequest>()))
      .ThrowsAsync(new Exception("Database error"));

        var responseMessage = new HttpResponseMessage
         {
      StatusCode = HttpStatusCode.OK,
          Content = new StringContent(JsonSerializer.Serialize(expectedResponse))
            };

 _httpHandlerMock.Protected()
           .Setup<Task<HttpResponseMessage>>(
   "SendAsync",
    ItExpr.IsAny<HttpRequestMessage>(),
         ItExpr.IsAny<CancellationToken>()
                )
         .ReturnsAsync(responseMessage);

            // Act & Assert
await Assert.ThrowsAsync<Exception>(() => _service.Transferencia(request));
        }

        [Fact]
        public async Task Transferencia_WithInvalidResponse_ThrowsException()
  {
            // Arrange
       var request = new transferenciaRequest
      {
     idContaCorrenteOrigem = 1,
     idContaCorrenteDestino = 2,
                valor = 100,
     dataMovimento = DateTime.Now
        };

          var responseMessage = new HttpResponseMessage
            {
           StatusCode = HttpStatusCode.OK,
           Content = new StringContent("Invalid JSON")
      };

      _httpHandlerMock.Protected()
      .Setup<Task<HttpResponseMessage>>(
    "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
ItExpr.IsAny<CancellationToken>()
      )
           .ReturnsAsync(responseMessage);

            // Act & Assert
 await Assert.ThrowsAsync<JsonException>(() => _service.Transferencia(request));
  }
    }
}