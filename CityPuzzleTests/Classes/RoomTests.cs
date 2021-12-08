using CityPuzzle.Classes;
using CityPuzzle.Rest_Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CityPuzzle.Classes.Tests
{
    public class RoomTests
    {
        [Theory]
        [MemberData(nameof(GetUsers))]
        public void setParticipantsTest(User user)
        {
            Room room = new Room() { ID = 100, Owner = 0, RoomSize = 0 };
            room.setParticipants(user);

            Assert.True(room.Participants.Any(part => part.UserId == user.ID && part.RoomId == room.ID));
        }

        [Theory]
        [MemberData(nameof(GetPuzzles))]
        public void setTaskTest(Puzzle puzzle)
        {
            Room room = new Room() { ID = 100, Owner = 0, RoomSize = 0 };
            room.SetTask(puzzle);

            Assert.True(room.RoomTasks.Any(rt => rt.PuzzleId == puzzle.ID && rt.RoomId == room.ID));
        }


        [Theory]
        [MemberData(nameof(GetPuzzles))]
        public void setUserTest(Puzzle puzzle)
        {
            APICommands WebServices = new APICommands("http://localhost:5000/api/");
            User user = new User() { Name = "Justasss", LastName = "Test", Email = "Test0", MaxQuestDistance = 10, Pass = "kazkas", UserName = "Justinaitis" };
            user.Save(WebServices);
            Task<List<User>> obTask = Task.Run(() => WebServices.GetUsers());
            obTask.Wait();
            List<User> useiai = obTask.Result;

            Assert.True(useiai.Any(rt => rt.Name == user.Name && rt.LastName == user.LastName));
        }

        public static IEnumerable<object[]> GetPuzzles
        {
            get
            {
                return new[]
                {
                    new object[] { new Puzzle() { ID = 12 } },
                    new object[] { new Puzzle() { ID = 13 } }
                };
            }
        }

        public static IEnumerable<object[]> GetUsers
        {
            get
            {
                return new[]
                {
                    new object[] { new User(new UserVerifier()) { ID = 1478 } },
                    new object[] { new User(new UserVerifier()) { ID = 1410 } }
                };
            }
        }
    }
}