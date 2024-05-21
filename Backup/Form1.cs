using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
namespace Charts
{
    public partial class Form1 : Form
    {
        int[] alWeight;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            alWeight = new int[] { 13, 23, 33, 15, 20, 10, 4, 11 };
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            
            //int[] alWeight = new int[] { 17, 9, 23, 19, 6, 10, 4, 11 };
            //int[] alWeight = new int[] { 13, 23, 33, 15, 20, 10, 4, 11 };
            //int[] alWeight = new int[] { 20,20,20,20,20,20,20,20 };

            DrawPieChart(e, alWeight);

            DrawSliceChart(e, alWeight);

            DrawBarChart(e, alWeight);

            DrawLineChart(e, alWeight);
       }

        private void DrawPieChart(PaintEventArgs e, int[] alWeight)
        {
            int numberOfSections = alWeight.Length;
            int x0 = 600;
            int y0 = 500;
            int radius = 200;
            int startAngle = 0;
            int sweepAngle = 360/numberOfSections;
            int[] height = new int[numberOfSections];
            int maxWeight = MaxValue(alWeight);
            Random rnd = new Random(10);
            SolidBrush brush = new SolidBrush(Color.Aquamarine);
            Pen pen = new Pen(Color.Black);
            for(int i =0;i<numberOfSections;i++)
            {
                height[i] = ((Convert.ToInt32(alWeight[i])) * radius) / maxWeight;
                brush.Color = Color.FromArgb(rnd.Next(200, 255), rnd.Next(255), rnd.Next(255), rnd.Next(255));
                e.Graphics.FillPie(brush, x0 - height[i], y0 - height[i], 2 * height[i], 2 * height[i],(startAngle+i*45), sweepAngle);
                e.Graphics.DrawPie(pen, x0 - height[i], y0 - height[i], 2 * height[i], 2 * height[i], (startAngle + i * 45), sweepAngle);
                    //e.Graphics.FillPie(Brushes.Blue, x0 - height[i], y0 - height[i], 2 * height[i], 2 * height[i], (startAngle + i * 45 -2), 2);
            }
        }
        private void DrawSliceChart(PaintEventArgs e, int[] alWeight)
        {
            int numberOfSections = alWeight.Length;
            int x0 = 100;
            int y0 = 100;
            int radius = 100;
            int startAngle = 0;
            int sweepAngle = 0;
            int[] height = new int[numberOfSections];
            int total = SumOfArray(alWeight);
            Random rnd = new Random();
            SolidBrush brush = new SolidBrush(Color.Aquamarine);
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < numberOfSections; i++)
            {
                
                brush.Color = Color.FromArgb(rnd.Next(200, 255), rnd.Next(255), rnd.Next(255), rnd.Next(255));
                // Since we have taken integer so last slice needs to be rounded off to fit into the remaining part.
                if (i == numberOfSections - 1)
                    sweepAngle = 360 - startAngle;
                else
                    sweepAngle = (360 * alWeight[i]) / total;
                e.Graphics.FillPie(brush, x0 - height[i], y0 - height[i], 2*radius, 2*radius, startAngle , sweepAngle);
                e.Graphics.DrawPie(pen, x0 - height[i], y0 - height[i], 2*radius, 2*radius, startAngle, sweepAngle);
                startAngle += sweepAngle;
                brush.Color = Color.FromKnownColor(KnownColor.Black);
                //e.Graphics.FillPie(Brushes.Blue, x0 - height[i], y0 - height[i], 2 * height[i], 2 * height[i], (startAngle + i * 45 -2), 2);
            }
        }

        private void DrawBarChart(PaintEventArgs e, int[] alWeight)
        {
            int numberOfSections = alWeight.Length;
            int lengthArea = 400;
            int heightArea = 250;
            int topX = 400;
            int topY = 20;
            int maxWeight = MaxValue(alWeight);
            int[] height = new int[numberOfSections];
            int total = SumOfArray(alWeight);
            Random rnd = new Random();
            SolidBrush brush = new SolidBrush(Color.Aquamarine);
            Pen pen = new Pen(Color.Gray);
            Rectangle rec = new Rectangle(topX,topY,lengthArea,heightArea);
            e.Graphics.DrawRectangle(pen,rec);
            pen.Color = Color.Black;
            int smallX = topX;
            int smallY = 0;
            int smallLength = (lengthArea/alWeight.Length);
            int smallHeight = 0;
             for (int i = 0; i < numberOfSections; i++)
            {
                 brush.Color = Color.FromArgb(rnd.Next(200, 255), rnd.Next(255), rnd.Next(255), rnd.Next(255));
                 smallHeight = ((alWeight[i] * heightArea )/ maxWeight);
                 smallY = topY + heightArea - smallHeight;
                 Rectangle rectangle = new Rectangle(smallX, smallY, smallLength, smallHeight);
                 e.Graphics.DrawRectangle(pen,rectangle);
                 e.Graphics.FillRectangle(brush, rectangle);
                 brush.Color = Color.FromKnownColor(KnownColor.Black);
                 e.Graphics.DrawRectangle(pen, rectangle);
                 smallX = smallX + smallLength;
             }
        }

        private void DrawLineChart(PaintEventArgs e,int[] alWeight)
        {
            int numberOfSections = alWeight.Length;
            int lengthArea = 400;
            int heightArea = 250;
            int topX = 20;
            int topY = 400;
            int maxWeight = MaxValue(alWeight);
            int[] height = new int[numberOfSections];
            int total = SumOfArray(alWeight);
            Random rnd = new Random();
            SolidBrush brush = new SolidBrush(Color.Aquamarine);
            Pen pen = new Pen(Color.Gray);
            Rectangle rec = new Rectangle(topX, topY, lengthArea, heightArea);
            e.Graphics.DrawRectangle(pen, rec);
            pen.Color = Color.Black;
            int smallX = topX;
            int smallY = 0;
            int smallLength = (lengthArea / (alWeight.Length + 1));
            int smallHeight = 0;
            Point p1 = new Point();
            Point p2 = new Point();
            for (int i = 0; i < numberOfSections; i++)
            {
                brush.Color = Color.FromArgb(rnd.Next(200, 255), rnd.Next(255), rnd.Next(255), rnd.Next(255));
                p1 = p2;
                p2.X = p2.X + smallLength;
                smallHeight = ((alWeight[i] * heightArea) / maxWeight);
                p2.Y = topY + heightArea - smallHeight;
                if (p1.X != 0 && p1.Y != 0)
                {
                    e.Graphics.DrawLine(pen, p1, p2);

                }
                DrawDots(e,p2);
                smallX = smallX + smallLength;
            }
        }

        private void DrawDots(PaintEventArgs e, Point p1)
        {
            Pen pen = new Pen(Color.SeaGreen);
            e.Graphics.DrawPie(pen, p1.X-5 , p1.Y-5, 10, 10, 0, 360);
            e.Graphics.FillPie(new SolidBrush(Color.Purple), p1.X - 5, p1.Y - 5, 10, 10, 0, 360);
        }

        private static int MaxValue(int[] intArray)
        {
            int maxVal = intArray[0];
            for (int i = 0; i < intArray.Length; i++)
            {
                if (intArray[i] > maxVal)
                    maxVal = intArray[i];
            }
            return maxVal;
        }
        private static int SumOfArray(int[] intArray)
        {
            int sum = 0;
            for (int i = 0; i < intArray.Length; i++)
            {
                
                    sum += intArray[i];
            }
            return sum;
        }
    }
}