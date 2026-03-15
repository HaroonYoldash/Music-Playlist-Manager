#nullable disable
using Project_2;
using System;
using System.IO;

namespace Project_2
{
    class Playlist
    { 
        private SongNode firsthead; // this one is my head of my linked list
        private SongNode currentSong;

        public bool IsEmpty()
        {
            return firsthead == null;
        }

        // this one is for load songs from CSV 
        public void ImportFromCSV(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("CSV file is not found \n");
               return;
            }

            string[]lines = File.ReadAllLines(path);

            if (lines.Length <=  1)
            {
                Console.WriteLine("CSV file is empty \n");
                return;
            }

            int added =0;

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string[] parts = line.Split(',');

                if (parts.Length < 6) { continue; }
                string title = parts[1].Trim();
                string artist = parts[2].Trim();
                string album = parts[3].Trim();
                string durationText = parts[4].Trim();
                string genre = parts[5].Trim();

                int durationSeconds = ConvertCsvDuration(durationText);
                if (durationSeconds < 0)
                    continue;

                addSongLast(title, artist, album, durationSeconds, genre);
                added++;
            }

            Console.WriteLine("Songs added successfully. added = " + added + "\n");
        }
        
        // this one convert time like 02:11 to seconds 
        private int ConvertCsvDuration(string duration)
        {
            try
            {
                string[] parts = duration.Split(':');
                int minutes =  int.Parse(parts [0]);
                int seconds = int.Parse(parts[1] );
                return minutes * 60 + seconds;
            }
            catch
            {
                return -1;
            }
        }
        private SongNode FindPrevious(SongNode target)
        {
            SongNode current =firsthead;
            while (current != null && current.Next != target)
            {
                current =current.Next;
            }
            return current;

        }

        public void addSongLast(string title, string artist, string album, int durationSeconds, string genre)
        {
            SongNode newNode = new SongNode(title, artist, album, durationSeconds, genre);

            if (IsEmpty())
            {
                firsthead = newNode;
            }
            else
            {
                SongNode current = firsthead;
                while (current.Next != null)
                    current = current.Next;

                current.Next = newNode;
            }

            Console.WriteLine("Song added.\n");
        }

        // this print all songs in a table like fromat 
        public void ShowSongs()
        {
            if (IsEmpty())
            {
                Console.WriteLine("Playlist is empty.\n");
                return;
            }

            Console.WriteLine("NO".PadRight(5) + "TITLE".PadRight(28) + "ARTIST".PadRight(22) + "ALBUM".PadRight(26) +  "DURATION".PadRight(10) + "GENRE");
            Console.WriteLine(new string('-', 110));

            SongNode current = firsthead;
            int index = 1;

            while (current != null)
            {
                Console.WriteLine(index.ToString().PadRight(5) + current.Title.PadRight(28) + current.Artist.PadRight(22) +  current.Album.PadRight(26) + current.GetFormattedDuration().PadRight(10) + current.Genre);
                current = current.Next;
                index++;
            }

            Console.WriteLine();
        }

        // deletse song by matching title text
        public void DeleteByTitle(string title)
        {
            if (IsEmpty())
            {
                Console.WriteLine("Playlist is empty.\n");
                return;
            }

            if (firsthead.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
            {
                firsthead = firsthead.Next;
                Console.WriteLine("Song deleted.\n");
                return;
            }

            SongNode current = firsthead;
            SongNode prviousNode = null;

            while (current != null)
            {
                if (current.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    prviousNode.Next = current.Next;
                    Console.WriteLine("Song deleted.\n");
                    return;
                }

                prviousNode = current;
                current = current.Next;
            }

            Console.WriteLine("Song does not exist in playlist.\n");
        }

        // delete song based on number shown in list
        public void DeleteByPosition(int position)
        {
            if (IsEmpty())
            {
                Console.WriteLine("Playlist is empty.\n");
                return;
            }

            if (position <= 0)
            {
                Console.WriteLine("Invalid position.\n");
                return;
            }

            if (position == 1)
            {
                firsthead = firsthead.Next;
                Console.WriteLine("Song deleted.\n");
                return;
            }

            SongNode current = firsthead;
            SongNode prviousNode = null;
            int index = 1;

            while (current != null)
            {
                if (index == position)
                {
                    prviousNode.Next = current.Next;
                    Console.WriteLine("Song deleted.\n");
                    return;
                }

                prviousNode = current;
                current = current.Next;
                index++;
            }

            Console.WriteLine("Position not found.\n");
        }
        // serches the playlist for a song title
        public void Search(string title)
        {
            if (IsEmpty())
            {
                Console.WriteLine("Playlist is empty.\n");
                return;
            }

            SongNode current = firsthead;
            int index = 1;

            while (current != null)
            {
                if (current.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"Song found at position: {index} \n");
                    return;
                }
                current = current.Next;
                index++;
            }

            Console.WriteLine("Song not found.\n");
        }

        public void PlayNext()
        {
            if (IsEmpty()) { Console.WriteLine("Playlist is empty.\n"); return; }

            if (currentSong == null)
            {
                currentSong = firsthead;
            }
            else if (currentSong.Next != null)
            {
                currentSong = currentSong.Next;
            }
            else
            {
                currentSong = firsthead;
                Console.WriteLine("End of playlist. ");
            }

            Console.WriteLine($" Now Playing (Next): {currentSong.Title} by {currentSong.Artist} ({currentSong.GetFormattedDuration()})\n");
        }

        public void PlayPrevious()
        {
            if (IsEmpty()) { Console.WriteLine("Playlist is empty.\n"); return; }

            if (currentSong == null)
            {
                currentSong = firsthead;
            }
            else if (currentSong == firsthead)
            {
                SongNode current = firsthead;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                currentSong = current;
                Console.WriteLine("Start playlist.");
            }
            else
            {
                currentSong = FindPrevious(currentSong);
            }

            if (currentSong != null)
            {
                Console.WriteLine($" Now Playing (Previous):{currentSong.Title} by {currentSong.Artist} ({currentSong.GetFormattedDuration()})\n");
            }
        }

        // this one is sort by artist name
        public void SortByArtist()
        {
            if (IsEmpty() || firsthead.Next == null)
            {
                Console.WriteLine("Not enough songs to sort.\n");
                return;
            }

            bool swapped;
            do
            {
                swapped = false;
                SongNode current = firsthead;

                while (current.Next != null)
                {
                    if (string.Compare(current.Artist, current.Next.Artist, true) > 0)
                    {
                        SwapData(current, current.Next);
                        swapped = true;
                    }
                    current = current.Next;
                }
            }
            while (swapped);

            Console.WriteLine("Playlist sorted by artist.\n");
        }
       
        public void SortByDuration()
        {
            if (IsEmpty() || firsthead.Next == null)
            {
                Console.WriteLine("Not enough songs to sort.\n");
                return;
            }

            bool swapped;
            do
            {
                swapped = false;
                SongNode current = firsthead;

                while (current.Next != null)
                {
                    if (current.DurationSeconds > current.Next.DurationSeconds)
                    {
                        SwapData(current, current.Next);
                        swapped = true;
                    }

                    current = current.Next;
                }
            }
            while (swapped);

            Console.WriteLine("Playlist sorted by duration.\n");
        }

        // shuffle by randomly swapping nodes
        public void Shuffle()
        {
            if (IsEmpty() || firsthead.Next == null)
            {
                Console.WriteLine("Not enough songs to shuffle.\n");
                return;
            }

            Random random = new Random();

            int count = 0;
            SongNode current = firsthead;
            while (current != null)
            {
                count++;
                current = current.Next;
            }

            for (int i = 0; i < count * 2; i++)
            {
                int pos1 = random.Next(1, count + 1);
                int pos2 = random.Next(1, count + 1);

                if (pos1 == pos2) continue;

                SongNode node1 = FindNodeAt(pos1);
                SongNode node2 = FindNodeAt(pos2);

                if (node1 != null && node2 != null)
                    SwapData(node1, node2);
            }

            Console.WriteLine("Playlist shuffled.\n");
        }

        // shuffle by randomly swapping nodes
        private SongNode FindNodeAt(int pos)
        {
            SongNode current = firsthead;
            int index = 1;

            while (current != null)
            {
                if (index == pos)
                    return current;

                current = current.Next;
                index++;
            }

            return null;
        }
        // this one is my extra feature show by genre
        public void FilterByGenre(string showByGenre)
        {
            if (IsEmpty())
            {
                Console.WriteLine("The Playlist is currenlty empty. \n");
                return;
            }

            SongNode currentnow = firsthead;
            int count = 0;

            Console.WriteLine($" \n Filtered Playlist (Genre: {showByGenre}) ");
            Console.WriteLine("NO".PadRight(5) +
                             "TITLE".PadRight(28 ) +
                             "ARTIST".PadRight( 22) +
                             "ALBUM".PadRight(26) +
                             "DURATION".PadRight (10) +
                             "GENRE");
            Console.WriteLine("");
            int index = 1;
            while (currentnow != null)
            {
                if (currentnow.Genre.Equals(showByGenre, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(
                        index.ToString().PadRight(5) +
                        currentnow.Title.PadRight(28) +
                        currentnow.Artist.PadRight(22) +
                        currentnow.Album.PadRight(26) +
                        currentnow.GetFormattedDuration().PadRight(10) +
                        currentnow.Genre
                    );
                    count++;
                }

                currentnow = currentnow.Next;
                index++;
            }

            if (count == 0)
            {
                Console.WriteLine($"\nNo songs found in the genre '{showByGenre}' \n");
            }
            else
            {
                Console.WriteLine($"\nFilter complete. {count} songs found \n");
            }
        }

        // this one is swap data between two nodes
        private void SwapData(SongNode a, SongNode b)
        {
            string tTitle = a.Title;
            string tArtist = a.Artist;
            string tAlbum = a.Album;
            int tDuration = a.DurationSeconds;
            string tGenre = a.Genre;

            a.Title = b.Title;
            a.Artist = b.Artist;
            a.Album = b.Album;
            a.DurationSeconds = b.DurationSeconds;
            a.Genre = b.Genre;

            b.Title = tTitle;
            b.Artist = tArtist;
            b.Album = tAlbum;
            b.DurationSeconds = tDuration;
            b.Genre = tGenre;
        }
    }
}
