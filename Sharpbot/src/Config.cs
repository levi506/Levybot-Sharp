using Newtonsoft.Json;
using Sharpbot.Services.ApiManager.Apis;
using Sharpbot.Services.Clients;
using Sharpbot.Services.Data.Utility;
using System.Collections.Generic;
using System.IO;

namespace Sharpbot
{
    public class Config
    {
        public SqlConfig Sql { private get; set; }
        public Owner Owner { get; set; }
        public List<Client> Clients { private get; set; }
        public Api ApiKeys { private get; set; }

        public static Config ReadJson(string filepath)
        {
            var s = File.ReadAllText(filepath);
            var Config = JsonConvert.DeserializeObject<Config>(s);
            DatabaseConn.ConnString = string.Format("server={0};user={1};database={2};port={3};password={4}", Config.Sql.Host, Config.Sql.User, Config.Sql.Database, Config.Sql.Port, Config.Sql.Password);
            Lavalink.GConfig = Config.ApiKeys.Lavalink;
            YoutubeApi.GConfig = Config.ApiKeys.Youtube;
            TwitchApi.GKey = Config.ApiKeys.Twitch;
            NSOnlineApi.GKey = Config.ApiKeys.NSO;
            ClientManager.Configs = Config.Clients;
            return Config;
        }
        
    }

    public struct SqlConfig
    {
        public string Host { get; set; }
        public string Port { get;  set; }
        public string Database { get;  set; }
        public string User { get;  set; }
        public string Password { get;  set; }
    }

    public struct Owner
    {
        public string Name { get; set; }

        public string Icon { get; set; }

        [JsonProperty("IDs")]
        public Ids Identifiers { get; set; }

        public string Contact { get; set; }
    }

    public struct Client
    {
        public ClientType Type { get; set; }
        public string Key { get; set; }
        public string ID { get; set; }
        public Auth Auth { get; set; }
        public string Twin { get; set; }

    }

    public struct Api
    {
        public LavalinkKey Lavalink { get; set; }
        public YoutubeKey Youtube { get; set;}
        public TwitchKey Twitch { get; set; }
        public NSOKey NSO { get; set; }
    }
    public struct LavalinkKey
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public string Password { get; set; }
    }
    public struct YoutubeKey
    {
        public string Key { get; set; }
    }
    public struct TwitchKey
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
    }
    public struct NSOKey
    {
        public string SessionToken { get; set; }
        public string CodeVerifier { get; set; }
    }
    public struct Ids
    {
        public string Twitch { get; set; }
        public string Discord { get; set; }
    }
    public struct Auth
    {
        public string Username { get; set; }
        public string OAuth { get; set; }
    }

    public enum ClientType
    {
        Discord,
        Twitch
    }
}
