using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CityPuzzle.Classes
{
    public class AddPageLogic
    {
        private int puzzleNr = -1;
        private List<Puzzle> allPuzzles;
        private Task<List<Puzzle>> data_collector_task;

        public List<Puzzle> SelectedPuzzles { get; private set; }

        public event EventHandler OnNoPuzzlesFound;
        public event EventHandler OnEndOfPuzzles;
        public event EventHandler<OnPuzzleChangeEventArgs> OnPuzzleChange;
        public class OnPuzzleChangeEventArgs : EventArgs { public string Name; public string ImgAdress; public string About; }

        public AddPageLogic(List<Puzzle> roomPuzzles)
        {
            data_collector_task = Task.Run(() => App.WebServices.GetPuzzles());
            SelectedPuzzles = roomPuzzles;
        }

        public void ChangePuzzle(bool direction)
        {
            data_collector_task.Wait();
            allPuzzles = data_collector_task.Result;

            if (allPuzzles.Count == 0)
            {
                OnNoPuzzlesFound?.Invoke(this, EventArgs.Empty);
            }
            else if (allPuzzles.Intersect(SelectedPuzzles).ToList().Count == allPuzzles.Count)  //(!allPuzzles.Except(SelectedPuzzles).Any())
                OnEndOfPuzzles?.Invoke(this, EventArgs.Empty);
            else if (puzzleNr == allPuzzles.Count - 1 && direction)
            {
                return;
            }
            else if (puzzleNr == 0 && !direction)
            {
                return;
            }
            else if (direction && (puzzleNr + 1) <= allPuzzles.Count - 1)
            {
                ++puzzleNr;

                if (SelectedPuzzles.Any(item => item.ID == allPuzzles[puzzleNr].ID))
                    ChangePuzzle(true);
                else
                    OnPuzzleChange?.Invoke(this, new OnPuzzleChangeEventArgs { Name = allPuzzles[puzzleNr].Name, ImgAdress = allPuzzles[puzzleNr].ImgAdress, About = allPuzzles[puzzleNr].About });
            }
            else if (!direction && (puzzleNr - 1) >= 0)
            {
                --puzzleNr;

                if (SelectedPuzzles.Any(item => item.ID == allPuzzles[puzzleNr].ID))
                    ChangePuzzle(true);
                else
                    OnPuzzleChange?.Invoke(this, new OnPuzzleChangeEventArgs { Name = allPuzzles[puzzleNr].Name, ImgAdress = allPuzzles[puzzleNr].ImgAdress, About = allPuzzles[puzzleNr].About });
            }
        }

        public void AddPuzzle()
        {
            SelectedPuzzles.Add(allPuzzles[puzzleNr]);
            if ((puzzleNr + 1) <= allPuzzles.Count - 1)
                ChangePuzzle(true);
            else if ((puzzleNr - 1) >= 0)
                ChangePuzzle(false);
        }
    }
}
