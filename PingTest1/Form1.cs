using System;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;

namespace PingTest1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            lblResultado1.Text = "Consultando...";
            var ping = new Ping();
            ping.PingCompleted += Ping_PingCompleted;
            object token = Stopwatch.StartNew();

            if (int.TryParse(txtTimeOut.Text, out int timeout) && timeout > 0 && timeout <= 30)
                ping.SendAsync(cboIP.Text, timeout * 1000, token);
            else
                ping.SendAsync(cboIP.Text, token);
        }

        private void Ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            button1.Enabled = true;

            var result = new StringBuilder();

            if (e.Reply != null)
                result.AppendLine(e.Reply.Status.ToString());
            else if (e.Error != null)
                result.AppendLine(e.Error.Message);
            else if (e.Cancelled)
                result.AppendLine("Cancelado!");
            else
                result.AppendLine("Falha na consulta!");

            var tempo = (Stopwatch)e.UserState;

            tempo.Stop();

            result.AppendLine($"{tempo.ElapsedMilliseconds / 1000.0:0.00} segundo(s)");

            lblResultado1.Text = result.ToString();
        }
    }
}
