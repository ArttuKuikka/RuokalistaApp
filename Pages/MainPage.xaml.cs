
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

        Header.Text = Header.Text.Replace("00", ruoka.WeekId.ToString());

        MaanantaiHeader.Text = MaanantaiHeader.Text.Replace("00.00", System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Monday).ToString("dd.MM"));
        TiistaiHeader.Text = TiistaiHeader.Text.Replace("00.00", System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Tuesday).ToString("dd.MM"));
        KeskiviikkoHeader.Text = KeskiviikkoHeader.Text.Replace("00.00", System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Wednesday).ToString("dd.MM"));
        TorstaiHeader.Text = TorstaiHeader.Text.Replace("00.00", System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Thursday).ToString("dd.MM"));
        PerjantaiHeader.Text = PerjantaiHeader.Text.Replace("00.00", System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Friday).ToString("dd.MM"));

        int day = (int)DateTime.Now.DayOfWeek;



        switch (day)
        {
            case 1:
                MaanantaiHeader.TextColor = Color.FromArgb("FFA500");
                MaanantaiRuoka.TextColor = Color.FromArgb("FFA500");
                break;
            case 2:
                TiistaiHeader.TextColor = Color.FromArgb("FFA500");
                TiistaiRuoka.TextColor = Color.FromArgb("FFA500");
                break;
            case 3:
                KeskiviikkoHeader.TextColor = Color.FromArgb("FFA500");
                KeskiviikkoRuoka.TextColor = Color.FromArgb("FFA500");
                break;
            case 4:
                TorstaiHeader.TextColor = Color.FromArgb("FFA500");
                TorstaiRuoka.TextColor = Color.FromArgb("FFA500");
                break;
            case 5:
                PerjantaiHeader.TextColor = Color.FromArgb("FFA500");
                PerjantaiRuoka.TextColor = Color.FromArgb("FFA500");
                break;
            default:
                break;

        }


        MaanantaiRuoka.Text = ruoka.Maanantai;
        TiistaiRuoka.Text = ruoka.Tiistai;
        KeskiviikkoRuoka.Text = ruoka.Keskiviikko;
        TorstaiRuoka.Text = ruoka.Torstai;
        PerjantaiRuoka.Text = ruoka.Perjantai;
    }

	
}

