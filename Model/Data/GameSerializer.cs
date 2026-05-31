using Model.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml;

using Formatting = Newtonsoft.Json.Formatting;
using JsonException = Newtonsoft.Json.JsonException;

namespace Model.Data
{
    public class GameSerializer
    {
        private readonly string _filePath;

        public GameSerializer(string filePath)
        {
            _filePath = filePath;
        }

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

            string json = JsonConvert.SerializeObject(dtos, Formatting.Indented);
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
            File.WriteAllText(_filePath, json);
        }

        public List<BoardGame> Load()
        {
            if (!File.Exists(_filePath))
                return new List<BoardGame>();

            string json = File.ReadAllText(_filePath);

            List<GameDto> dtos;
            try
            {
                dtos = JsonConvert.DeserializeObject<List<GameDto>>(json)
                       ?? new List<GameDto>();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Ошибка чтения файла игр: {ex.Message}", ex);
            }

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