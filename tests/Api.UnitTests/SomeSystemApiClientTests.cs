using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Template.Api.Interfaces;
using Template.Api.Models;
using Template.Api.Services;
using Xunit;

namespace Template.Api.UnitTests
{
    public class SomeSystemApiClientTests
    {
        private const string Url =
            "https://gist.githubusercontent.com/steve-gombos/baf2739c0a6cdac77f781bfb2f70b6c1/raw/c1491a261ce45f010add8df3b43c9198c47e4408/mock-users.json";

        private readonly IServiceCollection _services;

        public SomeSystemApiClientTests()
        {
            _services = new ServiceCollection();
        }

        [Fact]
        public async Task GetAllUserStatuses_Should_Throw()
        {
            // Arrange
            var handler = new MockHttpMessageHandler();
            var mockRequest = new MockedRequest(Url).Respond(HttpStatusCode.NotFound);
            handler.AddBackendDefinition(mockRequest);
            var client = handler.ToHttpClient();
            var sut = GetSomeSystemApiClient(client);

            // Act
            var action = async () => await sut.GetAllUserStatuses();

            // Assert
            await action.Should().ThrowAsync<HttpRequestException>().WithMessage("*404 (Not Found)*");
        }

        [Fact]
        public async Task GetAllUserStatuses_Should_Return_Collection()
        {
            // Arrange
            var mockStatuses = GetMockStatuses();
            var content = JsonSerializer.Serialize(mockStatuses);
            var handler = new MockHttpMessageHandler();
            var mockRequest = new MockedRequest(Url).Respond(req => new StringContent(content));
            handler.AddBackendDefinition(mockRequest);
            var client = handler.ToHttpClient();
            var sut = GetSomeSystemApiClient(client);

            // Act
            var actual = await sut.GetAllUserStatuses();

            // Assert
            actual.Should().BeOfType<UserStatusCollectionDto>();
            actual.Users.Count.Should().BeGreaterThan(0);
        }

        private ISomeSystemApiClient GetSomeSystemApiClient(HttpClient client)
        {
            var t = Substitute.For<IHttpClientFactory>();
            t.CreateClient("test").Returns(client);
            _services.AddHttpClient<ISomeSystemApiClient, SomeSystemApiClient>("test");
            _services.AddSingleton(t);
            return _services.BuildServiceProvider().GetService<ISomeSystemApiClient>();
        }

        private static UserStatusCollectionDto GetMockStatuses()
        {
            return new UserStatusCollectionDto
            {
                Users = new List<UserStatusDto>
                {
                    new UserStatusDto
                    {
                        UserId = 1,
                        IsActive = true,
                        IsComplete = true
                    }
                }
            };
        }
    }
}
