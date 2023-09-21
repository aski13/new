using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pirioga
{
    public partial class PIC : Form
    {
        public PIC()
        {
            InitializeComponent();
        }
        public void SetImage(byte[] imageBytes)
        {
            // Преобразование массива байтов в изображение
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                Image image = Image.FromStream(ms);

                // Установка изображения в PictureBox
                pictureBox2.Image = image;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
