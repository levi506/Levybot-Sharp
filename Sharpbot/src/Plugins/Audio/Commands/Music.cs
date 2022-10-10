using Discord;
using Discord.WebSocket;
using Sharpbot.Plugins.Commands.Utility;
using Sharpbot.Plugins.Commands.Utility.Extensions;
using Sharpbot.Services.ApiManager.Apis;
using Sharpbot.Services.Data.Utility;
using Sharpbot.Services.Data.Utility.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Victoria;

namespace Sharpbot.Plugins.Audio.Commands
{
    public class Music : CommandGroup
    {
        private AudioPlugin AudioSys { get; set; }
        
        public Music(AudioPlugin audio)
        {
            AudioSys = audio;
        }

        public override string Name { get; set; } = "Music";

        [CommandName("Connect")]
        [CommandName("Join")]
        [CommandMeta("", "Music")]
        [Usage(Location.Guild)]
        public class Connect : Command
        {
            public override Task Default(CommandRequest req)
            {
                req.SourceRequest.RespondText("You need to be in a voice channel in order to connect the bot.");
                return Task.CompletedTask;
            }

            public override async Task Excute(CommandRequest req)
            {
                var guildId = ulong.Parse(req.SourceRequest.ServerId);
                var userId = ulong.Parse(req.SourceRequest.AuthorId);
                var guild = req.SourceRequest.DiscordBot.GetGuild(guildId);
                var member = guild.GetUser(userId);

                if (member.VoiceChannel == null)
                {
                    Default(req);
                } if (guild.CurrentUser.VoiceChannel != null && guild.CurrentUser.VoiceChannel.Users.Count > 1) {
                    RejectAlreadyConnected(req);
                }
                else
                {
                    await OpenConnection(req, member);
                }
            }

            private async Task OpenConnection(CommandRequest req, SocketGuildUser member)
            {
                var group = Parent as Music;
                await group.AudioSys.RegisterConnection(req, member.VoiceChannel, req.SourceRequest.DiscordMessage.Channel as ITextChannel);
                req.SourceRequest.RespondText("Connected to 🔈`" + member.VoiceChannel.Name + "` and bound text feed to 📝`" + req.SourceRequest.DiscordMessage.Channel.Name + "` ");
            }

            private async Task RejectAlreadyConnected(CommandRequest req)
            {
                await req.SourceRequest.RespondText("Sorry I can't connect there right now.");
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }

        [CommandName("Disconnect")]
        [CommandName("Leave")]
        [CommandName("dc")]
        [CommandMeta("", "Music")]
        [Usage(Location.Guild)]
        public class Disconnect : Command
        {
            public override Task Default(CommandRequest req)
            {
                req.SourceRequest.RespondText("The bot needs to be connected to disconnect");
                return Task.CompletedTask;
            }

            public override async Task Excute(CommandRequest req)
            {
                var guildId = ulong.Parse(req.SourceRequest.ServerId);
                var userId = ulong.Parse(req.SourceRequest.AuthorId);
                var guild = req.SourceRequest.DiscordBot.GetGuild(guildId);
                var member = guild.GetUser(userId);

                if (guild.CurrentUser.VoiceChannel != null)
                {
                    if (guild.CurrentUser.VoiceChannel.Users.Count <= 1 || member.VoiceChannel.Id == guild.CurrentUser.VoiceChannel.Id)
                        await CloseConnection(req, guild.CurrentUser);
                    else
                        Default(req);
                }
                else
                {
                    Default(req);
                }
            }

            private async Task CloseConnection(CommandRequest req, IGuildUser member)
            {
                var group = Parent as Music;
                await group.AudioSys.DisconnectPlayer(req, member.VoiceChannel);
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }


        [CommandName("Play")]
        [CommandName("p")]
        [CommandName("Youtube")]
        [CommandName("yt")]
        [CommandName("Soundcloud")]
        [CommandName("sc")]
        [CommandMeta("", "Music")]
        [Usage(Location.Guild)]
        public class Play : Command
        {
            public override Task Default(CommandRequest req)
            {
                return Task.CompletedTask;
            }

            public override async Task Excute(CommandRequest req)
            {
                if (req.Params.Count <= 0)
                {
                    await Default(req);
                }
                else
                {
                    await PlayAudio(req);
                }
            }

