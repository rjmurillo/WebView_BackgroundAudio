using BackgroundAudioWinRT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace BackgroundAudio
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static readonly Uri MainHTMLPageUri = new Uri("ms-appx-web:///html/main.html");

        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += OnLoaded;

            App.Current.EnteredBackground += Current_EnteredBackground;
            App.Current.LeavingBackground += Current_LeavingBackground;
        }

        private void Current_EnteredBackground(object sender, Windows.ApplicationModel.EnteredBackgroundEventArgs e)
        {
            Unload();
        }

        private void Current_LeavingBackground(object sender, Windows.ApplicationModel.LeavingBackgroundEventArgs e)
        {
            Load();
        }

        private WebView wv1 = null;
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Unload()
        {
            Grid.Children.Remove(wv1);
            UnwireWebViewDiagnostics(wv1);

            if (wv1 != null)
            {
                wv1 = null;
            }

            GC.Collect();
        }
        private void Load()
        {
            if (wv1 == null)
            {
                wv1 = CreateWebView();
                wv1.Source = MainHTMLPageUri;
            }
        }

        private WebView CreateWebView()
        {
            var wv = new WebView(WebViewExecutionMode.SeparateProcess);
            wv.Settings.IsJavaScriptEnabled = true;
            wv.AddWebAllowedObject("mediaPlayer", PlaybackService.Instance);
            WireUpWebViewDiagnostics(wv);
            Grid.SetRow(wv, 0);
            this.Grid.Children.Add(wv);
            return wv;
        }

        private void WireUpWebViewDiagnostics(WebView webView)
        {
            webView.NavigationStarting += OnWebViewNavigationStarting;
            webView.SeparateProcessLost += OnWebViewSeparateProcessLost;
            webView.ScriptNotify += OnWebViewScriptNotify;
        }

        private void UnwireWebViewDiagnostics(WebView webView)
        {
            webView.NavigationStarting -= OnWebViewNavigationStarting;
            webView.SeparateProcessLost -= OnWebViewSeparateProcessLost;
            webView.ScriptNotify -= OnWebViewScriptNotify;
        }

        private async void OnWebViewScriptNotify(object sender, NotifyEventArgs e)
        {
            if (sender is WebView wv)
            {
                //If you want to trigger an exteranl event without passing in a WinRT object, 
                // use window.external.notify("some string") which will call this method. The string will 
                // be accessible via e.Value. 
            }
        }

        private void OnWebViewSeparateProcessLost(WebView sender, WebViewSeparateProcessLostEventArgs args)
        {
            UnwireWebViewDiagnostics(sender);
        }



        private void OnWebViewNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            Debug.WriteLine(args.Uri?.ToString());
        }

        private void ButtonUnload_Click(object sender, RoutedEventArgs e)
        {
            Unload();
        }
        private void ButtonLoad_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }
    }
}
