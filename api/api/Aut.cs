using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace api
{
    public partial class Aut : Form
    {
        public Aut()
        {
            InitializeComponent();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            string url = "https://oauth.vk.com/authorize?client_id=5295384&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.45";
            webBrowser1.Navigate(url);
            auth_data data = new auth_data();
            string url_new = webBrowser1.Url.ToString();
            string l = url_new.Split('#')[1];
            if (l[0] == 'a')
            {
                data.token = l.Split('&')[0].Split('=')[1];
                data.id= l.Split('=')[3];
                data.auth = true;
                this.Close();
            }
        }
    }
}
