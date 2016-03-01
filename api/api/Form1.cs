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
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace api
{
    public partial class Form1 : Form
    {
        public List<Songs> tracks;
      
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //Console.WriteLine("Enter the name of band");
            //string band = Console.ReadLine();
            string url = "https://api.vk.com/method/audio.search?q=" + "example" + "&v=5.45&access_token=cda1f4058077be320505e7408b86cf30b84644e18db478b5da331690ba101c1fbddbb5d27c82ed90fd449";
            WebRequest wr = WebRequest.Create(url);
            WebResponse resp = wr.GetResponse();
            Stream data = resp.GetResponseStream();
            StreamReader read = new StreamReader(data);
            string serv = read.ReadToEnd();
            read.Close();
            resp.Close();
            serv = HttpUtility.HtmlDecode(serv);

            JToken token = JToken.Parse(serv);
            tracks = Enumerable.Skip(token["response"].Children(), 1).Select(x => x.ToObject<Songs>()).ToList();

            this.Invoke((MethodInvoker)delegate
            {
                for (int i = 0; i < tracks.Count; i++)
                {
                    listBox1.Items.Add(tracks[i].artist);
                }
            });

            Console.ReadLine();
        }
    }
}
