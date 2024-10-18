using System;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
namespace RuokalistaApp.Pages;

public partial class SettingsPage : ContentPage
{
    private static int SecretCounter = 0;
	public SettingsPage()
	{
		InitializeComponent();

        CopyrightLabel.Text = $"© {DateTime.Now.Year} Arttu Kuikka";

		ThemePicker.SelectedIndex = Preferences.Default.Get("Teema", 0);
        if(Preferences.Default.Get("IsAdmin", false) == true)
        {
            LoginButton.Text = "Kirjaudu ulos";
        }
    }

	private async void Button_Clicked(object sender, EventArgs e)
	{
        if (Email.Default.IsComposeSupported)
        {

            string subject = "Ruokalista BugReport";
            string body = "Kirjoita tähän lyhyt kuvaus ongelmastasi, kerro myös laitteesi merkki ja malli";
            string[] recipients = new[] { "ruokalista@arttukuikka.fi" };

            var message = new EmailMessage
            {
                Subject = subject,
                Body = body,
                BodyFormat = EmailBodyFormat.PlainText,
                To = new List<string>(recipients)
            };

            await Email.Default.ComposeAsync(message);
        }
        else
        {
            await DisplayAlert("Automaattinen viestin lähettäminen epäonnistui", "Lähetä tietoa bugeista sähköpostilla osoitteeseen ruokalista@arttukuikka.fi", "OK");
        }
    }

   

    private void picker_SelectedIndexChanged(object sender, EventArgs e)
    {
        

       if(ThemePicker.SelectedIndex == 1)
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
            Preferences.Default.Set("Teema", 1);
        }
       else if(ThemePicker.SelectedIndex == 2)
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            Preferences.Default.Set("Teema", 2);
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Unspecified;
            Preferences.Default.Set("Teema", 0);
        }

       
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        if(LoginButton.Text == "Kirjaudu ulos")
        {
			SecureStorage.Default.RemoveAll();
			Preferences.Default.Set("IsAdmin", false);
			await DisplayAlert("Kirjauduttu ulos", "Viimeistelläksi uloskirjautumisen käynnistä sovellus uudelleen", "Ok");
			App.Current.Quit();
		}
        else
        {
			await Shell.Current.GoToAsync("LoginPage");
		}
        
    }

	private async void OnLabelTapped(object sender, EventArgs e)
	{
		var label = sender as Label;
		var argument = (string)((TapGestureRecognizer)label.GestureRecognizers[0]).CommandParameter;

		// Handle the tap event and use the argument
		if(argument == "gh")
        {
			await Launcher.OpenAsync(new Uri("https://github.com/ArttuKuikka"));

		}
        else if(argument == "rl")
        {
			await Launcher.OpenAsync(new Uri("https://ruokalista.arttukuikka.fi"));
		}
		else if (argument == "secret")
		{
            SecretCounter++;

            if(SecretCounter > 2)
            {


				string text = $"{6 - SecretCounter} klikkauksta jäljellä";
				ToastDuration duration = ToastDuration.Short;
				double fontSize = 14;

				var toast = Toast.Make(text, duration, fontSize);

				await toast.Show();
			}

            if(SecretCounter >= 6)
            {
                var isDev = Preferences.Default.Get("IsDeveloper", false);

                if (!isDev)
                {
                    Preferences.Default.Set("IsDeveloper", true);


					string text = $"Olet nyt kehittäjä, käynnistä sovellus uudelleen!";
					ToastDuration duration = ToastDuration.Long;
					double fontSize = 14;

					var toast = Toast.Make(text, duration, fontSize);

					await toast.Show();
				}
                else
                {
					Preferences.Default.Set("IsDeveloper", false);

					string text = $"Et ole enään kehittäjä, käynnistä sovellus uudelleen!";
					ToastDuration duration = ToastDuration.Long;
					double fontSize = 14;

					var toast = Toast.Make(text, duration, fontSize);

					await toast.Show();
				}
			}
		}
	}
}