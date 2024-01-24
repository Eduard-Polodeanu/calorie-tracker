using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorCaloric
{
    public partial class LoginForm : Form
    {

        private MainForm _mainForm;
        private SignUpForm _signUpForm;
        
        public LoginForm()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUser.Text.Trim();
            string password = textBoxPass.Text.Trim();

            if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
            {
                bool loggedIn = Database.CheckCredentials(username, password);

                if (loggedIn)
                {
                    this.Hide();
                    _mainForm = new MainForm();
                    _mainForm.Show();
                    
                }
                else 
                {
                    MessageBox.Show("Nume sau parola incorecta.", "Calorie tracker", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Nume sau parola invalida.", "Calorie tracker", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }

        private void linkLabelCreateAccount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_signUpForm == null)
            {
                _signUpForm = new SignUpForm();
            }
            this.Hide();
            _signUpForm.Show();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {   
            if (e.CloseReason == CloseReason.UserClosing)   // ALT+F4 or X
            {
                if (MessageBox.Show("Are you sure want to exit?", "Calorie tracker",
                                MessageBoxButtons.OKCancel, MessageBoxIcon.Information)
                    == DialogResult.OK)
                {
                    Environment.Exit(0);    // exit no error
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }
    }
}
