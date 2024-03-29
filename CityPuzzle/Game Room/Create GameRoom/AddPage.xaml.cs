﻿using CityPuzzle.Classes;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CityPuzzle.Classes.AddPageLogic;

namespace CityPuzzle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPage : ContentPage
    {
        private AddPageLogic addPageLogic;

        public AddPage(List<Puzzle> roomPuzzles)
        {
            InitializeComponent();
            addPageLogic = new AddPageLogic(roomPuzzles);
            addPageLogic.OnEndOfPuzzles += FinishSelecting_Clicked;
            addPageLogic.OnPuzzleChange += ShowPuzzle;
            addPageLogic.OnNoPuzzlesFound += EmptyListError;

            addPageLogic.ChangePuzzle(true);
        }

        private void AddPuzzle_Clicked(object sender, EventArgs e)
        {
            addPageLogic.AddPuzzle();
        }

        private void NextPuzzle_Clicked(object sender, EventArgs e)
        {
            addPageLogic.ChangePuzzle(true);
        }

        private void PrevPuzzle_Clicked(object sender, EventArgs e)
        {
            addPageLogic.ChangePuzzle(false);
        }

        private void FinishSelecting_Clicked(object sender, EventArgs e)
        {
            CreateRoomPage.RoomPuzzles = addPageLogic.SelectedPuzzles;
            Navigation.PopAsync();
        }

        private void ShowPuzzle(object sender, OnPuzzleChangeEventArgs e)
        {
            PuzzleName.Text = e.Name;
            PuzzleImg.Source = e.ImgAdress;
            PuzzleInfo.Text = e.About;
        }

        private async void EmptyListError(object sender, EventArgs e)
        {
            await DisplayAlert("Error", "Nepavyksta aptikti delionių.", "Uždaryti");
        }
    }
}