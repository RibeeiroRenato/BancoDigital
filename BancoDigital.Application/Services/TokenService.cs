using Azure.Core;
using BancoDidital.Infrastructure.Data.DbContext;
using BancoDidital.Infrastructure.Data.Models.ContaCorrente;
using BancoDigital.Application.Request;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BancoDigital.Application.Services
{
    public class TokenService
    {
        private readonly IConfiguration _Configuration;
        private readonly contaCorrenteContext _context;

        public TokenService(IConfiguration configuration, contaCorrenteContext context)
        {
            _Configuration = configuration;
            _context = context;
        }

        public string GenerateToken(ContaCorrenteRequest contaCorrente)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["Jwt:key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, contaCorrente.numeroContaCorrente.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, contaCorrente.Senha.ToString()),
                new Claim("nome", contaCorrente.nome)

            };

            var token = new JwtSecurityToken(
                issuer: _Configuration["Jwt:Issuer"],
                audience: _Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_Configuration["Jwt:ExpiresInMinutes"]!)),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_Configuration["Jwt:key"]!);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _Configuration["Jwt:Issuer"],
                    ValidAudience = _Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                }, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                var numeroContaCorrente = jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
                return numeroContaCorrente;
            }
            catch
            {
                return null;
            }
        }

        public async Task<string> RegisterTokenAsync(ContaCorrente contaCorrente)
        {
            if (string.IsNullOrWhiteSpace(contaCorrente.numeroContaCorrente) || string.IsNullOrWhiteSpace(contaCorrente.Senha))
            {
                return "Numero da conta ou senha inválidos.";
            }
            var existingUser = _context.contaCorrente
                .FirstOrDefault(u => u.numeroContaCorrente == contaCorrente.numeroContaCorrente);

            var newUser = new BancoDidital.Infrastructure.Data.Models.ContaCorrente.ContaCorrente
            {
                nome = contaCorrente.nome,
                numeroContaCorrente = contaCorrente.numeroContaCorrente,
                Senha = contaCorrente.Senha,
                ativo = 1,
                Saldo = contaCorrente.Saldo,
                cpf = contaCorrente.cpf
            };
            _context.contaCorrente.Add(newUser);
            await _context.SaveChangesAsync();

            return "Usuário registrado com sucesso!";
        }
    }
}
