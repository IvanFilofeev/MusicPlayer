using api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace interface_WindowsForms
{
    public partial class Form1 : Form
    {
        WMPLib.IWMPPlaylist ListWithURLs;
        WMPLib.IWMPMedia URL;

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            auth window = new auth();
            window.Show();
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
           
                this.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        ListWithURLs = axWindowsMediaPlayer1.playlistCollection.newPlaylist("vkPlayList");
                    
                    SearchEngine engine = new SearchEngine(textBox1.Text);
                    engine.Search();
                    listBox2.Items.Add(engine.GetFiles());

                    axWindowsMediaPlayer1.currentPlaylist = ListWithURLs;
                    axWindowsMediaPlayer1.Ctlcontrols.stop();
                    }
                    catch { }

                });
            
            
        }

            private void listBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.Ctlcontrols.currentItem = axWindowsMediaPlayer1.currentPlaylist.get_Item(listBox2.SelectedIndex);
                            
            }
        }

        
       
    }
    }

