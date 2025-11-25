using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PrzepisakApi.src.Database;
using PrzepisakApi.src.Features.UserProfile.Domain;
using PrzepisakApi.src.Features.UserProfile.Infrastructure;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PrzepisakApi.Tests.Features.UserProfiles
{
    public class UserProfileRepositoryTests
    {
        private readonly EfContext _efContext;
        private readonly UserProfileRepository _repository;

        public UserProfileRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<EfContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _efContext = new EfContext(options);
            _repository = new UserProfileRepository(_efContext);
        }

        [Fact]
        public async Task GetByUserIdAsync_ShouldReturnUser_WhenExists()
        {
            // Arrange
            var user = new User { Id = 10, IdentityUserId = "guid-1", IdentityUser = new Microsoft.AspNetCore.Identity.IdentityUser("test") };
            _efContext.Users.Add(user);
            await _efContext.SaveChangesAsync();

            // Act
            var result = await _repository.GetByUserIdAsync(10);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(10);
            result.IdentityUser.Should().NotBeNull();
        }
    }
}