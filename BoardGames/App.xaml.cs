using System;
using System.IO;
using System.Windows;
using Model.Core;
using Model.Data;

namespace BoardGames.WPF
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            string dataPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "data", "games.json");

            var serializer = new GameSerializer(dataPath);
            var catalog = new GameCatalog();

            var games = serializer.Load();
            if (games.Count == 0)
            {
                games = GameFactory.CreateGames();
                foreach (var g in games) catalog.AddGame(g);
                serializer.Save(catalog.GetAllGames());
            }
            else
            {
                foreach (var g in games) catalog.AddGame(g);
            }

            var mainWindow = new MainWindow(catalog, serializer);
            mainWindow.Show();
        }
    }
}