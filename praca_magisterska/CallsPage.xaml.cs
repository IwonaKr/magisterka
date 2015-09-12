using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using System.Diagnostics;

namespace praca_magisterska
{
    public partial class CallsPage : PhoneApplicationPage
    {
        public CallsPage()
        {
            InitializeComponent();
        }

        private void btnMakeCall_Click(object sender, RoutedEventArgs e)
        {
            this.makeCall();
        }

        private void makeCall()
        {
            string phoneNumber = this.inputPhoneNumber.Text,
                        displayName = this.inputDisplayName.Text;
            if (phoneNumber.Length > 2)
            {
                PhoneCallTask phoneCallTask = new PhoneCallTask();
                phoneCallTask.PhoneNumber = phoneNumber;
                phoneCallTask.DisplayName = displayName;
                phoneCallTask.Show();
            }
        }

        private void btnChooseContact_Click(object sender, RoutedEventArgs e)
        {
            PhoneNumberChooserTask phoneNumberChooserTask = new PhoneNumberChooserTask();
            phoneNumberChooserTask.Completed += phoneNumberChooser_Completed;
            phoneNumberChooserTask.Show();
        }

        private void phoneNumberChooser_Completed(object sender, PhoneNumberResult e)
        {
            this.inputPhoneNumber.Text = e.PhoneNumber == null ? "" : e.PhoneNumber;
            this.inputDisplayName.Text = e.DisplayName == null ? "" : e.DisplayName;

            this.makeCall();
        }
    }
}