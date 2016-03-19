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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            auth window = new auth();
            window.Show();

        }

        

        private void ButtonSearch_Click_1(object sender, RoutedEventArgs e)
        {
            SearchEngine engine = new SearchEngine(TextBoxSearch.Text);
            engine.Search();
            ListBoxSearch.ItemsSource = engine.GetFiles();
        }

        //private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    Thread thread = new Thread(SearchResult);
        //    thread.Start();
        //}


        //private void SearchResult()
        //{
        //    Thread.Sleep(TimeSpan.FromSeconds(5));


        //    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //        (ThreadStart)delegate ()
        //        {
        //            for (int i = 0; i < audiolist.Count(); i++)
        //            {

        //                ListBoxSearch.Items.Add(audiolist[i].artist + " - " + audiolist[i].title);

        //            }
        //        }

        //}
    }
}
