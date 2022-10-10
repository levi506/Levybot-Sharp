using Discord;
using Discord.WebSocket;
using LevyBotSharp.DataHandlers.Database;
using LevyBotSharp.Utility.Attributes;
using LevyBotSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LevyBotSharp.DataHandlers.Database.DataHandler;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    public class Profile : CommandGroupBase
    {
        public override string Name { get; } = "Profiles";

        [CommandMeta(new[] { "User" }, "Retrives Basic Info about a User")]
        [GlobalContext]
        [GuildContext]
        public async Task User(UserRequest req)
        {

            switch (req.Area)
            {
                case Locale.Guild:
                    {
                        var guild = req.Guild;

                        var check = req.Args.FirstOrDefault();
                        SocketUser param;
                        if (check != null)
                            param = await check.AsSocketUser(guild);
                        else
                            param = null;

                        var member = guild.GetUser(param?.Id ?? req.Requester.Id);

                        //Join Pos
                        var joinPos = guild.Users.SortMemByDate().IndexOf(member) + 1;

                        //User Status
                        var act = member.Activity;
                        var status = "";
                        if (act != null)
                        {
                            switch (act.Type)
                            {
                                case ActivityType.Streaming:
                                    var sAct = act as StreamingGame;
                                    status = $"[Streaming {act.Name}]({sAct.Url})";
                                    break;
                                case ActivityType.Listening:
                                    var mAct = act as SpotifyGame;
                                    status = $"[Listening To {act.Name}]({mAct.TrackUrl})";
                                    break;
                                case ActivityType.Playing:
                                    status = $"Playing {act.Name}";
                                    break;
                                case ActivityType.Watching:
                                    status = $"Watching {act.Name}";
                                    break;
                            }
                        }
                        else
                        {
                            status = member.Status.ToString();
                            status = status.Equals("DoNotDisturb") ? "Do Not Disturb" : member.Status.ToString();
                        }

                        //Roles
                        var roles = member.Roles.ToList();
                        var roleStr = "";
                        SocketRole highestRole = guild.EveryoneRole;
                        foreach (var role in roles)
                        {
                            if (role.Id.Equals(req.Guild.Id)) continue;
                            if (roles.Last().Equals(role))
                            {
                                roleStr += role.Name;
                            }
                            else
                            {
                                roleStr += role.Name + ", ";
                            }
                            if (highestRole.Position < role.Position && role.Color.ToString() != Color.Default.ToString())
                            {
                                highestRole = role;
                            }

                        }

                        //Embed Construction
                        var embed = new EmbedBuilder();

                        embed.WithAuthor(member.DisplayName());
                        embed.WithThumbnailUrl(member.GetAvatarUrl());
                        embed.AddField("Id", member.Id, true);
                        embed.AddField("Status", status, true);
                        embed.AddField("Join Date", member.JoinedAt?.UtcDateTime, true);
                        embed.AddField("Account Created", member.CreatedAt.UtcDateTime, true);
                        if (roleStr != "")
                        {
                            embed.AddField("Roles", roleStr);
                        }
                        embed.WithFooter("Member #" + joinPos, req.Bot.CurrentUser.GetAvatarUrl());
                        embed.WithCurrentTimestamp();
                        if (highestRole != null && highestRole.Color.ToString() != Color.Default.ToString())
                        {
                            embed.WithColor(highestRole.Color);
                        }
                        else
                        {
                            embed.WithColor(101, 205, 229);
                        }

                        await req.Channel.SendMessageAsync(string.Empty, embed: embed.Build());
                        break;
                    }
                case Locale.Dm:
                    {

                        //Status
                        var act = req.Requester.Activity;
                        Console.WriteLine(act);
                        var status = "";
                        if (act != null)
                        {
                            switch (act.Type)
                            {
                                case ActivityType.Streaming:
                                    var sAct = act as StreamingGame;
                                    status = $"[Streaming {act.Name}]({sAct.Url})";
                                    break;
                                case ActivityType.Listening:
                                    var mAct = act as SpotifyGame;
                                    status = $"[Listening To {act.Name}]({mAct.TrackUrl})";
                                    break;
                                case ActivityType.Playing:
                                    status = $"Playing {act.Name}";
                                    break;
                                case ActivityType.Watching:
                                    status = $"Watching {act.Name}";
                                    break;
                            }
                        }
                        else
                        {
                            status = req.Requester.Status.ToString();
                            status = status.Equals("DoNotDisturb") ? "Do Not Disturb" : req.Requester.Status.ToString();
                        }

                        //Embed Construction
                        var embed = new EmbedBuilder();

                        embed.WithAuthor(req.Requester.Username);
                        embed.WithThumbnailUrl(req.Requester.GetAvatarUrl());
                        embed.AddField("Id", req.Requester.Id, true);
                        embed.AddField("Status", status, true);
                        embed.AddField("Account Created", req.Requester.CreatedAt.UtcDateTime, true);
                        embed.WithFooter("Member #X", req.Bot.CurrentUser.GetAvatarUrl());
                        embed.WithCurrentTimestamp();
                        embed.WithColor(101, 205, 229);

                        await req.Channel.SendMessageAsync(string.Empty, embed: embed.Build());
                        break;
                    }
            }
        }

        [CommandMeta(new[] { "Social" }, "Returns an embed of a user's social and gamining info")]
        [GlobalContext]
        [GuildContext]
        public async Task SocialCom(UserRequest req)
        {
            var embed = new EmbedBuilder();
            var socials = new Dictionary<string, string>();
            switch (req.Area)
            {
                case Locale.Guild:
                    if (req.SpInstruct.ContainsKey("set"))
                    {
                        await SocialSet(req);
                        return;
                    }
                    else
                    {
                        var guild = req.Guild;

                        var check = req.Args.FirstOrDefault();
                        SocketUser param;
                        if (check != null)
                            param = await check.AsSocketUser(guild);
                        else
                            param = null;

                        var member = guild.GetUser(param?.Id ?? req.Requester.Id);

                        if (req.SpInstruct.ContainsKey("switch") || req.SpInstruct.ContainsKey("nintendo") || req.SpInstruct.ContainsKey("game") || req.SpInstruct.ContainsKey("console") || req.SpInstruct.ContainsKey("all") || req.SpInstruct.Keys.Count == 0)
                        {
                            socials.Add("switch", await GetSocialPlat(member.Id, SPlatform.Switch));
                        }
                        if (req.SpInstruct.ContainsKey("twitch") || req.SpInstruct.ContainsKey("social") || req.SpInstruct.ContainsKey("all") || req.SpInstruct.Keys.Count == 0)
                        {
                            socials.Add("twitch", await GetSocialPlat(member.Id, SPlatform.Twitch));
                        }
                        if (req.SpInstruct.ContainsKey("twitter") || req.SpInstruct.ContainsKey("social") || req.SpInstruct.ContainsKey("all") || req.SpInstruct.Keys.Count == 0 )
                        {
                            socials.Add("twitter", await GetSocialPlat(member.Id, SPlatform.Twitter));
                        }
                        if (req.SpInstruct.ContainsKey("xbox") || req.SpInstruct.ContainsKey("game") || req.SpInstruct.ContainsKey("console") || req.SpInstruct.ContainsKey("all") || req.SpInstruct.Keys.Count == 0)
                        {
                            socials.Add("xbox", await GetSocialPlat(member.Id, SPlatform.Xbox));
                        }
                        if (req.SpInstruct.ContainsKey("psn") || req.SpInstruct.ContainsKey("ps4") || req.SpInstruct.ContainsKey("playstation") || req.SpInstruct.ContainsKey("game") || req.SpInstruct.ContainsKey("console") || req.SpInstruct.ContainsKey("all") || req.SpInstruct.Keys.Count == 0)
                        {
                            socials.Add("psn", await GetSocialPlat(member.Id, SPlatform.PSN));
                        }
                        if (req.SpInstruct.ContainsKey("pc") || req.SpInstruct.ContainsKey("steam") || req.SpInstruct.ContainsKey("game") || req.SpInstruct.ContainsKey("all") || req.SpInstruct.Keys.Count == 0)
                        {
                            socials.Add("steam", await GetSocialPlat(member.Id, SPlatform.Steam));
                            socials.Add("steam-url", await GetSocialPlat(member.Id, SPlatform.SteamUrl));
                        }


                        embed.WithAuthor(member.DisplayName());
                        embed.WithCurrentTimestamp();
                        embed.WithThumbnailUrl(member.GetAvatarUrl());
                        embed.WithFooter(Program.GetVersion(), req.Bot.CurrentUser.GetAvatarUrl());
                        if (socials.TryGetValue("switch", out var sw) && !string.IsNullOrEmpty(sw))
                            embed.AddField("Switch", sw, true);
                        if (socials.TryGetValue("twitch", out var tch) && !string.IsNullOrEmpty(tch))
                            embed.AddField("Twitch", $"[{tch}]({"https://twitch.tv/" + tch})", true);
                        if (socials.TryGetValue("twitter", out var tw) && !string.IsNullOrEmpty(tw))
                            embed.AddField("Twitter", $"[@{tw}]({"https://twitter.com/" + tw})", true);
                        if (socials.TryGetValue("xbox", out var xb) && !string.IsNullOrEmpty(xb))
                            embed.AddField("Xbox Live", $"[{xb}]({"https://account.xbox.com/en-US/Profile?gamerTag=" + xb.Replace(" ", "%20")})", true);
                        if (socials.TryGetValue("psn", out var ps) && !string.IsNullOrEmpty(ps))
                            embed.AddField("Playstation Network", $"[{ps}]({"https://my.playstation.com/profile/" + ps.Replace(" ", "%20")})", true);
                        if (socials.TryGetValue("steam", out var st) && !string.IsNullOrEmpty(st)) {
                            if(socials.TryGetValue("steam-url", out var stl) && !string.IsNullOrEmpty(stl))
                                embed.AddField("Steam", $"[{st}]({stl.Replace(" ", "%20")})", true);
                            else
                                embed.AddField("Steam", st, true);
                        }

                        if(embed.Fields.Count < 1)
                        {
                            embed.WithDescription("There is nothing avalible that matches the tags requested");
                        } else
                        {
                            embed.WithDescription("Shared Accounts");
                        }

                        await req.Channel.SendMessageAsync(string.Empty, embed: embed.Build());
                    }

                    break;
                case Locale.Dm:
                    if (req.SpInstruct.ContainsKey("set"))
                    {
                        await SocialSet(req);
                        return;
                    }
                    else
                    {

                    }
                    break;
            }
        }



        [CommandMeta(new[] { "Avatar" }, "Returns the an embed of a user's avatar")]
        [GuildContext]
        public async Task AvatarCom(UserRequest req)
        {
            EmbedBuilder embed;
            switch (req.Area)
            {
                case Locale.Guild:
                    var guild = req.Guild;

                    var check = req.Args.FirstOrDefault();
                    SocketUser param;
                    if (check != null)
                        param = await check.AsSocketUser(guild);
                    else
                        param = null;

                    var member = guild.GetUser(param?.Id ?? req.Requester.Id);

                    embed = new EmbedBuilder();

                    embed.WithAuthor(member);
                    embed.WithImageUrl(member.GetAvatarUrl(size: 256));

                    if (member.Id == 319015824642015264)
                    {
                        embed.WithDescription("Icon made by [@AveryIsbellArt](https://twitter.com/AveryIsbellArt)");
                    }
                    else if (member.Id == 455194993993449482)
                    {
                        embed.WithDescription("Icon made by [@Nicoohai](https://twitter.com/Nicoohai)");
                    }

                    await req.Channel.SendMessageAsync(string.Empty, embed: embed.Build());

                    break;
                default:
                    var user = req.Requester;

                    embed = new EmbedBuilder();

                    embed.WithAuthor(user);
                    embed.WithImageUrl(user.GetAvatarUrl(size: 256));

                    await req.Channel.SendMessageAsync(string.Empty, embed: embed.Build());
                    break;
            }
        }

        private async Task SocialSet(UserRequest req)
        {
            if (req.SpInstruct.ContainsKey("switch"))
            {
                var sw = req.Args.Merge().AsString();
                if (!string.IsNullOrEmpty(sw))
                    await SetSocial(req.Requester.Id, sw, (int)SPlatform.Switch);
                else
                    await SetSocial(req.Requester.Id, "", (int)SPlatform.Switch);
            }
            if (req.SpInstruct.ContainsKey("twitch"))
            {
                req.SpInstruct.TryGetValue("twitch", out var tw);
                if(!string.IsNullOrEmpty(tw.Variance.AsString()))
                    await SetSocial(req.Requester.Id, tw.Variance.AsString(), (int) SPlatform.Twitch);
                else
                    await SetSocial(req.Requester.Id, "", (int)SPlatform.Twitch);

            }
            if (req.SpInstruct.ContainsKey("twitter"))
            {
                req.SpInstruct.TryGetValue("twitter", out var tw);
                if (!string.IsNullOrEmpty(tw.Variance.AsString()))
                    await SetSocial(req.Requester.Id, tw.Variance.AsString(), (int)SPlatform.Twitter);
                else
                    await SetSocial(req.Requester.Id, "", (int)SPlatform.Twitter);
            }
            if (req.SpInstruct.ContainsKey("xbox"))
            {
                req.SpInstruct.TryGetValue("xbox", out var tw);
                if (!string.IsNullOrEmpty(tw.Variance.AsString()))
                    await SetSocial(req.Requester.Id, tw.Variance.AsString(), (int)SPlatform.Xbox);
                else
                    await SetSocial(req.Requester.Id, "", (int)SPlatform.Xbox);
            }
            if (req.SpInstruct.ContainsKey("psn"))
            {
                req.SpInstruct.TryGetValue("psn", out var tw);
                if (!string.IsNullOrEmpty(tw.Variance.AsString()))
                    await SetSocial(req.Requester.Id, tw.Variance.AsString(), (int)SPlatform.PSN);
                else
                    await SetSocial(req.Requester.Id, "", (int)SPlatform.PSN);
            }
            if (req.SpInstruct.ContainsKey("steam"))
            {
                req.SpInstruct.TryGetValue("steam", out var tw);
                if (!string.IsNullOrEmpty(tw.Variance.AsString()))
                    await SetSocial(req.Requester.Id, tw.Variance.AsString(), (int)SPlatform.Steam);
                else
                    await SetSocial(req.Requester.Id, "", (int)SPlatform.Steam);
            }
            if (req.SpInstruct.ContainsKey("stream-url"))
            {
                req.SpInstruct.TryGetValue("stream-url", out var tw);
                if (!string.IsNullOrEmpty(tw.Variance.AsString()))
                    await SetSocial(req.Requester.Id, tw.Variance.AsString(), (int)SPlatform.SteamUrl);
                else
                    await SetSocial(req.Requester.Id, "", (int)SPlatform.SteamUrl);
            }

        }
    }
}
