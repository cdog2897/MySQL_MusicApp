using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQL_MusicApp
{
    public partial class Form1 : Form
    {
        BindingSource albumBindingSource = new BindingSource();
        BindingSource trackBindingSource = new BindingSource();

        List<Album> albums = new List<Album>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AlbumsDAO albumsDAO = new AlbumsDAO();

            albumBindingSource.DataSource = albumsDAO.getAllAlbums();
            dataGridView1.DataSource = albumBindingSource;

            pictureBox1.Load("https://upload.wikimedia.org/wikipedia/en/6/60/Coldplay_-_A_Rush_of_Blood_to_the_Head_Cover.png");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AlbumsDAO albumsDAO = new AlbumsDAO();

            albumBindingSource.DataSource = albumsDAO.searchTitles(textBox1.Text);
            dataGridView1.DataSource = albumBindingSource;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;
            int rowClicked = dataGridView.CurrentRow.Index;
            String imageURL = dataGridView.Rows[rowClicked].Cells[4].Value.ToString();

            try
            {
                pictureBox1.Load(imageURL);
            }
            catch 
            {
                pictureBox1.Image = null;
            }
           

            AlbumsDAO albumsDAO = new AlbumsDAO();
            trackBindingSource.DataSource = albumsDAO.getTracksUsingJoin((int)dataGridView.Rows[rowClicked].Cells[0].Value);
            
            dataGridView2.DataSource = trackBindingSource;

        }

        private void btn_addAlbum_Click(object sender, EventArgs e)
        {
            Album album = new Album
            {
                AlbumName = txt_name.Text,
                ArtistName = txt_artist.Text,
                Year = int.Parse(txt_year.Text),
                ImageURL = txt_url.Text,
                Description = txt_description.Text
            };

            AlbumsDAO albumsDAO = new AlbumsDAO();
            int result = albumsDAO.AddOneAlbum(album);
            MessageBox.Show(result + " new row(s) inserted");

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            // get the row number clicked
            int rowClicked = dataGridView.CurrentRow.Index;

            String videoURL = dataGridView.Rows[rowClicked].Cells[3].Value.ToString();

            webView21.Source = new Uri(videoURL);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int rowClicked = dataGridView2.CurrentRow.Index;
            //MessageBox.Show("you clicked row " + rowClicked);
            string trackIDstring = dataGridView2.Rows[rowClicked].Cells[0].Value.ToString();
            int trackID = Int32.Parse(trackIDstring);
            //MessageBox.Show("ID of track: " + trackID);

            AlbumsDAO albumsDAO = new AlbumsDAO();
            int result = albumsDAO.deleteTrack(trackID);
            MessageBox.Show("REsult: " + result);
        }
    }
}
