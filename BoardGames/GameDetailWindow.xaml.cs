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

            // Заполняем TextBlock-и через свойство .Text (не .Content как у Label).
            lblName.Text = _game.Name;
            lblType.Text = _game.GetGameType(); // ПОЛИМОРФИЗМ
            lblPlayers.Text = $"{_game.MinPlayers}–{_game.MaxPlayers} чел.";
            lblAge.Text = $"{_game.AgeRestriction}+";
            lblDescription.Text = _game.Description;

            // ПРИВЕДЕНИЕ К ТИПУ (downcast) — определяем реальный тип игры
            // и показываем уникальное поле. Это #3, #4, #5 из счётчика приведений.
            if (_game is CardGame cg) // ПРИВЕДЕНИЕ #3
            {
                lblExtraKey.Text = "Колода:";
                lblExtraValue.Text = $"{cg.DeckSize} карт";
            }
            else if (_game is EuroGame eg) // ПРИВЕДЕНИЕ #4
            {
                lblExtraKey.Text = "Сложность:";
                lblExtraValue.Text = eg.GameComplexity.ToString();
            }
            else if (_game is PartyGame pg) // ПРИВЕДЕНИЕ #5
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
                    // BitmapImage — WPF-класс для загрузки изображений (не System.Drawing.Image).
                    // UriKind.Absolute — передаём полный путь.
                    // CacheOption.OnLoad — загружаем сразу, не держим файл открытым.
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(fullPath, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();

                    // Image.Source принимает ImageSource, BitmapImage — его наследник.
                    imgCover.Source = bitmap;
                }
                catch
                {
                    imgCover.Source = null; // не падаем при битом файле
                }
            }
        }
    }
}