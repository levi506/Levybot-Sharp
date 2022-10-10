using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Discord.WebSocket;
using LevyBotSharp.Utility.Helpers;

namespace LevyBotSharp.Utility.Command
{

    public class Parameter
    {
        public enum ParamType
        {
            UserId,
            Int,
            Long,
            User,
            StrictUser,
            Guild,
            TextChannel,
            VoiceChannel,
            GuildUser,
            Role,
            String,
            Emote,
            Id
        }
        public static Regex IdRegex = new Regex("([0-9]{16,22})");
        public static Regex EmoteRegex = new Regex("<{1}a?:{1}.*:{1}[0-9]{16,22}>{1}");
        public static Regex UserPing = new Regex("<{1}@{1}!?[0-9]{16,22}>{1}");
        public static Regex TextChannel = new Regex("<{1}#{1}[0-9]{16,22}>{1}");


        private readonly string _value;
        private readonly DiscordSocketClient _client;

        public List<ParamType> PossibleTypes;

        public ulong GuildId { get; private set; }

        public Parameter(string value,DiscordSocketClient c,ulong guild = 0)
        {
            GuildId = guild;
            _value = value;
            _client = c;
            PossibleTypes = new List<ParamType>();
            //SoftCheck();
        }

        /**private void SoftCheck()
        {
            if(GuildId != 0)
            {
                if(this.AsUlong())
                if (this.AsSocketUser(GuildId).GetAwaiter().GetResult())
                {
                    PossibleTypes.Add(ParamType.GuildUser);
                }
            }


        }**/

        public int? AsInt()
        {
            if (int.TryParse(_value, out var i))
            {
                return i;
            }

            return null;
        }

        public string AsString()
        {
            return _value;
        }

        public ulong? AsUlong()
        {
            if (IdRegex.IsMatch(_value))
            {
                var idStr = IdRegex.Match(_value).Value;
                if (ulong.TryParse(idStr, out var j))
                {
                    return j;
                }
            }
            if (ulong.TryParse(_value, out var i))
            {
                return i;
            }
            return null;
        }

        public async Task<SocketUser> AsSocketUser(SocketGuild guild, bool strict = false)
        {
            var id = AsUlong();
            if (id.HasValue)
            {
                var client = _client;
                if (GuildId != 0)
                    return client.GetGuild(GuildId).GetUser(id.Value);
            }

            if (strict) return null;
            await guild.DownloadUsersAsync();
            var members = guild.Users.Where(x => x.DisplayName().ToLower().Contains(_value.ToLower()));
            var list = members.ToList().OrderbyName(_value);
            return list.FirstOrDefault();

        }

        public SocketUser AsGlobalSocketUser()
        {
            var id = AsUlong();
            if (id.HasValue)
            {
                var client = _client;
                return client.GetUser(id.Value);
            }

            return null;
        }

        public SocketGuild AsSocketGuild()
        {
            var id = AsUlong();
            if (id.HasValue)
            {
                var client = _client;
                return client.GetGuild(id.Value);
            }

            return null;
        }

        public SocketTextChannel AsTextSocketChannel()
        {
            var id = AsUlong();
            if (id.HasValue)
            {
                var client = _client;
                if (GuildId != 0)
                    return client.GetGuild(GuildId).GetTextChannel(id.Value);
            }

            return null;
        }
        public SocketVoiceChannel AsVoiceSocketChannel()
        {
            var id = AsUlong();
            if (id.HasValue)
            {
                var client = _client;
                if (GuildId != 0)
                    return client.GetGuild(GuildId).GetVoiceChannel(id.Value);
            }

            return null;
        }

        public SocketRole ASocketRole()
        {
            var id = AsUlong();
            var client = _client;
            var guild = client.GetGuild(GuildId);
            if (id.HasValue)
            {
                 return guild.GetRole(id.Value);
            }

            
            var roles = guild.Roles.ToList();
            foreach (var role in roles)
            {
                if (!role.Name.Contains(_value))
                {
                    roles.Remove(role);
                }
            }
            
            return roles.OrderbyName(_value).FirstOrDefault();
            
        }

        public override string ToString()
        {
            return AsString();
        }

        public DiscordSocketClient GetClient()
        {
            return _client;
        }

    }
}
