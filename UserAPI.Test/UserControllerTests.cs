using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using UserAPI.Models;
using UserAPI.Controllers;
using FakeItEasy;

namespace UserAPI.Test
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        public async void GetAllUsers_ShouldReturnAllUsers()
        {
            int count = 5;
            var fakeUsers = A.CollectionOfDummy<User>(count).AsEnumerable();
            var dataStore = A.Fake<UserDbContext>();
            A.CallTo(() => dataStore.Get()).Returns(Task.fromResult(fakeUsers))
            var controller = new UserController(dataStore);

            var actionResult = await controller.Get();

            var result = actionResult.Result;
            Assert.Equal(count, result.Count());

        }
    }
}
