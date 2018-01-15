using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Uniconta.API.System;
using Uniconta.Common;
using ZendoPlugins.DataModels;
using ZendoPlugins.Models;

namespace ZendoPlugins
{
    public class LastFMAPI
    {
        // Fields
        private const string APIKEY = "8bc23d26f70f8fd1067533b6b524f3bb";

        private CrudAPI crudAPI;

        public LastFMAPI(CrudAPI crudAPI)
        {
            this.crudAPI = crudAPI;
        }

        public List<LastFMTrack> GetTop50()
        {
            using (var webClient = new WebClient())
            {
                string url = $"http://ws.audioscrobbler.com/2.0/?method=chart.gettoptracks&api_key={APIKEY}&format=json";
                var jsonStr = webClient.DownloadString(url);
                var response = JsonConvert.DeserializeObject<LastFMGetTopTracksResponce>(jsonStr);

                return response.Tracks.Track;
            }
        }

        public LastFMTrack GetTrackInfo(LastFMTrack track)
        {
            using (var webClient = new WebClient())
            {
                string url = $"http://ws.audioscrobbler.com/2.0/?method=track.getInfo&api_key={APIKEY}&artist={track.Artist.Name}&track={track.Name}&format=json";
                var jsonStr = webClient.DownloadString(url);
                var response = JsonConvert.DeserializeObject<LastFMGetInfoResponse>(jsonStr);

                return response.Track;
            }
        }
    }
}
