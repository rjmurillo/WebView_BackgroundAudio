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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BackgroundAudio
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static readonly Uri Selection = new Uri("ms-appx-web:///Assets/selection.html");
        private static readonly Uri Player = new Uri("ms-appx-web:///Assets/player.html");

        public MainPage()
        {
            this.InitializeComponent();

            this.Loaded += OnLoaded;

            App.Current.EnteredBackground += Current_EnteredBackground;
            App.Current.LeavingBackground += Current_LeavingBackground;
        }

        private void Current_EnteredBackground(object sender, Windows.ApplicationModel.EnteredBackgroundEventArgs e)
        {
            if (wv1 != null)
            {
                wv1 = null;
            }
            if (wv2 != null)
            {
                wv2 = null;
            }
        }

        private void Current_LeavingBackground(object sender, Windows.ApplicationModel.LeavingBackgroundEventArgs e)
        {
            Load();
        }

        private WebView wv1 = null;
        private WebView wv2 = null;
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void Load()
        {
            if (wv1 == null)
            {
                wv1 = CreateWebView(0);
                wv1.Source = Selection;
            }
            if (wv2 == null)
            {
                wv2 = CreateWebView(1);
                wv2.Source = Player;
            }
        }

        private WebView CreateWebView(int row)
        {
            var wv = new WebView(WebViewExecutionMode.SeparateProcess);
            wv.Settings.IsJavaScriptEnabled = true;
            wv.AddWebAllowedObject("mediaPlayer", PlaybackService.Instance);
            WireUpWebViewDiagnostics(wv);
            Grid.SetRow(wv, row);
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
////                if (wv.Source.PathAndQuery.Equals(Selection.PathAndQuery, StringComparison.OrdinalIgnoreCase))
////                {
////                    Debug.WriteLine("Invoking script on " + wv.Source + ". Args: " + e.Value);
////#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
////                    Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
////                    {
////                        var wv2 = this.Grid.Children[1] as WebView;
////                        wv2?.InvokeScriptAsync("mediaPlayback", new[] { e.Value });
////                    });
////#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
////                } else if (wv.Source.PathAndQuery.Equals(Player.PathAndQuery, StringComparison.OrdinalIgnoreCase))
////                {
////                    var wv1 = this.Grid.Children[0] as WebView;
////                    wv1?.InvokeScriptAsync("loadState", new []{e.Value});
////                }
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
    }
}
