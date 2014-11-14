using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Thinktecture.IdentityModel.Client;

namespace Thinktecture.Samples
{
    public partial class LoginWebView : Window
    {
        public AuthorizeResponse AuthorizeResponse { get; set; }
        public event EventHandler<AuthorizeResponse> Done;
        public TaskCompletionSource<AuthorizeResponse> _source = new TaskCompletionSource<AuthorizeResponse>();
        Uri _callbackUri;

        private string _clientid;

        public LoginWebView(string clientid)
        {
            InitializeComponent();
            webView.Navigating += webView_Navigating;

            Closing += LoginWebView_Closing;
            _clientid = clientid;
        }
        public Task<AuthorizeResponse> Completion
        {
            get
            {
                return _source.Task;
            }
        }
        void LoginWebView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          //  e.Cancel = true;
            this.Visibility = Visibility.Hidden;
            if(!_source.Task.IsCompleted)
                _source.SetResult(null);
        }

        public void Start(Uri startUri, Uri callbackUri)
        {
            _callbackUri = callbackUri;
            webView.Navigate(startUri);
        }

        private void webView_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (e.Uri.ToString().StartsWith(_callbackUri.AbsoluteUri))
            {
                AuthorizeResponse = new AuthorizeResponse(e.Uri.AbsoluteUri);

                //e.Cancel = true;
                this.Visibility = System.Windows.Visibility.Hidden;
               
                if (Done != null)
                {
                    Done.Invoke(this, AuthorizeResponse);
                }
                _source.SetResult(AuthorizeResponse);
                 this.Close();
            }
        }

        public void RequestToken(string authuri, string scope, string responseType)
        {
            var additional = new Dictionary<string, string>
            {
                { "nonce", "nonce" },
                // { "login_hint", "idp:Google" }
            };

            var client = new OAuth2Client(new Uri(authuri));
            var startUrl = client.CreateAuthorizeUrl(
               _clientid,
                responseType,
                scope,
                "oob://localhost/wpfclient",
                "state",
                additional);


            this.Show();
            this.Start(new Uri(startUrl), new Uri("oob://localhost/wpfclient"));
        }
    }
}