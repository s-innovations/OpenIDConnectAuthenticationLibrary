﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Thinktecture.IdentityModel.Client;
using Thinktecture.Samples;

namespace OpenIDConnectAuthenticationLibrary
{
    public class OpenIDConnectContext
    {
        private readonly string m_authorizeEndpoint;
        private readonly string m_clientId;
        private readonly string m_callback;
        public OpenIDConnectContext(string authorizeEndpoint, string clientid , string callback)
        {
            m_authorizeEndpoint = authorizeEndpoint;
            m_clientId = clientid;
            m_callback = callback;
        }

        public Task<AuthorizeResponse> RequestTokenAsync(string scope, string responseType)
        {
            LoginWebView webview = null;
            var task = StartSTATask<AuthorizeResponse>(() =>
            {

                Application app = new Application();
                webview = new LoginWebView();
                webview.RequestToken(m_clientId, m_authorizeEndpoint, scope, responseType, m_callback);
                app.Run(webview);
                
                return webview.Completion.Result;              

            });
            return task;
           

        }
        public static Task<T> StartSTATask<T>(Func<T> func)
        {
            var tcs = new TaskCompletionSource<T>();
            Thread thread = new Thread(() =>
            {
                try
                {
                    tcs.SetResult(func());
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            return tcs.Task;
        }
    }
}
