using System;
using System.Collections.Generic;
using WhitePaperBible.Views;
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
		}

		void Handle_Navigating(object sender, Xamarin.Forms.ShellNavigatingEventArgs e)
		{
			Console.WriteLine($"SOURCE: {e?.Source.ToString()} | CURRENT: {e.Current?.Location.ToString()} | TARGET: {e.Target?.Location.ToString()}");

		}


	}
}
