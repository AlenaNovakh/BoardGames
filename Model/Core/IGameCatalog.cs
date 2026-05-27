namespace Model.Core
{
    // interface — контракт. Описывает ЧТО умеет делать каталог, но не КАК.
    // Любой класс, реализующий IGameCatalog, ОБЯЗАН иметь эти три метода.
    // ПРИНЦИП РАЗДЕЛЕНИЯ ИНТЕРФЕЙСА (ISP из SOLID):
    //   этот интерфейс отвечает только за операции с каталогом (добавить/удалить/получить).
    //   Статистика — в другом интерфейсе (IAnalizer).
    public interface IGameCatalog
    {
        void AddGame(BoardGame game);
        void RemoveGame(BoardGame game);

        // IReadOnlyList — возвращаем список только для чтения снаружи.
        // Внутри мы используем List<BoardGame>, но наружу отдаём "только читать".
        // Это защита данных — внешний код не сможет напрямую вызвать _games.Add().
        IReadOnlyList<BoardGame> GetAllGames();
    }
}