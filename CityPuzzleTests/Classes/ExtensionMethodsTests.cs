using CityPuzzle.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static CityPuzzle.Classes.Structs;

namespace CityPuzzle.Classes.Tests
{
    public class ExtensionMethodsTests
    {
        [Theory]
        [MemberData(nameof(GetUsers))]
        public void Top10Test(List<User> users)
        {

            foreach (var user in users)
            {
                for (int i = 0; i < user.ID; ++i)
                {
                    
                    user.QuestsCompleted.Add(new CompletedPuzzle());
                }
            }

            List<UserInfo> topUsers = ExtensionMethods.Top10(users);

            UserInfo curr;
            UserInfo prev = new UserInfo();
            for (int i = 0; i < topUsers.Count; ++i)
            {
                curr = topUsers[i];
                if (i != 0)
                {
                    //Test if users are sorted properly
                    Assert.True(curr.Score <= prev.Score);

                    if (curr.Score == prev.Score)
                    {
                        //Test if users with matching scores have the same index
                        Assert.Equal(curr.Index, prev.Index);
                    }
                }
                prev = curr;
            }

            //Test if the maximum index of topUsers is 10
            Assert.Equal(10, topUsers[topUsers.Count - 1].Index);
        }

        public void Top10CastTest()
        {

        }

        public static IEnumerable<object[]> GetUsers
        {
            get
            {
                return new[]
                {
                    new object[]
                    {
                        new List<User>
                        {
                            new User(new UserVerifier()) { ID = 1, UserName = "u1" },
                            new User(new UserVerifier()) { ID = 2, UserName = "u2" },
                            new User(new UserVerifier()) { ID = 3, UserName = "u3" },
                            new User(new UserVerifier()) { ID = 4, UserName = "u4" },
                            new User(new UserVerifier()) { ID = 5, UserName = "u5" },
                            new User(new UserVerifier()) { ID = 5, UserName = "u12" },
                            new User(new UserVerifier()) { ID = 5, UserName = "u13" },
                            new User(new UserVerifier()) { ID = 6, UserName = "u6" },
                            new User(new UserVerifier()) { ID = 7, UserName = "u7" },
                            new User(new UserVerifier()) { ID = 8, UserName = "u8" },
                            new User(new UserVerifier()) { ID = 9, UserName = "u9" },
                            new User(new UserVerifier()) { ID = 10, UserName = "u10" },
                            new User(new UserVerifier()) { ID = 11, UserName = "u11" },
                        }
                    }
                };
            }
        }
    }
}
