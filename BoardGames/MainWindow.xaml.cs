using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Model.Core;
using Model.Data;

namespace BoardGames.WPF
{
    public partial class MainWindow : Window
    {
        private readonly GameCatalog _catalog;
        private readonly GameSerializer _serializer;

        public MainWindow(GameCatalog catalog, GameSerializer serializer)
        {
            InitializeComponent();

            _catalog = catalog;
            _serializer = serializer;

            LoadGamesToComboBox();
            UpdateAnalytics();
        }

        private void LoadGamesToComboBox()
        {
            cmbGames.ItemsSource = _catalog.GetAllGames()
                                           .OrderBy(g => g.Name)
                                           .ToList();

            btnShowDetails.IsEnabled = false;
        }

        private void cmbGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnShowDetails.IsEnabled = cmbGames.SelectedItem != null;
        }

        private void btnShowDetails_Click(object sender, RoutedEventArgs e)
        {
            BoardGame selectedGame = cmbGames.SelectedItem as BoardGame;
            if (selectedGame == null) return;

            var detailWindow = new GameDetailWindow(selectedGame);
            detailWindow.Owner = this;
            detailWindow.ShowDialog();
        }

        private void btnOpenCatalog_Click(object sender, RoutedEventArgs e)
        {
            var catalogWindow = new CatalogWindow(_catalog, _serializer);
            catalogWindow.Owner = this;
            catalogWindow.ShowDialog();

            LoadGamesToComboBox();
            UpdateAnalytics();
        }

        private void UpdateAnalytics()
        {
            IAnalizer analizer = _catalog;
            IGameCatalog gameCatalog = _catalog;

            lblTotalGames.Text = $"Всего игр: {gameCatalog.GetAllGames().Count}";
            lblAvgMin.Text = $"Среднее мин. игроков: {analizer.AverageMinAmount():F1}";
            lblAvgMax.Text = $"Среднее макс. игроков: {analizer.AverageMaxAmount():F1}";
            lblMeanRange.Text = $"Средний диапазон: {analizer.MeanAmount():F1}";
            lblMeanAge.Text = $"Средний возраст: {analizer.MeanAgeRestriction():F1}+";
        }
    }
}