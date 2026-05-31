using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Model.Core;

namespace BoardGames.WPF
{
    public partial class GameDetailWindow : Window
    {
        private readonly BoardGame _game;

        public GameDetailWindow(BoardGame game)
        {
            InitializeComponent();
            _game = game;
            PopulateWindow();
        }

        private void PopulateWindow()
        {
            this.Title = _game.Name;

            lblName.Text = _game.Name;
            lblType.Text = _game.GetGameType();
            lblPlayers.Text = $"{_game.MinPlayers}–{_game.MaxPlayers} чел.";
            lblAge.Text = $"{_game.AgeRestriction}+";
            lblDescription.Text = _game.Description;

            if (_game is CardGame cg)
            {
                lblExtraKey.Text = "Колода:";
                lblExtraValue.Text = $"{cg.DeckSize} карт";
            }
            else if (_game is EuroGame eg)
            {
                lblExtraKey.Text = "Сложность:";
                lblExtraValue.Text = eg.GameComplexity.ToString();
            }
            else if (_game is PartyGame pg)
            {
                lblExtraKey.Text = "Шумная:";
                lblExtraValue.Text = pg.IsNoisy ? "да" : "нет";
            }

            LoadImage(_game.ImagePath);
        }

        private void LoadImage(string imagePath)
        {
            string fullPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, imagePath ?? "");

            if (File.Exists(fullPath))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    imgCover.Source = bitmap;
                }
                catch
                {
                    imgCover.Source = null;
                }
            }
        }
    }
}