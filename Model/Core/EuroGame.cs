namespace Model.Core
{
    // enum — перечисление. Позволяет использовать понятные имена вместо чисел.
    // Хранится внутри пространства имён, доступно всем классам в Model.Core.
    public enum Complexity { Low, Medium, High }

    public class EuroGame : BoardGame
    {
        // Уникальное поле — сложность правил, характерная для евро-игр.
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
            // Превращаем enum в читаемую строку через switch.
            string complexityStr = GameComplexity switch
            {
                Complexity.Low => "Лёгкая",
                Complexity.Medium => "Средняя",
                Complexity.High => "Сложная",
                _ => "Неизвестно"  // _ — это "по умолчанию" в switch-выражении
            };
            return base.GetShortInfo() + $" | Сложность: {complexityStr}";
        }
    }
}