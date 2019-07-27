using Android.Content;
using Android.OS;
using Android.Views;
using WhitePaperBible.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WhitePaperBible.Views.WebView), typeof(CustomWebViewRender))]
namespace WhitePaperBible.Droid.Renderers
{
    public class CustomWebViewRender : WebViewRenderer
    {
        public CustomWebViewRender(Context ctx) : base(ctx)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);

            var oldWebView = e.OldElement as WhitePaperBible.Views.WebView;
            if (oldWebView != null)
            {
                oldWebView.EvaluateJavascript = null;
            }

            var newWebView = e.NewElement as WhitePaperBible.Views.WebView;
            if (newWebView != null)
            {

            }

            if (this.Control != null && e.NewElement != null)
            {
                this.SetupControl();
            }

            
        }

        /// <summary>
        ///     Sets up various settings for the Android WebView
        /// </summary>
        private void SetupControl()
        {
            // Ensure common functionality is enabled
            this.Control.Settings.DomStorageEnabled = true;
            this.Control.Settings.JavaScriptEnabled = true;

            // Must remove minimum font size otherwise SAP PDF's go massive
            this.Control.Settings.MinimumFontSize = 0;

            // Because Android 4.4 and below doesn't respect ViewPort in HTML
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                this.Control.Settings.UseWideViewPort = true;
            }

            this.Control.ScrollChange += Control_ScrollChange;
        }

        private void Control_ScrollChange(object sender, ScrollChangeEventArgs e)
        {
            (this.Element as Views.WebView).ScrollY = e.ScrollY;
        }

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            Parent.RequestDisallowInterceptTouchEvent(true);
            return base.DispatchTouchEvent(e);
        }
    }
}
