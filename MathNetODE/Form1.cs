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
            result = new Result(MathNet.Numerics.OdeSolvers.RungeKutta.FourthOrder(
                Vector<double>.Build.Dense(new double[] { 5.0, 1.0 }), 
                0, 20, 1000, 
                (t, x) => Vector<double>.Build.Dense(new double[] 
                {
                    x[0] - x[0] * x[1],
                    -x[1] + x[0] * x[1]
                })));

            MainChart.ChartAreas[0].AxisX.Maximum = result.MaxX;
            MainChart.ChartAreas[0].AxisX.Minimum = 0;
            MainChart.ChartAreas[0].AxisY.Maximum = result.MaxY;
            MainChart.ChartAreas[0].AxisY.Minimum = 0;

            t = -1;
            timer1.Enabled = true;

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
    }
}
