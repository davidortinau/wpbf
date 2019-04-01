using System;
using RestSharp;
using WhitePaperBible.Core.Services;
using WhitePaperBible.Core.Models;
using System.Net;
using WhitePaperBible.Core;
using System.Threading.Tasks;
using System.Threading;
using Xamarin.Forms;

namespace WhitePaperBible.Droid.Services
{
    public class WebClient : IJSONWebClient
    {
        public AppModel AM;

        public async Task OpenURL(string url, MethodEnum method = MethodEnum.GET, bool includeSessionCookie = false)
        {

            AM = DependencyService.Resolve<AppModel>();
            var client = new RestClient();
            if (includeSessionCookie)
            {
                var cookieJar = new CookieContainer();
                cookieJar.Add(new Uri(Constants.BASE_URI), new Cookie(AM.UserSessionCookie.Name, AM.UserSessionCookie.Value));
                client.CookieContainer = cookieJar;
            }

            var request = new RestRequest(url, (Method)method) { RequestFormat = DataFormat.Json };
            var cancellationTokenSource = new CancellationTokenSource();
            var response = await client.ExecuteTaskAsync(request, cancellationTokenSource.Token);
            if (response.Cookies.Count > 0)
            {
                foreach (var cookie in response.Cookies)
                {
                    if (cookie.Name == "_whitepaperbible_session")
                    {
                        UserSessionCookie = new SessionCookie
                        {
                            Name = cookie.Name,
                            Value = cookie.Value
                        };
                    }
                }
            }

            ResponseText = response.Content;
            //			RemoveNetworkActivity (url);
            if (response.ResponseStatus == ResponseStatus.Error)
            {
                DispatchError();
            }
            else
            {
                DispatchComplete();
            }
        }

        public event EventHandler RequestComplete = delegate { };
        public event EventHandler RequestError = delegate { };

        public string ResponseText
        {
            get;
            private set;
        }

        public SessionCookie UserSessionCookie
        {
            get;
            private set;
        }

        public WebClient()
        {
        }

        #region IJSONWebClient implementation
        private void DispatchComplete()
        {
            RequestComplete(this, EventArgs.Empty);
        }

        private void DispatchError()
        {
            RequestError(this, EventArgs.Empty);
        }


        #endregion
    }
}

