using System.Collections.Generic;
using System.Linq;

namespace Model.Core
{
    // partial — означает, что класс разбит на несколько файлов.
    // Компилятор склеивает все части в один класс при компиляции.
    // Зачем: разделяем ответственность по файлам, не раздувая один файл.
    // GameCatalog реализует оба интерфейса — C# позволяет указать несколько через запятую.
    public partial class GameCatalog : IGameCatalog, IAnalizer
    {
        // Приватное поле — хранилище игр.
        // Приватное — доступно ТОЛЬКО внутри этого класса (всех его partial-частей).
        // List<BoardGame> — обобщённый тип (generic): список, хранящий BoardGame.
        // ИНКАПСУЛЯЦИЯ: наружу список не отдаём напрямую.
        private readonly List<BoardGame> _games = new List<BoardGame>();

        // --- Реализация IGameCatalog ---

        public void AddGame(BoardGame game)
        {
            // Проверка: не добавляем null и не добавляем дубли по имени.
            if (game == null) throw new ArgumentNullException(nameof(game));
            if (_games.Any(g => g.Name == game.Name))
                throw new InvalidOperationException($"Игра '{game.Name}' уже есть в каталоге.");
            _games.Add(game);
        }

        public void RemoveGame(BoardGame game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            // Remove возвращает false, если элемента не было — выбрасываем понятную ошибку.
            bool removed = _games.Remove(game);
            if (!removed)
                throw new InvalidOperationException($"Игра '{game.Name}' не найдена в каталоге.");
        }

        // Возвращаем копию как IReadOnlyList — снаружи нельзя изменить.
        // AsReadOnly() оборачивает List в ReadOnlyCollection.
        public IReadOnlyList<BoardGame> GetAllGames() => _games.AsReadOnly();
    }
}