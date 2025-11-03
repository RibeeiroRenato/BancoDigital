using BancoDigital.Application.Exceptions;
using BancoDigital.Application.Interface;
using BancoDigital.Application.Request;
using BancoDigital.Application.Response;
using BancoDigital.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace BancoDigital.Tests.Services
{
    public class ContaCorrenteServiceTests
    {
        private readonly Mock<ILogger<ContaCorrenteService>> _loggerMock;
        private readonly Mock<IContaCorrenteRepository> _repositoryMock;
      private readonly Mock<TokenService> _tokenServiceMock;
        private readonly ContaCorrenteService _service;

      public ContaCorrenteServiceTests()
        {
   _loggerMock = new Mock<ILogger<ContaCorrenteService>>();
         _repositoryMock = new Mock<IContaCorrenteRepository>();
            _tokenServiceMock = new Mock<TokenService>();
            _service = new ContaCorrenteService(_loggerMock.Object, _repositoryMock.Object, _tokenServiceMock.Object);
      }

   [Theory]
    [InlineData("12345678909")] // Valid CPF
        [InlineData("98765432100")] // Valid CPF
        public void ValidaCPF_WithValidCPF_ReturnsTrue(string cpf)
        {
    // Act
     var result = ContaCorrenteService.ValidaCPF(cpf);

       // Assert
  Assert.True(result);
        }

        [Theory]
        [InlineData("12345678")] // Too short
    [InlineData("111111111111")] // Too long
        [InlineData("11111111111")] // All same digits
     [InlineData("")] // Empty
        [InlineData(null)] // Null
        [InlineData("abcdefghijk")] // Non-numeric
        public void ValidaCPF_WithInvalidCPF_ReturnsFalse(string cpf)
  {
   // Act
      var result = ContaCorrenteService.ValidaCPF(cpf);

// Assert
        Assert.False(result);
      }

   [Fact]
        public async Task CadastrarContaCorrente_WithValidData_Succeeds()
     {
      // Arrange
    var request = new ContaCorrenteRequest
            {
            cpf = "12345678909",
        nome = "Test User",
       Senha = "123456"
         };

            _repositoryMock.Setup(x => x.cadastrarContaCorrente(It.IsAny<ContaCorrenteRequest>()))
      .Returns(Task.CompletedTask);

         // Act
   await _service.cadastrarContaCorrente(request);

   // Assert
            _repositoryMock.Verify(x => x.cadastrarContaCorrente(It.Is<ContaCorrenteRequest>(r => 
        r.cpf == request.cpf && 
         r.nome == request.nome && 
   r.Senha == request.Senha)), Times.Once);
        }

        [Fact]
      public async Task CadastrarContaCorrente_WithInvalidCPF_ThrowsException()
        {
     // Arrange
            var request = new ContaCorrenteRequest
            {
     cpf = "11111111111", // Invalid CPF
                nome = "Test User",
       Senha = "123456"
   };

          // Act & Assert
    await Assert.ThrowsAsync<BusinessValidationException>(() => 
         _service.cadastrarContaCorrente(request));
   }

        [Fact]
        public async Task MovimentoContaCorrente_WithValidData_Succeeds()
        {
            // Arrange
       var movimento = new movimentoRequest
            {
      idContaCorrente = 1,
      valor = 100,
       tipoMovimento = "D",
                dataMovimento = DateTime.Now
      };

            var contaCorrente = new contaCorrenteResponse
            {
           idContaCorrente = 1,
           ativo = true,
           numeroContaCorrente = "123456",
 Senha = "123456",
    nome = "Test User"
       };

   _repositoryMock.Setup(x => x.getContaCorrente(It.IsAny<ContaCorrenteRequest>()))
    .ReturnsAsync(contaCorrente);

     _tokenServiceMock.Setup(x => x.GenerateToken(It.IsAny<ContaCorrenteRequest>()))
      .Returns("valid-token");

      _tokenServiceMock.Setup(x => x.ValidateToken(It.IsAny<string>()))
         .Returns("valid-token");

 _repositoryMock.Setup(x => x.MovimentoContaCorrente(It.IsAny<movimentoRequest>()))
 .Returns(Task.CompletedTask);

  // Act
   await _service.MovimentoContaCorrente(movimento);

     // Assert
            _repositoryMock.Verify(x => x.MovimentoContaCorrente(It.IsAny<movimentoRequest>()), Times.Once);
        }

  [Fact]
        public async Task MovimentoContaCorrente_WithInactiveAccount_ThrowsException()
        {
            // Arrange
  var movimento = new movimentoRequest
      {
   idContaCorrente = 1,
       valor = 100,
  tipoMovimento = "D"
            };

            var contaCorrente = new contaCorrenteResponse
        {
 idContaCorrente = 1,
    ativo = false
    };

   _repositoryMock.Setup(x => x.getContaCorrente(It.IsAny<ContaCorrenteRequest>()))
  .ReturnsAsync(contaCorrente);

            // Act & Assert
      await Assert.ThrowsAsync<BusinessValidationException>(() => 
_service.MovimentoContaCorrente(movimento));
        }

        [Fact]
        public async Task GetSaldoContaCorrente_WithValidData_ReturnsCorrectBalance()
 {
          // Arrange
     var request = new ContaCorrenteRequest { idContaCorrente = 1 };
 var expectedResponse = new movimentoResponse { valor = 1000 };

     _repositoryMock.Setup(x => x.getSaldoContaCorrente(It.IsAny<ContaCorrenteRequest>()))
           .ReturnsAsync(expectedResponse);

            // Act
            var result = await _service.GetSaldoContaCorrente(request);

        // Assert
   Assert.Equal(expectedResponse.valor, result.valor);
   }

        [Fact]
        public async Task GetContaCorrente_WithValidData_ReturnsAccount()
        {
            // Arrange
            var request = new ContaCorrenteRequest { idContaCorrente = 1 };
    var expectedResponse = new contaCorrenteResponse
            {
   idContaCorrente = 1,
       nome = "Test User",
 ativo = true
       };

      _repositoryMock.Setup(x => x.getContaCorrente(It.IsAny<ContaCorrenteRequest>()))
 .ReturnsAsync(expectedResponse);

     // Act
     var result = await _service.GetContaCorrente(request);

            // Assert
            Assert.Equal(expectedResponse.idContaCorrente, result.idContaCorrente);
  Assert.Equal(expectedResponse.nome, result.nome);
            Assert.Equal(expectedResponse.ativo, result.ativo);
        }

        [Fact]
        public async Task GetContaCorrente_WithInvalidAccount_ThrowsException()
  {
         // Arrange
        var request = new ContaCorrenteRequest { idContaCorrente = 1 };

            _repositoryMock.Setup(x => x.getContaCorrente(It.IsAny<ContaCorrenteRequest>()))
         .ReturnsAsync((contaCorrenteResponse)null);

     // Act & Assert
   await Assert.ThrowsAsync<BusinessValidationException>(() => 
        _service.GetContaCorrente(request));
        }
    }
}