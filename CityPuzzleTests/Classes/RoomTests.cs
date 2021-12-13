using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace CityPuzzle.Classes.Tests
{
    public class RoomTests
    {
        [Theory]
        [MemberData(nameof(GetUsers))]
        public void setParticipantsTest(User user)
        {
            Room room = new Room() { ID = 100, Owner = 0, RoomSize = 0};
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