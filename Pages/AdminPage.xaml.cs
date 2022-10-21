

using Newtonsoft.Json;
using RuokalistaApp.Models;

namespace RuokalistaApp.Pages;

public partial class AdminPage : ContentPage
{
	public AdminPage()
	{
		InitializeComponent();
		dothething();

	}

	public async void dothething()
	{
		await Load();
	}

	public async Task Load()
	{
		RuokaView.Children.Clear();

        var result = "";
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.Default.GetAsync("token"));
            client.BaseAddress = new Uri("https://ruokalista.arttukuikka.fi/");

            //GET Method
            HttpResponseMessage response = await client.GetAsync("api/v1/Ruokalista/Get/20");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();

            }
            else
            {
                if(response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await DisplayAlert("Virhe " + response.StatusCode.ToString(), "Kirjautumisesi on vanhentunut, kirjaudu uudelleen sisään asetukset välilehdeltä", "ok");
                    Preferences.Default.Set("IsAdmin", false);
                }
                else
                {
                    await DisplayAlert("Virhe " + response.StatusCode.ToString(), "virhe ladatessa sisältöä", "ok");
                }
            }
        }

        var ruokaarray = JsonConvert.DeserializeObject<Ruokalista[]>(result);

        foreach(var ruoka in ruokaarray)
        {
            var Ruoka = new HorizontalStackLayout() { Padding = new Thickness(0, 5), Spacing = 15 };
            var tekstit = new VerticalStackLayout();
            tekstit.Children.Add(new Label() { FontSize = 20, Text = "Viikko " + ruoka.WeekId.ToString() });
            tekstit.Children.Add(new Label() { Text = ruoka.Year.ToString() });
            Ruoka.Children.Add(tekstit);
            var tiedot = new Button() { Text = "Tiedot" };
            var navigationParameter = new Dictionary<string, object>
    {
         ["Ruokalista"] = ruoka
    };
            tiedot.Clicked += async (sender, args) => await Shell.Current.GoToAsync("Tiedot", navigationParameter);
            Ruoka.Children.Add(tiedot);
            Ruoka.Children.Add(new Button() { Text = "Muokkaa" });
            RuokaView.Children.Add(Ruoka);
        }

        

	}

	private async void CreateNewButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("LuoUusi");
	}
}