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

namespace ClientTCP
{
    public partial class Form1 : Form
    {
        static int port = 4001;
        static string address = "192.168.1.105"; // адрес сервера
        
        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "Выбор индекса";
            comboBox1.Items.Add("50001");
            comboBox1.Items.Add("50002");
            comboBox1.Items.Add("50003");
        }

        private void Connect()
        {
            try
            {
                IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
                //IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, port);

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(iPEndPoint);

                string message = comboBox1.SelectedItem.ToString();

                byte[] data = Encoding.Unicode.GetBytes(message);

                socket.Send(data);

                data = new byte[1024];
                StringBuilder stringBuilder = new StringBuilder();

                int bytes = 0;

                do
                {
                    bytes = socket.Receive(data, data.Length, 0);
                    stringBuilder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    //listBox1.Items.Add(stringBuilder.ToString());
                }
                while (socket.Available > 0);

                 listBox1.Items.Add(stringBuilder.ToString());

                // закрываем сокет
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Connect();
        }
    }
}
