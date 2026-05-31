using System.Linq;

namespace Model.Core
{
    public partial class GameCatalog
    {
        public double AverageMinAmount()
        {
            if (!_games.Any()) return 0;
            return _games.Average(g => g.MinPlayers);
        }

        public double AverageMaxAmount()
        {
            if (!_games.Any()) return 0;
            return _games.Average(g => g.MaxPlayers);
        }
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