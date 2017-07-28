using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.CvEnum;
namespace transformacionGamma
{
    public partial class Form1 : Form
    {
        double fps;
        Image<Bgr, byte> img1;
        Image<Bgr, byte> img2;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            img1 = new Image<Bgr, byte>("imagen1.jpg");
            imageBox1.Image = img1;

        }

        public void Correccion_Gamma()
        {
         
            fps = (double)trackBar1.Value / 100;
            label1.Text = fps.ToString();
            Image<Gray, float> img = img1.Convert<Gray, float>();
            Matrix<float> Mimg = new Matrix<float>(img.Size);
            CvInvoke.cvCopy(img, Mimg, IntPtr.Zero);
            CvInvoke.Pow(Mimg, fps, Mimg);
            CvInvoke.Normalize(Mimg, Mimg, 0.0d, 255.0, NormType.MinMax);
            CvInvoke.cvCopy(Mimg, img, IntPtr.Zero);
            imageBox2.Image = img;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Correccion_Gamma();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Image<Gray, float> img = img1.Convert<Gray, float>();
            Matrix<float> Mimg = new Matrix<float>(img.Size);
            CvInvoke.cvCopy(img, Mimg, IntPtr.Zero);
            CvInvoke.AddWeighted(Mimg, 1.0d, Mimg, 0.0d, 1.0d, Mimg);
            CvInvoke.Log(Mimg, Mimg);
            CvInvoke.Normalize(Mimg, Mimg, 0.0d, 255.0, NormType.MinMax);
            CvInvoke.cvCopy(Mimg, img, IntPtr.Zero);
            imageBox2.Image = img;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MostrarHistograma form1 = new MostrarHistograma();
            DenseHistogram Histo = new DenseHistogram(255, new RangeF(0, 255));
            Image<Gray, Byte> img2Blue = img1[0];
            Image<Gray, Byte> img2Green = img1[1];
            Image<Gray, Byte> img2Red = img1[2];

            Histo.Calculate(new Image<Gray, Byte>[] { img2Red }, true, null);
            Histo.CopyTo(form1.DatosHistR);
            Histo.Clear();

            Histo.Calculate(new Image<Gray, Byte>[] { img2Green }, true, null);
            Histo.CopyTo(form1.DatosHistG);
            Histo.Clear();

            Histo.Calculate(new Image<Gray, Byte>[] { img2Blue }, true, null);

            Histo.CopyTo(form1.DatosHistB);
            Histo.Clear();


            form1.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            float[] HistRed = new float[256];
            float[] HistGreen = new float[256];
            float[] HistBlue = new float[256];


           DenseHistogram Histo = new DenseHistogram(255, new RangeF(0, 255));

            Image<Gray, Byte> img2Blue = img1[0];
            Image<Gray, Byte> img2Green = img1[1];
            Image<Gray, Byte> img2Red = img1[2];

            Histo.Calculate(new Image<Gray, Byte>[] { img2Red }, true, null);
            Histo.CopyTo(HistRed);
            Histo.Clear();

            Histo.Calculate(new Image<Gray, Byte>[] { img2Green }, true, null);
            Histo.CopyTo(HistGreen);
            Histo.Clear();

            Histo.Calculate(new Image<Gray, Byte>[] { img2Blue }, true, null);
            Histo.CopyTo(HistBlue);
            Histo.Clear();

            float Total = img1.Height * img1.Width;

            HistRed[0] /= Total;
            HistGreen[0] /= Total;
            HistBlue[0] /= Total;

            for (int i = 1; i < 256; i++)
            {
                HistRed[i] = HistRed[i - 1] + HistRed[i] / Total;
                HistGreen[i] = HistRed[i - 1] + HistRed[i] / Total;
                HistBlue[i] = HistRed[i - 1] + HistRed[i] / Total;

            }
            double varRed = 0;
            double varGreen = 0;
            double varBlue = 0;

            for (int x = 0; x < img1.Height; x++)
                for (int y = 0; y < img1.Height; y++)
                {
                    varRed = HistRed[img1.Data[x, y, 0]] * 255;
                    varGreen = HistGreen[img1.Data[x, y, 1]] * 255;
                    varRed = HistBlue[img1.Data[x, y, 2]] * 255;
                    img1[x, y] = new Bgr(varRed, varGreen, varBlue);
                }
            imageBox2.Image = img1;
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Matrix<float> kernel = new Matrix<float>(
              new float[,]
              {
                    {-1.0f, -1.0f, -1.0f},
                    {-1.0f, 8.0f, -1.0f},
                    {-1.0f, -1.0f,  -1.0f},
              });
            CvInvoke.Filter2D(img1, img1, kernel, new Point(-1, -1));
            imageBox2.Image = img1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Matrix<float> kernel = new Matrix<float>(
             new float[,]
             {
                    {1.0f, 2.0f, 1.0f},
                    {2.0f, 8.0f, 2.0f },
                    {1.0f, 2.0f,  1.0f},
             });
            CvInvoke.Filter2D(img1, img1, kernel, new Point(-1, -1));
            imageBox2.Image = img1;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Matrix<float> kernel = new Matrix<float>(
             new float[,]
             {
                    {-1.0f, -2.0f, -1.0f},
                    {0.0f, 0.0f, 0.0f},
                    {1.0f, 2.0f,  1.0f},
             });
            CvInvoke.Filter2D(img1, img1, kernel, new Point(-1, -1));
            imageBox2.Image = img1;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Matrix<float> kernel = new Matrix<float>(
             new float[,]
             {
                    {-1.0f, 0.0f, 1.0f},
                    {-2.0f, 0.0f, 2.0f},
                    {-1.0f, 0.0f, 1.0f},
             });
            CvInvoke.Filter2D(img1, img1, kernel, new Point(-1, -1));
            imageBox2.Image = img1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Matrix<float> kernel = new Matrix<float>(
             new float[,]
             {
                    {0.0f, 1.0f, 0.0f},
                    {1.0f, -4.0f, 1.0f},
                    {0.0f, 1.0f,  0.0f},
             });
            CvInvoke.Filter2D(img1, img1, kernel, new Point(-1, -1));
            imageBox2.Image = img1;
        }

