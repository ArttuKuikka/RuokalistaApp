using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase.Messaging;

namespace RuokalistaApp;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    internal static readonly string Channel_ID = "Main";
    internal static readonly int NotificationID = 101;
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        CreateNotificationChannel();

        
    }

    private void CreateNotificationChannel()
    {
        FirebaseMessaging.Instance.SubscribeToTopic("Ilmoitukset");

        if(OperatingSystem.IsOSPlatformVersionAtLeast("android", 26))
        {
            var channel = new NotificationChannel(Channel_ID, "kaikki Ilmoitukset", NotificationImportance.Default);

            var notificationmanager = (NotificationManager)GetSystemService(Android.Content.Context.NotificationService);
            notificationmanager.CreateNotificationChannel(channel);
        }
        
    }
}
