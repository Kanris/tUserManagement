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
        private static string dbName = "CodeFirstDB"; //db connection string
        public static UserManagementContext dbContext { private set; get; } //db context

        public MainWindow()
        {
            InitializeComponent();

            dbContext = new UserManagementContext(dbName); //connect to db
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataFromDB(); //fill datagrid with users information
        }

        //load users information from database
        private void LoadDataFromDB()
        {
            if (!ReferenceEquals(dbContext, null)) //if db connection established
            {
                dbContext.Users.Load(); //load info from db
                dgUsers.ItemsSource = dbContext.Users.Local; //connect datagrid with db
            }
        }

        //if menu item - Add clicked
        private void miAddUser_Click(object sender, RoutedEventArgs e)
        {
            var addUserWindow = new AddEditUserWindow(); 
            if (addUserWindow.ShowDialog() == true) //open AddEditUserWindow (add mode)
            {
                dgUsers.Items.Refresh(); //update datagrid 
            }
        }

        //if double clicked on datagrid
        private void dgUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedUser = dgUsers.SelectedItem as User; //get selected row

            if (!ReferenceEquals(selectedUser, null))
            {
                var editUserWindow = new AddEditUserWindow
                    (AddEditUserWindow.WindowOperation.Edit, selectedUser);

                if (editUserWindow.ShowDialog() == true) //open AddEditUserWindow (edit mode)
                {
                    dgUsers.Items.Refresh();//update datagrid 
                }
            }
        }
    }
}
