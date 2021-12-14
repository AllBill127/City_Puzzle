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
        [MemberData(nameof(GetRooms))]
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

        public static IEnumerable<object[]> GetRooms
        {
            get
            {
                return new[]
                {
                    new object[] { new Room() { Owner=10, RoomSize=100, RoomPin = "Test Room" } },
                    new object[] { new Room() { Owner=9, RoomSize=111, RoomPin = "Test Room 2" } }
                };
            }
        }
    }
}