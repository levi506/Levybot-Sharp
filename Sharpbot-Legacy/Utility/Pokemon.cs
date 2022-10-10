using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace LevyBotSharp.Utility
{
    public class Pokedex
    {
        [JsonProperty] public List<PokemonData> Pokemon { get; private set; }

        public static Pokedex FromJson(string filepath)
        {
            var s = File.ReadAllText(filepath);
            return JsonConvert.DeserializeObject<Pokedex>(s);
        }

        public PokemonData GetPokeById(int speciesId)
        {
            return Pokemon.FirstOrDefault(x => x.id == speciesId);
        }

    }

    public class PokemonData
    {
        [JsonProperty] public int id { get; private set; }

        [JsonProperty] public string Species { get; private set; }

        [JsonProperty] public List<string> Typing { get; private set; }

        [JsonProperty] public Stats BaseStats { get; private set; }

        [JsonProperty] public double GenderRatio { get; private set; }

        [JsonProperty] public string GrowthRate { get; private set; }

        [JsonProperty] public int BaseExp { get; private set; }

        [JsonProperty] public Stats EvReward { get; private set; }

        [JsonProperty] public int Catchrate { get; private set; }

        [JsonProperty] public int BaseHappiness { get; private set; }

        [JsonProperty] public List<string> Abilities { get; private set; }

        [JsonProperty] public List<string> HiddenAbility { get; private set; }

        [JsonProperty] public Moves Moves { get; private set; }

        [JsonProperty] public List<string> EggGroups { get; private set; }

        [JsonProperty] public int HatchSteps { get; private set; }

        [JsonProperty] public double Height { get; set; }

        [JsonProperty] public double Weight { get; set; }

        [JsonProperty] public string Color { get; set; }

        [JsonProperty] public string Habitat { get; set; }

        [JsonProperty] public string Kind { get; set; }

        [JsonProperty] public string Pokedex { get; set; }
    }

    class LvMove
    {
        [JsonProperty] public string Name { get; private set; }

        [JsonProperty] public int Level { get; private set; }
    }

    class EvoMethod
    {
        [JsonProperty] public string Species { get; private set; }

        [JsonProperty] public string Method { get; private set; }

        [JsonProperty] public string Data { get; private set; }
    }

    public struct Stats
    {
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpecialAttack { get; set; }
        public int SpecialDefense { get; set; }
        public int Speed { get; set; }
    }
    public struct Moves
    {
        public List<LvlMove> LevelMoves { get; set; }
        public List<string> EggMoves { get; set; }
        public List<int> MachineMoves { get; set; }
        public List<string> TutorMoves { get; set; }
    }

    public struct LvlMove
    {
        public string Name { get; set; }
        public int Level { get; set; }
    }

}
