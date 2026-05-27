namespace Model.Core
{
    // НАСЛЕДОВАНИЕ: CardGame — это BoardGame, но с дополнительным полем DeckSize.
    // Двоеточие после имени класса означает "наследуется от".
    public class CardGame : BoardGame
    {
        // Уникальное поле — только у карточных игр есть размер колоды.
        public int DeckSize { get; set; }

        // В конструкторе наследника нужно вызвать конструктор родителя через : base(...)
        // Это передаёт общие параметры в BoardGame, а DeckSize инициализируем здесь.
        public CardGame(string name, int minPlayers, int maxPlayers,
                        int ageRestriction, string description, string imagePath, int deckSize)
            : base(name, minPlayers, maxPlayers, ageRestriction, description, imagePath)
        {
            DeckSize = deckSize;
        }

        // override — ОБЯЗАТЕЛЬНАЯ реализация абстрактного метода из BoardGame.
        // Слово override явно говорит компилятору: "я знаю, что переопределяю родительский метод".
        public override string GetGameType() => "Карточная";

        // Переопределяем GetShortInfo, чтобы добавить информацию о колоде.
        // Вызываем base.GetShortInfo() — это вызов родительской версии метода,
        // чтобы не переписывать уже готовый код.
        public override string GetShortInfo()
        {
            return base.GetShortInfo() + $" | Колода: {DeckSize} карт";
        }
    }
}