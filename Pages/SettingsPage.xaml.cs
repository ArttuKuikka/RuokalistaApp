namespace RuokalistaApp.Pages;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();

        ThemePicker.SelectedIndex = Preferences.Default.Get("Teema", 0);
    }

	private async void Button_Clicked(object sender, EventArgs e)
	{
        if (Email.Default.IsComposeSupported)
        {

            string subject = "Ruokalista BugReport";
            string body = "Kirjoita tähän lyhyt kuvaus ongelmastasi, kerro myös laitteesi merkki ja malli";
            string[] recipients = new[] { "arttu@arttukuikka.fi" };

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
            await DisplayAlert("Automaattinen viestin lähettäminen epäonnistui", "Lähetä tietoa bugeista sähköpostilla osoitteeseen arttu@arttukuikka.fi", "ok");
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

    private void Button_Clicked_1(object sender, EventArgs e)
    {
       
    }
}