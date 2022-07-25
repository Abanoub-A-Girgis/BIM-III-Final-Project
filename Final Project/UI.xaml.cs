using Autodesk.Revit.DB;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Export_UI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UI : Window
    {
        public UI()
        {
            InitializeComponent();
        }
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }
        string path;
        private void SelectFolder(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                path = dialog.SelectedPath;
            }
        }
        bool beams, roofs, columns, walls, doors, windows, floors;
        private void Beams_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                beams = true;
            }
            else
            {
                beams = false;
            }
        }

        private void Roofs_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                roofs = true;
            }
            else
            {
                roofs = false;
            }
        }

        private void Columns_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                columns = true;
            }
            else
            {
                columns = false;
            }
        }

        private void Walls_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                walls = true;
            }
            else
            {
                walls = false;
            }
        }

        private void Doors_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                doors = true;
            }
            else
            {
                doors = false;
            }
        }

        private void Windows_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                windows = true;
            }
            else
            {
                windows = false;
            }
        }

        private void Floors_Checked(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                floors = true;
            }
            else
            {
                floors = false;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Final_Project.Helpers.setPath(path);
            bool[] boolList = {beams, roofs, columns, walls, doors, windows, floors};
            Final_Project.Helpers.setBool(boolList);
            Close();
        }
    }
}