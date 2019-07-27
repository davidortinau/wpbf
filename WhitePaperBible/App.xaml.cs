using System.Threading.Tasks;
using WhitePaperBible.Core.Models;
using Xamarin.Forms;

namespace WhitePaperBible
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<AppModel>();

            MainPage = new AppShell();
        }

    }
}
