﻿using Foundation;

namespace CarCustomChecker
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp()
        {
            var app = MauiProgram.CreateMauiApp();

            Google.MobileAds.MobileAds.SharedInstance.Start(completionHandler: null);

            return app;
        }
    }
}
