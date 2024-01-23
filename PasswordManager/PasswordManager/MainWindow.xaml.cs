using PMUtils;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PasswordManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        public void AddProductsToList(Account[] items)
        {
            Manager_Datagrid_Website.Items.Clear();
            Manager_Datagrid_Website.Items.Refresh();

            foreach (var item in items)
                Manager_Datagrid_Website.Items.Add(new Account(
                    item.Website,
                    item.Name,
                    item.Password));
        }

        PMUtils.Account GetSelectedItem()
        {
            return Manager_Datagrid_Website.SelectedItem as Account;
        }

        Account CreateItem()
        {
            return new Account(
                Edit_TextBox_Website.Text,
                Edit_TextBox_Username.Text,
                Edit_TextBox_Password.Text);
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

            Edit_TextBox_Username.Text = Manager.SelectedItem.Name;
            Edit_TextBox_Website.Text = Manager.SelectedItem.Website;
            Edit_TextBox_Password.Text = Manager.SelectedItem.Password;
            Main_Account.IsSelected = true;
        }

        private void ContextMenuName_Click(object sender, RoutedEventArgs e)
        {
            Manager.SelectedItem = GetSelectedItem();
            Clipboard.SetText(Manager.SelectedItem.Name);
        }

        private void ContextMenuPassword_Click(object sender, RoutedEventArgs e)
        {
            Manager.SelectedItem = GetSelectedItem();
            Clipboard.SetText(Manager.SelectedItem.Password);
        }

        private void Edit_Button_Account_Click(object sender, RoutedEventArgs e)
        {
            if (Manager.EditItem(CreateItem()))
            {
                MessageBox.Show($"{Edit_TextBox_Website.Text} successfully edited!");
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
            if (Manager.CreateItem(new Account(Create_TextBox_Website.Text, Create_TextBox_Username.Text, Create_TextBox_Password.Text)))
            {
                MessageBox.Show($"{Create_TextBox_Website.Text} added to list!");
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
            Create_TextBox_Password.Text = Manager.GeneratePassword(int.Parse(Create_ComboBox_Length.SelectedItem.ToString()!));
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
