namespace Model.Core
{
    public interface IGameCatalog
    {
        void AddGame(BoardGame game);
        void RemoveGame(BoardGame game);
        IReadOnlyList<BoardGame> GetAllGames();
    }
}