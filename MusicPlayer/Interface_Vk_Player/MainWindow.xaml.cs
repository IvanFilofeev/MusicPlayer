using api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        //
        List<string> _Result = new List<string>();
        List<string> _ResultUrls = new List<string>();
        public string URLToDownLoad;
        public string NameOfSong;
        //
        List<string> _MyMusicResult = new List<string>();
        List<string> _MyMusicResultUrls = new List<string>();
        public string URLToDownLoadMyMusic;
        public string NameOfSongMyMusic;

        //


        WMPLib.IWMPMedia URL;
        WMPLib.IWMPMedia MyMusicURL;
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
         //Часть отвечающая за музыку из моего аккаунта (Начало)
        private void ButtonMyMusic_Click(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(LoadMyMusic);
            thread.Start();
        }
        private void LoadMyMusic()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

           
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    MyMusicEngine engineMyMusic = new MyMusicEngine();
                    engineMyMusic.MyMusicLoad();
                    _MyMusicResult = engineMyMusic.MyMusicLoadGetFiles();
                    _MyMusicResultUrls = engineMyMusic.MyMusicGetUrls();
                   
                    MyMusicListWithURLs = activeXMediaPlayerMyMusic.playlistCollection.newPlaylist("MyMusicPlayList");

                    for (int i = 0; i < _MyMusicResultUrls.Count(); i++)
                    {

                        MyMusicURL = activeXMediaPlayerMyMusic.newMedia(_MyMusicResultUrls[i]);
                        MyMusicListWithURLs.appendItem(MyMusicURL);

                    }
                    activeXMediaPlayerMyMusic.currentPlaylist = MyMusicListWithURLs;
                    activeXMediaPlayerMyMusic.Ctlcontrols.stop();

                    ListBoxMyMusic.ItemsSource = engineMyMusic.MyMusicLoadGetFiles();

                    
                }
            );

        }

        private void ListBoxMyMusic_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxMyMusic.SelectedIndex != -1)

            {
                activeXMediaPlayerMyMusic.Ctlcontrols.play();
                activeXMediaPlayerMyMusic.Ctlcontrols.currentItem = activeXMediaPlayerMyMusic.currentPlaylist.get_Item(ListBoxMyMusic.SelectedIndex);
                int k = ListBoxMyMusic.SelectedIndex;
                URLToDownLoadMyMusic = _MyMusicResultUrls[k];
                NameOfSongMyMusic = _MyMusicResult[k] + ".mp3";

            }

        }
        private void DownLoadMyMusic_Click_1(object sender, RoutedEventArgs e)
        {
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(CompletedMyMusic);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChangedMyMusic);
            webClient.DownloadFileAsync(new Uri(URLToDownLoadMyMusic), @"C:\загрузки_из_vk-player\\" + NameOfSongMyMusic);
        }
        private void ProgressChangedMyMusic(object sender, DownloadProgressChangedEventArgs e)
        {
            ___progressBarMyMusic.Value = e.ProgressPercentage;
        }

        private void CompletedMyMusic(object sender, AsyncCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("Трек загружен");
        }
        //Часть отвечающая за музыку из моего аккаунта (Конец)




        //Часть отвечающая за поиск музыки (Начало)



        private void ButtonSearch_Click_1(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(LoadSearcResult);
            thread.Start();
        }

        private void LoadSearcResult()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

          
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
        

        private void DownLoad_Click(object sender, RoutedEventArgs e)
        {
            
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri(URLToDownLoad), @"C:\загрузки_из_vk-player\\" + NameOfSong);         


        }    




        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            ___progressBar.Value= e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            System.Windows.MessageBox.Show("Трек загружен");
        }
        //Часть отвечающая за поиск музыки (конец)







        List<string> mediaFileList;
        string mediaFolder = string.Empty;


        private void ButtonLoadDownloadedMusic_Click(object sender, RoutedEventArgs e)

        {
            mediaFileList = new List<string>();

            System.Windows.Forms.FolderBrowserDialog fbd = new  System.Windows.Forms.FolderBrowserDialog();

            if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)

            {

                mediaFolder = fbd.SelectedPath;
                DirectoryInfo dir = new DirectoryInfo(mediaFolder);
                foreach (FileInfo file in dir.GetFiles("*.*", SearchOption.AllDirectories))

                {

                    if (file.Extension == ".mp3" )

                    {

                        mediaFileList.Add(file.Name);

                    }

                }



                if (mediaFileList != null)

                {

                    ListBoxDownloadedMusic.ItemsSource = null;
                    ListBoxDownloadedMusic.ItemsSource = mediaFileList;

                }

            }

        }

        private void ListBoxDownloadedMusic_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxDownloadedMusic.SelectedIndex != -1)

            {

                AxWMPLib.AxWindowsMediaPlayer axWmp =

                  winFormsHostDownloadedMusic.Child as AxWMPLib.AxWindowsMediaPlayer;

                activeXMediaPlayerDownloadedMusic.URL = mediaFolder + "\\" + ListBoxDownloadedMusic.SelectedItem.ToString();

            }
        }

        
       
    }
}
