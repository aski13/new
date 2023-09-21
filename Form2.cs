using System;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace pirioga
{
    public partial class Form2 : Form
    {
        private readonly string connString = @"Data Source=DBD.db;Version=3;";


        public Form2()
        {
            InitializeComponent();
            FillDataGridView();
            FillComboBox();
        }

        private void FillDataGridView()
        {
            string query = "SELECT * FROM Image";
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable dtFines = new DataTable();
                _ = adapter.Fill(dtFines);

                // добавляем столбы для datagridview, для их отображения
                dataGridView2.DataSource = dtFines;
                dataGridView2.Columns["ID"].Visible = false; // Прячем эту колонку
                dataGridView2.Columns["FIO"].HeaderText = "ФИО";
                dataGridView2.Columns["FOTO"].HeaderText = "Фото";
                dataGridView2.Columns["FOTO2"].HeaderText = "Фото 2";
            }
        }

        private void FillComboBox()
        {
            string query = "SELECT ФИО FROM Люди";
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable dtPeople = new DataTable();
                adapter.Fill(dtPeople);

                comboBox1.DisplayMember = "ФИО"; // Отображаемое значение - колонка "ФИО"
                comboBox1.DataSource = dtPeople;
            }
        }

        private void btnFoto1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";
            openFileDialog.Title = "Выберите изображение";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;
                pictureBox1.Image = Image.FromFile(selectedImagePath);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            // Получение выбранного ФИО из ComboBox
            string selectedFIO = comboBox1.Text;

            // Получение выбранного изображения из PictureBox
            Image selectedImage = pictureBox1.Image;

            Image selectedImages = pictureBox2.Image;
            // Конвертация изображения в массив байтов

            byte[] imageBytes = ImageToByteArray(selectedImage);
            byte[] imageBytess = ImageToByteArray(selectedImages);




            // Создание подключения к базе данных SQLite
            using (var connection = new SQLiteConnection("Data Source=DBD.db; Version=3"))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    // Создание SQL-запроса для вставки записи в таблицу "Image"
                    command.CommandText = "INSERT INTO Image (FIO, FOTO, FOTO2) VALUES (@FIO, @FOTO, @FOTO2)";

                    // Добавление параметров в SQL-запрос
                    command.Parameters.AddWithValue("@FIO", selectedFIO);
                    command.Parameters.AddWithValue("@FOTO", imageBytes);
                    command.Parameters.AddWithValue("@FOTO2", imageBytess);


                    // Выполнение SQL-запроса
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            // Очистка PictureBox и ComboBox после сохранения
            pictureBox1.Image = null;
            pictureBox2.Image = null;
            comboBox1.SelectedIndex = -1;
        }
        private byte[] ImageToByteArray(Image image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }

        }


        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверка, что клик произошел по столбцу "ФОТО" или "ФОТО2"
            if ((e.ColumnIndex == dataGridView2.Columns["FOTO"].Index || e.ColumnIndex == dataGridView2.Columns["FOTO2"].Index) && e.RowIndex >= 0)
            {
                int personID = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["ID"].Value);

                // Получение объекта Person из базы данных
                Person person = GetPersonFromDatabase(personID);

                // Создание экземпляра второй формы
                PIC PIC = new PIC();

                // Установка картинки в PictureBox второй формы
                if (e.ColumnIndex == dataGridView2.Columns["FOTO"].Index)
                {
                    PIC.SetImage(person.Photo);
                }
                else if (e.ColumnIndex == dataGridView2.Columns["FOTO2"].Index)
                {
                    PIC.SetImage(person.Photo2);
                }

                // Открытие второй формы
                PIC.Show();
            }
        }


        private Person GetPersonFromDatabase(int personID)
        {
            Person person = null;

            using (SQLiteConnection connection = new SQLiteConnection("Data Source=DBD.db;Version=3;"))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM Image WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", personID);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            person = new Person
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Name = reader["FIO"].ToString(),
                                Photo = (byte[])reader["FOTO"],
                                Photo2 = (byte[])reader["FOTO2"],
                            };
                        }
                    }
                }
            }

            return person;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM Image";
            using (SQLiteConnection connection = new SQLiteConnection(connString))
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection);
                DataTable dtFines = new DataTable();
                _ = adapter.Fill(dtFines);

                // добавляем столбы для datagridview, для их отображения
                dataGridView2.DataSource = dtFines;
                dataGridView2.Columns["ID"].Visible = false; // Прячем эту колонку
                dataGridView2.Columns["FIO"].HeaderText = "ФИО";
                dataGridView2.Columns["FOTO"].HeaderText = "Фото";
                dataGridView2.Columns["FOTO2"].HeaderText = "Фото 2";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.gif; *.bmp)|*.png; *.jpg; *.jpeg; *.gif; *.bmp";
            openFileDialog.Title = "Выберите изображение";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;
                pictureBox2.Image = Image.FromFile(selectedImagePath);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            // Создаем подключение к баз
            SQLiteConnection connection = new SQLiteConnection(connString);

            // Создаем команду для выборки данных из таблицы Image
            string query = "SELECT FOTO, FOTO2 FROM Image WHERE FIO = @fio";
            SQLiteCommand command = new SQLiteCommand(query, connection);

            // Добавляем параметр для команды выборки
            command.Parameters.AddWithValue("@fio", comboBox1.SelectedItem.ToString());

            // Открываем подключение к базе данных
            connection.Open();

            // Выполняем команду выборки и получаем результат
            SQLiteDataReader reader = command.ExecuteReader();

            // Если данные были найдены, то отображаем их в PictureBox
            if (reader.Read())
            {
                // Получаем данные из поля FOTO и отображаем их в PictureBox1
                byte[] fotoData = (byte[])reader["FOTO"];
                if (fotoData != null)
                {
                    using (MemoryStream ms = new MemoryStream(fotoData))
                    {
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                }

                // Получаем данные из поля FOTO2 и отображаем их в PictureBox2
                byte[] foto2Data = (byte[])reader["FOTO2"];
                if (foto2Data != null)
                {
                    using (MemoryStream ms = new MemoryStream(foto2Data))
                    {
                        pictureBox2.Image = Image.FromStream(ms);
                    }
                }
            }

            // Закрываем подключение к базе данных
            connection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
