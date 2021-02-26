using System;

namespace DPFL.LastFm.Domain.Models
{
    public class AlbumResponse
    {
        public Album Album { get; set; }
    }
    
    public class Album
    {
        public string Name { get; set; }

        public string Artist { get; set; }

        public int Id { get; set; }

        public Guid Mbid { get; set; }

        public Uri Url { get; set; }

        public DateTime ReleaseDate { get; set; }

        public long Listeners { get; set; }
        
        public long PlayCount { get; set; }

        public long? UserPlayCount { get; set; }
        
        public Tags Tags { get; set; }

        public Tracks Tracks { get; set; }
        
    }

    public class Tracks
    {
        public ChildTrack[] Track { get; set; }
    }
}