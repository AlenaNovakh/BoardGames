using System.Linq;

namespace Model.Core
{
    // Та же partial часть, тот же класс — просто в другом файле.
    // Здесь реализуем IAnalizer.
    public partial class GameCatalog
    {
        // --- Реализация IAnalizer ---

        // LINQ (Language Integrated Query) — встроенный язык запросов C#.
        // Average() считает среднее арифметическое по коллекции.
        // g => g.MinPlayers — лямбда-выражение: "для каждого g вернуть g.MinPlayers".

        public double AverageMinAmount()
        {
            if (!_games.Any()) return 0; // защита от деления на ноль
            return _games.Average(g => g.MinPlayers);
        }

        public double AverageMaxAmount()
        {
            if (!_games.Any()) return 0;
            return _games.Average(g => g.MaxPlayers);
        }

        // MeanAmount — средний диапазон мест: среднее (MaxPlayers - MinPlayers).
        public double MeanAmount()
        {
            if (!_games.Any()) return 0;
            return _games.Average(g => g.MaxPlayers - g.MinPlayers);
        }

        public double MeanAgeRestriction()
        {
            if (!_games.Any()) return 0;
            return _games.Average(g => g.AgeRestriction);
        }
    }
}