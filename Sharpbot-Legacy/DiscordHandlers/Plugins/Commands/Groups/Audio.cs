using LevyBotSharp.DiscordHandlers.Plugins.Audio;
using LevyBotSharp.Utility.Attributes;
using LevyBotSharp.Utility.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    public class Audio : CommandGroupBase {


        public override string Name { get; } = "Audio";

        [CommandMeta(new[] { "Connect", "Join" })]
        [GuildContext]
        public async Task ConnectCom(UserRequest req)
        {
            if (req.GetMember().VoiceChannel != null)
            {
                if (req.Guild.CurrentUser.VoiceChannel == null)
                    await AudioHandler.RegisterConnection(req);
                else
                    await req.Channel.SendMessageAsync("Sorry I am already connected elsewhere!");
            }

        }

        [CommandMeta(new[] { "Youtube", "yt" })]
        [GuildContext]
        public async Task YtCom(UserRequest req)
        {
            if (req.GetMember().VoiceChannel != null)
            {
                var query = req.Args.Merge().AsString();
                await AudioHandler.QueueYtAudio(req, query);
            }

        }

        [CommandMeta(new[] { "Soundcloud", "sc" })]
        [GuildContext]
        public async Task ScCom(UserRequest req)
        {
            if (req.GetMember().VoiceChannel != null)
            {
                var query = req.Args.Merge().AsString();
                await AudioHandler.QueueScAudio(req, query);
            }

        }

        [CommandMeta(new[] { "Play", "p" }, "Finds and plays a thing with the thing", "Audio")]
        [GuildContext]
        public async Task PlayCom(UserRequest req)
        {
            if (req.GetMember().VoiceChannel != null)
            {
                var query = req.Args.Merge().AsString();
                await AudioHandler.QueueRanAudio(req, query);
            }
        }

        [CommandMeta(new[] { "Playing", "Song" }, "Shows Info For the Currently Playing Song", "Audio")]
        [GuildContext]
        public async Task PlayingCom(UserRequest req)
        {
            if (req.Guild.CurrentUser.VoiceChannel !=null)
            {
                var playing = AudioHandler.GetPlaying(req);
                var embed = QueueHandler.MakeTrackEmbed(playing, req.GetBotGuild());
                await req.Channel.SendMessageAsync(embed: embed);
            }
        }

        [CommandMeta(new[] { "Volume", "v" })]
        [GuildContext]
        public async Task VolCom(UserRequest req)
        {
            var vol = req.Args.First().AsInt();
            if (vol.HasValue)
                await AudioHandler.SetVolume(req, vol.Value);

            var volume = AudioHandler.GetVolume(req);
            if (volume != null)
                await req.SendSimpleEmbedAsync($"The Volume is currently set to {volume}%", "Volume : 🔉");
            else
                await req.Channel.SendMessageAsync("I am not in any channels so there is no volume :(");
        }

        [CommandMeta(new[] { "Skip", "s" }, "Skips Current Playing Track", "Audio")]
        [GuildContext]
        public async Task SkipCom(UserRequest req)
        {
            if (req.GetMember().VoiceChannel != null && req.Guild.CurrentUser.VoiceChannel != null)
            {
                await AudioHandler.SkipCurrent(req);
               
            }
        }

        [CommandMeta(new[] { "Shuffle", "sq" }, "Skips Current Playing Track", "Audio")]
        [GuildContext]
        public async Task ShuffleCom(UserRequest req)
        {
            if (req.GetMember().VoiceChannel != null && req.Guild.CurrentUser.VoiceChannel != null)
            {
                AudioHandler.ShuffleQueue(req);
                await req.SendSimpleEmbedAsync("Queue Shuffle", "The Queue has been randomized");
            }
        }

        [CommandMeta(new[] { "Pause", "pp" }, "Pauses the music playing", "Audio")]
        [GuildContext]
        public async Task PauseCom(UserRequest req)
        {
            if (req.GetMember().VoiceChannel != null && req.Guild.CurrentUser.VoiceChannel != null)
            {
                await AudioHandler.PausePlayer(req);
                await req.SendSimpleEmbedAsync("Paused the Playback.", "Pause State : ⏸");
            }

        }

        [CommandMeta(new[] { "Resume", "rr" }, "Resumes paused music", "Audio")]
        [GuildContext]
        public async Task ResumeCom(UserRequest req)
        {
            if (req.GetMember().VoiceChannel != null && req.Guild.CurrentUser.VoiceChannel != null)
            {
                await AudioHandler.ResumePlayer(req);
                await req.SendSimpleEmbedAsync("Resuming Playing!", "Pause State : ▶");
            }

        }

        [CommandMeta(new[] { "Disconnect", "Leave" })]
        [GuildContext]
        public async Task DisconnectCom(UserRequest req)
        {
            if (req.GetMember().VoiceChannel != null)
            {
                await AudioHandler.RegisterConnection(req);
                await AudioHandler.Disconnect(req);
                await req.SendSimpleEmbedAsync("Disconnected from the channel!", "Audio");
            }
            

        }

        [CommandMeta(new[] { "Loop", "lp", "ls" })]
        [GuildContext]
        public async Task LoopSong(UserRequest req)
        {
            if (req.GetMember().VoiceChannel != null)
            {
                if (req.GetMember().VoiceChannel.Users.Count >= 2)
                {
                    if (req.Command == "loop")
                    {
                        var tooltip = $"`{req.Settings.SettCache.PriPrefix}lp` to loop the queue or `{req.Settings.SettCache.PriPrefix}ls` to loop the song quickly!";
                        var current = AudioHandler.GetLoopState(req);
                        if (current == LoopingState.None)
                        {
                            AudioHandler.SetLoopState(req, LoopingState.Playlist);
                            await req.SendSimpleEmbedAsync($"The Queue is now set to loop!\n{tooltip}", "Audio Looping : 🔁");
                        }
                        else if (current == LoopingState.Playlist)
                        {
                            AudioHandler.SetLoopState(req, LoopingState.Song);
                            await req.SendSimpleEmbedAsync($"The Song is now set to loop!\n{tooltip}", "Audio Looping : 🔂");
                        }
                        else if (current == LoopingState.Song)
                        {
                            AudioHandler.SetLoopState(req, LoopingState.None);
                            await req.SendSimpleEmbedAsync($"The Queue is no longer set to loop!\n{tooltip}", "Audio Looping : 1️⃣");
                        }
                    }
                    else if (req.Command == "lp")
                    {
                        if (AudioHandler.GetLoopState(req) == LoopingState.None)
                        {
                            AudioHandler.SetLoopState(req, LoopingState.Playlist);
                            await req.SendSimpleEmbedAsync("The Queue is now set to loop!", "Audio Looping : 🔁");
                        }
                        else
                        {
                            AudioHandler.SetLoopState(req, LoopingState.None);
                            await req.SendSimpleEmbedAsync("The Queue is no longer set to loop!", "Audio Looping : 1️⃣");
                        }
                    }
                    else if (req.Command == "ls")
                    {
                        if (AudioHandler.GetLoopState(req) == LoopingState.None)
                        {
                            AudioHandler.SetLoopState(req, LoopingState.Song);
                            await req.SendSimpleEmbedAsync("The Song is now set to loop!", "Audio Looping : 🔂");
                        }
                        else
                        {
                            AudioHandler.SetLoopState(req, LoopingState.None);
                            await req.SendSimpleEmbedAsync("The Queue is no longer set to loop!", "Audio Looping : 1️⃣");
                        }
                    }
                } 

            }

        }

    }
}
