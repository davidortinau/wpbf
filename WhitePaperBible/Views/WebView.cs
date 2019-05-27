using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhitePaperBible.Views
{
    public class WebView : Xamarin.Forms.WebView
    {
        public event EventHandler Scrolled;

        public static BindableProperty EvaluateJavascriptProperty =
            BindableProperty.Create(nameof(EvaluateJavascript), typeof(Func<string, Task<string>>), typeof(WebView),
                                    null, BindingMode.OneWayToSource);

        public static BindableProperty RefreshProperty =
            BindableProperty.Create(nameof(Refresh), typeof(Action), typeof(WebView), null,
                                    BindingMode.OneWayToSource);

        public static BindableProperty GoBackProperty =
            BindableProperty.Create(nameof(GoBack), typeof(Action), typeof(WebView), null,
                                    BindingMode.OneWayToSource);

        public static BindableProperty CanGoBackFunctionProperty =
            BindableProperty.Create(nameof(CanGoBackFunction), typeof(Func<bool>), typeof(WebView), null,
                                    BindingMode.OneWayToSource);

        public static BindableProperty GoBackNavigationProperty =
            BindableProperty.Create(nameof(GoBackNavigation), typeof(Action), typeof(WebView), null,
                                    BindingMode.OneWay);

        public Func<string, Task<string>> EvaluateJavascript
        {
            get { return (Func<string, Task<string>>)this.GetValue(EvaluateJavascriptProperty); }
            set { this.SetValue(EvaluateJavascriptProperty, value); }
        }

        public Action Refresh
        {
            get { return (Action)this.GetValue(RefreshProperty); }
            set { this.SetValue(RefreshProperty, value); }
        }

        public Func<bool> CanGoBackFunction
        {
            get { return (Func<bool>)this.GetValue(CanGoBackFunctionProperty); }
            set { this.SetValue(CanGoBackFunctionProperty, value); }
        }

        public Action GoBackNavigation
        {
            get { return (Action)this.GetValue(GoBackNavigationProperty); }
            set { this.SetValue(GoBackNavigationProperty, value); }
        }

        private double _scrollY;
        public double ScrollY
        {
            get
            {
                return _scrollY;
            }
            set
            {
                Debug.WriteLine($"Scrolled Y: {value}");
                _scrollY = value;
                if (Scrolled != null)
                    Scrolled.Invoke(this, EventArgs.Empty);
            }

        }
    }
}
