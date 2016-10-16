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

        private void button1_Click(object sender, EventArgs e)
        {

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

            chart1.ChartAreas[0].AxisX.Maximum = result.MaxX;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = result.MaxY;
            chart1.ChartAreas[0].AxisY.Minimum = 0;

            t = -1;
            timer1.Enabled = true;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            t++;
            //chart1.Series[0].Points.Clear();
            if (t == result.T.Length - 1)
            {
                timer1.Enabled = false;
            }
            chart1.Series[0].Points.AddXY(result.X[t], result.Y[t]);
        }
    }
}
