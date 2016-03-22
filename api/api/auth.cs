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
    public partial class auth : Form
    {
        public auth()
        {
            InitializeComponent();
        }

        private void auth_Load(object sender, EventArgs e)
        {
            string url = "https://oauth.vk.com/authorize?client_id=5369634&display=page&redirect_uri=https://oauth.vk.com/blank.html&scope=audio&response_type=token&v=5.45";
            webBrowser1.Navigate(url);

        }

        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
               
                string url = webBrowser1.Url.ToString();
                string l = url.Split('#')[1];
                if (l[0] == 'a')
                {
                    auth_property.Default.token = l.Split('&')[0].Split('=')[1];
                    auth_property.Default.id = l.Split('=')[3];
                    auth_property.Default.security = true;
                    
                    this.Close();
                    MessageBox.Show("Поздравляю!Авторизация прошла успешно. ");
                }
            }

            catch { }
        }

        
    }
}
