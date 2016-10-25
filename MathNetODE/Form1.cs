using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;

namespace MathNetODE
{
    public partial class Form1 : Form
    {
        Result result;
        int t = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            t++;
            if (t == result.T.Length - 1)
            {
                timer1.Enabled = false;
            }
            MainChart.Series[0].Points.AddXY(result.X[t], result.Y[t]);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double x0 = Convert.ToDouble(textBox1.Text.Replace(".", ","));
            double y0 = Convert.ToDouble(textBox2.Text.Replace(".", ","));
            double a1 = Convert.ToDouble(textBox3.Text.Replace(".", ","));
            double b1 = Convert.ToDouble(textBox4.Text.Replace(".", ","));
            double c1 = Convert.ToDouble(textBox5.Text.Replace(".", ","));
            double a2 = Convert.ToDouble(textBox6.Text.Replace(".", ","));
            double b2 = Convert.ToDouble(textBox7.Text.Replace(".", ","));
            double c2 = Convert.ToDouble(textBox8.Text.Replace(".", ","));
            System.Windows.Forms.DataVisualization.Charting.Series newSeries = new System.Windows.Forms.DataVisualization.Charting.Series();
            newSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            Result currentGraph = new Result(MathNet.Numerics.OdeSolvers.RungeKutta.FourthOrder(
                Vector<double>.Build.Dense(new double[] { x0, y0 }),
                0, 20, 3000,
                (t, x) => Vector<double>.Build.Dense(new double[]
                {
                    x[0] * (a1 + b1*x[0] + c1*x[1]),
                    x[1] * (a2 + b2*x[1] + c2*x[0])
                })));            
            for (int i = 0; i<currentGraph.T.Length; i++)
            {
                newSeries.Points.AddXY(currentGraph.X[i], currentGraph.Y[i]);
            }
            MainChart.Series.Add(newSeries);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MainChart.Series.Clear();
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            MainChart.Size = new Size(this.Size.Width - 130, this.Size.Height - 50);// 1078; 662
            panel1.Location = new Point(MainChart.Width + 10, 40);
        }
    }
}
