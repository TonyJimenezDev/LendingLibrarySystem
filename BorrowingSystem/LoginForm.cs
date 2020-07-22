using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BorrowingSystem
{
    public partial class Login : Form
    {
        /// <summary>
        /// Login page: Connects to your database with Username and password as validations.
        /// Need to create a SQL database with a table named LoginTable and 3 columns id = int primary, username - varchar() not null, pass - varchar()
        /// Add your connection string into the app.config file.
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }

        // Allows Form Movement
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }
            base.WndProc(ref m);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // Make sure to set focus to title first
            ActiveControl = titleOfSystem_Label;
            titleOfSystem_Label.Focus();
        }


        #region Ui user interactions
        private void Login_Btn_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection();
            
            connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["loginDatabase"];
            SqlCommand command = new SqlCommand();
            command.Connection = connection;

            command.CommandText = $"Select * From LoginTable where Username = '{username_TB.Text}' AND pass = '{password_TB.Text}'";
            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            // Correct Credetials
            if(dataSet.Tables[0].Rows.Count != 0)
            {
                Program.UserSuccessfullyAuthenticated = true;
                Close();
            }
            else // Incorrect Credentials
            {
                MessageBox.Show("Wrong username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                password_TB.Clear();
                password_TB.Focus();
            }
        }

        private void Username_TB_Enter(object sender, EventArgs e)
        {
            if (username_TB.Text == "Username") username_TB.Clear();
        }

        private void Password_TB_Enter(object sender, EventArgs e)
        {
            if (password_TB.Text == "Password") password_TB.Clear();
            password_TB.PasswordChar = '*';
        }

        private void username_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) Login_Btn_Click(this, new EventArgs());
        }

        private void password_TB_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) Login_Btn_Click(this, new EventArgs());
        }

        private void Icon_label_Click(object sender, EventArgs e) => Process.Start("https://icons8.com");
        private void Close_Btn_Click(object sender, EventArgs e) => Application.Exit();
        private void Minimize_Btn_Click(object sender, EventArgs e) => WindowState = FormWindowState.Minimized;

        #endregion

        

    }
}
