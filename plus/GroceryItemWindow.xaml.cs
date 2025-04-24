using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Npgsql;

namespace GroceryApp
{
	public partial class GroceryItemWindow : Window
	{
		public GroceryItem SavedItem { get; set; }

		public GroceryItemWindow(GroceryItem Item)
		{
			SavedItem = Item;
			DataContext = SavedItem;

			InitializeComponent();
		}

		private void AddingValue(object sender, RoutedEventArgs e)
		{
			GroceryItemCounter.Text = (int.Parse(GroceryItemCounter.Text) + 1).ToString();
		}

		private void RemoveValue(object sender, RoutedEventArgs e)
		{
			if (int.Parse(GroceryItemCounter.Text) > 1)
			GroceryItemCounter.Text = (int.Parse(GroceryItemCounter.Text) - 1).ToString();
        }
	}
}