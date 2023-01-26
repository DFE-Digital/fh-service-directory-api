﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FamilyHubs.ServiceDirectoryApi.FunctionalTests;

public abstract class BaseWhenUsingOpenReferralApiUnitTests
{
    protected readonly HttpClient _client;
    protected readonly JwtSecurityToken _token;

    public BaseWhenUsingOpenReferralApiUnitTests()
    {
        var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                 .AddEnvironmentVariables()
                 .Build();

        List<Claim> authClaims = new() { new Claim(ClaimTypes.Role, "LAAdmin") };
        _token = CreateToken(authClaims, config);

        var webAppFactory = new MyWebApplicationFactory();

        _client = webAppFactory.CreateDefaultClient();
        _client.BaseAddress = new Uri("https://localhost:7128/");

    }

    private JwtSecurityToken CreateToken(List<Claim> authClaims, IConfiguration configuration)
    {
        var secret = configuration["JWT:Secret"] ?? "JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr";
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        if (!int.TryParse(configuration["JWT:TokenValidityInMinutes"], out var tokenValidityInMinutes))
        {
            tokenValidityInMinutes = 30;
        }

        var token = new JwtSecurityToken(
            configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
}
