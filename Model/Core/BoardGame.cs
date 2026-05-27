// Пространство имён — важно, чтобы все файлы в Model/Core использовали одно и то же
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

        // abstract метод — у него НЕТ тела здесь.
        // Каждый наследник ОБЯЗАН его реализовать через override.
        // Это ПОЛИМОРФИЗМ: один вызов game.GetGameType() вернёт разный результат
        // в зависимости от реального типа объекта.
        public abstract string GetGameType();

        // virtual метод — имеет реализацию по умолчанию, но наследник МОЖЕТ переопределить.
        // Это тоже ПОЛИМОРФИЗМ.
        public virtual string GetShortInfo()
        {
            return $"{Name} | {GetGameType()} | {MinPlayers}-{MaxPlayers} игроков | {AgeRestriction}+";
        }

        // ПЕРЕГРУЗКА МЕТОДА (overloading) — два метода с одним именем, но разными параметрами.
        // C# различает их по сигнатуре (типам параметров).
        public virtual string GetShortInfo(bool includeDescription)
        {
            string base_ = GetShortInfo(); // вызываем версию без параметров
            return includeDescription ? base_ + "\n" + Description : base_;
        }

        // ПЕРЕГРУЗКА ОПЕРАТОРОВ — позволяет писать game1 < game2.
        // Нужно для оценки 5, но добавляем сразу, лишним не будет.
        // Оператор < сравнивает по имени в алфавитном порядке.
        // string.Compare возвращает отрицательное число, если first < second.
        public static bool operator <(BoardGame left, BoardGame right)
        {
            return string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) < 0;
        }

        public static bool operator >(BoardGame left, BoardGame right)
        {
            return string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase) > 0;
        }

        // ToString() — стандартный метод всех объектов C#, переопределяем для удобства.
        // Теперь при Console.WriteLine(game) или в ComboBox без форматирования
        // будет выводиться имя игры.
        public override string ToString() => Name;
    }
}