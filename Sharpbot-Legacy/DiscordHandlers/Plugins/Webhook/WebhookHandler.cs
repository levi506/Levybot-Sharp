using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Webhook;
using LevyBotSharp.DataHandlers.Database;

namespace LevyBotSharp.DiscordHandlers.Plugins.Webhook
{
    public enum HookPurpose
    {
        GeneralLog,
        UserFlow,
        UserRole,
        Messages,
        BulkDelete,
        Edit,
        Server,
        ChannelChange,
        Invites,
        HookChange,
    }
    /*
    public static class WebhookHandler
    {
        

        public static async Task<HookResp> GetHook(HookRequest Req)
        {
            var guild = await Req.Bot.GetGuildAsync(Req.GuildId);
            var hook = new DiscordWebhookClient();

        }



        
    }
    public struct HookRequest
    {
        public IDiscordClient Bot;
        public ulong GuildId;
        public HookPurpose Purp;
        public int HookId;
    }

    public class HookResp
    {
        public DiscordWebhookClient Hook { get; private set; }
        public IDiscordClient Bot { get; private set; }
        public Hook

        public HookResp(HookRequest req,DiscordWebhookClient hook)
        {

        }
    }
    */
}