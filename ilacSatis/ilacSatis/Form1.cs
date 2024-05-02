using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ilacSatis
{
    public partial class Form1 : Form
    {
        MySqlConnection baglanti = new MySqlConnection("server = localhost; uid = root; database = eczane");
        MySqlCommand komut;
        MySqlDataAdapter adaptor;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToLongDateString();
            label6.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
            listele();

        }

        private void listele()
        {
            adaptor = new MySqlDataAdapter("Select * From ilactablo",baglanti);
            DataTable tablo = new DataTable();
            adaptor.Fill(tablo);
            dataGridView1.DataSource = tablo;
            tbilacAdi.Text = "";
            tbİlacKodu.Text = "";
            tbMiktar.Text = "";
            tbURL.Text = "";
            pictureBox1.Image = null;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            komut = new MySqlCommand("Insert Into ilactablo (ilac_adi, ilac_kodu, stok, resim) values ('" + tbilacAdi.Text + "', '" + int.Parse(tbİlacKodu.Text) + "', '" + int.Parse(tbMiktar.Text) + "', @resim)", baglanti);
            baglanti.Open();
            komut.Parameters.AddWithValue("resim",resim.Text);
            komut.ExecuteNonQuery();
            baglanti.Clone();
            listele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string urlresim = tbURL.Text;
            pictureBox1.Load(urlresim);
            resim.Text = tbURL.Text;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tbilacAdi.Text= dataGridView1.CurrentRow.Cells[1].Value.ToString();
            tbİlacKodu.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            tbMiktar.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            tbURL.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();

            if (dataGridView1.CurrentRow.Cells[4].Value.ToString().Contains("http"))
            {
                pictureBox1.Load(dataGridView1.CurrentRow.Cells[4].Value.ToString());
            }
            else
            {
                pictureBox1.ImageLocation = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Filter = "Resim Dosyaları |* .jpg; * .jpeg; * .png";
            fd.ShowDialog();
            pictureBox1.ImageLocation= fd.FileName;
            resim.Text = fd.FileName.ToString();
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            komut = new MySqlCommand("Delete from ilactablo", baglanti);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            komut = new MySqlCommand("Delete from ilactablo where ilac_adi = '"+ dataGridView1.CurrentRow.Cells[1].Value.ToString() + "'",baglanti);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            listele();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label5.Text = DateTime.Now.ToLongDateString();
            label6.Text = DateTime.Now.ToLongTimeString();
        }

        private void btnSatis_Click(object sender, EventArgs e)
        {
            int gecici = int.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
            gecici = gecici - 1;
            komut = new MySqlCommand("Update ilactablo set stok = '"+gecici+"' where ilac_adi = '"+ dataGridView1.CurrentRow.Cells[1].Value.ToString() + "'", baglanti);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();  
            listele();
        }
    }
}
