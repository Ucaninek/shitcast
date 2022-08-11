using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;

namespace shitcastClient
{
    public partial class Form1 : Form
    {

        bool debug = false;
        public Form1()
        {
            InitializeComponent();
        }

        string murl;

        private void updateLabel(bool owo)
        {
            if(owo)
            {
                label3.ForeColor = Color.Green;
                label3.Text = "=> this is a shitcast server";
                button1.Enabled = true;
            } else
            {
                label3.ForeColor = Color.Red;
                label3.Text = "=> this is not a shitcast server";
                button1.Enabled = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(debug)
            {
                Client client = new Client("http://127.0.0.1:4207");
                this.Hide();
                client.ShowDialog();
                Application.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client client = new Client(murl);
            this.Hide();
            client.ShowDialog();
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            okk();
        }

        async void okk()
        {
            HttpClient client = new HttpClient();
            string url = textBox1.Text;
            url = url.Replace("localhost", "127.0.0.1");
            if (!url.Contains("http")) url = "http://" + url;
            if (!IsValidURL(url)) return;
            murl = url;
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    var dataObjects = response.Content.ReadAsStringAsync().Result;  //Make sure to add a reference to System.Net.Http.Formatting.dll
                    string builder = "";
                    foreach (var d in dataObjects)
                    {
                        builder += d;
                    }
                    updateLabel(builder.Contains("shitcast"));
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch
            {
                updateLabel(false);
            }
        }

        public static bool IsValidURL(string url)
        {
            var period = url.IndexOf(".");
            if (period > -1 && !url.Contains("@"))
            {
                // Check if there are remnants where the url scheme should be.
                // Dont modify string if so
                var colon = url.IndexOf(":");
                var slash = url.IndexOf("/");
                if ((colon == -1 || period < colon) &&
                    (slash == -1 || period < slash))
                {
                    url = $"http://{url}";
                }
            }

            System.Uri uriResult = null;
            var result = System.Uri.TryCreate(url, System.UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == System.Uri.UriSchemeHttp || uriResult.Scheme == System.Uri.UriSchemeHttps);
            return result;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            okk();
        }
    }
}
