using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace shitcastClient
{
    public partial class Client : Form
    {
        private static string url;
        string uname = Environment.UserName;
        Screen screen = Screen.PrimaryScreen;
        class Message
        {
            public string author;
            public string timestamp;
            public string message;
        }

        class User
        {
            public string name;
        }

        public Client(string urlv)
        {
            InitializeComponent();
            url = urlv;
        }

        private void Client_Load(object sender, EventArgs e)
        {
            addUser(Environment.UserName);
            reloadEverything();
        }

        private void send_Click(object sender, EventArgs e)
        {
            if ((textBox1.Focused || textBox1.Text != "") && textBox1.Text != "")
            {
                sendMsg(textBox1.Text, uname);
                textBox1.Text = "";
            }
        }

        void sendMsg(string msg, string author)
        {
            string strResponseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/messages");
            request.Method = "POST";


            request.ContentType = "application/json";
            using (StreamWriter swJSONPayload = new StreamWriter(request.GetRequestStream()))
            {
                swJSONPayload.Write("{\"message\":\"{0}\",\"author\":\"{1}\"}".Replace("{0}", msg).Replace("{1}", author));
                swJSONPayload.Close();
            }

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                //Proecess the resppnse stream... (could be JSON, XML or HTML etc..._
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
            reloadMsgs();
        }

        void delUser(string name)
        {
            string strResponseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/users");
            request.Method = "POST";


            request.ContentType = "application/json";
            using (StreamWriter swJSONPayload = new StreamWriter(request.GetRequestStream()))
            {
                swJSONPayload.Write("{\"user\":\"{0}\",\"del\":true}".Replace("{0}", name));
                swJSONPayload.Close();
            }

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                //Proecess the resppnse stream... (could be JSON, XML or HTML etc..._
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
            reloadUsers();
        }

        void addUser(string name)
        {
            string strResponseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/users");
            request.Method = "POST";


            request.ContentType = "application/json";
            using (StreamWriter swJSONPayload = new StreamWriter(request.GetRequestStream()))
            {
                swJSONPayload.Write("{\"user\":\"{0}\"}".Replace("{0}", name));
                swJSONPayload.Close();
            }



            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                //Proecess the resppnse stream... (could be JSON, XML or HTML etc..._
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                            if (strResponseValue == "1") //user already exists or smth
                            {
                                MessageBox.Show("it just so happens that there is already a user logged in with the name {0}.".Replace("{0}", Environment.UserName), "well well well", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                inputBox box = new inputBox("lets get you a new username :>", "cool");
                                box.ShowDialog();
                                if (box.box == "") Application.Exit();
                                uname = box.box;
                                addUser(box.box);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
            reloadUsers();
        }

        static int GetGreatestCommonDivisor(int a, int b)
        {
            return b == 0 ? a : GetGreatestCommonDivisor(b, a % b);
        }

        Message[] getMsgs()
        {

                    string strResponseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/messages");
            request.Method = "GET";
            request.ContentType = "application/json";
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
            Message[] obj = JsonConvert.DeserializeObject<Message[]>(strResponseValue);
            return obj;
        }

        User[] GetUsers()
        {
            string strResponseValue = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/users");
            request.Method = "GET";
            request.ContentType = "application/json";
            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            strResponseValue = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strResponseValue = "{\"errorMessages\":[\"" + ex.Message.ToString() + "\"],\"errors\":{}}";
            }
            finally
            {
                if (response != null)
                {
                    ((IDisposable)response).Dispose();
                }
            }
            User[] obj = JsonConvert.DeserializeObject<User[]>(strResponseValue);
            return obj;
        }

        void reloadMsgs()
        {
            textBox2.Text = "";
            Message[] messages = getMsgs();
            foreach (Message msg in messages)
            {
                textBox2.Text += String.Format("[{1}] {0}: {2}\r\n", msg.author, msg.timestamp, msg.message);
            }
            scrollToEnd();
        }

        void scrollToEnd()
        {
            textBox2.SelectionStart = textBox2.TextLength;
            textBox2.ScrollToCaret();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            reloadEverything();
        }

        private void reloadEverything()
        {
            reloadMsgs();
            reloadUsers();
        }

        private void reloadUsers()
        {
            listBox1.Items.Clear();
            User[] users = GetUsers();
            foreach (User user in users)
            {
                if (user.name == uname)
                {
                    listBox1.Items.Add(user.name + " (YOU)");
                }
                else listBox1.Items.Add(user.name);
                //if (user.frame != "") pictureBox1.Image = Image.FromStream(new MemoryStream(Convert.FromBase64String(user.frame)));
            }
        }

        private static Image GetCompressedBitmap(Bitmap bmp, long quality)
        {
            using (var mss = new MemoryStream())
            {
                EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                ImageCodecInfo imageCodec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(o => o.FormatID == ImageFormat.Jpeg.Guid);
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = qualityParam;
                bmp.Save(mss, imageCodec, parameters);
                return Image.FromStream(mss);
            }
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            delUser(uname);
        }
    }
}
