using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Template.Api.IntegrationTests.Attributes;
using Template.Api.IntegrationTests.Extensions;
using Template.Api.IntegrationTests.Helpers;
using Template.Api.Models;
using Xunit;
using Xunit.Abstractions;

namespace Template.Api.IntegrationTests
{
    [TestCaseOrderer("Template.Api.IntegrationTests.Orderers.PriorityOrderer", "Template.Api.IntegrationTests")]
    public class UsersControllerTests : IClassFixture<ApiClientFactory>
    {
        private readonly ApiClientFactory _clientFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public UsersControllerTests(ApiClientFactory clientFactory, ITestOutputHelper output)
        {
            _clientFactory = clientFactory;
            _clientFactory.Output = output;
            _jsonSerializerOptions = TestOptions.GetJsonSerializerOptions();
        }

        [Fact]
        [TestPriority(0)]
        public async Task GetValues_Should_Return_200()
        {
            // Arrange
            var path = "v1/users";
            var client = _clientFactory.CreateDefaultClient();

            // Act
            var response = await client.GetAsync(path);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var deserializedResponse = await response.DeserializeResponse<IList<User>>(_jsonSerializerOptions);
            deserializedResponse.Should().HaveCount(3);
            deserializedResponse.All(x => x.Id != 0).Should().BeTrue();
            deserializedResponse.All(x => !string.IsNullOrWhiteSpace(x.FirstName)).Should().BeTrue();
            deserializedResponse.All(x => !string.IsNullOrWhiteSpace(x.LastName)).Should().BeTrue();
        }

        [Theory]
        [InlineData(1, "user", "one", false, true)]
        [InlineData(2, "user", "two", true, true)]
        [InlineData(3, "user", "three", false, true)]
        [TestPriority(5)]
        public async Task GetUserById_Should_Return_200(int userId, string expectedFirstName, string expectedLastName,
            bool expectedIsComplete, bool expectedIsActive)
        {
            // Arrange
            var path = $"v1/users/{userId}";
            var client = _clientFactory.CreateDefaultClient();

            // Act
            var response = await client.GetAsync(path);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var deserializedResponse = await response.DeserializeResponse<User>(_jsonSerializerOptions);
            deserializedResponse.Id.Should().Be(userId);
            deserializedResponse.FirstName.Should().Be(expectedFirstName);
            deserializedResponse.LastName.Should().Be(expectedLastName);
            deserializedResponse.IsComplete.Should().Be(expectedIsComplete);
            deserializedResponse.IsActive.Should().Be(expectedIsActive);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [TestPriority(10)]
        public async Task RefreshUserById_Should_Return_200(int userId)
        {
            // Arrange
            var path = $"v1/users/{userId}/refresh";
            var client = _clientFactory.CreateDefaultClient();

            // Act
            var response = await client.PostAsync(path, null);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var deserializedResponse = await response.DeserializeResponse<User>(_jsonSerializerOptions);
            deserializedResponse.Id.Should().Be(userId);
        }

        [Theory]
        [InlineData(1, "user", "one", true, true)]
        [InlineData(2, "user", "two", true, true)]
        [InlineData(3, "user", "three", false, false)]
        [TestPriority(15)]
        public async Task GetUserById_After_Refresh_Should_Return_200(int userId, string expectedFirstName,
            string expectedLastName, bool expectedIsComplete, bool expectedIsActive)
        {
            // Arrange
            var path = $"v1/users/{userId}";
            var client = _clientFactory.CreateDefaultClient();

            // Act
            var response = await client.GetAsync(path);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var deserializedResponse = await response.DeserializeResponse<User>(_jsonSerializerOptions);
            deserializedResponse.FirstName.Should().Be(expectedFirstName);
            deserializedResponse.LastName.Should().Be(expectedLastName);
            deserializedResponse.IsComplete.Should().Be(expectedIsComplete);
            deserializedResponse.IsActive.Should().Be(expectedIsActive);
        }
    }
}
