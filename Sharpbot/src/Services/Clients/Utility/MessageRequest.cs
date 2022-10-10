using Discord;
using Discord.WebSocket;
using Sharpbot.Services.Data.Utility;
using System;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace Sharpbot.Services.Clients.Utility
{


    public class MessageRequest
    {
        //Source Objects
        public IMessage DiscordMessage { get; private set; }
        public ChatMessage TwitchMessage { get; private set; }
        public WhisperMessage TwitchWhisper { get; private set; }

        //Bots
        public TwitchClient TwitchBot { get; private set; }
        public BaseSocketClient DiscordBot { get; private set; }

        //Message Data
        public Source MsgSource { get; private set; }
        public Location MessageMedium { get; private set; }
        public string Content { get; private set; }
        public string AuthorId { get; private set; }
        public string MessageId { get; private set; }
        public string ServerId { get; private set; }
        public string ChannelId { get; private set; }
        public string Username { get; private set; }
        public string Nickname { get; private set; }

        public MessageRequest(IMessage msg,BaseSocketClient bot)
        {
            MsgSource = Source.Discord;
            DiscordBot = bot;
            MessageMedium = msg.Channel is IGuildChannel ? Location.Guild : Location.DM;
            DiscordMessage = msg;
            Content = msg.Content;
            MessageId = msg.Id + "";
            ChannelId = msg.Channel.Id + "";
            
            if(MessageMedium == Location.Guild)
            {
                var gChannel = msg.Channel as IGuildChannel;
                ServerId = gChannel.GuildId + "";
                Nickname = gChannel.GetUserAsync(msg.Author.Id).GetAwaiter().GetResult().Nickname;


            }
            else
            {
                ServerId = "0";
            }
            if(msg is IUserMessage)
            {
                var uMsg = msg as IUserMessage;
                Username = msg.Author.Username;
                AuthorId = msg.Author.Id + "";
                if (Nickname == null || Nickname.Equals(""))
                {
                    Nickname = Username;
                }
            }
            
        }

        public MessageRequest(ChatMessage msg,TwitchClient bot)
        {
            MsgSource = Source.Twitch;
            MessageMedium = Location.Channel;
            TwitchBot = bot;
            TwitchMessage = msg;
            Content = msg.Message;
            Username = msg.Username;
            Nickname = msg.DisplayName;
            AuthorId = msg.UserId;
            MessageId = msg.Id;
            ServerId = msg.Channel;
            ChannelId = msg.RoomId;
            
        }

        public MessageRequest(WhisperMessage msg,TwitchClient bot)
        {
            MsgSource = Source.Twitch;
            MessageMedium = Location.Whisper;
            TwitchWhisper = msg;
            TwitchBot = bot;
            Content = msg.Message;
            Username = msg.Username;
            Nickname = msg.DisplayName;
            AuthorId = msg.UserId;
            MessageId = msg.MessageId;
            ServerId = "berrymocha";
        }


        public async Task RespondText(string msg)
        {
            if((MessageMedium & (Location.DM | Location.Guild)) != 0)
            {
                await DiscordMessage.Channel.SendMessageAsync(msg);
            } else if(MessageMedium == Location.Channel)
            {
                TwitchBot.SendMessage(TwitchMessage.Channel, msg);
            } else if(MessageMedium == Location.Whisper)
            {
                TwitchBot.SendWhisper(TwitchWhisper.Username, msg);
            }
        }

    }
}
