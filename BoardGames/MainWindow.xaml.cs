using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Model.Core;
using Model.Data;

namespace BoardGames.WPF
{
    // partial — этот класс продолжается в MainWindow.xaml (сгенерированный код).
    // : Window — наследуемся от базового класса WPF-окна.
    public partial class MainWindow : Window
    {
        private readonly GameCatalog _catalog;
        private readonly GameSerializer _serializer;

        public MainWindow(GameCatalog catalog, GameSerializer serializer)
        {
            // InitializeComponent() — сгенерированный метод, который разбирает xaml
            // и создаёт все контролы (cmbGames, btnShowDetails и т.д.).
            // После его вызова все x:Name-элементы доступны как поля класса.
            InitializeComponent();

            _catalog = catalog;
            _serializer = serializer;

            LoadGamesToComboBox();
            UpdateAnalytics();
        }

        private void LoadGamesToComboBox()
        {
            // В WPF выпадающий список работает через ItemsSource.
            // Мы передаём коллекцию объектов, а DisplayMemberPath="Name" в xaml
            // говорит: "показывай свойство Name каждого объекта".
            // Это чище, чем добавлять Items.Add() вручную — привязка к данным.
            cmbGames.ItemsSource = _catalog.GetAllGames()
                                           .OrderBy(g => g.Name)
                                           .ToList();

            btnShowDetails.IsEnabled = false;
        }

        // Обработчик события SelectionChanged — вызывается при выборе элемента в списке.
        // sender — объект, вызвавший событие (сам ComboBox).
        // e — аргументы события (что было выбрано, что добавилось/убралось).
        private void cmbGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnShowDetails.IsEnabled = cmbGames.SelectedItem != null;
        }

        private void btnShowDetails_Click(object sender, RoutedEventArgs e)
        {
            // SelectedItem в WPF возвращает object — приводим к BoardGame.
            // ПРИВЕДЕНИЕ К БАЗОВОМУ ТИПУ: SelectedItem хранится как object,
            // но реально там CardGame/EuroGame/PartyGame.
            BoardGame selectedGame = cmbGames.SelectedItem as BoardGame;
            if (selectedGame == null) return;

            // В WPF нет ShowDialog/Close с using — используем Owner для привязки окна.
            var detailWindow = new GameDetailWindow(selectedGame);
            detailWindow.Owner = this; // дочернее окно привязано к главному
            detailWindow.ShowDialog(); // модальный режим — блокирует MainWindow
        }

        private void btnOpenCatalog_Click(object sender, RoutedEventArgs e)
        {
            var catalogWindow = new CatalogWindow(_catalog, _serializer);
            catalogWindow.Owner = this;
            catalogWindow.ShowDialog();

            // После закрытия каталога обновляем данные в главном окне.
            LoadGamesToComboBox();
            UpdateAnalytics();
        }

        private void UpdateAnalytics()
        {
            // ПРИВЕДЕНИЕ К ИНТЕРФЕЙСУ #1: GameCatalog → IAnalizer
            IAnalizer analizer = _catalog;
            // ПРИВЕДЕНИЕ К ИНТЕРФЕЙСУ #2: GameCatalog → IGameCatalog
            IGameCatalog gameCatalog = _catalog;

            // В WPF Text у TextBlock, не Text у Label (у Label это Content).
            lblTotalGames.Text = $"Всего игр: {gameCatalog.GetAllGames().Count}";
            lblAvgMin.Text = $"Среднее мин. игроков: {analizer.AverageMinAmount():F1}";
            lblAvgMax.Text = $"Среднее макс. игроков: {analizer.AverageMaxAmount():F1}";
            lblMeanRange.Text = $"Средний диапазон: {analizer.MeanAmount():F1}";
            lblMeanAge.Text = $"Средний возраст: {analizer.MeanAgeRestriction():F1}+";
        }
    }
}