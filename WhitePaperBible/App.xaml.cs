using MonkeyCache.SQLite;
using System;
using System.Threading.Tasks;
using MonkeyCache;
using TinyMessenger;
using WhitePaperBible.Core.Models;
using Xamarin.Forms;

namespace WhitePaperBible
{
    public partial class App : Application
    {

        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzczMzAzQDMxMzgyZTM0MmUzMEoxS09YZ0lRdmtxYmxDMmg3Y2NrMzVuYWRPdlR1T0FzcEdQNURqYkJqWkk9");

            InitializeComponent();

            Barrel.ApplicationId = "com.simplyprofound.whitepaperbible";
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            BarrelUtils.SetBaseCachePath(folder);

            DependencyService.Register<AppModel>();
            DependencyService.Register<TinyMessengerHub>();

            InitUser();

            MainPage = new AppShell();
        }

        private void InitUser()
        {
            AppModel m;
            
            //Dev handles checking if cache is expired
            if (Barrel.Current != null && !Barrel.Current.IsExpired(key: nameof(AppModel)))
            {
                m = Barrel.Current.Get<AppModel>(key: nameof(AppModel));

                AppModel AM = DependencyService.Resolve<AppModel>();

                AM.User = m.User;
                AM.Papers = m.Papers;
                AM.Favorites = m.Favorites;
                AM.Tags = m.Tags;
                AM.UserSessionCookie = m.UserSessionCookie;
                AM.IsLoggedIn = (AM.User != null && AM.UserSessionCookie != null);
            }

            //Barrel.Current.Add(key: url, data: monkeys, expireIn: TimeSpan.FromDays(1));
        }
    }
}
