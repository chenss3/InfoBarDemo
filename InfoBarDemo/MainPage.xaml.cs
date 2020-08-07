using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using InfoBarDemo.Models;
using File = InfoBarDemo.Models.File;
using System.ComponentModel;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InfoBarDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private int refreshed = 1;
        private bool saved = false;

        private List<File> Files;
        public MainPage()
        {
            this.InitializeComponent();
            Files = FileManager.GetFiles();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            if (refreshed == 3)
            {
                System.Threading.Thread.Sleep(500);
                InformationalBar.IsOpen = true;
                refreshed += 1;
            } 
            else if (refreshed == 1)
            {
                System.Threading.Thread.Sleep(500);
                CriticalBar.IsOpen = true;
                refreshed += 1;
            } else if (refreshed == 2)
            {
                System.Threading.Thread.Sleep(500);
                CriticalBar.IsOpen = false;
                refreshed += 1; 
            }

            
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!saved)
            {
                SaveRing.IsActive = true;
                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
                worker.RunWorkerAsync();
                saved = true;
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(1500);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SaveRing.IsActive = false;
            WarningBar.IsOpen = true;
        }
        private void WarningBar_ActionButtonClick(object sender, RoutedEventArgs e)
        {

            SaveRing.IsActive = true;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted2);
            worker.RunWorkerAsync();

        }

        private void worker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            SaveRing.IsActive = false;
            WarningBar.IsOpen = false;
        }

        private void InformationalBar_ActionButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateProgress.Visibility = Visibility.Visible;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += worker_DoWork2;
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted3);
            worker.RunWorkerAsync();
        }

        private void worker_DoWork2(object sender, DoWorkEventArgs e)
        {
            System.Threading.Thread.Sleep(2000);
        }

        private void worker_RunWorkerCompleted3(object sender, RunWorkerCompletedEventArgs e)
        {
            UpdateProgress.Visibility = Visibility.Collapsed;
            InformationalBar.IsOpen = false;
            SuccessBar.IsOpen = true;
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int curItem = IconsListBox.SelectedIndex;
            if (curItem == 1)
            {
                Frame.Navigate(typeof(FavoritesPage));
            }
        }
    }
}
