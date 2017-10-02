using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EloBot.Api
{
    public class Summoner
    {
        public int profileIconId { get; set; }
        public string name { get; set; }
        public int summonerLevel { get; set; }
        public int accountId { get; set; }
        public int id { get; set; }
        public long revisionDate { get; set; }
    }

    public class MatchList
    {
        public List<MatchReference> Matches { get; set; }
        public int TotalGames { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }

    }

    public class MatchReference
    {
        public long GameId { get; set; }
        public string Lane { get; set; }
        public int Champion { get; set; }
        public string PlatformId { get; set; }
        public int Season { get; set; }
        public int Queue { get; set; }
        public string Role { get; set; }
        public long TimeStamp { get; set; }
        
    }
}