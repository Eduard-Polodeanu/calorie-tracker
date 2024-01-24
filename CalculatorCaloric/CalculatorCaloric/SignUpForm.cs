using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorCaloric
{
    public partial class SignUpForm : Form
    {
        private LoginForm _loginForm;

        private string _username;
        private string _email;
        private string _password;
        private string _passwordConfirmation;
        private string _invalidFields;

        public SignUpForm()
        {
            InitializeComponent();
        }

        private bool AreAllFieldsFilled()
        {
            if (String.IsNullOrEmpty(_username) ||
                String.IsNullOrEmpty(_email) ||
                String.IsNullOrEmpty(_password) ||
                String.IsNullOrEmpty(_passwordConfirmation))
                return false;
            return true;
        }

        private void readTextboxes()
        {
            _username = textBoxUsername.Text.Trim();
            _email = textBoxEmail.Text.Trim();
            _password = textBoxPassword.Text.Trim();
            _passwordConfirmation = textBoxPasswordConfirmation.Text.Trim();
        }

        private bool IsUsernameValid(string username)
        {
            //min 6 max 24 characters, only alphanumeric and '-' '_'
            if (username.Length < 6 || 
                username.Length > 24 ||
                !Regex.IsMatch(username, @"^[a-zA-Z0-9_-]*$"))
            {
                return false;
            }
            return true;
        }

        private bool IsEmailValid(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        private bool IsPasswordValid(string password)
        {
            if (password.Length <= 8 || password.Length >=24)
            {
                return false;
            }
            return true;
        }

        private bool IsConfirmationValid(string password, string passwordConfirmation)
        {
            if (password == passwordConfirmation)
            {
                return true;
            }
            return false;
        }

        private bool AreAllFieldsValid()
        {
            _invalidFields = "";
            if (!IsUsernameValid(_username))
            {
                _invalidFields += "Username\r\n";
            }
            if (!IsEmailValid(_email))
            {
                _invalidFields += "Email\r\n";
            }
            if (!IsPasswordValid(_password))
            {
                _invalidFields += "Password\r\n";
            }
            if (!IsConfirmationValid(_password, _passwordConfirmation))
            {
                _invalidFields += "Password confirmation\r\n";
            }

            if (String.IsNullOrEmpty(_invalidFields))
            {
                return true;
            }
            return false;
        }

        public static string CheckAvailability(string usernameField, string emailField)
        {
            string unavailableFields = "";
            if (Database.isValueInTable("Users", "Username", usernameField))
            {
                unavailableFields += "Username\r\n";
            }
            if (Database.isValueInTable("Users", "Email", emailField))
            {
                unavailableFields += "Email\r\n";
            }
            return unavailableFields; 
        }

        private void buttonSignUp_Click(object sender, EventArgs e)
        {
            readTextboxes();
            if (AreAllFieldsFilled())
            {
                if (AreAllFieldsValid())
                {
                    this.Hide();
                    if (_loginForm == null)
                    {
                        _loginForm = new LoginForm();
                    }
                    _loginForm.Show();


                    string unavailableFields = CheckAvailability(_username, _email);
                    if (String.IsNullOrEmpty(unavailableFields))
                    {
                        Database.AddNewUserToDB(_username, _password, _email);
                    }
                    else
                    {
                        MessageBox.Show("Urmatoarele campuri au fost utilizate deja:\r\n" + unavailableFields, 
                            "Calorie tracker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    
                }
                else
                {
                    MessageBox.Show("Urmatoarele campuri sunt invalide:\r\n" + _invalidFields, "Calorie tracker", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Completati toate campurile", "Calorie tracker", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (_loginForm == null)
            {
                _loginForm = new LoginForm();
            }
            _loginForm.Show();
        }

        private void SignUpForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)   // ALT+F4 or X
            {
                if (MessageBox.Show("Are you sure want to exit?", "Calorie tracker",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                    == DialogResult.OK)
                {
                    Environment.Exit(0);    // exit with no error code
                }
                else
                {
                    e.Cancel = true;

                }
            }
        }
    }
}
