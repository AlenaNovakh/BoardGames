using System.Collections.Generic;

namespace Model.Core
{
    public static class GameFactory
    {
        public static List<BoardGame> CreateGames()
        {
            return new List<BoardGame>
            {
                // CardGame(name, minP, maxP, age, desc, imagePath, deckSize)
                new CardGame("Dominion", 2, 4, 13,
                    "Классическая колодостроительная игра. Собирайте провинции быстрее соперников.",
                    "images/dominion.jpg", 500),
                
                new CardGame("Magic: The Gathering", 2, 2, 13,
                    "Коллекционная карточная игра — дуэли магов с уникальными картами.",
                    "images/mtg.jpg", 60),

                new CardGame("Uno", 2, 10, 7,
                    "Простая и весёлая карточная игра. Сбросьте все карты первым!",
                    "images/uno.jpg", 108),

                new CardGame("Pandemic", 2, 4, 8,
                    "Кооперативная игра: команда врачей останавливает эпидемии по всему миру.",
                    "images/pandemic.jpg", 96),

                // EuroGame(name, minP, maxP, age, desc, imagePath, complexity)
                new EuroGame("Catan", 3, 4, 10,
                    "Строите поселения и города на острове Катан, торгуете ресурсами.",
                    "images/catan.jpg", Complexity.Medium),

                new EuroGame("Agricola", 1, 5, 12,
                    "Управляете фермой: сеете зерно, разводите скот, расширяете дом.",
                    "images/agricola.jpg", Complexity.High),

                new EuroGame("Ticket to Ride", 2, 5, 8,
                    "Прокладывайте маршруты поездов по карте Европы или Америки.",
                    "images/ticket.jpg", Complexity.Low),

                new EuroGame("7 Wonders", 2, 7, 10,
                    "Постройте одно из семи чудес света за три эпохи цивилизации.",
                    "images/7wonders.jpg", Complexity.Medium),

                new EuroGame("Puerto Rico", 2, 5, 12,
                    "Управляете колонией: выращивайте товары и отправляйте корабли.",
                    "images/puertorico.jpg", Complexity.High),
                
                new EuroGame("Wingspan", 1, 5, 10,
                    "Привлекайте птиц в свой заповедник. Красивая и спокойная евро-игра.",
                    "images/wingspan.jpg", Complexity.Medium),

                // PartyGame(name, minP, maxP, age, desc, imagePath, isNoisy)
                new PartyGame("Крокодил", 4, 20, 6,
                    "Показывайте слова жестами — команда должна угадать.",
                    "images/crocodile.jpg", true),

                new PartyGame("Диксит", 3, 6, 8,
                    "Рассказывайте истории по красивым иллюстрациям. Угадайте карту рассказчика.",
                    "images/dixit.jpg", false),

                new PartyGame("Имаджинариум", 3, 7, 8,
                    "Русская версия Диксита с иллюстрациями российских художников.",
                    "images/imaginarium.jpg", false),

                new PartyGame("Alias", 4, 12, 10,
                    "Объясняйте слова без использования однокоренных — на скорость.",
                    "images/alias.jpg", true),

                new PartyGame("Мафия", 6, 16, 12,
                    "Классическая психологическая игра. Мирные жители против мафии.",
                    "images/mafia.jpg", true),
            };
        }
    }
}