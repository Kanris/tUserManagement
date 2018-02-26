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
        public enum WindowOperation { Add, Edit }; //window type
        private User user = null; //user info
        private WindowOperation windowOperation; //current window type

        public AddEditUserWindow() : this(WindowOperation.Add) //add window
        { }

        public AddEditUserWindow(WindowOperation windowOperation, User user = null) //edit window
        {
            InitializeComponent();
            InitializeWindow(windowOperation, user); //fill window with user information (if edit window)

            this.user = user; //save user information
            this.windowOperation = windowOperation; //save current operation
        }

        //fill window with user information
        private void InitializeWindow(WindowOperation windowOperation, User user)
        {
            if (windowOperation.Equals(WindowOperation.Edit)) //if window type is Edit
            {
                btnAddEdit.Content = "Edit"; //change AddEditButton Content
                btnDelete.Visibility = Visibility.Visible; //show delete button

                //save user information
                if (!ReferenceEquals(user, null)) 
                {
                    tbName.Text = user.Name;
                    tbEmail.Text = user.Email;
                    tbAge.Text = user.Age.ToString();
                }
            }
        }

        //delete button clicked
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            //show alert box
            if (MessageBox.Show("Are you sure?", "Delete user", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                //if user information is not null
                if (!ReferenceEquals(user, null))
                {
                    MainWindow.dbContext.Users.Remove(user); //remove user information from db
                    CloseWindow(); //save changes to db and close this window
                }
            }
        }

        //btn add/edit clicked
        private void btnAddEdit_Click(object sender, RoutedEventArgs e)
        {
            //depending on window type
            switch(windowOperation)
            {
                case WindowOperation.Add: //window type is add
                    AddNewUser(); //add new user to db
                    break;
                case WindowOperation.Edit: //window type is edit
                    EditUser(); //edit user in db
                    break;
            }
        }

        //add new user
        private void AddNewUser()
        {
            //get values from textboxes
            var newUserInfo = GetValuesFromTextBlocks();

            //if user information is not null
            if (!ReferenceEquals(newUserInfo, null))
            {
                var newUser = new User() { Name = newUserInfo.Name, Email = newUserInfo.Email, Age = newUserInfo.Age };

                MainWindow.dbContext.Users.Add(newUser); //add user information to db
                CloseWindow(); //save changes to db and close this window
            }
        }

        //edit user information
        private void EditUser()
        {
            //get values from textboxes
            var newUserInfo = GetValuesFromTextBlocks();

            //if user information is not null
            if (!ReferenceEquals(newUserInfo, null))
            {
                //find user in db
                var result = MainWindow.dbContext.Users.FirstOrDefault(x => x.Id == user.Id);

                //if founded user information is not null
                if (!ReferenceEquals(result, null))
                {
                    //change values in founded user
                    result.Name = newUserInfo.Name;
                    result.Email = newUserInfo.Email;
                    result.Age = newUserInfo.Age;

                    MainWindow.dbContext.Entry(result).State = EntityState.Modified; //inform collection that item has been changed
                    CloseWindow(); //save changes to db and close this window
                }
            }
        }

        //close this window and save information
        private void CloseWindow()
        {
            //save user info to db
            MainWindow.dbContext.SaveChanges(); 

            this.DialogResult = true;
            this.Close(); //close this window
        }

        //get values from text blocks
        private User GetValuesFromTextBlocks()
        {
            try
            {
                var name = GetName(); //get name value
                var email = GetEmail(); //get email value
                var age = GetAge(); //get age value

                return new User() { Name = name, Email = email, Age = age }; // create new user
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            return null;
        }

        //get name value
        private string GetName()
        {
            var name = tbName.Text; //get name value

            if (String.IsNullOrEmpty(name)) //if name is empty
                throw new NullReferenceException("Name can't be empty.");

            if (name.Length < 2) //if name length is less than two
                throw new Exception("Name length must be greater than two.");

            if (!Regex.IsMatch(name, @"^([A-Za-z]+)$")) //if name contains not only letters
                throw new Exception("Name must contain only letters.");

            return name;
        }

        //get email value
        private string GetEmail()
        {
            var email = tbEmail.Text; //get email value

            if (String.IsNullOrEmpty(email)) //if email is empty
                throw new NullReferenceException("Email can't be empty.");

            if (!Regex.IsMatch(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")) //if email address is not valid
                throw new Exception("Invalid email address!");

            return email;
        }

        //get age value
        private int GetAge()
        {
            var age = int.Parse(tbAge.Text); //get age value

            if (age < 18) //if age value is less than 18
                throw new Exception("Age can't be less than 18.");

            return age;
        }
    }
}
