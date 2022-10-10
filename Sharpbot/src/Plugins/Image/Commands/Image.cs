using Discord;
using Sharpbot.Plugins.Commands.Utility;
using Sharpbot.Services.Data.Utility;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Sharpbot.Plugins.Images.Commands
{
    class ImagesCom : CommandGroup
    {
        public override string Name { get; set; } = "Image";

        private ImagePlugin ImgSys { get; set; }
        public ImagesCom(ImagePlugin img)
        {
            ImgSys = img;
        }

        [CommandName("Jumoji")]
        [CommandMeta("Makes Emotes Large", "Image")]
        [Usage(Location.Guild | Location.DM)]
        public class Jumboji : Command
        {
            public override async Task Default(CommandRequest req)
            {
                await req.SourceRequest.RespondText("An emote is need for jumbo-ing");
            }

            public override async Task Excute(CommandRequest req)
            {
                if(req.Params.Count > 0 && req.Params.First().ParamTypes.TryGetValue(ParamType.Emote,out var isEmote) && isEmote)
                {
                    await JumboizeEmote(req);
                } 
                else
                {
                    await Default(req);
                }
            }

            private async Task JumboizeEmote(CommandRequest req)
            {
                var Par = Parent as ImagesCom;
                var ImgSys = Par.ImgSys;
                int size = 256;
                if (req.SpInstructions.ContainsKey("size") && req.SpInstructions.TryGetValue("size",out var sizeInstruct))
                {
                    var internalParam = sizeInstruct.Variance.First();
                    if(internalParam.ParamTypes.TryGetValue(ParamType.Int,out var isInt) && isInt)
                    {
                        size = int.Parse(internalParam.RawData);
                    }
                    if(size > 512)
                    {
                        size = 512;
                    }
                }
                
                if (Emote.TryParse(req.Params.First().RawData, out var emote))
                {
                    MemoryStream jumoji;
                    var file = ImgSys.GetFile(emote.Url);
                    var channel = req.SourceRequest.DiscordMessage.Channel as IMessageChannel;
                    if (ImgSys.LinkIsImage(emote.Url))
                    {
                        if (ImgSys.LinkIsGif(emote.Url))
                       {
                            jumoji = ImgSys.ResizeGif(emote.Url, size, size);
                       }
                       else
                       {
                            jumoji = OperatingSystem.IsWindows()?ImgSys.EffResizeImage(emote.Url, size, size):ImgSys.ResizeImage(emote.Url,size,size);
                            
                       }
                            Embed embed;
                        if (!emote.Animated)
                        {
                            embed = new EmbedBuilder()
                                  .WithAuthor(req.SourceRequest.DiscordBot.CurrentUser)
                                  .WithTitle(emote.Name)
                                  .WithImageUrl($"attachment://jumoji.png")
                                  .Build();
                            jumoji.Position = 0;
                            channel.SendFileAsync(jumoji, "jumoji.png", embed: embed);
                        }
                        else
                        {
                            embed = new EmbedBuilder()
                                .WithAuthor(req.SourceRequest.DiscordBot.CurrentUser)
                                .WithTitle(emote.Name)
                                .WithImageUrl($"attachment://jumoji.gif")
                                .Build();
                            jumoji.Position = 0;
                            channel.SendFileAsync(jumoji, "jumoji.gif", embed: embed);
                        }
                        channel.SendFileAsync(jumoji,"jumoji.gif", embed:embed);

                        jumoji.DisposeAsync();
                    }

                    
                }
            }

            public override Task SendHelp(CommandRequest req)
            {
                throw new NotImplementedException();
            }
        }
    }
}
