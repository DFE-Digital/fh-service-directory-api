using Newtonsoft.Json;
using FluentAssertions;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Builders;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Configuration;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Models;
using FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Builders.Http;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Tests.Steps;

public class ApiInfoSteps
{
    readonly ConfigModel config;
    readonly string baseUrl;
    HttpResponseMessage lastResponse;

    public ApiInfoSteps()
    {
        config = ConfigAccessor.GetApplicationConfiguration();
        baseUrl = config.BaseUrl;
    }

    public async Task ICheckTheApiInfo()
    {
        lastResponse = await HttpRequestFactory.Get(baseUrl, "api/info");
    }

    public async Task AnOkStatusCodeIsReturned()
    {
        lastResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}