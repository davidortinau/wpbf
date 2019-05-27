using System;
using System.Threading.Tasks;
using UIKit;
using WebKit;
using WhitePaperBible.iOS.Renderers;
using WhitePaperBible.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(WhitePaperBible.Views.WebView), typeof(CustomWebViewRender))]
namespace WhitePaperBible.iOS.Renderers
{
    public class CustomWebViewRender : WkWebViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            //if (this.NativeView != null && e.NewElement != null)
            //{
            //    this.InitializeCommands((WebViewer) e.NewElement);
            //}

            var webView = e.NewElement as Views.WebView;
            //if (webView != null)
            //{
            //    webView.EvaluateJavascript = js => { return Task.FromResult(this.EvaluateJavascript(js)); };
            //}

            var ctl = (WKWebView)this.NativeView;
            if (ctl != null)
            {
                ctl.ScrollView.Scrolled += ScrollView_Scrolled;
                ctl.ScrollView.ZoomScale = 4;

            }
        }

        private void ScrollView_Scrolled(object sender, EventArgs e)
        {
            (this.Element as Views.WebView).ScrollY = (this.NativeView as WKWebView).ScrollView.ContentOffset.Y;
        }
    }

}


