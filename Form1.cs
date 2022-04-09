using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace GraphicsThreads
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Thread RedThread; // Thread instance
        Thread BlueThread; // Thread instance
        int D1, D2 = 0;
        double p, q, x, y;
        Pen pen = new Pen(Color.Red, 2); // determines colour of pen and its size
        Pen pen2 = new Pen(Color.Blue, 2); // determines colour of pen and its size
        Random RDM = new Random(); // Generate random number for the boxes to be generated at a random location
        private object thisLock1 = new Object(); // ThreadLocking

        private void button1_Click(object sender, EventArgs e)
        {
            RedThread = new Thread(ThreadRed);
            RedThread.Name = "First Thread"; // Create Thread name
            RedThread.Priority = ThreadPriority.AboveNormal; // Thread Priority Increase
            RedThread.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RedThread = new Thread(ThreadRed);
            BlueThread = new Thread(ThreadBlue);
            RedThread.Name = "First Thread"; // Create Thread name
            BlueThread.Name = "Second Thread"; // Create Thread name
            RedThread.Priority = ThreadPriority.AboveNormal; // Thread Priority Increase
            BlueThread.Priority = ThreadPriority.AboveNormal; // Thread Priority Increase
            RedThread.Start();
            BlueThread.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BlueThread = new Thread(ThreadBlue);
            BlueThread.Name = "Second Thread"; // Create Thread name
            BlueThread.Priority = ThreadPriority.AboveNormal; // Thread Priority Increase
            BlueThread.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private object thisLock2 = new Object(); // ThreadLocking

        public class calculate
        {
            public double process3(int D1) // RedThread utilises this method
            {
                double answer3;
                return answer3 = 100 * Math.Sin((255 - D1) * (Math.PI / 255)); // math for drawing the needles
            }
            public double process4(int D1) // RedThread utilises this method
            {
                double answer4;
                return answer4 = 100 * Math.Cos((255 - D1) * (Math.PI / 255)); // math for drawing the needles
            }

            public double process1(int D2) // BlueThread utilises this method
            {
                double answer1;
                return answer1 = 100 * Math.Sin((255 - D2) * ((Math.PI * 2) / 255)); // math for drawing the needles
            }
            public double process2(int D2) // BlueThread utilises this method
            {
                double answer2;
                return answer2 = 100 * Math.Cos((255 - D2) * ((Math.PI * 2) / 255)); // math for drawing the needles 6.285
            }
        };

        // Performance can be improved by using the CompareExchange method instead, as follows:
        // System.Threading.Interlocked.CompareExchange(ref x, y, null); 

        public void ThreadRed() // object obj
        {
            lock (thisLock1) // Lock is used to restrict additional threads from entering this code section
            {
                for (int i = 0; i < 10000; i++)
                {
                    CreateGraphics().DrawRectangle(new Pen(Brushes.Red, 4), new Rectangle(RDM.Next(0, this.Width), RDM.Next(0, this.Height), 40, 20)); // Draws a rectangle: 40 by 20
                    D1 = D1 + 1;
                    calculate qcalc = new calculate();
                    calculate pcalc = new calculate();
                    q = qcalc.process3(D1); //q = 80 * Math.Sin((255 - D1) * (Math.PI / 255)); // math for drawing the needles
                    p = pcalc.process4(D1); //p = 80 * Math.Cos((255 - D1) * (Math.PI / 255)); // math for drawing the needles

                    CreateGraphics().DrawLine(pen, 200, 200, 200 + (int)p, 200 - (int)q); // point for drawing the needle

                    /*try
                    {
                        StreamWriter fileSW1 = File.AppendText("C:\\Users\\Username\\Desktop\\Artificial_Graphics_Data.txt");   //Windows 7
                        fileSW1.WriteLine(DateTime.Now);
                        fileSW1.WriteLine("Testing Data: " + D1); // Write data in txt file
                        fileSW1.WriteLine("Thread Name: " + Thread.CurrentThread.Name); // Write data in txt file
                        fileSW1.WriteLine("__________________");
                        fileSW1.Close(); // close file
                    }
                    catch (IOException) // System.IO.IOException
                    {
                        label1.Text = "Exception Thrown!!";
                    }*/
                    //Thread.Sleep(10);
                }
                //BlueThread.Join();
            }
            MessageBox.Show("RedThread has now completed!!");
            RedThread.Abort();
        }

        public void ThreadBlue() // object obj
        {
            lock (thisLock2) // Lock is used to restrict additional threads from entering this code section
            {
                for (int i = 0; i < 10000; i++)
                {
                    CreateGraphics().DrawRectangle(new Pen(Brushes.Blue, 4), new Rectangle(RDM.Next(0, this.Width), RDM.Next(0, this.Height), 40, 20)); // Draws a rectangle: 40 by 20
                    D2 = D2 + 1;
                    calculate ycalc = new calculate();
                    calculate xcalc = new calculate();
                    y = ycalc.process1(D2); // 100 * Math.Sin((255 - D2) * ((Math.PI * 2) / 255)); // math for drawing the needles
                    x = xcalc.process2(D2); // 100 * Math.Cos((255 - D2) * ((Math.PI * 2) / 255)); // math for drawing the needles
                    CreateGraphics().DrawLine(pen2, 400, 400, 400 + (int)x, 400 - (int)y); // point for drawing the needle

                    /*try
                    {
                        StreamWriter fileSW2 = File.AppendText("C:\\Users\\Username\\Desktop\\Artificial_Graphics_Data2.txt");   //Windows 7
                        fileSW2.WriteLine(DateTime.Now);
                        fileSW2.WriteLine("Testing Data: " + D2); // Write data in txt file
                        fileSW2.WriteLine("Thread Name: " + Thread.CurrentThread.Name); // Write data in txt file
                        fileSW2.WriteLine("__________________");
                        fileSW2.Close(); // close file
                    }
                    catch (IOException) // System.IO.IOException
                    {
                        label1.Text = "Exception Thrown!!";
                    }*/
                    //Thread.Sleep(10);
                }
            }
            MessageBox.Show("BlueThread has now completed!!");
            BlueThread.Abort();
        }
    }
}
