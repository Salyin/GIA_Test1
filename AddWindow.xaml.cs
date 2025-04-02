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

namespace GiaTest2
{
    /// <summary>
    /// Логика взаимодействия для AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window
    {
        public event EventHandler OnClosed;
        public bool IsAlterd = false;
        public AddWindow()
        {
            InitializeComponent();
        }

        private void SaveClick(object obj, RoutedEventArgs e)
        {
            IsAlterd = true;
            this.Close();
        }
        private void DenyClick(object obj, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
