﻿using System;
using System.Net.Http;

namespace Glimpse.Agent
{
    public class RemoteHttpMessagePublisher : BaseMessagePublisher, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClientHandler _httpHandler;

        public RemoteHttpMessagePublisher()
        {
            _httpHandler = new HttpClientHandler();
            _httpClient = new HttpClient(_httpHandler);
        }

        public override void PublishMessage(IMessage message)
        {
            // TODO: Try shifting to async and await
            // TODO: Needs error handelling
            // TODO: Find out what happened to System.Net.Http.Formmating - PostAsJsonAsync

            var newMessage = ConvertMessage(message);

            _httpClient.PostAsJsonAsync("http://localhost:15999/Glimpse/Agent", newMessage)
                .ContinueWith(requestTask =>
                    {
                        // Get HTTP response from completed task.
                        HttpResponseMessage response = requestTask.Result;

                        // Check that response was successful or throw exception
                        response.EnsureSuccessStatusCode();

                        // Read response asynchronously as JsonValue and write out top facts for each country
                        var result = response.Content.ReadAsStringAsync().Result;
                    });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
            _httpHandler.Dispose();
        }
    }
}