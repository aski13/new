using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace pirioga
{
    public partial class Authentication : Form
    {
        private readonly string connString = @"Data Source=DBD.db;Version=3;";
        public Authentication()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM Users WHERE LOGIN=@LOGIN AND PASS=@PASS";

            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                {
                    _ = command.Parameters.AddWithValue("@LOGIN", textBox1.Text);
                    _ = command.Parameters.AddWithValue("@PASS", textBox2.Text);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                                Form1 fMain = new Form1();
                                fMain.Show();
                                fMain.FormClosed += new FormClosedEventHandler(Authentication_FormClosed);
                                Hide();
                            
                        }
                        else
                        {
                            MessageBox.Show("Неверный логин или пароль");
                        }
                    }
                }
            }
        }

        private void Authentication_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            registration fMain = new registration();
            fMain.Show();
            fMain.FormClosed += new FormClosedEventHandler(Authentication_FormClosed);
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
