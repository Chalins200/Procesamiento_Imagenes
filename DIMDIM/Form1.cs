using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Emgu.CV;
using System.Drawing;
using Emgu.CV.Structure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.CvEnum;

namespace DIMDIM
{
    public partial class Form1 : Form
    {
        Image<Bgr, Byte> img1;
        Image<Gray, Byte> img2;
        Image<Gray, Byte> image;
        
        double fps;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            if (open.ShowDialog()==DialogResult.OK)
            {
                img1 = new Image<Bgr, Byte>(open.FileName);
                imageBox1.Image = img1;
            }
        }

        private void transformarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void abrirImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            if (open.ShowDialog() == DialogResult.OK)
            {
                img1 = new Image<Bgr, Byte>(open.FileName);
                image = new Image<Gray, Byte>(open.FileName);
                imageBox1.Image = img1;
            }
        }

        private void binariaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            img2 = img1.Convert<Gray, Byte>();
            img2 = img2.ThresholdBinary(new Gray(80), new Gray(255));
            imageBox1.Image = img2;
        }

        private void guardarImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog();
            guardar.Filter = "Image Files (*.tif; *.dcm; *.jpg; *.jpeg; *.bmp)|*.tif; *.dcm; *.jpg; *.jpeg; *.bmp";
            if (guardar.ShowDialog() == DialogResult.OK)
            {
                imageBox2.Image = img2;
                img2.Save(guardar.FileName);
            }
        }

        private void escalaDeGrisesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            img2 = img1.Convert<Gray, Byte>();
            imageBox1.Image = img2;
        }

        private void negativoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            imageBox1.Image = img1.Not();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void recortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> image2;
            MCvMoments moment = image.GetMoments(true);
            int Y = (int)(moment.M01 / moment.M00);
            int X = (int)(moment.M10 / moment.M00);
            Rectangle rec = new Rectangle(X - 100, Y - 100, 200, 200);
            CvInvoke.Rectangle(image, rec, new Bgr(Color.White).MCvScalar, 2);
            imageBox1.Image = image;
            image2 = image.GetSubRect(rec);
            imageBox2.Image = image2;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Correccion_Gamma();
        }
        public void Correccion_Gamma()
        {
            fps = (double)trackBar1.Value / 100;
            label1.Text = fps.ToString();
            Image<Gray, float> img = img1.Convert<Gray, float>();
            Matrix<float> Mimg = new Matrix<float>(img.Size);
            CvInvoke.cvCopy(img, Mimg, IntPtr.Zero);
            CvInvoke.Pow(Mimg, fps, Mimg);
            CvInvoke.Normalize(Mimg,Mimg,0.0d,255.0, NormType.MinMax);
            CvInvoke.cvCopy(Mimg, img, IntPtr.Zero);
            img2 = img.Convert<Gray, Byte>();
            imageBox2.Image = img2;
        }
    }
}
