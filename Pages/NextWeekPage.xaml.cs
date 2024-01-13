using Newtonsoft.Json;
using RuokalistaApp.Models;

namespace RuokalistaApp.Pages;

public partial class NextWeekPage : ContentPage, IQueryAttributable
{
    public NextWeekPage()
	{
		InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		var response = query["response"] as HttpResponseMessage;

        var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

        var ruoka = JsonConvert.DeserializeObject<Ruokalista>(result);

        var lista = new List<Day>();
        try
        {
            lista.Add(new Day() { ruoka = ruoka.Maanantai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Monday) });
            lista.Add(new Day() { ruoka = ruoka.Tiistai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Tuesday) });
            lista.Add(new Day() { ruoka = ruoka.Keskiviikko, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Wednesday) });
            lista.Add(new Day() { ruoka = ruoka.Torstai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Thursday) });
            lista.Add(new Day() { ruoka = ruoka.Perjantai, dateTime = System.Globalization.ISOWeek.ToDateTime(ruoka.Year, ruoka.WeekId, DayOfWeek.Friday) });
        }
        catch (Exception)
        {
            DisplayAlert("Virhe", "Virhe ladatessa tietoja!", "Ok");

        }


        MainStack.Children.Clear();

        MainStack.Children.Add(new Label() { Text = String.Format("Seuraavan\nviikon ({0})\nruokalista", ruoka.WeekId.ToString()), FontSize = 45, FontAttributes = FontAttributes.Bold });



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

