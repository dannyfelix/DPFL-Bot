using System;

namespace DPFL.LastFm.Domain.Models
{
    public class ArtistResponse
    {
        public Artist Artist { get; set; }
    }
    
    public class Artist
    {
        public string Name { get; set; }

        public Guid Mbid { get; set; }

        public Uri Url { get; set; }

        public int Streamable { get; set; }

        public Stats Stats { get; set; }

        public Tags Tags { get; set; }

        public Bio Bio { get; set; }
    }

    public class Stats
    {
        public long Listeners { get; set; }
        
        public long PlayCount { get; set; }

        public long? UserPlayCount { get; set; }
    }

    public class Tags
    {
        public Tag[] Tag { get; set; }
    }

    public class Bio
    {
        public string Published { get; set; }
        
        public string Summary { get; set; }
        
        public string Content { get; set; }
    }
}