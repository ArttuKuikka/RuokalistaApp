namespace RuokalistaApp.Pages;

public partial class CreateNewPage : ContentPage
{
	public CreateNewPage()
	{
		InitializeComponent();

		VuosiEntry.Text = DateTime.Now.Year.ToString();
		ViikkoEntry.Text = System.Globalization.ISOWeek.GetWeekOfYear(DateTime.Now).ToString();

    }

	private void Kuvatunnistus_Clicked(object sender, EventArgs e)
	{

	}

    private void Valmis_Clicked(object sender, EventArgs e)
    {

    }

    private async void Peruuta_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
    }
}