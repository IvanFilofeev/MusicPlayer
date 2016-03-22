using api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Interface_Vk_Player
{
    
    public partial class MainWindow : Window
    {
        public string str;
        List<string> _Result = new List<string>();
        List<string> _ResultUrls = new List<string>();
        List<string> _MyMusicResultUrls = new List<string>();
        public string URLToDownLoad;
        public string NameOfSong;

        WMPLib.IWMPMedia URL;
        WMPLib.IWMPPlaylist ListWithURLs;
        WMPLib.IWMPPlaylist MyMusicListWithURLs;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            auth window = new auth();
            window.Show();
        }

        private void ButtonMyMusic_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(LoadMyMusic);
            thread.Start();
        }
        private void LoadMyMusic()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            // Получить диспетчер от текущего окна и использовать его для вызова кода обновления
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    MyMusicEngine engineMyMusic = new MyMusicEngine();
                    engineMyMusic.MyMusicLoad();
                    _MyMusicResultUrls = engineMyMusic.MyMusicGetUrls();
                    MyMusicListWithURLs = activeXMediaPlayerMyMusic.playlistCollection.newPlaylist("MyMusicPlayList");

                    for (int i = 0; i < _MyMusicResultUrls.Count(); i++)
                    {

                        URL = activeXMediaPlayerMyMusic.newMedia(_MyMusicResultUrls[i]);
                        MyMusicListWithURLs.appendItem(URL);

                    }
                    activeXMediaPlayerMyMusic.currentPlaylist = MyMusicListWithURLs;
                    activeXMediaPlayerMyMusic.Ctlcontrols.stop();

                    ListBoxMyMusic.ItemsSource = engineMyMusic.MyMusicLoadGetFiles();

                }
            );

        }
        private void ButtonSearch_Click_1(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(LoadSearcResult);
            thread.Start();
        }

        private void LoadSearcResult()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            // Получить диспетчер от текущего окна и использовать его для вызова кода обновления
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    SearchEngine engine = new SearchEngine(TextBoxSearch.Text);
                    engine.Search();
                    _Result = engine.GetFiles();
                    _ResultUrls = engine.GetUrls();
                    ListWithURLs = activeXMediaPlayer.playlistCollection.newPlaylist("vkPlayList");

                    for (int i = 0; i < _ResultUrls.Count(); i++)
                    {

                        URL = activeXMediaPlayer.newMedia(_ResultUrls[i]);
                        ListWithURLs.appendItem(URL);

                    }
                    activeXMediaPlayer.currentPlaylist = ListWithURLs;
                    activeXMediaPlayer.Ctlcontrols.stop();

                    ListBoxSearch.ItemsSource = engine.GetFiles();

                }
                );


        }


        //private void ButtonSearch_Click_1(object sender, RoutedEventArgs e)
        //{

            //    SearchEngine engine = new SearchEngine(TextBoxSearch.Text);
            //    engine.Search();
            //    _Result = engine.GetFiles();
            //    _ResultUrls = engine.GetUrls();
            //    ListWithURLs = activeXMediaPlayer.playlistCollection.newPlaylist("vkPlayList");

            //    for (int i = 0; i < _ResultUrls.Count(); i++)
            //    {

            //        URL = activeXMediaPlayer.newMedia(_ResultUrls[i]);
            //        ListWithURLs.appendItem(URL);

            //    }
            //    activeXMediaPlayer.currentPlaylist = ListWithURLs;
            //    activeXMediaPlayer.Ctlcontrols.stop();

            //    ListBoxSearch.ItemsSource = engine.GetFiles();



            //}

        private void ListBoxSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxSearch.SelectedIndex != -1)

            {
                activeXMediaPlayer.Ctlcontrols.play();
                activeXMediaPlayer.Ctlcontrols.currentItem = activeXMediaPlayer.currentPlaylist.get_Item(ListBoxSearch.SelectedIndex);  

                int t = ListBoxSearch.SelectedIndex;
                URLToDownLoad = _ResultUrls[t];
                NameOfSong = _Result[t]+".mp3";
            }
        }
        

        private void ListBoxMyMusic_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxMyMusic.SelectedIndex != -1)

            {
                activeXMediaPlayerMyMusic.Ctlcontrols.play();
                activeXMediaPlayerMyMusic.Ctlcontrols.currentItem = activeXMediaPlayerMyMusic.currentPlaylist.get_Item(ListBoxMyMusic.SelectedIndex);
            }

        }
        private void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri(URLToDownLoad), @"Test\\" + NameOfSong );
            


        }
        



        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ___progressBar.Value= e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("Трек загружен");
        }

       
    }
}
