using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _receiveThread;
        public Form1()
        {
            InitializeComponent();
            ConnectToServer();

        }

        private void ConnectToServer()
        {
            _client = new TcpClient("192.168.0.165", 8888); // Change IP and port as needed
            _stream = _client.GetStream();
            _receiveThread = new Thread(Receive);
            _receiveThread.Start();
        }

        private void Receive()
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = _stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                DisplayReceivedMessage(message);
            }
        }

        private void DisplayReceivedMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() => DisplayReceivedMessage(message)));
                return;
            }
            textBoxReceived.AppendText(message + Environment.NewLine);
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            string message = textBoxSend.Text;
            byte[] buffer = Encoding.ASCII.GetBytes(message);
            _stream.Write(buffer, 0, buffer.Length);
            textBoxSend.Clear();
        }
    }
}
