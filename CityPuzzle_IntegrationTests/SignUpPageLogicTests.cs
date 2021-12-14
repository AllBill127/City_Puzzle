using CityPuzzle;
using System;
using System.Collections.Generic;
using Xunit;
using Xamarin.Forms;
using CityPuzzle.Classes;
using CityPuzzle.Rest_Services.Client;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;

namespace CityPuzzle.Tests
{
    public class SignUpPageLogicTests
    {
        [Theory]
        [InlineData("SPLTest", "", "A", "B", "test@test.com", "3")]
        public static void CreateUserTest(string userName, string pass, string name, string lastName, string email, string dist)
        {
            APICommands WebServices = new APICommands("http://localhost:5000/api/");
            var user = new User();
            user.ChangeService(WebServices);

            SignUpPageLogic SPL = new SignUpPageLogic();

            Thread save = new Thread(() => SPL.CreateUser(userName, pass, name, lastName, email, dist));
            save.Start();
            save.Join();

            Task<List<User>> obTask = Task.Run(() => WebServices.GetUsers());
            obTask.Wait();
            List<User> users = obTask.Result;

            Assert.True(users.Any(user => user.UserName == userName && user.Name == name && user.LastName == lastName && user.Email == email && user.MaxQuestDistance == int.Parse(dist)));
        }
    }
}