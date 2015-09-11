﻿using System;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace KryptPadCSApp.UserControls
{
    public sealed partial class AuthenticateUserControl : UserControl
    {
        public AuthenticateUserControl()
        {
            this.InitializeComponent();
        }

        private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            //fire command if user hit enter
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                e.Handled = true;

                //execute the command of our button
                if (UnlockButton.Command.CanExecute(null))
                {
                    UnlockButton.Command.Execute(null);
                }
            }
        }
    }
}
