using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace FamilyHubs.ServiceDirectory.Api.AcceptanceTests.Builders.Http;
public class HttpRequestBuilder
{
    private HttpMethod method;
    private string path;
    private string baseUrl;
    private HttpContent content;
    private string bearerToken;
    private string acceptHeader;
    private Dictionary<string, string> parameters = null;
    private Dictionary<string, string> headers = null;

    public HttpRequestBuilder AddMethod(HttpMethod method)
    {
        this.method = method;
        return this;
    }

    public HttpRequestBuilder AddRequestUri(string baseUrl, string requestUri)
    {
        this.baseUrl = baseUrl;
        this.path = requestUri;

        return this;
    }

    public HttpRequestBuilder AddParameters(Dictionary<string, string> parameters)
    {
        this.parameters = parameters;
        return this;
    }

    public HttpRequestBuilder AddContent(HttpContent content)
    {
        this.content = content;
        return this;
    }

    public HttpRequestBuilder AddCustomHeaders(Dictionary<string, string> headers)
    {
        this.headers = headers;
        return this;
    }

    public HttpRequestBuilder AddBearerToken(string bearerToken)
    {
        this.bearerToken = bearerToken;
        return this;
    }

    public HttpRequestBuilder AddAcceptHeader(string acceptHeader)
    {
        this.acceptHeader = acceptHeader;
        return this;
    }

    public async Task<HttpResponseMessage> SendAsync()
    {
        //Create the request message based on the request in the builder
        var request = new HttpRequestMessage
        {
            Method = this.method,
            RequestUri = new Uri($"{this.baseUrl}{this.path}")
        };

        //Add parameters to Uri
        if(parameters != null)
        {
            request.RequestUri = new Uri($"{this.baseUrl}{this.path}?{CreateQueryString(parameters)}");
        }

        //Add any custom headers
        if(headers != null)
        {
            foreach(KeyValuePair<string, string> header in this.headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }

        //Add content if present in the request
        if (this.content != null)
        {
            request.Content = this.content;
        }

        //Add bearer token if present in the request
        if (!string.IsNullOrEmpty(this.bearerToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", this.bearerToken);
        }

        //Clear then add the Accept header if if exists in the request
        request.Headers.Accept.Clear();
        if (!string.IsNullOrEmpty(this.acceptHeader))
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(this.acceptHeader));
        }

        //Creates or Gets an existing HttpClient for the BaseUrl being used
        var httpClient = HttpClientFactory.GetHttpClientInstance(baseUrl);

        return await httpClient.SendAsync(request);
    }

    //Creates a querystring. E.g. dictKey1=dictValue1&dictKey2=dictValue2
    private static string CreateQueryString(IDictionary<string, string> dict)
    {
        var list = new List<string>();
        foreach (var item in dict)
        {
            list.Add($"{item.Key}={item.Value}");
        }
        return string.Join("&", list);
    }
}