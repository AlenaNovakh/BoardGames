using Model.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml;

// Явно указываем, КАКОЙ именно Formatting и JsonException использовать.
// Это устраняет конфликт между Newtonsoft.Json и System.Xml / System.Text.Json.
using Formatting = Newtonsoft.Json.Formatting;
using JsonException = Newtonsoft.Json.JsonException;

namespace Model.Data
{
    // Этот класс отвечает ТОЛЬКО за сохранение и загрузку игр из файлов.
    // ПРИНЦИП ЕДИНОЙ ОТВЕТСТВЕННОСТИ (SRP): один класс — одна задача.
    // ПРИНЦИП ИНВЕРСИИ ЗАВИСИМОСТЕЙ (DIP): GameCatalog не знает про JSON,
    //   он работает с BoardGame-объектами. Сериализация — отдельный слой.
    public class GameSerializer
    {
        // Путь к файлу. Передаём снаружи — не хардкодим внутри класса.
        // Это делает класс гибким: можно передать любой путь.
        private readonly string _filePath;

        public GameSerializer(string filePath)
        {
            _filePath = filePath;
        }

        // Сохранение: BoardGame → GameDto → JSON-строка → файл.
        //public void Save(IEnumerable<BoardGame> games)
        //{
        //    // Конвертируем каждую игру в DTO.
        //    var dtos = new List<GameDto>();
        //    foreach (var game in games)
        //    {
        //        // ПРИВЕДЕНИЕ К БАЗОВОМУ ТИПУ / ИНТЕРФЕЙСУ:
        //        // Используем is-паттерн (pattern matching) — современный C# способ.
        //        // "game is CardGame cg" — это одновременно проверка типа И объявление переменной.
        //        var dto = new GameDto
        //        {
        //            Name = game.Name,
        //            MinPlayers = game.MinPlayers,
        //            MaxPlayers = game.MaxPlayers,
        //            AgeRestriction = game.AgeRestriction,
        //            Description = game.Description,
        //            ImagePath = game.ImagePath
        //        };

        //        if (game is CardGame cg)
        //        {
        //            dto.GameType = "CardGame";
        //            dto.DeckSize = cg.DeckSize;
        //        }
        //        else if (game is EuroGame eg)
        //        {
        //            dto.GameType = "EuroGame";
        //            dto.Complexity = eg.GameComplexity.ToString();
        //        }
        //        else if (game is PartyGame pg)
        //        {
        //            dto.GameType = "PartyGame";
        //            dto.IsNoisy = pg.IsNoisy;
        //        }

        //        dtos.Add(dto);
        //    }

        //    // JsonConvert.SerializeObject из Newtonsoft.Json.
        //    // Indented — форматирует JSON с отступами (читаемый файл).
        //    var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };

        //    // Создаём папку, если не существует, потом пишем файл.
        //    Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? ".");
        //    File.WriteAllText(_filePath, json);
        //}

        public void Save(IEnumerable<BoardGame> games)
        {
            var dtos = new List<GameDto>();
            foreach (var game in games)
            {
                var dto = new GameDto
                {
                    Name = game.Name,
                    MinPlayers = game.MinPlayers,
                    MaxPlayers = game.MaxPlayers,
                    AgeRestriction = game.AgeRestriction,
                    Description = game.Description,
                    ImagePath = game.ImagePath
                };

                if (game is CardGame cg)
                {
                    dto.GameType = "CardGame";
                    dto.DeckSize = cg.DeckSize;
                }
                else if (game is EuroGame eg)
                {
                    dto.GameType = "EuroGame";
                    dto.Complexity = eg.GameComplexity.ToString();
                }
                else if (game is PartyGame pg)
                {
                    dto.GameType = "PartyGame";
                    dto.IsNoisy = pg.IsNoisy;
                }

                dtos.Add(dto);
            }

            var settings = new JsonSerializerSettings { Formatting = Formatting.Indented };
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? ".");

            // ИСПРАВЛЕНО: сохраняем результат сериализации
            string jsonString = JsonConvert.SerializeObject(dtos, settings);
            File.WriteAllText(_filePath, jsonString);
        }

        // Загрузка: файл → JSON-строка → List<GameDto> → List<BoardGame>.
        // Возвращает пустой список если файл не существует.
        public List<BoardGame> Load()
        {
            if (!File.Exists(_filePath))
                return new List<BoardGame>();

            string json = File.ReadAllText(_filePath);

            // Десериализуем в список DTO. Может выбросить исключение при битом файле.
            List<GameDto> dtos;
            try
            {
                dtos = JsonConvert.DeserializeObject<List<GameDto>>(json)
                       ?? new List<GameDto>();
            }
            catch (JsonException ex)
            {
                // Пробрасываем с понятным сообщением, не глотаем исключение.
                throw new InvalidOperationException($"Ошибка чтения файла игр: {ex.Message}", ex);
            }

            // Конвертируем DTO → конкретные классы.
            var result = new List<BoardGame>();
            foreach (var dto in dtos)
            {
                BoardGame game = dto.GameType switch
                {
                    "CardGame" => new CardGame(dto.Name, dto.MinPlayers, dto.MaxPlayers,
                                               dto.AgeRestriction, dto.Description, dto.ImagePath,
                                               dto.DeckSize),
                    "EuroGame" => new EuroGame(dto.Name, dto.MinPlayers, dto.MaxPlayers,
                                               dto.AgeRestriction, dto.Description, dto.ImagePath,
                                               Enum.Parse<Complexity>(dto.Complexity ?? "Medium")),
                    "PartyGame" => new PartyGame(dto.Name, dto.MinPlayers, dto.MaxPlayers,
                                                 dto.AgeRestriction, dto.Description, dto.ImagePath,
                                                 dto.IsNoisy),
                    _ => throw new InvalidOperationException($"Неизвестный тип игры: {dto.GameType}")
                };
                result.Add(game);
            }

            return result;
        }
    }
}