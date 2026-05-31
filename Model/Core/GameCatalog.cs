using System.Collections.Generic;
using System.Linq;

namespace Model.Core
{
    public partial class GameCatalog : IGameCatalog, IAnalizer
    {
        private readonly List<BoardGame> _games = new List<BoardGame>();
        public void AddGame(BoardGame game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            if (_games.Any(g => g.Name == game.Name))
                throw new InvalidOperationException($"Игра '{game.Name}' уже есть в каталоге.");
            _games.Add(game);
        }

        public void RemoveGame(BoardGame game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            bool removed = _games.Remove(game);
            if (!removed)
                throw new InvalidOperationException($"Игра '{game.Name}' не найдена в каталоге.");
        }
        public IReadOnlyList<BoardGame> GetAllGames() => _games.AsReadOnly();
    }
}