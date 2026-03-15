#nullable disable
using Project_2;
using System;

namespace Project_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Playlist playlist = new Playlist();

            Console.WriteLine("MUSIC PLAYLIST MANAGER \n");

            while (true)
            {
                Console.WriteLine("1. Load songs from CSV");
                Console.WriteLine("2. View playlist");
                Console.WriteLine("3. Add a new song");
                Console.WriteLine("4. Delete song by title");
                Console.WriteLine("5. Delete song by position");
                Console.WriteLine("6. Sort by artist");
                Console.WriteLine("7. Sort by duration");
                Console.WriteLine("8. Search a song");
                Console.WriteLine("9. Shuffle playlist");
                Console.WriteLine("10. Play next song"); 
                Console.WriteLine("11. Play previous song");
                Console.WriteLine("12. Show by Genre");
                Console.WriteLine("13. Exit");

                Console.Write("\nChoose one option: ");
                string choice =Console.ReadLine();
                Console.WriteLine();

                if (choice == "1")
                {
                    Console.Write("Please type the CSV path: ");
                    string filePath = Console.ReadLine();
                    playlist.ImportFromCSV(filePath);
                }
                else if (choice ==  "2")
                {
                    playlist.ShowSongs();
                }
                else if (choice == "3")
                {
                    Console.Write("Enter song title: ");
                    string title = Console.ReadLine();

                    Console.Write("Enter artist: ");
                    string artist = Console.ReadLine();

                    Console.Write("Enter album: ");
                    string album = Console.ReadLine();

                    Console.Write("Enter duration (mm:ss OR 'min sec'): ");
                    string duration = Console.ReadLine();
                    int durationSec = ConvertDuration(duration);

                    if (durationSec < 0)
                    {
                        Console.WriteLine("Invalid duration.\n");
                        continue;
                    }

                    Console.Write("Enter genre: ");
                    string genre = Console.ReadLine();

                    playlist.addSongLast(title, artist, album, durationSec, genre);
                }
                else if (choice == "4")
                {
                    Console.Write("Enter title to delete: ");
                    playlist.DeleteByTitle(Console.ReadLine());
                }
                else if (choice == "5")
                {
                    Console.Write("Enter position to delete: ");
                    int number = int.Parse(Console.ReadLine());
                    playlist.DeleteByPosition(number);
                }
                else if (choice == "6")
                {
                    playlist.SortByArtist();
                }
                else if (choice == "7")
                {
                    playlist.SortByDuration();
                }
                else if (choice == "8")
                {
                    Console.Write("Enter title to search: ");
                    playlist.Search(Console.ReadLine());
                }
                else if (choice == "9")
                {
                    playlist.Shuffle();
                }
                else if (choice == "10")
                {
                    playlist.PlayNext();
                }
                else if (choice == "11")
                {
                    playlist.PlayPrevious();
                }

                else if (choice == "12")
                {
                    Console.Write("Enter genre to show by: ");
                    playlist.FilterByGenre(Console.ReadLine());
                }
                else if (choice == "13") 
                {
                     break;
                }
               

            }
        }

        static int ConvertDuration( string input )
        {
            input = input.Trim();

            try
            {
                if (input.Contains (":") )
                {
                    string[] parts = input.Split(':');
                    int minutes = int.Parse(parts[0]);
                    int seconds = int.Parse(parts[1]);
                    return minutes * 60 + seconds;
                }
                else
                {

                    string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length == 2)
                    {
                        int minutes = int.Parse(parts[0]);
                        int seconds = int.Parse(parts[1]);
                        return minutes * 60 + seconds;
                    }
                }
            }
            catch
            {
                return -1;
            }

            return -1;
        }
    }
}