            private async Task PlayAudio(CommandRequest req)
            {
                var group = Parent as Music;
                var AudioSys = group.AudioSys;
                var guildId = ulong.Parse(req.SourceRequest.ServerId);
                var userId = ulong.Parse(req.SourceRequest.AuthorId);
                var guild = req.SourceRequest.DiscordBot.GetGuild(guildId);
                var member = guild.GetUser(userId);
                if (member.VoiceChannel != null)
                {
                    if (!await AudioSys.DoesPlayerExist(req, guild))
                    {
                        await CheckForConnection(req, guild, member);
                    }
                    LavaTrack track;
                    if (req.CommandHead.ToLower().Equals("youtube") || req.CommandHead.ToLower().Equals("yt"))
                    {
                        track = await AudioSys.QueryYoutube(req, req.Params.Merge());
                    }
                    else if (req.CommandHead.ToLower().Equals("soundcloud") || req.CommandHead.ToLower().Equals("sc"))
                    {
                        track = await AudioSys.QuerySoundcloud(req, req.Params.Merge());
                    }
                    else
                    {
                        track = await AudioSys.QueryAny(req, req.Params.Merge());
                    }
                    if (track != null)
                        AudioSys.QueueAudio(req, track, guild);
                }
            }

            private async Task CheckForConnection(CommandRequest req, SocketGuild guild, SocketGuildUser member)
            {
                if (member.VoiceChannel == null)
                {
                    Default(req);
                }
                if (guild.CurrentUser.VoiceChannel != null && guild.CurrentUser.VoiceChannel.Users.Count > 1)
                {
                    await RejectAlreadyConnected(req);
                }
                else
                {
                    await OpenConnection(req, member);
                }
            }

            private async Task OpenConnection(CommandRequest req, SocketGuildUser member)
            {
                var group = Parent as Music;
                await group.AudioSys.RegisterConnection(req, member.VoiceChannel, req.SourceRequest.DiscordMessage.Channel as ITextChannel);
            }

            private async Task RejectAlreadyConnected(CommandRequest req)
            {
                await req.SourceRequest.RespondText("Sorry I can't connect there right now.");
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }

        [CommandName("Volume")]
        [CommandName("vol")]
        [CommandName("v")]
        [CommandMeta("Sets the volume of the bot", "Music")]
        [Usage(Location.Guild)]
        public class Volume : Command
        {
            public override Task Default(CommandRequest req)
            {
                return Task.CompletedTask;
            }

            public override async Task Excute(CommandRequest req)
            {
                if (IsSameVoice(req))
                {
                    if (req.Params.Count > 0 && req.Params.First().ParamTypes.TryGetValue(ParamType.Int, out var val) && val)
                    {
                        await SetVol(req);
                    }
                    else
                    {
                        await GetVol(req);
                    }
                } else
                {
                    Default(req);
                }
            }

            private async Task GetVol(CommandRequest req)
            {
                var group = Parent as Music;
                var AudioSys = group.AudioSys;
                var vol = await AudioSys.GetPlayerVolume(req, GetGuild(req));
                req.SourceRequest.RespondText("Volume is set to " + vol + "%");
            }

            private async Task SetVol(CommandRequest req)
            {
                var group = Parent as Music;
                var AudioSys = group.AudioSys;
                var vol = ushort.Parse(req.Params.First().RawData);
                await AudioSys.SetPlayerVolume(req, GetGuild(req), vol);
                req.SourceRequest.RespondText("Volume was set to " + vol + "%");
            }

            private SocketGuild GetGuild(CommandRequest req)
            {
                var guildId = ulong.Parse(req.SourceRequest.ServerId);

                var guild = req.SourceRequest.DiscordBot.GetGuild(guildId);
                return guild;
            }

            private bool IsSameVoice(CommandRequest req)
            {
                var userId = ulong.Parse(req.SourceRequest.AuthorId);
                var guild = GetGuild(req);
                var member = guild.GetUser(userId);
                return member.VoiceChannel?.Id == guild.CurrentUser.VoiceChannel.Id;
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }

        [CommandName("Skip")]
        [CommandMeta("Skips currently playing audio","Music")]
        [Usage(Location.Guild)]
        public class Skip : Command
        {
            public override Task Default(CommandRequest req)
            {
                return Task.CompletedTask;
            }

            public override async Task Excute(CommandRequest req)
            {
                if (IsSameVoice(req))
                {
                    await AttemptSkip(req);
                } 
                else
                {
                    Default(req);
                }
            }

