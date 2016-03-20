using api;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string str;
        List<string> _Result = new List<string>();
        List<string> _ResultUrls = new List<string>();
        WMPLib.IWMPMedia URL;
        WMPLib.IWMPPlaylist ListWithURLs;




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
            MyMusicEngine engineMyMusic = new MyMusicEngine();
            engineMyMusic.MyMusicLoad();

            ListBoxMyMusic.ItemsSource = engineMyMusic.MyMusicLoadGetFiles();

        }

        private void ButtonSearch_Click_1(object sender, RoutedEventArgs e)
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





        private void ListBoxSearch_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListBoxSearch.SelectedIndex != -1)

            {
                activeXMediaPlayer.Ctlcontrols.play();
                activeXMediaPlayer.Ctlcontrols.currentItem = activeXMediaPlayer.currentPlaylist.get_Item(ListBoxSearch.SelectedIndex);              


            }
        }
    }
}
