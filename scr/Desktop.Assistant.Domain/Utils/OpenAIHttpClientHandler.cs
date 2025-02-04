﻿using Desktop.Assistant.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Desktop.Assistant.Domain.Utils
{
    public class OpenAIHttpClientHandler : HttpClientHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.LocalPath == "/v1/chat/completions")
            {
                Regex regex = new Regex(@"(https?)://([^/]+)/(.*)");
                Match match = regex.Match(OpenAIOption.EndPoint);
                if (match.Success)
                {
                    string xieyi = match.Groups[1].Value;
                    string host = match.Groups[2].Value;
                    string route = match.Groups[3].Value;
                    //替换代理
                    UriBuilder uriBuilder = new UriBuilder(request.RequestUri)
                    {
                        // 这里是你要修改的 URL
                        Scheme =$"{xieyi}://{host}/",
                        Host = host,
                        Path = route+"v1/chat/completions",
                    };
                    request.RequestUri = uriBuilder.Uri;
                }
            }

            // 接着，调用基类的 SendAsync 方法将你的修改后的请求发出去
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