            private async Task AttemptSkip(CommandRequest req)
            {
                var group = Parent as Music;
                var AudioSys = group.AudioSys;
                var guild = GetGuild(req);
                await AudioSys.SkipTrack(req, guild);
            }

            private bool IsSameVoice(CommandRequest req)
            {
                var userId = ulong.Parse(req.SourceRequest.AuthorId);
                var guild = GetGuild(req);
                var member = guild.GetUser(userId);
                return member.VoiceChannel?.Id == guild.CurrentUser.VoiceChannel.Id;

            }

            private SocketGuild GetGuild(CommandRequest req)
            {
                var guildId = ulong.Parse(req.SourceRequest.ServerId);

                var guild = req.SourceRequest.DiscordBot.GetGuild(guildId);
                return guild;
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }


        [CommandName("Loop")]
        [CommandName("ls")]
        [CommandName("lp")]
        [CommandMeta("","Music")]
        [Usage(Location.Guild)]
        public class Loop : Command
        {
            public override Task Default(CommandRequest req)
            {
                return Task.CompletedTask;
            }

            public override async Task Excute(CommandRequest req)
            {
                if (IsSameVoice(req))
                {
                    await Functional(req);
                } else
                {
                    Default(req);
                }
            }

            public async Task Functional(CommandRequest req)
            {
                var group = Parent as Music;
                var AudioSys = group.AudioSys;
                var guild = GetGuild(req);

                var com = req.CommandHead.ToLower();
                var CurrentState = await AudioSys.GetLoopState(req,guild);
                LoopingState? toSet = null;
                if (com.Equals("ls"))
                {
                    if(CurrentState != LoopingState.Song)
                        toSet = LoopingState.Song;
                }
                else if (com.Equals("lp"))
                {
                    if (CurrentState != LoopingState.Playlist)
                        toSet = LoopingState.Playlist;
                }
                else
                {
                    switch (CurrentState)
                    {
                        case LoopingState.None:
                            toSet = LoopingState.Playlist;
                            break;
                        case LoopingState.Song:
                            toSet = LoopingState.None;
                            break;
                        case LoopingState.Playlist:
                            toSet = LoopingState.Song;
                            break;
                    }
                }
                if(!toSet.HasValue || toSet == null)
                {
                    toSet = LoopingState.None;
                }
                await AudioSys.SetLoopState(req, guild, toSet.Value);

                switch (toSet)
                {
                    case LoopingState.None:
                        req.SourceRequest.RespondText("The Queue will no longer loop.");
                        break;
                    case LoopingState.Song:
                        req.SourceRequest.RespondText("The Queue will loop on this song. 🔂");
                        break;
                    case LoopingState.Playlist:
                        req.SourceRequest.RespondText("The Queue will loop on the whole playlist. 🔁");
                        break;
                }
            }

            private bool IsSameVoice(CommandRequest req)
            {
                var userId = ulong.Parse(req.SourceRequest.AuthorId);
                var guild = GetGuild(req);
                var member = guild.GetUser(userId);
                return member.VoiceChannel?.Id == guild.CurrentUser.VoiceChannel.Id;
            }

            private SocketGuild GetGuild(CommandRequest req)
            {
                var guildId = ulong.Parse(req.SourceRequest.ServerId);

                var guild = req.SourceRequest.DiscordBot.GetGuild(guildId);
                return guild;
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }

        [CommandName("Shuffle")]
        [CommandMeta("Shuffles the Music Queue","Music")]
        [Usage(Location.Guild)]
        public class Shuffle : Command
        {
            public override Task Default(CommandRequest req)
            {
                return Task.CompletedTask;
            }

            public override Task Excute(CommandRequest req)
            {
                if (IsSameVoice(req))
                {
                    return Functional(req);
                }
                else
                {
                    return Default(req);
                }
            }

            private async Task Functional(CommandRequest req)
            {
                var group = Parent as Music;
                var AudioSys = group.AudioSys;
                var guild = GetGuild(req);

                await AudioSys.ShuffleQueue(req, guild);
                req.SourceRequest.RespondText("The Queue has been shuffled! 🔀");
            }

            private bool IsSameVoice(CommandRequest req)
            {
                var userId = ulong.Parse(req.SourceRequest.AuthorId);
                var guild = GetGuild(req);
                var member = guild.GetUser(userId);
                return member.VoiceChannel?.Id == guild.CurrentUser.VoiceChannel.Id;
            }

