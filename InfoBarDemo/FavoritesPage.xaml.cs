﻿using InfoBarDemo.Models;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InfoBarDemo
{

    
    public sealed partial class FavoritesPage : Page
    {
        private List<FavoritesFiles> Favorites;
        public FavoritesPage()
        {
            this.InitializeComponent();
            Favorites = FavoritesFileManager.GetFavoritesFiles();
        }

        private void IconsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int curItem = IconsListBox.SelectedIndex;
            if (curItem == 0)
            {
                Frame.Navigate(typeof(MainPage));
            }
        }
    }
}
