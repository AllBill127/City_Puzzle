using CityPuzzle.Classes;
using CityPuzzle.Rest_Services.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        [MemberData(nameof(SaveUser))]
        public void setUserSaveTest(User user)
        {
            APICommands WebServices = new APICommands("http://localhost:5000/api/");
            user.ChangeService(WebServices);
            Thread save=new Thread(()=>user.Save());
            save.Start();
            save.Join();
            while (user.ID == 0)
                Thread.Sleep(100);
            Task<List<User>> obTask = Task.Run(() => WebServices.GetUsers());
            obTask.Wait();
            List<User> useiai = obTask.Result;
            Assert.True(useiai.Any(rt => rt.ID == user.ID));
        }
        [Theory]
        [MemberData(nameof(SavePuzzles))]
        public void setPuzzleSaveTest(Puzzle puzzle)
        {
            APICommands WebServices = new APICommands("http://localhost:5000/api/");
            puzzle.ChangeService(WebServices);
            Thread save = new Thread(() => puzzle.Save());
            save.Start();
            save.Join();
            while (puzzle.ID == 0)
                Thread.Sleep(100);
            Task<List<Puzzle>> obTask = Task.Run(() => WebServices.GetPuzzles());
            obTask.Wait();
            List<Puzzle> puzles = obTask.Result;
            Assert.True(puzles.Any(rt => rt.ID == puzzle.ID));
        }
        [Theory]
        [MemberData(nameof(SaveRooms))]
        public void setRoomSaveTest(Room room)
        {
            APICommands WebServices = new APICommands("http://localhost:5000/api/");
            room.ChangeService(WebServices);
            var list = new List<int>() { 8, 3, 2 };
            Thread save = new Thread(() => room.Save(list));
            save.Start();
            save.Join();
            while (room.ID == 0)
                Thread.Sleep(100);
            Task<List<Room>> obTask = Task.Run(() => WebServices.GetRooms());
            obTask.Wait();
            List<Room> allRooms = obTask.Result;
            Assert.True(allRooms.Any(rt => rt.ID == room.ID));
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
        public static IEnumerable<object[]> SaveRooms
        {
            get
            {
                return new[]
                {
                    new object[] { new Room() { Owner=10,RoomSize=100,RoomPin="Test Room" } },
                };
            }
        }
        public static IEnumerable<object[]> SavePuzzles
        {
            get
            {
                return new[]
                {
                    new object[] { new Puzzle() { Name="Test_Puzzles", About= "Test_Puzzle", ImgAdress= "Test_Puzzle", Latitude=55.00, Longitude=100, Quest= "Test_Puzzle" } },
                };
            }
        }
        public static IEnumerable<object[]> SaveUser
        {
            get
            {
                return new[]
                {
                    new object[] { new User() { Name = "Test7", LastName = "Test1", Email = "Test1", MaxQuestDistance = 10, Pass = "kazkas", UserName = "Test1" } },
                    new object[] { new User() { Name = "Test8", LastName = "Test2", Email = "Test2", MaxQuestDistance = 10, Pass = "kazkas", UserName = "Test2" } }
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