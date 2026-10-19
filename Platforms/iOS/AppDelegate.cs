using Foundation;
using UIKit;
using YandexAdsBridge;

namespace CarCustomChecker
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        public override bool FinishedLaunching(UIApplication application, NSDictionary? launchOptions)
        {
#if DEBUG
            YABAds.EnableLogging();
#endif
            YABAds.Start(() => Console.WriteLine($"Yandex Mobile Ads {YABAds.SdkVersion} initialized"));
            return base.FinishedLaunching(application, launchOptions);
        }
    }
}
