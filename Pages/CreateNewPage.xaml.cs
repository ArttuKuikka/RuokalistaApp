using Newtonsoft.Json;
using RuokalistaApp.Models;
using System.Net;
using System.Text;
using RuokalistaApp.Misc;
namespace RuokalistaApp.Pages;

public partial class CreateNewPage : ContentPage
{
	public CreateNewPage()
	{
		InitializeComponent();

		VuosiEntry.Text = DateTime.Now.Year.ToString();
		ViikkoEntry.Text = System.Globalization.ISOWeek.GetWeekOfYear(DateTime.Now).ToString();

    }

	private async void Kuvatunnistus_Clicked(object sender, EventArgs e)
	{
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                // save the file into local storage
                string localFilePath = Path.Combine(FileSystem.CacheDirectory, "ruokalista.jpg");

                using Stream sourceStream = await photo.OpenReadAsync();
                using FileStream localFileStream = File.OpenWrite(localFilePath);

                await sourceStream.CopyToAsync(localFileStream);

                sourceStream.Dispose();
                localFileStream.Dispose();

                var ocr = new OCR();
                try
                {
                    await ocr.PerformOCR();
                }
                catch(Exception err)
                {
                    await DisplayAlert("Virhe", err.Message, "Ok");
                }
            }
        }
    }

    private async void Valmis_Clicked(object sender, EventArgs e)
    {
		VaroitusLabel.Text = "";

		var vuosioikein = int.TryParse(VuosiEntry.Text, out int vuosiparsed);
		var viikkooikein = int.TryParse(ViikkoEntry.Text, out int viikkoparsed);
		if (!vuosioikein || vuosiparsed < 2000 || vuosiparsed > 2050)
		{
			VaroitusLabel.Text += "\nVuosi v‰‰rin";
			return;
		}
		if (!viikkooikein)
		{
			VaroitusLabel.Text += "\nViikko v‰‰rin";
			return;
		}
		if(MaanantaiEntry.Text == null || TiistaiEntry.Text == null || KeskiviikkoEntry.Text == null || TorstaiEntry.Text == null || PerjantaiEntry.Text == null)
		{
            VaroitusLabel.Text += "\nTyhji‰ kentti‰";
            return;
        }
		if(MaanantaiEntry.Text.Length < 1 || TiistaiEntry.Text.Length < 1 || KeskiviikkoEntry.Text.Length < 1 || TorstaiEntry.Text.Length < 1 || PerjantaiEntry.Text.Length < 1)
		{
            VaroitusLabel.Text += "\nTyhji‰ kentti‰";
			return;
        }

		var r = new Ruokalista()
		{
			WeekId = viikkoparsed,
			Year = vuosiparsed,
			Maanantai = MaanantaiEntry.Text,
			Tiistai = TiistaiEntry.Text,
			Keskiviikko = KeskiviikkoEntry.Text,
			Torstai = TorstaiEntry.Text,
			Perjantai = PerjantaiEntry.Text
        };
        var json = JsonConvert.SerializeObject(r);

        HttpContent c = new StringContent(json, Encoding.UTF8, "application/json");

        
        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.Default.GetAsync("token"));
            HttpRequestMessage request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://ruokalista.arttukuikka.fi/api/v1/Ruokalista/Create"),
                Content = c,
                
            };

            try
            {
                HttpResponseMessage result = await client.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        await DisplayAlert("Virhe", "Virheellinen k‰ytt‰j‰tunnus tai salasana, kirjaudu uudelleen sis‰‰n", "Ok");
                        return;
                    }
                    else
                    {
                        await DisplayAlert("Virhe " + result.StatusCode.ToString(), "Virhe yhdist‰ess‰ palvelimeen tai virhe palvelimella", "Ok");
                        return;
                    }
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Virhe yhdist‰ess‰ palvelimeen", "Virhe: " + ex.Message, "Ok");
                return;
            }
        }
    }

    private async void Peruuta_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..", true);
    }
}