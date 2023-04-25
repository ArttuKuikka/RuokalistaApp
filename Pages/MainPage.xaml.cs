
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using RuokalistaApp.Models;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;


namespace RuokalistaApp;

public partial class MainPage : ContentPage
{
	

	public MainPage()
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
        //await Task.Delay(10000);
        
        bool err = false;

        var code = 0;
        var result = "";
        try
        {
           

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://ruokalista.arttukuikka.fi/");

                var url = "api/v1/Ruokalista";
                bool seuraavaViikko = false;
                if(DateTime.Today.DayOfWeek == DayOfWeek.Sunday || DateTime.Today.DayOfWeek == DayOfWeek.Saturday)
                {
                    var nextweek = System.Globalization.ISOWeek.GetWeekOfYear(DateTime.Now) + 1;
					url = $"api/v1/Ruokalista/{DateTime.Now.Year}/{nextweek}";
                    seuraavaViikko=true;
                }
                
                //GET Method
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    
                }
                else
                {
                    code = (int)response.StatusCode;
                    if(code == 404)
                    {
						MainStack.Children.Clear();
						if (seuraavaViikko)
                        {
							MainStack.Children.Add(new Label() { Text = "Seuraavan viikon ruokalistaa ei ole vielä julkaistu", FontSize = 45, FontAttributes = FontAttributes.Bold });
						}
                        else
                        {
							MainStack.Children.Add(new Label() { Text = "Tämän viikon ruokalistaa ei ole vielä julkaistu", FontSize = 45, FontAttributes = FontAttributes.Bold });
						}
                        return;
                    }
                    throw new Exception("");
                }
            }
        }
        catch(Exception)
        {
            err = true;
        }

        if(err || result == "" || result == null)
        {
            if(File.Exists(Path.Combine(FileSystem.Current.CacheDirectory, "ruoka.json")) && File.ReadAllText(Path.Combine(FileSystem.Current.CacheDirectory, "ruoka.json")) != null)
            {
                
                App.Current.MainPage.DisplayAlert("Virhe " + code.ToString(), "Virhe yhdistäessä palvelimeen, näytetaan viimeisin paikallisesti tallennettu versio ruokalistasta", "OK");

                result = File.ReadAllText(Path.Combine(FileSystem.Current.CacheDirectory, "ruoka.json"));
            }
            else
            {
                
                await App.Current.MainPage.DisplayAlert("Virhe " + code.ToString(), "Virhe yhdistäessä palvelimeen, Tarkista verkkoasetukset. Sovellus sulkeutuu automaattisesti", "OK");
                Application.Current.Quit();


            }
        }

        var ruoka = JsonConvert.DeserializeObject<Ruokalista>(result);

        File.WriteAllText(Path.Combine(FileSystem.Current.CacheDirectory, "ruoka.json"), result);

        var lista = new List<Day>();
        try
        {
            lista.Add(new Day() { ruoka = ruoka.Maanantai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Monday) });
            lista.Add(new Day() { ruoka = ruoka.Tiistai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Tuesday) });
            lista.Add(new Day() { ruoka = ruoka.Keskiviikko, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Wednesday) });
            lista.Add(new Day() { ruoka = ruoka.Torstai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Thursday) });
            lista.Add(new Day() { ruoka = ruoka.Perjantai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Friday) });
        }
        catch(Exception)
        {
            await App.Current.MainPage.DisplayAlert("Virhe", "Virhellistä tietoa havaittu, sovelluksen suoritusta ei voida jatkaa", "Ok");
            Application.Current.Quit();
           
        }


        MainStack.Children.Clear();

        MainStack.Children.Add(new Label() { Text = String.Format("Tämän\nviikon ({0})\nruokalista", ruoka.WeekId.ToString()), FontSize = 45, FontAttributes = FontAttributes.Bold });



        var paivat = new Dictionary<int, string>() { { 1, "Maanantai" }, { 2, "Tiistai" }, { 3, "Keskiviikko" }, { 4, "Torstai" }, { 5, "Perjantai" } };

        foreach (var paiva in lista)
        {
            if (paiva.dateTime.Day == DateTime.Now.Day)
            {
                MainStack.Children.Add(new Label() { Text = String.Format("{0} {1}", paivat[(int)paiva.dateTime.DayOfWeek], paiva.dateTime.ToString("dd.MM")), HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold, FontSize = 20, TextColor = Color.FromArgb("FFA500") });
                MainStack.Children.Add(new Label() { Text = paiva.ruoka, FontSize = 20, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center, TextColor = Color.FromArgb("FFA500") });
            }
            else
            {
                MainStack.Children.Add(new Label() { Text = String.Format("{0} {1}", paivat[(int)paiva.dateTime.DayOfWeek], paiva.dateTime.ToString("dd.MM")), HorizontalOptions = LayoutOptions.Center, FontAttributes = FontAttributes.Bold, FontSize = 20 });
                MainStack.Children.Add(new Label() { Text = paiva.ruoka, FontSize = 20, HorizontalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Center });
            }
        }


    }

	
}

