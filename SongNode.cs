using System;
#nullable disable

//This class is basically the node for my linked list
namespace Project_2
{
    class SongNode
    {
        public string Title;
        public string Artist;
        public string Album;
        public int  DurationSeconds;
        public string Genre;
        public SongNode Next;

        // this is simple constructer for filling the song info
        public SongNode(string title, string artist, string album, int durationSeconds, string genre)
        {
            Title = title;
            Artist = artist;
            Album = album;
            DurationSeconds =  durationSeconds;
            Genre = genre;
              Next = null;
        }


        public string GetFormattedDuration ()
        {
            int minutes = DurationSeconds/  60;
            int seconds = DurationSeconds % 60;
            return minutes.ToString("00") + ":" +seconds.ToString("00");
        }
    }
}
