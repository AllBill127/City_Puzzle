using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CityPuzzle.Classes.Tests
{
    public class RoomTests
    {
        [Theory]
        [MemberData(nameof(GetUsers))]
        public void setParticipantsTestPass(User user)
        {
            Room room = new Room() { ID = "", Owner = 0, RoomSize = 0};

            room.setParticipants(user);

            Assert.Contains(1478, room.ParticipantIDs);
            Assert.Contains(1410, room.ParticipantIDs);
        }

        [Theory]
        [MemberData(nameof(GetUsers))]
        public void setParticipantsTestFail(User user)
        {
            Room room = new Room() { ID = "", Owner = 0, RoomSize = 0 };

            room.setParticipants(user);

            Assert.DoesNotContain(1889, room.ParticipantIDs);
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