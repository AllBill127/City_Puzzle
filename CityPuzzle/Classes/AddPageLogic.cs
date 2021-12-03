using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class AddPageLogic
    {
        private int puzzleNr = -1;
        private List<Lazy<Puzzle>> allPuzzles;
        public List<Lazy<Puzzle>> SelectedPuzzles { get; private set; }

        public event EventHandler OnNoPuzzlesFound;
        public event EventHandler OnEndOfPuzzles;
        public event EventHandler<OnPuzzleChangeEventArgs> OnPuzzleChange;
        public class OnPuzzleChangeEventArgs : EventArgs { public string Name; public string ImgAdress; public string About; }

        public AddPageLogic()
        {
            allPuzzles = Sql.ReadPuzzles();
        }

        public void ChangePuzzle(bool direction)
        {
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
                OnPuzzleChange?.Invoke(this, new OnPuzzleChangeEventArgs { Name = allPuzzles[puzzleNr].Value.Name, ImgAdress = allPuzzles[puzzleNr].Value.ImgAdress, About = allPuzzles[puzzleNr].Value.About });
            }
            else if (puzzleNr == 0 && !direction)
            {
                return;
            }
            else if (!direction)
            {
                puzzleNr--;
                OnPuzzleChange?.Invoke(this, new OnPuzzleChangeEventArgs { Name = allPuzzles[puzzleNr].Value.Name, ImgAdress = allPuzzles[puzzleNr].Value.ImgAdress, About = allPuzzles[puzzleNr].Value.About });
            }
        }

        public void AddPuzzle()
        {
            SelectedPuzzles.Add(allPuzzles[puzzleNr]);
            ChangePuzzle(true);
        }
    }
}
