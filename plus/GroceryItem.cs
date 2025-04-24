using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GroceryApp
{
    public class GroceryCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public BitmapImage Image { get; set; }
        public System.Windows.Media.Color PanelColor { get; set; }

        public GroceryCategory(int id, string name, Uri imgPath, System.Windows.Media.Color color)
        {
            Id = id;
            Name = name;
            Image = new BitmapImage(imgPath);
            PanelColor = color;
        }
    }

    public class GroceryItem
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public BitmapImage Image { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public float Price { get; set; }

        public GroceryItem(int id, int categoryId, string imagePath, string name, string desc, string detail, float price)
        {
            Id = id;
            CategoryId = categoryId;

            if (imagePath == "")
                Image = new BitmapImage(new Uri("pack://application:,,,/Materials/MainMenuCarrot.png"));
            else
                Image = new BitmapImage(new Uri($"pack://application:,,,{imagePath}"));

            Name = name;
            Detail = detail;
            Description = desc;
            Price = price;
        }
    }
}
