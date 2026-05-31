namespace Model.Data
{
    public class GameDto
    {
        public string GameType { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int AgeRestriction { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        public int DeckSize { get; set; }
        public string Complexity { get; set; } = string.Empty;
        public bool IsNoisy { get; set; }
    }
}