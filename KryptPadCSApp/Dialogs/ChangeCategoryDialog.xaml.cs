﻿using KryptPad.Api.Models;
using KryptPadCSApp.Models.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KryptPadCSApp.Dialogs
{
    public sealed partial class ChangeCategoryDialog : ContentDialog
    {
        
        public ChangeCategoryDialog()
        {
            this.InitializeComponent();

            // Determine the command's can execute state, and hook into the changed event
            var m = DataContext as ChangeCategoryDialogViewModel;
            if (m.PrimaryCommand != null)
            {
                m.PrimaryCommand.CanExecuteChanged += (sender, e) =>
                {
                    IsPrimaryButtonEnabled = (sender as ICommand).CanExecute(null);
                };
            }
        }

       
        #region Events
        
        private async void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var m = DataContext as ChangeCategoryDialogViewModel;

            await m.LoadCategoriesAsync();
        }
        #endregion


    }
}
