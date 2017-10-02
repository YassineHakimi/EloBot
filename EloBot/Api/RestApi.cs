using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace EloBot.Api
{
    public class RestApi
    {
        private const string URL = ".api.riotgames.com/lol/";
        private const string API_KEY = "?api_key=RGAPI-ec0a5458-a8ac-405d-9465-61997b437edb";
        
        public async Task<MatchList> GetRecentMatches(string server, string accountId)
        {
            string MatchesQuery = $"match/v3/matchlists/by-account/{accountId}/recent";
            string MatchesUri = "https://" + server + URL + MatchesQuery + API_KEY;

            HttpClient client = new HttpClient();

            var response = await client.GetAsync(MatchesUri);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<MatchList>(jsonResponse);
                return data;
            }

            return null;
        }

        public async Task<Summoner> GetSummonerByName(string server, string name)
        {
            string SummonerQuery = "summoner/v3/summoners/by-name/" + name;
            string SummonerUri = "https://" + server + URL + SummonerQuery + API_KEY;

            HttpClient client = new HttpClient();
            
            var response = await client.GetAsync(SummonerUri);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<Summoner>(jsonResponse);
                return data;
            }

            return null;
        }
    }
}