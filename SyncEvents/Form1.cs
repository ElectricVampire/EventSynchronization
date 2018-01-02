using System;
using LoggerNameSpace;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;


namespace SyncEvents
{
    public delegate void MyDel();

    public partial class Form1 : Form
    {
        event MyDel Event1;
        event MyDel Event2;
        public Form1()
        {
            InitializeComponent();
            // Some logger initialization stuff
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
            Trace.Indent();
        }

        private void Event2_Handler()
        {
            Logger.LogThisLine(">>> Event2_Handler");

            DoWork();

            Logger.LogThisLine("<<< Event2_Handler");
        }

        private void Event1_Handler()
        {

            Logger.LogThisLine(">>> Event1_Handler");

            DoWork();

            Logger.LogThisLine("<<< Event1_Handler");
            // syncEvent.Set();

        }
        public void DoWork()
        {
            Logger.LogThisLine(">>> DoWork");
            for (int i = 0; i < 5; i++)
            {
                // We get out of loop after 500 ms or when Heavy work is completed
                // completion of heavy work is notified by event hence Application.DoEvents is needed
                Logger.LogThisLine("Heavy work");
                Application.DoEvents();
                Thread.Sleep(100);
            }
            Logger.LogThisLine("<<< DoWork");
        }
        public void RaiseEvents()
        {
            Event1.Invoke();

            // have difference of 20 ms between both events
            Thread.Sleep(20);

            Event2.Invoke();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Created a simple UI app with only one button
            // Registered Event1 and Event2
            this.Event1 += () => this.BeginInvoke((MethodInvoker)Event1_Handler);
            this.Event2 += () => this.BeginInvoke((MethodInvoker)Event2_Handler);

            // Raise both the events from background thread
            new Thread(new ThreadStart(RaiseEvents)).Start();
        }
    }
}
