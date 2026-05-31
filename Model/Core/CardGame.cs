namespace Model.Core
{
    public class CardGame : BoardGame
    {
        public int DeckSize { get; set; }

        public CardGame(string name, int minPlayers, int maxPlayers,
                        int ageRestriction, string description, string imagePath, int deckSize)
            : base(name, minPlayers, maxPlayers, ageRestriction, description, imagePath)
        {
            DeckSize = deckSize;
        }
        public override string GetGameType() => "Карточная";
        public override string GetShortInfo()
        {
            return base.GetShortInfo() + $" | Колода: {DeckSize} карт";
        }
    }
}