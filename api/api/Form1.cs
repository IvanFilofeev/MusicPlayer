using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace api
{
    public partial class Form1 : Form
    {
        public List<Songs> audiolist;
      
        public Form1()
        {
            InitializeComponent();
        }
        

        private void Form1_Load(object sender, EventArgs e)
        {
            new auth().Show();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!auth_property.Default.secur)
            {
                Thread.Sleep(500);
                string url = "https://api.vk.com/method/audio.search?q=example&access_token=" + auth_property.Default.token;
                WebRequest request = WebRequest.Create(url);

                try
                {
                    WebResponse resp = request.GetResponse();
                    Stream stream = resp.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
                    string js_a = reader.ReadToEnd();
                    reader.Close();
                    resp.Close();
                    js_a = HttpUtility.HtmlDecode(js_a);

                    JToken token = JToken.Parse(js_a);
                    audiolist = token["response"].Children().Skip(1).Select(c => c.ToObject<Songs>()).ToList();
                    //can be commented 
                    this.Invoke((MethodInvoker)delegate
                    {                  
                        for (int i = 0; i < audiolist.Count(); i++)
                        {
                            listBox1.Items.Add(audiolist[i].artist + " - " + audiolist[i].title);
                        }
                        
                    });
                    //
                }
                catch { }
            }
        }

        

        
    }
}
