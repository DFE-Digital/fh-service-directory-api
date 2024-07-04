using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Builders.Http;
//This static factory ensures that we are using one HttpClient per BaseUrl used in the solution.
//This prevents a large number sockets being left open after the tests are run
public static class HttpClientFactory
{
    private static ConcurrentDictionary<string, HttpClient> httpClientList = new ConcurrentDictionary<string, HttpClient>();

    public static HttpClient GetHttpClientInstance(string baseUrl)
    {
        if (!httpClientList.ContainsKey(baseUrl))
            httpClientList.TryAdd(baseUrl, new HttpClient());

        return httpClientList[baseUrl];
    }
}