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

        public static AppShell Shell => Current.MainPage as AppShell;

        private static NavigableElement navigationRoot;

        public static NavigableElement NavigationRoot
        {
            get => GetShellSection(navigationRoot) ?? navigationRoot;
            set => navigationRoot = value;
        }

        // It provides a navigatable section for elements which aren't explicitly defined within the Shell. For example,
        // ProductCategoryPage: it's accessed from the fly-out through a MenuItem but it doesn't belong to any section
        internal static ShellSection GetShellSection(Element element)
        {
            if (element == null)
            {
                return null;
            }

            var parent = element;
            var parentSection = parent as ShellSection;

            while (parentSection == null && parent != null)
            {
                parent = parent.Parent;
                parentSection = parent as ShellSection;
            }

            return parentSection;
        }

        internal static async Task NavigateBackAsync()
        {
            await NavigationRoot.Navigation.PopAsync();
        }

        internal static async Task NavigateModallyBackAsync() => await NavigationRoot.Navigation.PopModalAsync();

        internal static async Task NavigateToAsync(Page page, bool closeFlyout = false)
        {
            //if (closeFlyout)
            //{
            //    await AppShell.CloseFlyoutAsync();
            //}

            await NavigationRoot.Navigation.PushAsync(page, true);//.ConfigureAwait(false);


        }

        internal static async Task NavigateModallyToAsync(Page page, bool animated = true)
        {
            //await Shell.CloseFlyoutAsync();
            await NavigationRoot.Navigation.PushModalAsync(page, animated).ConfigureAwait(false);
        }
    }
}
