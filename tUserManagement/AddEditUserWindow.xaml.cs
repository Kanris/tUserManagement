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
using System.Text.RegularExpressions;
using System.Data.Entity;
using UserManagementLibrary;

namespace tUserManagement
{
    /// <summary>
    /// Interaction logic for AddEditUserWindow.xaml
    /// </summary>
    public partial class AddEditUserWindow : Window
    {
        public enum WindowOperation { Add, Edit };
        private User user = null;
        private WindowOperation windowOperation;

        public AddEditUserWindow() : this(WindowOperation.Add)
        { }

        public AddEditUserWindow(WindowOperation windowOperation, User user = null)
        {
            InitializeComponent();
            InitializeWindow(windowOperation, user);

            this.user = user;
            this.windowOperation = windowOperation;
        }

        private void InitializeWindow(WindowOperation windowOperation, User user)
        {
            if (windowOperation.Equals(WindowOperation.Edit))
            {
                btnAddEdit.Content = "Edit";
                btnDelete.Visibility = Visibility.Visible;

                if (!ReferenceEquals(user, null))
                {
                    tbName.Text = user.Name;
                    tbEmail.Text = user.Email;
                    tbAge.Text = user.Age.ToString();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Delete user", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (!ReferenceEquals(user, null))
                {
                    MainWindow.dbContext.Users.Remove(user);
                    MainWindow.dbContext.SaveChanges();

                    this.DialogResult = true;
                    this.Close();
                }
            }
        }

        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            switch(windowOperation)
            {
                case WindowOperation.Add:
                    AddNewUser();
                    break;
                case WindowOperation.Edit:
                    EditUser();
                    break;
            }
        }

        private void AddNewUser()
        {
            var newUserInfo = GetValuesFromTextBlocks();

            if (!ReferenceEquals(newUserInfo, null))
            {
                var newUser = new User() { Name = newUserInfo.Name, Email = newUserInfo.Email, Age = newUserInfo.Age };

                MainWindow.dbContext.Users.Add(newUser);
                CloseWindow();
            }
        }

        private void EditUser()
        {
            var newUserInfo = GetValuesFromTextBlocks();

            if (!ReferenceEquals(newUserInfo, null))
            {
                var result = MainWindow.dbContext.Users.FirstOrDefault(x => x.Id == user.Id);

                if (!ReferenceEquals(result, null))
                {
                    result.Name = newUserInfo.Name;
                    result.Email = newUserInfo.Email;
                    result.Age = newUserInfo.Age;

                    MainWindow.dbContext.Entry(result).State = EntityState.Modified;
                    CloseWindow();
                }
            }
        }

        private void CloseWindow()
        {
            MainWindow.dbContext.SaveChanges();

            this.DialogResult = true;
            this.Close();
        }

        private User GetValuesFromTextBlocks()
        {
            try
            {
                var name = GetName();
                var email = GetEmail();
                var age = GetAge();

                return new User() { Name = name, Email = email, Age = age };
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            return null;
        }

        private string GetName()
        {
            var name = tbName.Text;

            if (String.IsNullOrEmpty(name))
                throw new NullReferenceException("Name can't be empty.");

            if (name.Length < 2)
                throw new Exception("Name length must be greater than two.");

            if (!Regex.IsMatch(name, @"^([A-Za-z]+)$"))
                throw new Exception("Name must contain only letters.");

            return name;
        }

        private string GetEmail()
        {
            var email = tbEmail.Text;

            if (String.IsNullOrEmpty(email))
                throw new NullReferenceException("Email can't be empty.");

            if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                throw new Exception("Invalid email address!");

            return email;
        }

        private int GetAge()
        {
            var age = int.Parse(tbAge.Text);

            if (age < 18)
                throw new Exception("Age can't be less than 18.");

            return age;
        }
    }
}
