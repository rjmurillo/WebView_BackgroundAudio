# AGENTS.md

UWP app on modern .NET. A XAML WebView hosts html/main.html. JS drives audio through a C#/WinRT bridge.

## Index

|path|what|
|---|---|
|BackgroundAudio/BackgroundAudio.csproj|UWP app. UseUwp, MSIX, Native AOT, x86/x64/ARM64|
|BackgroundAudio/MainPage.xaml.cs|Creates the WebView. AddWebAllowedObject("mediaPlayer", PlaybackService.Instance)|
|BackgroundAudio/html/js/main.js|JS caller of the mediaPlayer bridge|
|BackgroundAudioWinRT/PlaybackService.cs|[AllowForWeb] sealed class exposed to JS|
|BackgroundAudioWinRT/BackgroundAudioWinRT.csproj|CsWinRTComponent, UseUwpTools=false, no XAML|
|global.json|SDK pin. Renovate bumps it|

## Rules

|rule|why|
|---|---|
|Build with full Visual Studio MSBuild on Windows, not `dotnet build`|`dotnet build` skips the UWP XAML compiler. `InitializeComponent` goes missing|
|Keep both TFMs at `net10.0-windows10.0.26100.0`, min `10.0.17763.0`|UWP XAML on .NET needs SDK 26100+. 17763 is the lowest .NET Windows TFM|
|Keep `UseUwp`, `EnableMsixTooling`, `PublishAot`, `DisableRuntimeMarshalling` in the app|These make the .NET 10 UWP and Native AOT build work|
|Do not try a Linux build|`cswinrt.exe` is a Windows binary. It fails with `Exec format error`|
|Do not add CI that runs the app or sideloads the MSIX|The 2018 test cert expired (APPX0108). No runner can run UWP audio or the WebView bridge|
|CI may build and pack only, with `/p:AppxPackageSigningEnabled=false`|Unsigned MSIX is the most CI can prove. Report runtime as not verified|
|`PlaybackService` stays a public sealed WinRT class with `[AllowForWeb]` in BackgroundAudioWinRT|`AddWebAllowedObject` needs a WinRT type with that attribute. JS loses the bridge otherwise|
|Do not move `PlaybackService` into the app project|AddWebAllowedObject docs require a separate WinRT component. The type ships in BackgroundAudioWinRT.winmd|
