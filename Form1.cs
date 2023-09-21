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

namespace pirioga
{
    public partial class Form1 : Form
    {
        private readonly string connString = @"Data Source=DBD.db;Version=3;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            string query = "SELECT * FROM Люди ";
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable dtFines = new DataTable();
                _ = adapter.Fill(dtFines);

                // добавляем столбы для datagridview, для их отображения
                dataGridView1.DataSource = dtFines;
                dataGridView1.Columns["ID"].Visible = false; // Прячем эту колонку
                dataGridView1.Columns["ФИО"].HeaderText = "ФИО";
                dataGridView1.Columns["ОПИСАНИЕ"].HeaderText = "Описание";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 fMain = new Form2();
            fMain.Show();
            fMain.FormClosed += new FormClosedEventHandler(Form1_FormClosed);
            Hide();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
