using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace GroceryApp
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainMenu : Window
	{
		private int UserId;

		private List<GroceryItem> ItemsList = [];

		public void LoadItems()
		{
			ItemsList.Clear();

			try
			{
				using var v_cmd = DbConnection.Command(@"
					SELECT itm.id, itm.item_category, img.image_path, itm.item_name, itm.item_desc, itm.item_detail, itm.price::numeric FROM grocery_item itm
					LEFT JOIN grocery_image img ON itm.item_image = img.id
				");

				using var v_reader = v_cmd.ExecuteReader();

				while (v_reader.Read())
				{
					int v_id = v_reader.GetInt32(0);
					int v_categoryId = v_reader.GetInt32(1);
					string v_imagePath = v_reader.IsDBNull(2) ? "" : v_reader.GetString(2);
					string v_itemName = v_reader.GetString(3);
					string v_itemDesc = v_reader.GetString(4);
					string v_itemDetail = v_reader.GetString(5);
					float v_price = v_reader.GetFloat(6);

					ItemsList.Add(new GroceryItem(v_id, v_categoryId, v_imagePath, v_itemName, v_itemDesc, v_itemDetail, v_price));
				}
			}
			catch { }
		}

		public MainMenu(int userId)
		{
			UserId = userId;
			LoadItems();

			InitializeComponent();
			ItemListView.ItemsSource = ItemsList;
		}

		private void UpdateVisibility(Panel panel, bool condition, Action<int>? callback)
		{
			panel.Visibility = condition ? Visibility.Visible : Visibility.Hidden;

			if (condition && callback != null)
				callback(0);
		}

		private void SwitchTab(int tabIdx)
		{
			UpdateVisibility(StoreTabGrid   , tabIdx == 0, (_) => LoadItems());
			UpdateVisibility(ExploreTabGrid , tabIdx == 1, null);
			UpdateVisibility(CartTabGrid    , tabIdx == 2, null);
			UpdateVisibility(FavoriteTabGrid, tabIdx == 3, null);
			UpdateVisibility(AccountTabGrid , tabIdx == 4, (_) => OnAccountTabLoaded());
		}

		private void OnAccountTabLoaded()
		{
			AccountTabAccountName.Text = DbConnection.GetAccountName(UserId);
		}

		private void StoreTabBtn_Click(object sender, RoutedEventArgs e) => SwitchTab(0);

		private void ExploreTabBtn_Click(object sender, RoutedEventArgs e) => SwitchTab(1);

		private void CartTabBtn_Click(object sender, RoutedEventArgs e) => SwitchTab(2);

		private void FavoriteTabBtn_Click(object sender, RoutedEventArgs e) => SwitchTab(3);

		private void AccountTabBtn_Click(object sender, RoutedEventArgs e) => SwitchTab(4);

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			var v_item = (sender as FrameworkElement)!.DataContext;
			int v_index = ItemListView.Items.IndexOf(v_item);

			var v_groceryWindow = new GroceryItemWindow(ItemsList[v_index]);
			v_groceryWindow.Owner = this;
			v_groceryWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
			v_groceryWindow.ShowDialog();
		}
	}
}