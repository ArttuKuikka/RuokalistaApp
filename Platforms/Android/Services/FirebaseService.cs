using Android.App;
using AndroidX.Core.App;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuokalistaApp.Platforms.Android.Services
{
    [Service(Exported = true)]
    [IntentFilter(new[] {"com.google.firebase.MESSAGING_EVENT"})]
    public class FirebaseService : FirebaseMessagingService
    {
        public FirebaseService()
        {

        }

        public override void OnNewToken(string token)
        {
            base.OnNewToken(token);
            if (Preferences.ContainsKey("DeviceToken"))
            {
                Preferences.Remove("DeviceToken");
            }
            Preferences.Set("DeviceToken", token);
        }

        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            var notification = message.GetNotification();

            SendNotification(notification.Body, notification.Title, message.Data, notification.ChannelId);
        }

        private void SendNotification(string messageBody, string title, IDictionary<string, string> data, string channel)
        {

            var notificationbuilder = new NotificationCompat.Builder(this, channel)
                .SetContentTitle(title)
                .SetContentTitle(messageBody)
                .SetChannelId(channel)
                .SetSubText(channel)
				.SetSmallIcon(Resource.Drawable.logo);

            var notificationmanager = NotificationManager.FromContext(this);
            notificationmanager.Notify(Random.Shared.Next(0, 100), notificationbuilder.Build());
        }

    }

    
}
