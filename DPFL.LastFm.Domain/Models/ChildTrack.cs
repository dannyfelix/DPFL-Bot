using System;

namespace DPFL.LastFm.Domain.Models
{
    public class ChildTrack
    {
        public string Name { get; set; }

        public int Duration { get; set; }

        public Guid Mbid { get; set; }
        
        public Uri Url { get; set; }

        public ChildArtist Artist { get; set; }
    }
}