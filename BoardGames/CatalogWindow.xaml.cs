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

        // Загружаем данные в DataGrid через ItemsSource.
        // DataGrid отображает коллекцию объектов — каждый объект = одна строка.
        // Столбцы берут данные через Binding = свойства объекта.
        // Поэтому нам нужен GameViewModel, у которого есть свойство GameType
        // (у BoardGame есть метод GetGameType(), но не свойство — привязка к методам не работает).
        private void LoadTable()
        {
            // Преобразуем BoardGame → GameViewModel для отображения в таблице.
            // Select — LINQ-проекция: для каждого game создаём новый GameViewModel.
            var viewModels = _catalog.GetAllGames()
                                     .Select(g => new GameViewModel(g))
                                     .ToList();

            dgvGames.ItemsSource = viewModels;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddGameWindow();
            addWindow.Owner = this;

            if (addWindow.ShowDialog() == true) // true = пользователь нажал "Добавить"
            {
                try
                {
                    // ПРИВЕДЕНИЕ К ИНТЕРФЕЙСУ #6: GameCatalog → IGameCatalog
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
            // SelectedItem в DataGrid возвращает объект строки — у нас это GameViewModel.
            if (dgvGames.SelectedItem is not GameViewModel selectedVm)
            {
                MessageBox.Show("Выберите игру для удаления.", "Внимание",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Находим реальный BoardGame объект по имени.
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
                    // ПРИВЕДЕНИЕ К ИНТЕРФЕЙСУ #7
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

    // ViewModel — промежуточный объект для привязки данных к DataGrid.
    // Зачем: у BoardGame нет свойства GameType (есть метод GetGameType()),
    // а WPF Binding работает только со свойствами.
    // Это также принцип РАЗДЕЛЕНИЯ ОТВЕТСТВЕННОСТИ: модель данных ≠ модель отображения.
    public class GameViewModel
    {
        public string Name { get; } = string.Empty;
        public string GameType { get; } = string.Empty;  // вызываем GetGameType() один раз
        public int MinPlayers { get; }
        public int MaxPlayers { get; }
        public int AgeRestriction { get; }
        public string Description { get; } = string.Empty;

        public GameViewModel(BoardGame game)
        {
            // ПРИВЕДЕНИЕ К БАЗОВОМУ ТИПУ (upcast, неявное):
            // game может быть CardGame/EuroGame/PartyGame, но мы работаем с ним как BoardGame.
            Name = game.Name;
            GameType = game.GetGameType(); // ПОЛИМОРФИЗМ
            MinPlayers = game.MinPlayers;
            MaxPlayers = game.MaxPlayers;
            AgeRestriction = game.AgeRestriction;
            Description = game.Description;
        }
    }
}