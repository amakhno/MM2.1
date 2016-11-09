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
using System.Windows.Forms.DataVisualization.Charting;

namespace MathNetODE
{
    public partial class Form1 : Form
    {
        Result result;
        int t = -1;

        public Form1()
        {
            InitializeComponent();
            MainChart.Series.Clear();
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
            double tEnd = Convert.ToDouble(textBox9.Text.Replace(".", ","));
            if (checkBox1.Checked)
            {                
                System.Windows.Forms.DataVisualization.Charting.Series newSeries1 = new System.Windows.Forms.DataVisualization.Charting.Series();
                newSeries1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                System.Windows.Forms.DataVisualization.Charting.Series newSeries2 = new System.Windows.Forms.DataVisualization.Charting.Series();
                newSeries2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                Result currentGraph = new Result(MathNet.Numerics.OdeSolvers.RungeKutta.FourthOrder(
                    Vector<double>.Build.Dense(new double[] { x0, y0 }),
                    0, tEnd, 3000,
                    (t, x) => Vector<double>.Build.Dense(new double[]
                    {
                    x[0] * (a1 - b1*x[0] - c2*x[1]),
                    x[1] * (a2 - b2*x[1] - c1*x[0])
                    })));
                for (int i = 0; i < currentGraph.T.Length; i++)
                {
                    newSeries1.Points.AddXY((double)i/(currentGraph.T.Length)*tEnd, currentGraph.X[i]);
                    newSeries2.Points.AddXY((double)i/(currentGraph.T.Length)*tEnd, currentGraph.Y[i]);
                }
                MainChart.Series.Add(newSeries1);
                MainChart.Series.Add(newSeries2);
            }
            else
            {
                if (checkBox2.Checked)
                {
                    Series series2;
                    Series series3;
                    try
                    { 
                        series2 = MainChart.Series["SeriesLine1"];
                        series3 = MainChart.Series["SeriesLine2"];
                    }
                    catch
                    {
                        series2 = new Series();
                        series3 = new Series();
                        series2.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
                        series2.BorderWidth = 2;
                        series2.ChartArea = "Default";
                        series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        series2.Color = System.Drawing.Color.Gray;
                        series2.Legend = "Default";
                        series2.Name = "SeriesLine1";
                        series3.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.DashDotDot;
                        series3.BorderWidth = 2;
                        series3.ChartArea = "Default";
                        series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                        series3.Color = System.Drawing.Color.Gray;
                        series3.Legend = "Default";
                        series3.Name = "SeriesLine2";                        
                        this.MainChart.Series.Add(series2);
                        this.MainChart.Series.Add(series3);
                    }
                    series2.Points.Clear();
                    series3.Points.Clear();                    
                    series2.Points.AddXY(0, a1 / c2);
                    series2.Points.AddXY(a1 / b1, 0);
                    series3.Points.AddXY(0, a2 / b2);
                    series3.Points.AddXY(a2 / c1, 0);
                }
                System.Windows.Forms.DataVisualization.Charting.Series newSeries = new System.Windows.Forms.DataVisualization.Charting.Series();
                newSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                Result currentGraph = new Result(MathNet.Numerics.OdeSolvers.RungeKutta.FourthOrder(
                    Vector<double>.Build.Dense(new double[] { x0, y0 }),
                    0, tEnd, 3000,
                    (t, x) => Vector<double>.Build.Dense(new double[]
                    {
                    x[0] * (a1 - b1*x[0] - c2*x[1]),
                    x[1] * (a2 - b2*x[1] - c1*x[0])
                    })));
                for (int i = 0; i < currentGraph.T.Length; i++)
                {
                    newSeries.Points.AddXY(currentGraph.X[i], currentGraph.Y[i]);
                }
                MainChart.Series.Add(newSeries);
            }
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SerializeInfo info = new SerializeInfo(textBox1.Text, textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text, textBox6.Text,
                textBox7.Text, textBox8.Text, textBox9.Text, checkBox1.Checked, MainChart.Series.ToList<System.Windows.Forms.DataVisualization.Charting.Series>());
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            { 
                info.Save(saveFileDialog.FileName);
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            var dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                SerializeInfo info1 = new SerializeInfo(openFileDialog.FileName);
                textBox1.Text = info1.text1;
                textBox2.Text = info1.text2;
                textBox3.Text = info1.text3;
                textBox4.Text = info1.text4;
                textBox5.Text = info1.text5;
                textBox6.Text = info1.text6;
                textBox7.Text = info1.text7;
                textBox8.Text = info1.text8;
                textBox9.Text = info1.text9;
                checkBox1.Checked = info1.@checked;
                for (int i = 0; i < info1.seriesInfoList.Count(); i++)
                {
                    info1.BuildSeriesCollection(MainChart.Series);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MainChart.Series.Remove(MainChart.Series.Last());
        }
    }
}
