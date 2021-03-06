﻿using KryptPadCSApp.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.ComponentModel;
using KryptPadCSApp.Exceptions;
using KryptPad.Api;

// The Content Dialog item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KryptPadCSApp.Dialogs
{
    public sealed partial class ChangePassphraseDialog : ContentDialog
    {

        #region Properties

        private string _oldPassphrase;
        /// <summary>
        /// gets or sets the old passphrase
        /// </summary>
        public string OldPassphrase
        {
            get { return _oldPassphrase; }
            set
            {
                _oldPassphrase = value;
                // Enable primary button?
                IsPrimaryButtonEnabled = CanChangePassphrase;
            }
        }


        private string _newPassphrase;
        /// <summary>
        /// Gets or sets the new passphrase
        /// </summary>
        public string NewPassphrase
        {
            get { return _newPassphrase; }
            set
            {
                _newPassphrase = value;
                // Enable primary button?
                IsPrimaryButtonEnabled = CanChangePassphrase;
            }
        }

        
        private string _confirmPassphrase;
        /// <summary>
        /// Gets or sets the confirmed passphrase. Used to determine if the user entered it in correctly
        /// </summary>
        public string ConfirmPassphrase
        {
            get { return _confirmPassphrase; }
            set
            {
                _confirmPassphrase = value;
                // Enable primary button?
                IsPrimaryButtonEnabled = CanChangePassphrase;
            }
        }
        
        #endregion

        public ChangePassphraseDialog()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        #region Events
        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Disable the primary button
            IsPrimaryButtonEnabled = false;

            // Force the dialog to stay open until the operation completes.
            // We will call Hide() when the api calls are done.
            args.Cancel = true;

            try
            {
                // Change the passphrase
                await KryptPadApi.ChangePassphraseAsync(OldPassphrase, NewPassphrase);

                // Done
                await DialogHelper.ShowMessageDialogAsync("Profile passphrase changed successfully.");

                // Hide dialog
                Hide();
            }
            catch (WarningException ex)
            {
                // Operation failed
                await DialogHelper.ShowMessageDialogAsync(ex.Message);
            }
            catch (WebException ex)
            {
                // Operation failed
                await DialogHelper.ShowMessageDialogAsync(ex.Message);
            }

            // Restore the button
            IsPrimaryButtonEnabled = CanChangePassphrase;
        }

        #endregion

        #region Helper methods
        /// <summary>
        /// Gets whether the primary button is enabled or not
        /// </summary>
        public bool CanChangePassphrase =>
            !string.IsNullOrWhiteSpace(OldPassphrase)
            && !string.IsNullOrWhiteSpace(NewPassphrase)
            && NewPassphrase.Equals(ConfirmPassphrase);
        #endregion



    }
}