            private SocketGuild GetGuild(CommandRequest req)
            {
                var guildId = ulong.Parse(req.SourceRequest.ServerId);

                var guild = req.SourceRequest.DiscordBot.GetGuild(guildId);
                return guild;
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }

        [CommandName("Queue")]
        [CommandName("q")]
        [CommandMeta("Shows the Music Queue", "Music")]
        [Usage(Location.Guild)]
        public class Queue : Command
        {
            public override Task Default(CommandRequest req)
            {
                return Task.CompletedTask;

            }

            public Task Connected(CommandRequest req)
            {
                req.SourceRequest.RespondText("There needs to be audio playing to check the queue!");
                return Task.CompletedTask;
            }

            public override async Task Excute(CommandRequest req)
            {
                if (IsConnected(req))
                {
                    if (await IsPlaying(req))
                    {
                        Functional(req);
                    }
                    else
                    {
                        Default(req);
                    }
                }
                else
                {
                    Default(req);
                }
            }

            private async Task Functional(CommandRequest req)
            {
                var group = Parent as Music;
                var AudioSys = group.AudioSys;
                var guild = GetGuild(req);
                var playing = await AudioSys.GetPlaying(req, guild);
                var queue = await AudioSys.GetQueue(req, guild);
            }

            private bool IsConnected(CommandRequest req)
            {
                
                var guild = GetGuild(req);
                var member = guild.CurrentUser;

                return member.VoiceChannel != null;
            }

            private async Task<bool> IsPlaying(CommandRequest req)
            {
                var group = Parent as Music;
                var AudioSys = group.AudioSys;
                var guild = GetGuild(req);
                return await AudioSys.IsPlaying(req, guild);
            }

            private SocketGuild GetGuild(CommandRequest req)
            {
                var guildId = ulong.Parse(req.SourceRequest.ServerId);

                var guild = req.SourceRequest.DiscordBot.GetGuild(guildId);
                return guild;
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }

        [CommandName("Song")]
        [CommandName("Track")]
        [CommandName("ss")]
        [CommandMeta("Shows Current Audio Playing", "Music")]
        [Usage(Location.Guild)]
        public class SongInfo : Command
        {
            public override Task Default(CommandRequest req)
            {
                return Task.CompletedTask;
            }

            public Task Connected(CommandRequest req)
            {
                req.SourceRequest.RespondText("There needs to be a song playing for info to be displayed!");
                return Task.CompletedTask;
            }

            public override async Task Excute(CommandRequest req)
            {
                if (IsConnected(req))
                {
                    if (await IsPlaying(req))
                    {
                        Functional(req);
                    }
                    else
                    {
                        Connected(req);
                    }
                }
                else
                {
                    Default(req);
                }
            }

            private async Task Functional(CommandRequest req)
            {
                var group = Parent as Music;
                var AudioSys = group.AudioSys;
                var guild = GetGuild(req);
                var track = await AudioSys.GetPlaying(req, guild);
                var infoLine = await AudioSys.MakeInfoLine(req, guild);
                var iconUri = await track.FetchArtworkAsync();
                var embedBuilder = new EmbedBuilder();
                embedBuilder.WithThumbnailUrl(iconUri);
                embedBuilder.WithTitle(track.Title);
                embedBuilder.WithUrl(track.Url);
                embedBuilder.WithDescription(infoLine);
                embedBuilder.AddField("Author", track.Author, true);
                embedBuilder.AddField("Length", track.Position.NumericString() + "/" + track.Duration.NumericString(),true);
                embedBuilder.WithCurrentTimestamp();

                var em = embedBuilder.Build();
                await req.SourceRequest.DiscordMessage.Channel.SendMessageAsync(embeds:new Embed[] { em });


            }

            

            private bool IsConnected(CommandRequest req)
            {

                var guild = GetGuild(req);
                var member = guild.CurrentUser;

                return member.VoiceChannel != null;
            }

            private async Task<bool> IsPlaying(CommandRequest req)
            {
                var group = Parent as Music;
                var AudioSys = group.AudioSys;
                var guild = GetGuild(req);
                return await AudioSys.IsPlaying(req, guild);
            }

            private SocketGuild GetGuild(CommandRequest req)
            {
                var guildId = ulong.Parse(req.SourceRequest.ServerId);

                var guild = req.SourceRequest.DiscordBot.GetGuild(guildId);
                return guild;
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }
    }
}
