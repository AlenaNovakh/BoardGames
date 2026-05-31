namespace Model.Core
{
    // abstract означает: нельзя создать объект BoardGame напрямую (new BoardGame() — ошибка).
    // Только через наследников: CardGame, EuroGame, PartyGame.
    // Это и есть АБСТРАКЦИЯ — мы описываем "игру вообще", не конкретный тип.
    public abstract class BoardGame
    {
        // Свойства с { get; set; } — это автосвойства C#.
        // Они автоматически создают скрытое поле и геттер/сеттер.
        // ИНКАПСУЛЯЦИЯ: данные хранятся внутри объекта, доступ — только через свойства.
        public string Name { get; set; } = string.Empty;
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
        public int AgeRestriction { get; set; }   // минимальный возраст
        public string Description { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;     // путь к картинке обложки

        // Конструктор — вызывается при создании объекта (new CardGame(...)).
        // "protected" — доступен только внутри класса и его наследников, но не снаружи.
        // Это защищает от случайного создания "голого" BoardGame снаружи.
        protected BoardGame(string name, int minPlayers, int maxPlayers,
                            int ageRestriction, string description, string imagePath)
        {
            Name = name;
            MinPlayers = minPlayers;
            MaxPlayers = maxPlayers;
            AgeRestriction = ageRestriction;
            Description = description;
            ImagePath = imagePath;
        }
        public abstract string GetGameType();
        public virtual string GetShortInfo()
        {
            return $"{Name} | {GetGameType()} | {MinPlayers}-{MaxPlayers} игроков | {AgeRestriction}+";
        }
        public virtual string GetShortInfo(bool includeDescription)
        {
            string base_ = GetShortInfo();
            return includeDescription ? base_ + "\n" + Description : base_;
        }
        public static bool operator <(BoardGame left, BoardGame right)
        {
            return string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) < 0;
        }
        public static bool operator >(BoardGame left, BoardGame right)
        {
            return string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) > 0;
        }
        public override string ToString() => Name;
    }
}