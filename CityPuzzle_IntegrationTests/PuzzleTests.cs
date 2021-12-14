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
    public class PuzzleTests
    {
        [Theory]
        [MemberData(nameof(GetPuzzles))]
        public void SaveTest(Puzzle puzzle)
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

        public static IEnumerable<object[]> GetPuzzles
        {
            get
            {
                return new[]
                {
                    new object[] { new Puzzle() { Name="Test_Puzzles", About= "Test_Puzzle", ImgAdress= "Test_Puzzle", Latitude=55.00, Longitude=100, Quest= "Test_Puzzle" } },
                    new object[] { new Puzzle() { Name="Test_Puzzles 2", About= "Test_Puzzle 2", ImgAdress= "Test_Puzzle 2", Latitude=30.00, Longitude=600, Quest= "Test_Puzzle 2" } }
                };
            }
        }
    }
}
