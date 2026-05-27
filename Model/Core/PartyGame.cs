namespace Model.Core
{
    public class PartyGame : BoardGame
    {
        // Уникальное поле — шумная ли игра (важно для домашних вечеринок).
        public bool IsNoisy { get; set; }

        public PartyGame(string name, int minPlayers, int maxPlayers,
                         int ageRestriction, string description, string imagePath, bool isNoisy)
            : base(name, minPlayers, maxPlayers, ageRestriction, description, imagePath)
        {
            IsNoisy = isNoisy;
        }

        public override string GetGameType() => "Пати";

        public override string GetShortInfo()
        {
            return base.GetShortInfo() + $" | Шумная: {(IsNoisy ? "да" : "нет")}";
        }
    }
}