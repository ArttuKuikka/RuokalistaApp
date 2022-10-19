using Newtonsoft.Json;
using System.Net;
using System.Text;
using RuokalistaApp.Models;
using Newtonsoft.Json.Linq;

namespace RuokalistaApp.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	private async void Button_Clicked(object sender, EventArgs e)
	{
        var item = new LoginModel()
        {
            id = 0,
            username = SahkopostiEntry.Text,
            password = SalasanaEntry.Text
        };

        HttpContent c = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

        var response = string.Empty;
        using (var client = new HttpClient())
        {
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://ruokalista.arttukuikka.fi/api/Authenticate/login"),
                Content = c
            };

            HttpResponseMessage result = await client.SendAsync(request);
            if (result.IsSuccessStatusCode)
            {
                response = await result.Content.ReadAsStringAsync();
            }
            else
            {
                if(result.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await DisplayAlert("Virhe", "Virheellinen käyttäjätunnus tai salasana", "Ok");
                    return;
                }
                else
                {
                    await DisplayAlert("Virhe " + result.StatusCode.ToString(), "Virhe yhdistäessä palvelimeen tai virhe palvelimella", "Ok");
                    return;
                }
            }
        }
        JObject json = null;
        try
        {
            json = JObject.Parse(response);
        }
        catch (Exception er)
        {
            await DisplayAlert("Virhe palvelimen vastuksen käsittelyssä", er.Message, "Ok");
        }
        if(json != null)
        {
            await SecureStorage.Default.SetAsync("token", json["token"].ToString());
            Preferences.Default.Set("IsAdmin", true);
            await DisplayAlert("Kirjautuminen onnistui", "Käynnistä sovellus uudelleen muutos tallentamiseksi", "Ok");
            Application.Current.Quit();
            //tee ehkä et vaan goto admin sivulle
        }
        else
        {
            await DisplayAlert("Virhe palvelimen vastuksen käsittelyssä", "Json was null", "Ok");
        }
        

    }
}