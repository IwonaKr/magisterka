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

namespace praca_magisterska
{
    public partial class MessagesPage : PhoneApplicationPage
    {
        public MessagesPage()
        {
            InitializeComponent();
        }

        private void btn_sendSms_Click(object sender, RoutedEventArgs e)
        {
            this.sendSms();
        }
        private void sendSms()
        {
            string phoneNumber = this.inputPhoneNumber.Text,
                        messageText = this.inputMessage.Text;
            if (phoneNumber.Length > 2)
            {
                SmsComposeTask smsComposeTask = new SmsComposeTask();
                smsComposeTask.To = phoneNumber;
                smsComposeTask.Body = messageText;
                smsComposeTask.Show();
            }
        }
    }
}