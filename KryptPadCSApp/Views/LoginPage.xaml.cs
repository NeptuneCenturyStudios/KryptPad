﻿using KryptPadCSApp.Classes;
using KryptPadCSApp.Models;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KryptPadCSApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();

        }

        private void TermsHyperlink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            // Go to terms page
            NavigationHelper.Navigate(typeof(TermsPage), null);
        }

        private void PrivacyHyperlink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            // Go to privacy page
            NavigationHelper.Navigate(typeof(PrivacyPage), null);
        }

        private async void DonateLink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            try
            {
                // Launch the uri
                await Windows.System.Launcher.LaunchUriAsync(
                    new Uri("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=5547784"));
            }
            catch (Exception)
            {
                // Failed
                await DialogHelper.ShowMessageDialogAsync("Could not launch the requested url.");
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Clear backstack
            NavigationHelper.ClearBackStack();

            await (DataContext as LoginPageViewModel).AutoLoginAsync();
        }

        private void Login_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                // Trigger login
                var m = DataContext as LoginPageViewModel;

                if (m.LogInCommand.CanExecute(null))
                {
                    m.LogInCommand.Execute(null);
                }
            }
        }

        private async void ForgotPasswordLink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            var m = DataContext as LoginPageViewModel;
            await m.SendForgotPasswordLinkAsync();
        }

        
    }
}
