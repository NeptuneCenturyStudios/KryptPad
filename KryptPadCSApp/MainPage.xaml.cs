﻿using KryptPadCSApp.Models;
using KryptPadCSApp.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Security.Authentication.OnlineId;
using KryptPadCSApp.Classes;
using KryptPad.Api;
using Windows.UI.Core;
using System.Net;
//using OneCode.Windows.UWP.Controls;
using System.ComponentModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace KryptPadCSApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        #region Fields
        
        // List of ValueTuple holding the Navigation Tag and the relative Navigation Page  
        private readonly IList<(string Tag, Type Page, NavigationHelper.NavigationType NavType)> _pages = 
            new List<(string Tag, Type Page, NavigationHelper.NavigationType NavType)>
        {
            ("home", typeof(ItemsPage), NavigationHelper.NavigationType.Main),
            ("favs", typeof(FavoritesPage), NavigationHelper.NavigationType.Main),
            ("signin", typeof(LoginPage), NavigationHelper.NavigationType.Root),
            ("about", typeof(AboutPage), NavigationHelper.NavigationType.Main),
            ("feedback", typeof(FeedbackPage), NavigationHelper.NavigationType.Main),
            ("donate", typeof(DonatePage), NavigationHelper.NavigationType.Main),
            ("switch", typeof(SelectProfilePage), NavigationHelper.NavigationType.Root),
            ("signout", typeof(LoginPage), NavigationHelper.NavigationType.Root),
            ("settings", typeof(SettingsPage), NavigationHelper.NavigationType.Current)
        };
        #endregion

        #region Properties
        /// <summary>
        /// Gets the main frame for content
        /// </summary>
        public Frame RootFrame { get { return NavigationFrame; } }
        
        #endregion

        #region Constructor

        public MainPage()
        {
            this.InitializeComponent();

            ShowSignedInControls();
        }

        #endregion

        #region Navigation

        private void NavView_Loaded(object sender, RoutedEventArgs e)
        {
            // This turns the back button on if the frame can go back
            UpdateBackButtonVisibility();

            // Set up back request handler
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            // Set the nav frame events
            NavigationFrame.Navigated += On_Navigated;

        }

        private void NavigateToPage(string navItemTag)
        {
            var item = _pages.First(p => p.Tag.Equals(navItemTag));

            // If the page is select profile, clear the back stack
            if (item.Page == typeof(SelectProfilePage))
            {
                // Clear backstack
                NavigationHelper.ClearBackStack();

            }

            NavigationHelper.Navigate(item.Page, null, item.NavType);
        }
        
        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

            if (args.IsSettingsInvoked)
                NavigationHelper.Navigate(typeof(SettingsPage), null);
            else
            {
                // Getting the Tag from Content (args.InvokedItem is the content of NavigationViewItem)
                var navItemTag = NavView.MenuItems
                    .OfType<NavigationViewItem>()
                    .First(i => args.InvokedItem.Equals(i.Content))
                    .Tag.ToString();

                NavigateToPage(navItemTag);
            }
        }


        private void On_Navigated(object sender, NavigationEventArgs e)
        {


            if (NavigationFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of NavView.MenuItems, and doesn't have a Tag
                NavView.SelectedItem = (NavigationViewItem)NavView.SettingsItem;
            }
            else
            {
                var item = _pages.FirstOrDefault(p => p.Page == e.SourcePageType);
                if (item.Page != null)
                {
                    NavView.SelectedItem = NavView.MenuItems
                        .OfType<NavigationViewItem>()
                        .First(n => n.Tag.Equals(item.Tag));
                }
                else
                {
                    // No page in the list
                    NavView.SelectedItem = null;
                }
            }

            // Each time a navigation event occurs, update the Back button's visibility
            UpdateBackButtonVisibility();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            // If we didn't handle the requerst, do default
            if (!e.Handled)
            {
                NavigationHelper.GoBack(e);
            }
        }

        #endregion

        
        #region Helper Methods

        /// <summary>
        /// Shows or hides the back button
        /// </summary>
        private void UpdateBackButtonVisibility()
        {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                NavigationFrame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Shows or hides the pane
        /// </summary>
        /// <param name="value"></param>
        public void ShowSignedInControls()
        {
            var signinStatus = (App.Current as App).SignInStatus;

            // Hide buttons we can't access yet
            var signedInWithProfile = signinStatus == SignInStatus.SignedInWithProfile ? Visibility.Visible : Visibility.Collapsed;
            HomeNavButton.Visibility = signedInWithProfile;
            FavNavButton.Visibility = signedInWithProfile;
            SwitchProfileButton.Visibility = signedInWithProfile;
            SignOutButton.Visibility = signedInWithProfile;

            // Show settings if user is not signed out
            NavView.IsSettingsVisible = signinStatus != SignInStatus.SignedOut;
            NavView.IsPaneOpen = false;

            // Show sign in if user is not signed in with a profile
            SignInNavButton.Visibility = signinStatus != SignInStatus.SignedInWithProfile ? Visibility.Visible : Visibility.Collapsed;
        }
        
        #endregion

        
        
    }
}
