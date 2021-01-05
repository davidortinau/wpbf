using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;
using WhitePaperBible.Views;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.UI.Views.Options;
using Xamarin.Forms;

namespace WhitePaperBible
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();

			Routing.RegisterRoute("paper", typeof(PaperDetailPage));
			Routing.RegisterRoute("login", typeof(LoginModalPage));
			Routing.RegisterRoute("tag/papers", typeof(TagPapersPage));
			Routing.RegisterRoute("view", typeof(ReadTextPage));
		}

		void Handle_Navigating(object sender, Xamarin.Forms.ShellNavigatingEventArgs e)
		{
			Debug.WriteLine($"SOURCE: {e?.Source.ToString()} | CURRENT: {e.Current?.Location.ToString()} | TARGET: {e.Target?.Location.ToString()}");
			
				var querystring = e.Target?.Location.OriginalString;
				if (querystring.IndexOf("?msg") > -1)
				{
					var substring = querystring.Substring(querystring.IndexOf("?"));
					var nvc = HttpUtility.ParseQueryString(substring);
					var options = new SnackBarOptions()
					{ 
						BackgroundColor = Color.FromHex("#CC0000"),
						MessageOptions = new MessageOptions
						{
							Message = nvc["msg"],
							Foreground = Color.White,
							Font =  Font.SystemFontOfSize(16)
						}
					};
					App.Current.MainPage.DisplayToastAsync(options);
				}
		}


	}
}
