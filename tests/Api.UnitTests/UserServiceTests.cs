using FluentAssertions;
using NSubstitute;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template.Api.Interfaces;
using Template.Api.Models;
using Template.Api.Persistence;
using Template.Api.Services;
using Template.Api.UnitTests.Extensions;
using Xunit;

namespace Template.Api.UnitTests
{
    public class UserServiceTests
    {
        private readonly UserContext _context;
        private readonly ISomeSystemApiClient _someSystemApiClient;
        private readonly IUserService _sut;

        public UserServiceTests()
        {
            _context = Substitute.For<UserContext>();
            _someSystemApiClient = Substitute.For<ISomeSystemApiClient>();
            _sut = new UserService(_context, _someSystemApiClient);
        }

        [Fact]
        public void GetUsers_Should_Return_Collection()
        {
            // Arrange
            var mockUsers = GetMockUsers().CreateMockDbSet();
            _context.Users.Returns(mockUsers);

            // Act
            var users = _sut.GetUsers();

            // Assert
            users.Should().HaveCount(3);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task GetUserById_Should_Return_Expected(int userId)
        {
            // Arrange
            var mockUsers = GetMockUsers().CreateMockDbSet();
            _context.Users.Returns(mockUsers);

            // Act
            var user = await _sut.GetUserById(userId);

            // Assert
            user.Id.Should().Be(userId);
        }

        [Theory]
        [InlineData(1, true, true)]
        [InlineData(2, false, true)]
        [InlineData(3, false, true)]
        public async Task RefreshUserById_Should_Return_Expected(int userId, bool expectedIsComplete,
            bool expectedIsActive)
        {
            // Arrange
            var mockUsers = GetMockUsers().CreateMockDbSet();
            _context.Users.Returns(mockUsers);
            var mockStatuses = GetMockUserStatuses();
            _someSystemApiClient.GetAllUserStatuses().Returns(mockStatuses);

            // Act
            var user = await _sut.RefreshUserById(userId);

            // Assert
            user.Id.Should().Be(userId);
            user.IsComplete.Should().Be(expectedIsComplete);
            user.IsActive.Should().Be(expectedIsActive);
        }

        private static IList<User> GetMockUsers()
        {
            return new List<User>
            {
                new User
                {
                    Id = 1,
                    IsComplete = false,
                    IsActive = true
                },
                new User
                {
                    Id = 2,
                    IsComplete = false,
                    IsActive = true
                },
                new User
                {
                    Id = 3,
                    IsComplete = false,
                    IsActive = true
                }
            };
        }

        private static UserStatusCollectionDto GetMockUserStatuses()
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
