using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CityPuzzle.Classes
{
    public class AddPageLogic
    {
        private int puzzleNr = 0;
        private List<Puzzle> allPuzzles;
        public List<Puzzle> SelectedPuzzles { get; private set; }
        private Task<List<Puzzle>> data_collector_task;

        public event EventHandler OnNoPuzzlesFound;
        public event EventHandler OnEndOfPuzzles;
        public event EventHandler<OnPuzzleChangeEventArgs> OnPuzzleChange;
        public class OnPuzzleChangeEventArgs : EventArgs { public string Name; public string ImgAdress; public string About; }

        public AddPageLogic()
        {
            data_collector_task = Task.Run(() => App.WebServices.GetPuzzles());
        }

        public void ChangePuzzle(bool direction)
        {
            data_collector_task.Wait();
            allPuzzles=data_collector_task.Result;
            if (allPuzzles.Count == 0)
            {
                OnNoPuzzlesFound?.Invoke(this, EventArgs.Empty);
            }
            else if (SelectedPuzzles.Contains(allPuzzles[puzzleNr++]) && direction)
            {
                ChangePuzzle(true);
            }
            else if (SelectedPuzzles.Contains(allPuzzles[puzzleNr--]) && !direction)
            {
                ChangePuzzle(false);
            }
            else if (puzzleNr == allPuzzles.Count - 1 && direction)
            {
                OnEndOfPuzzles?.Invoke(this, EventArgs.Empty);
            }
            else if (direction)
            {
                puzzleNr++;
                OnPuzzleChange?.Invoke(this, new OnPuzzleChangeEventArgs { Name = allPuzzles[puzzleNr].Name, ImgAdress = allPuzzles[puzzleNr].ImgAdress, About = allPuzzles[puzzleNr].About });
            }
            else if (puzzleNr == 0 && !direction)
            {
                return;
            }
            else if (!direction)
            {
                puzzleNr--;
                OnPuzzleChange?.Invoke(this, new OnPuzzleChangeEventArgs { Name = allPuzzles[puzzleNr].Name, ImgAdress = allPuzzles[puzzleNr].ImgAdress, About = allPuzzles[puzzleNr].About });
            }
        }

        public void AddPuzzle()
        {
            Console.WriteLine("imu");
            SelectedPuzzles.Add(allPuzzles[puzzleNr]);
            ChangePuzzle(true);
        }
    }
}
