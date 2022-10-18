
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using RuokalistaApp.Models;
using System.Drawing;
using Color = Microsoft.Maui.Graphics.Color;
using Java.Time.Temporal;

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

        

        var url = "https://ruokalista.arttukuikka.fi/api/v1/Ruokalista";

        var httpRequest = (HttpWebRequest)null;
        
        try
        {
            httpRequest = (HttpWebRequest)WebRequest.Create(url);
        }
        catch (Exception e)
        {
            await DisplayAlert("Error", e.Message, "K");
        }

        var result = "";
        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            result = streamReader.ReadToEnd();
        }

        var ruoka = JsonConvert.DeserializeObject<Ruokalista>(result);

        File.WriteAllText(Path.Combine(FileSystem.Current.CacheDirectory, "ruoka.json"), result);

        var lista = new List<Day>();
        lista.Add(new Day() { ruoka = ruoka.Maanantai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Monday) });
        lista.Add(new Day() { ruoka = ruoka.Tiistai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Tuesday) });
        lista.Add(new Day() { ruoka = ruoka.Keskiviikko, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Wednesday) });
        lista.Add(new Day() { ruoka = ruoka.Torstai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Thursday) });
        lista.Add(new Day() { ruoka = ruoka.Perjantai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Friday) });

        
        MainStack.Children.Clear();

        MainStack.Children.Add(new Label() { Text = String.Format("Tämän\nviikon ({0})\nruokalista", ruoka.WeekId.ToString()), FontSize = 45, FontAttributes = FontAttributes.Bold });

        

        var paivat = new Dictionary<int, string>() { { 1,"Maanantai"}, { 2, "Tiistai"}, { 3, "Keskiviikko"}, { 4, "Torstai"}, { 5, "Perjantai"} };

        foreach (var paiva in lista)
        {
           if(paiva.dateTime.Day == DateTime.Now.Day)
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