        private void button8_Click(object sender, EventArgs e) { 
        // textBox1 = new TextBox;
        //    TextBox= numero;
        //{
        //    Matrix<float> kernel = new Matrix<float>(
        //     new float[,]
        //     {
        //            {-1.0f, -1.0f, -1.0f},
        //            {-1.0f, numero, -1.0f},
        //            {-1.0f, -1.0f,  -1.0f},
        //     });
        //    CvInvoke.Filter2D(img1, img1, kernel, new Point(-1, -1));
        //    imageBox2.Image = img1;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Matrix<float> kernel = new Matrix<float>(
             new float[,]
             {
                    {-1.0f, 0.0f},
                    {0.0f, 1.0f },
                   
             });
            CvInvoke.Filter2D(img1, img1, kernel, new Point(-1, -1));
            imageBox2.Image = img1;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Matrix<float> kernel = new Matrix<float>(
             new float[,]
             {
                    {0.0f, -1.0f},
                    {1.0f, 0.0f },

             });
            CvInvoke.Filter2D(img1, img1, kernel, new Point(-1, -1));
            imageBox2.Image = img1;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            img2 = img1.SmoothMedian(15);
            imageBox2.Image = img2;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            img2 = img1.SmoothGaussian(11);
            imageBox2.Image = img2;
        }

        private void button16_Click(object sender, EventArgs e)
        {
            img2 = img1.SmoothBilatral(10, 255, 120);
            imageBox2.Image = img2;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Image<Gray, byte> Img3 = img1.Convert<Gray, byte>();
            //Image<Gray, Single> Img4 = Img3.Sobel(1,0,3);
            Image<Gray, Single> Img4 = Img3.Sobel(1, 0, 3).Add(Img3.Sobel(0, 1, 3)).AbsDiff(new Gray(0.0));
            imageBox2.Image = Img4;
        }
    }
}
