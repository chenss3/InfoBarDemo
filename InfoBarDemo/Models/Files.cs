using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoBarDemo.Models
{
    public class File
    {
        public string Name { get; set; }
        public string Date { get; set; }
        public string Thumbnail { get; set; }
    }

    public class FileManager
    {
        public static List<File> GetFiles()
        {
            var files = new List<File>();
            files.Add(new File { Name = "Documents", Date = "4h ago", Thumbnail = "Assets/Folder.png" });
            files.Add(new File { Name = "Design Research", Date = "2h ago", Thumbnail = "Assets/Folder.png" });
            files.Add(new File { Name = "Early Modern", Date = "Last week", Thumbnail = "Assets/Folder.png" });
            files.Add(new File { Name = "Starry Night.jpeg", Date = "June 14, 2020", Thumbnail = "Assets/Starrynight.jpg" });
            files.Add(new File { Name = "Water Lillies.jpeg", Date = "May 23, 2020", Thumbnail = "Assets/Monet.jpg" });
            files.Add(new File { Name = "La Grande Jatte.jpeg", Date = "April 2, 2020", Thumbnail = "Assets/garden.jpg" });
            files.Add(new File { Name = "Wendell.jpeg", Date = "May 21, 2020", Thumbnail = "Assets/WendellWaterColor.jpg" });
            return files;
        }
    }
}
