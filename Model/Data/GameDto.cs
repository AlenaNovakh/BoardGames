namespace Model.Data
{
    // DTO (Data Transfer Object) — промежуточный класс для сериализации.
    // Зачем: BoardGame абстрактный, его нельзя напрямую десериализовать из JSON.
    // Поэтому сохраняем "плоскую" структуру + поле GameType, чтобы знать,
    // какой именно класс создавать при загрузке.
    public class GameDto
    {
        public string GameType { get; set; } = string.Empty;    // "CardGame", "EuroGame", "PartyGame"
        public string Name { get; set; } = string.Empty;
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int AgeRestriction { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;

        // Поля уникальных типов — будут null/0 для неподходящих типов, это нормально.
        public int DeckSize { get; set; }       // для CardGame
        public string Complexity { get; set; } = string.Empty;  // для EuroGame
        public bool IsNoisy { get; set; }       // для PartyGame
    }
}