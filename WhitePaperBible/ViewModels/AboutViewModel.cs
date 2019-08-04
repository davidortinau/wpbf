using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WhitePaperBible.ViewModels
{
    public class AboutViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OpenURL { get; set; }

        public ICommand SendEmail { get; set; }

        public AboutViewModel()
        {
            OpenURL = new Command<string>(async (url) =>
            {
                await Xamarin.Essentials.Browser.OpenAsync(url);
            });

            SendEmail = new Command<string>(async (address) => await OnSendEmail(address));
        }

        public async Task OnSendEmail(string address)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "White Paper Bible App",
                    Body = "",
                    To = new List<string>() { address },
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

    }

}

