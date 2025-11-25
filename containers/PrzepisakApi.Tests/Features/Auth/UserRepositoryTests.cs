using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.Auth.Infrastructure;
using PrzepisakApi.src.Features.UserProfile.Domain;
using PrzepisakApi.src.Features.UserProfiles.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PrzepisakApi.Tests.Features.Auth
{
    public class UserRepositoryTests
    {
        private readonly EfContext _efContext;
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<EfContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _efContext = new EfContext(options);
            _userManagerMock = GetMockUserManager();
            _repository = new UserRepository(_userManagerMock.Object, _efContext);
        }

        private static Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            return new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        [Fact]
        public async Task CreateUserAsync_ShouldCallUserManager()
        {
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _repository.CreateUserAsync("testUser", "Password123!");

            result.UserName.Should().Be("testUser");
            _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<IdentityUser>(), "Password123!"), Times.Once);
        }

        [Fact]
        public async Task GetUserProfileAsync_ShouldReturnUser()
        {
            var user = new User { Id = 1, IdentityUserId = "guid-abc" };
            _efContext.Users.Add(user);
            await _efContext.SaveChangesAsync();

            var result = await _repository.GetUserProfileAsync("guid-abc");

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
        }

        [Fact]
        public async Task SetRefreshTokenAsync_ShouldUpdateToken()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                IdentityUserId = "guid-abc",
                RefreshToken = new RefreshToken { Token = "old", RefreshTokenExpiration = DateTime.Now }
            };
            _efContext.Users.Add(user);
            await _efContext.SaveChangesAsync();

            // Act
            var newDate = DateTime.UtcNow.AddDays(1);
            await _repository.SetRefreshTokenAsync("guid-abc", "new-token", newDate);

            // Assert
            var updatedUser = await _efContext.Users.Include(u => u.RefreshToken).FirstAsync(u => u.Id == 1);
            updatedUser.RefreshToken.Token.Should().Be("new-token");
            updatedUser.RefreshToken.RefreshTokenExpiration.Should().BeCloseTo(newDate, TimeSpan.FromSeconds(1));
        }
    }
}