
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

        var url = "https://ruokalista.arttukuikka.fi/api/v1/Ruokalista";

        var httpRequest = (HttpWebRequest)WebRequest.Create(url);

        var result = "";
        var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        {
            result = streamReader.ReadToEnd();
        }

       var ruoka = JsonConvert.DeserializeObject<Ruokalista>(result);

        int day = (int)DateTime.Now.DayOfWeek;
        if(day == 1)
        {
            MaanantaiHeader.TextColor = Color.FromArgb("FFA500");
            MaanantaiRuoka.TextColor = Color.FromArgb("FFA500");
        }

        //tee switch juttu

        MaanantaiRuoka.Text = ruoka.Maanantai;
        TiistaiRuoka.Text = ruoka.Tiistai;
        KeskiviikkoRuoka.Text = ruoka.Keskiviikko;
        TorstaiRuoka.Text = ruoka.Torstai;
        PerjantaiRuoka.Text = ruoka.Perjantai;

        
    }

	
}

