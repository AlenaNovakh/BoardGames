namespace Model.Core
{
    public enum Complexity { Low, Medium, High }

    public class EuroGame : BoardGame
    {
        public Complexity GameComplexity { get; set; }

        public EuroGame(string name, int minPlayers, int maxPlayers,
                        int ageRestriction, string description, string imagePath,
                        Complexity complexity)
            : base(name, minPlayers, maxPlayers, ageRestriction, description, imagePath)
        {
            GameComplexity = complexity;
        }

        public override string GetGameType() => "Евро";

        public override string GetShortInfo()
        {
            string complexityStr = GameComplexity switch
            {
                Complexity.Low => "Лёгкая",
                Complexity.Medium => "Средняя",
                Complexity.High => "Сложная",
                _ => "Неизвестно"
            };
            return base.GetShortInfo() + $" | Сложность: {complexityStr}";
        }
    }
}