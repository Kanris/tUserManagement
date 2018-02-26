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
using UserManagementLibrary;
using System.Data.Entity;

namespace tUserManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string dbName = "CodeFirstDB";
        public static UserManagementContext dbContext { private set; get; }

        public MainWindow()
        {
            InitializeComponent();

            dbContext = new UserManagementContext(dbName);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataFromDB();
        }

        private void LoadDataFromDB()
        {
            if (!ReferenceEquals(dbContext, null))
            {
                dbContext.Users.Load();
                dgUsers.ItemsSource = dbContext.Users.Local;
            }
        }

        private void miAddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddEditUserWindow();
            if (addUserWindow.ShowDialog() == true)
            {
                dgUsers.Items.Refresh();
            }
        }

        private void dgUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedUser = dgUsers.SelectedItem as User;

            if (!ReferenceEquals(selectedUser, null))
            {
                var editUserWindow = new AddEditUserWindow
                    (AddEditUserWindow.WindowOperation.Edit, selectedUser);

                if (editUserWindow.ShowDialog() == true)
                {
                    dgUsers.Items.Refresh();
                }
            }
        }
    }
}
