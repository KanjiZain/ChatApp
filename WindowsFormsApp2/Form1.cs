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

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private TcpListener _server;
        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _receiveThread;


        public Form1()
        {
            InitializeComponent();
            StartServer();
        }

        private void StartServer()
        {
            _server = new TcpListener(IPAddress.Any, 8888);
            _server.Start();
            textBoxReceived.AppendText("Waiting for client to connect...\n");

            // Accept client connection
            _client = _server.AcceptTcpClient();
            _stream = _client.GetStream();

            // Clear the "Waiting for client to connect..." message
            ClearStatusMessage();

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

        private void ClearStatusMessage()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(ClearStatusMessage));
                return;
            }
            textBoxReceived.Clear();
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
