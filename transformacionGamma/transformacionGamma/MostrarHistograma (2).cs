using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace transformacionGamma
{
    public partial class MostrarHistograma : Form
    {

        public float[] DatosHistR = new float[256];
        public float[] DatosHistG = new float[256];
        public float[] DatosHistB = new float[256];
        public MostrarHistograma()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void MostrarHistograma_Load(object sender, EventArgs e)
        {
            var series1 = new Series
            {
                Name = "Canal rojo",
                Color = Color.Red,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line

            };

            var series2 = new Series
            {
                Name = "Canal verde",
                Color = Color.Green,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line



            };


            var series3 = new Series
            {
                Name = "Canal Azul",
                Color = Color.Blue,
                IsVisibleInLegend = false,
                IsXValueIndexed = true,
                ChartType = SeriesChartType.Line

            };

            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Series.Add(series3);


            int i = 0;

            foreach (float valor in DatosHistR)
            {
                series1.Points.AddXY(i, valor);
                series2.Points.AddXY(i, DatosHistG[i]);
                series3.Points.AddXY(i, DatosHistB[i]);
                i++;
            }


        }
    }
}
