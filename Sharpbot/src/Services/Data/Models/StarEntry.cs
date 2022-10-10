using Discord;
using Sharpbot.Services.Data.SQLStatements.Starboard;
using Sharpbot.Services.Data.Utility.Interfaces;
using System.Threading.Tasks;

namespace Sharpbot.Services.Data.Models
{
    public class StarEntry : IDatabaseObject
    {
        public string Id { get; private set; }
        public int Stars { get; private set; }
        public ulong User { get; private set; }
        public ulong Message { get; private set; }
        public ulong Guild { get; private set; }

        private StarEntry(IUserMessage message)
        {
            var channel = message.Channel as IGuildChannel;
            User = message.Author.Id;
            Message = message.Id;
            Guild = channel.GuildId;
        }

        public static StarEntry BuildStarboardEntry(IUserMessage message)
        {
            if (message.Channel is IGuildChannel && message.Type == MessageType.Default)
            {
                return new StarEntry(message);
            }

            return null;
        }


        public int IncrementStars()
        {
            Stars++;
            UpdateData();
            return Stars;
        }

        public int DecrementStars()
        {
            Stars--;
            UpdateData();
            return Stars;
        }

        public async Task UpdateData()
        {
            var statement = new StarEntryUpdate(this);
            await statement.Execute();
        }

        public async Task PostData()
        {
            var statement = new StarEntryPost(this);
            await statement.Execute();
        }
    }
}
