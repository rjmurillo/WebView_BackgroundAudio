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
|global.json|SDK pin, now 11.0.100. PR #4 verified the build on SDK 10.0.403 only|

## Rules

|rule|why|
|---|---|
|Build with full Visual Studio MSBuild on Windows, not `dotnet build`|`dotnet build` skips the UWP XAML compiler. `InitializeComponent` goes missing|
|Keep both `TargetFramework` at `net10.0-windows10.0.26100.0` and `TargetPlatformMinVersion` at `10.0.17763.0`|UWP XAML on .NET needs SDK 26100+. 17763 is the lowest Windows version .NET supports|
|Keep `UseUwp`, `EnableMsixTooling`, `PublishAot`, `DisableRuntimeMarshalling` in the app|PR #4 needed these for the UWP and Native AOT build|
|Do not try a Linux build|`cswinrt.exe` is a Windows binary. It fails with `Exec format error`|
|Do not add CI that runs the app or sideloads the MSIX|The committed test cert expired (APPX0108), so CI cannot sign or install the package. Runtime audio and the WebView bridge stay manual checks|
|CI may build and pack only, with `/p:AppxPackageSigningEnabled=false`|Unsigned MSIX is the most CI can prove. Report runtime as not verified|
|`PlaybackService` stays a public sealed `[AllowForWeb]` class in BackgroundAudioWinRT, never in the app|`AddWebAllowedObject` requires an `AllowForWeb` class from a separate WinRT component. JS loses `mediaPlayer` otherwise|
