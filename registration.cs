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
    public partial class registration : Form
    {
        private readonly string connString = @"Data Source=DBD.db;Version=3;";
        public registration()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Получаем логин и пароль из текстовых полей и удаляем пробелы в начале и конце строк
            string LOGIN = textBox1.Text.Trim();
            string PASS = textBox2.Text.Trim();

            // Проверяем, что поля логина и пароля не пустые
            if (LOGIN == "" || PASS == "")
            {
                _ = MessageBox.Show("Заполните все поля");
                return;
            }

            // Формируем запрос на проверку наличия пользователя с таким же логином в базе данных
            string checkQuery = $"SELECT COUNT(*) FROM Users WHERE LOGIN='{LOGIN}'";

            // Создаем новое подключение к базе данных SQLite
            using (SQLiteConnection conn = new SQLiteConnection(connString))
            {
                // Открываем соединение с базой данных
                conn.Open();

                // Создаем новый объект команды SQL с запросом на проверку наличия пользователя с таким же логином в базе данных
                using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, conn))
                {
                    // Получаем результат выполнения запроса на проверку наличия пользователя с таким же логином в базе данных
                    int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                    // Проверяем, что пользователь с таким логином уже не зарегистрирован в базе данных
                    if (count > 0)
                    {
                        _ = MessageBox.Show("Пользователь с таким логином уже зарегистрирован");
                        return;
                    }
                }

                // Формируем запрос на добавление нового пользователя в базу данных
                string insertQuery = $"INSERT INTO Users (LOGIN, PASS) VALUES ('{LOGIN}', '{PASS}');";

                // Создаем новый объект команды SQL с запросом на добавление нового пользователя в базу данных
                using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, conn))
                {
                    // Выполняем запрос на добавление нового пользователя в базу данных
                    _ = insertCmd.ExecuteNonQuery();
                }
            }

            // Выводим сообщение об успешной регистрации пользователя
            _ = MessageBox.Show("Вы успешно зарегистрировались");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Authentication fMain = new Authentication();
            fMain.Show();
            fMain.FormClosed += new FormClosedEventHandler(registration_FormClosed);
            Hide();
        }

        private void registration_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
