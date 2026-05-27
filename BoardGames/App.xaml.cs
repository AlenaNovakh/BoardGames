using System;
using System.IO;
using System.Windows;
using Model.Core;
using Model.Data;

namespace BoardGames.WPF
{
    public partial class App : Application
    {
        // OnStartup — вызывается при старте приложения.
        // Переопределяем его вместо Main(), чтобы создать каталог ДО открытия окна.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Путь к файлу с играми — рядом с exe в папке data.
            string dataPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "data", "games.json");

            var serializer = new GameSerializer(dataPath);
            var catalog = new GameCatalog();

            var games = serializer.Load();
            if (games.Count == 0)
            {
                // Первый запуск: создаём дефолтные игры и сохраняем.
                games = GameFactory.CreateDefaultGames();
                foreach (var g in games) catalog.AddGame(g);
                serializer.Save(catalog.GetAllGames());
            }
            else
            {
                foreach (var g in games) catalog.AddGame(g);
            }

            // Создаём главное окно и передаём зависимости через конструктор.
            // DEPENDENCY INJECTION — MainWindow не знает про файлы, только про каталог.
            var mainWindow = new MainWindow(catalog, serializer);
            mainWindow.Show();
        }
    }
}