using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using EloBot.Api;

namespace EloBot.Dialogs
{
    [LuisModel("cdcac19f-af31-497d-bcb1-479c2db8f6b9", "70ec9287f07a44d390eb1a7fcb5c7106")]
    [Serializable]
    public class SummonerInfoDialog : LuisDialog<object>
    {
        List<string> servers = new List<string>
        {
            "NA1", "EUW1", "EUN1", "KR", "BR1", "OC1", "JP1", "TR1", "LA1", "LA2", "RU"
        };

        string name = "";
        string server = "";

        [LuisIntent("SummonerInfo")]
        public async Task GetSummonerInfo(IDialogContext context, LuisResult result)
        {
            EntityRecommendation NameRec;
            EntityRecommendation ServerRec;

            if(result.TryFindEntity("summoner", out NameRec))
            {
                name = NameRec.Entity;

                if(name.ToUpper().Equals("STAKTW") || name.ToUpper().Equals("YøP".ToUpper()))
                {
                    var msg = context.MakeMessage();
                    msg.Attachments.Add(new Attachment
                    {
                        ContentUrl = "https://scontent-atl3-1.cdninstagram.com/t51.2885-19/s150x150/14676663_725784750893621_8005160500998438912_a.jpg",
                        ContentType = "image/jpg",
                        Name = "stayek"
                    });
                    await context.PostAsync(msg);
                }

                if(result.TryFindEntity("server", out ServerRec))
                {
                    server = ServerRec.Entity;

                    if (IsServer(server))
                    {
                        var res = await new RestApi().GetSummonerByName(server, name);

                        if (res != null)
                        {
                            var matches = await new RestApi().GetRecentMatches(server, res.accountId.ToString());

                            await context.PostAsync($"hello {res.name}");

                            if (matches != null)
                            {
                                await context.PostAsync($"You played {matches.TotalGames} games");
                            }
                        }
                        else
                        {
                            await context.PostAsync($"{name} is not available in {server}");
                        }
                        
                        context.Wait(MessageReceived);
                    }
                    else
                    {
                        PromptOptions<string> options = new PromptOptions<string>
                            ("Choose one of these servers please",
                            "Try again please", "You're a try hard," +
                            " i get it !!",
                            servers,
                            2
                            );
                        PromptDialog.Choice(context, GetInfoAsync, options);
                    }
                }
            }
            else
            {
                await context.PostAsync("I couldn't understand what you're saying, i hope your skills at playing are better than your writing !!");
                context.Wait(MessageReceived);
            }
        }

        public async Task GetInfoAsync(IDialogContext context, IAwaitable<string> result)
        {
            server = await result;
            var res = await new RestApi().GetSummonerByName(server, name);

            if (res != null)
            {
                await context.PostAsync($"Hello {res.name}");
            }
            else
            {
                await context.PostAsync($"{name} is not available in {server}");
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("What ???");
            context.Wait(MessageReceived);
        }

        private bool IsServer(string server)
        {
            foreach(var s in servers)
            {
                if (s.Equals(server.ToUpper()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}