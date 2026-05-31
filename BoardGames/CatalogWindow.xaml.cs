using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Model.Core;
using Model.Data;

namespace BoardGames.WPF
{
    public partial class CatalogWindow : Window
    {
        private readonly GameCatalog _catalog;
        private readonly GameSerializer _serializer;

        public CatalogWindow(GameCatalog catalog, GameSerializer serializer)
        {
            InitializeComponent();
            _catalog = catalog;
            _serializer = serializer;
            LoadTable();
        }

        private void LoadTable()
        {
            var viewModels = _catalog.GetAllGames()
                                     .Select(g => new GameViewModel(g))
                                     .ToList();

            dgvGames.ItemsSource = viewModels;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddGameWindow();
            addWindow.Owner = this;

            if (addWindow.ShowDialog() == true)
            {
                try
                {
                    IGameCatalog gameCatalog = _catalog;
                    gameCatalog.AddGame(addWindow.CreatedGame);
                    _serializer.Save(_catalog.GetAllGames());
                    LoadTable();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dgvGames.SelectedItem is not GameViewModel selectedVm)
            {
                MessageBox.Show("Выберите игру для удаления.", "Внимание",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            BoardGame gameToRemove = _catalog.GetAllGames()
                                             .FirstOrDefault(g => g.Name == selectedVm.Name);
            if (gameToRemove == null) return;

            var result = MessageBox.Show(
                $"Удалить игру «{gameToRemove.Name}»?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    IGameCatalog gameCatalog = _catalog;
                    gameCatalog.RemoveGame(gameToRemove);
                    _serializer.Save(_catalog.GetAllGames());
                    LoadTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    public class GameViewModel
    {
        public string Name { get; } = string.Empty;
        public string GameType { get; } = string.Empty;
        public int MinPlayers { get; }
        public int MaxPlayers { get; }
        public int AgeRestriction { get; }
        public string Description { get; } = string.Empty;

        public GameViewModel(BoardGame game)
        {
            Name = game.Name;
            GameType = game.GetGameType();
            MinPlayers = game.MinPlayers;
            MaxPlayers = game.MaxPlayers;
            AgeRestriction = game.AgeRestriction;
            Description = game.Description;
        }
    }
}