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

namespace PasswordManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public void AddProductsToList(Item[] items)
        {
            Manager_Datagrid_Website.Items.Clear();
            Manager_Datagrid_Website.Items.Refresh();

            foreach (var item in items)
                Manager_Datagrid_Website.Items.Add(item);
        }

        Item GetSelectedItem()
        {
            return (Item)Manager_Datagrid_Website.SelectedItem;
        }

        Item CreateItem()
        {
            return new Item
            {
                WEBSITE = Edit_TextBox_Website.Text,
                NAME = Edit_TextBox_Username.Text,
                PASSWORD = Edit_TextBox_Password.Text
            };
        }


        #region UI
        private void Window_Initialized(object sender, EventArgs e)
        {
            for (int i = 6; i < 30 + 1; i++)
                Create_ComboBox_Length.Items.Add(i.ToString());
            Create_ComboBox_Length.SelectedIndex = 2;

            if (Manager.Startup())
                AddProductsToList(Manager.Sort(null));
            else
                MessageBox.Show("Failed to load data!");
        }

        private void ContextMenuEdit_Click(object sender, RoutedEventArgs e)
        {
            Manager.SelectedItem = GetSelectedItem();

            Edit_TextBox_Username.Text = Manager.SelectedItem.NAME;
            Edit_TextBox_Website.Text = Manager.SelectedItem.WEBSITE;
            Edit_TextBox_Password.Text = Manager.SelectedItem.PASSWORD;
            Main_Account.IsSelected = true;
        }

        private void ContextMenuName_Click(object sender, RoutedEventArgs e)
        {
            Manager.SelectedItem = GetSelectedItem();
            Clipboard.SetText(Manager.SelectedItem.NAME);
        }

        private void ContextMenuPassword_Click(object sender, RoutedEventArgs e)
        {
            Manager.SelectedItem = GetSelectedItem();
            Clipboard.SetText(Manager.SelectedItem.PASSWORD);
        }

        private void Edit_Button_Account_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.EditItem(CreateItem()))
            {
                Edit_TextBox_Website.Text = "";
                Edit_TextBox_Username.Text = "";
                Edit_TextBox_Password.Text = "";

                Window_Initialized(null, null);
                Main_Manager.IsSelected = true;
            }
            else
                MessageBox.Show("Failed to edit account!\nCheck your input.");
        }

        private void Create_Button_Account_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.CreateItem(new Item { NAME = Create_TextBox_Username.Text, WEBSITE = Create_TextBox_Website.Text, PASSWORD = Create_TextBox_Password.Text }))
            {
                Create_TextBox_Website.Text = "";
                Create_TextBox_Username.Text = "";
                Create_TextBox_Password.Text = "";

                Window_Initialized(null, null);
                Main_Manager.IsSelected = true;
            }
            else
                MessageBox.Show("Failed to add account!\nCheck your input.");
        }

        private void Manager_TextBox_Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            AddProductsToList(Manager.Sort(Manager_TextBox_Search.Text));
        }

        private void Create_Button_Password_Click(object sender, RoutedEventArgs e)
        {
            Create_TextBox_Password.Text = Manager.GeneratePassword(int.Parse(Create_ComboBox_Length.SelectedItem.ToString()));
        }

        private void ContextMenuDelete_Click(object sender, RoutedEventArgs e)
        {
            Manager.SelectedItem = GetSelectedItem();
            Manager.DeleteItem();
            Window_Initialized(null, null);
        }
        #endregion
    }
}
