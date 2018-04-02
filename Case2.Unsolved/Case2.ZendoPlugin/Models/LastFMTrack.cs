using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Case2.ZendoPlugins.Models
{
    public class LastFMTrack
    {
        public string Name { get; set; }

        public int Duration { get; set; }

        public int PlayCount { get; set; }

        public int Listeners { get; set; }

        public string MBId { get; set; }

        public string Url { get; set; }

        public LastFMArtist Artist { get; set; }

        public LastFMTopTags TopTags { get; set; }
    }
}
