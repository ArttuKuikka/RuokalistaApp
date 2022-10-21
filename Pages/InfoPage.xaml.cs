namespace RuokalistaApp.Pages;
using RuokalistaApp.Models;
using System.Collections.Generic;

public partial class InfoPage : ContentPage, IQueryAttributable
{
	
    public InfoPage()
	{
		InitializeComponent();
		

       
	}

	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		var RuokaL = query["Ruokalista"] as Ruokalista;

        if (RuokaL != null)
        {
            ViikkoLabel.Text = RuokaL.WeekId.ToString();
            VuosiLabel.Text = RuokaL.Year.ToString();
            MaanantaiLabel.Text = RuokaL.Maanantai;
            TiistaiLabel.Text = RuokaL.Tiistai;
            KeskiviikkoLabel.Text = RuokaL.Keskiviikko;
            TorstaiLabel.Text = RuokaL.Torstai;
            PerjantaiLabel.Text = RuokaL.Perjantai;
        }
        else
        {
            DisplayAlert("Virhe", "virhe ladatessa tietoja", "Ok");
        }

    }

	private async void Takaisin_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", true);
    }
}