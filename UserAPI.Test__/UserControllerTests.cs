using Microsoft.AspNetCore.Mvc;
using UserAPI.Controllers;
using UserAPI.Data;
using UserAPI.Models;

namespace UserAPI.Test
{
    [TestClass]
    public class UserControllerTests
    {
        private static User MockUser = new("Ana", "ana@mail.com");
  
        [TestMethod]
        public async Task GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            UserDbContext userDbContext = await GetDatabaseContext();

            int numOfUsers = 10;
            var fakeUsers = A.CollectionOfDummy<User>(numOfUsers).AsEnumerable();
            userDbContext.Users.AddRange(fakeUsers);
            await userDbContext.SaveChangesAsync();

            UserController userController = new (userDbContext);

            // Act
            IEnumerable<User> users = await userController.Get();
            
            // Assert
            Assert.AreEqual(numOfUsers, users.Count());
        }

        [TestMethod]
        public async Task GetUserById_ShouldReturnCorrectUser()
        {
            // Arrange
            UserDbContext userDbContext = await GetDatabaseContext();
            await CreateMockUser(userDbContext);

            UserController userController = new (userDbContext);

            // Act
            var okResult = await userController.GetById(MockUser.Id) as OkObjectResult;
            
            // Assert
            Assert.IsNotNull(okResult);

            var user = okResult.Value as User;

            Assert.IsNotNull(user);
            Assert.AreEqual(MockUser, user);
        }

        [TestMethod]
        public async Task GetUserById_ShouldFail()
        {
            // Arrange
            UserDbContext userDbContext = await GetDatabaseContext();
            await CreateMockUser(userDbContext);

            UserController userController = new (userDbContext);

            // Act
            var result = await userController.GetById(MockUser.Id + 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task CreateUser_ShouldCreateUser()
        {
            // Arrange
            UserDbContext userDbContext = await GetDatabaseContext();
            UserController userController = new (userDbContext);
            var mockUser = new User("Ana", "ana@mail.com");

            // Act
            var result = await userController.Create(mockUser);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            Assert.AreEqual(1, userDbContext.Users.Count());
        }

        [TestMethod]
        public async Task DeleteUser_ShouldDeleteUser()
        {
            // Arrange
            UserDbContext userDbContext = await GetDatabaseContext();
            await CreateMockUser(userDbContext);

            UserController userController = new (userDbContext);
            
            // Act
            var result = await userController.Delete(MockUser.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            Assert.AreEqual(0, userDbContext.Users.Count());
        }

        [TestMethod]
        public async Task DeleteUser_ShouldNotDeleteUserIfIdDoesNotExist()
        {
            // Arrange
            UserDbContext userDbContext = await GetDatabaseContext();
            await CreateMockUser(userDbContext);

            UserController userController = new (userDbContext);

            // Act
            var result = await userController.Delete(MockUser.Id + 1);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            Assert.AreEqual(1, userDbContext.Users.Count());
        }

        [TestMethod]
        public async Task UpdateUser_ShouldUpdateUser()
        {
            // Arrange
            UserDbContext userDbContext = await GetDatabaseContext();
            await CreateMockUser(userDbContext);

            UserController userController = new (userDbContext);
            MockUser.Name = "Ana B.";

            // Act
            var result = await userController.Update(MockUser.Id, MockUser);
            var okResult = await userController.GetById(MockUser.Id) as OkObjectResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            
            Assert.IsNotNull(okResult);

            var updatedMockUser = okResult.Value as User;

            Assert.IsNotNull(updatedMockUser);
            Assert.AreEqual(updatedMockUser, MockUser);
        }

        private static async Task CreateMockUser(UserDbContext userDbContext)
        {
            userDbContext.Add(MockUser);
            await userDbContext.SaveChangesAsync();
        }

        private static async Task<UserDbContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<UserDbContext>()
                .EnableSensitiveDataLogging()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            UserDbContext userDbContext = new (options);
            userDbContext.Database.EnsureCreated();
            await userDbContext.SaveChangesAsync();

            return userDbContext;
        }


    }
}