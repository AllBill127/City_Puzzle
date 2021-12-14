using CityPuzzle.Classes;
using CityPuzzle.Rest_Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CityPuzzle_IntegrationTests
{
    public class UserTests
    {
        [Theory]
        [MemberData(nameof(GetUsers))]
        public void SaveTest(User user)
        {
            APICommands WebServices = new APICommands("http://localhost:5000/api/");
            user.ChangeService(WebServices);

            Thread save = new Thread(() => user.Save());
            save.Start();
            save.Join();
            while (user.ID == 0)
                Thread.Sleep(100);

            Task<List<User>> obTask = Task.Run(() => WebServices.GetUsers());
            obTask.Wait();
            List<User> useiai = obTask.Result;

            Assert.True(useiai.Any(rt => rt.ID == user.ID));
        }

        public static IEnumerable<object[]> GetUsers
        {
            get
            {
                return new[]
                {
                    new object[] { new User() { Name = "Test7", LastName = "Test1", Email = "Test1", MaxQuestDistance = 10, Pass = "kazkas", UserName = "Test1" } },
                    new object[] { new User() { Name = "Test8", LastName = "Test2", Email = "Test2", Pass = "kazkas", UserName = "Test2" } }
                };
            }
        }
    }
}
