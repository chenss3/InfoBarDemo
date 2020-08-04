using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBarDemo.Models
{
    public class FavoritesFiles
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Thumbnail { get; set; }
    }

    public class FavoritesFileManager
    {
        public static List<FavoritesFiles> GetFavoritesFiles()
        {
            var files = new List<FavoritesFiles>();
            files.Add(new FavoritesFiles { Name = "Microsoft.jpeg", Date = "January 4, 2029", Thumbnail = "Assets/MicrosoftBldg.jpg" });
            files.Add(new FavoritesFiles { Name = "Seahawks.jpeg", Date = "1h ago", Thumbnail = "Assets/SeattleSeahawk.jpg" });
            files.Add(new FavoritesFiles { Name = "Ivars.jpeg", Date = "Last week", Thumbnail = "Assets/Ivars.jpg" });
            files.Add(new FavoritesFiles { Name = "UW.jpeg", Date = "April 2, 2020", Thumbnail = "Assets/UDub.jpg" });
            files.Add(new FavoritesFiles { Name = "MtRainier.jpeg", Date = "May 23, 2020", Thumbnail = "Assets/MtRainier.jpg" });
            return files;
        }
    }
}
