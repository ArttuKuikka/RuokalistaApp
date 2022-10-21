using Newtonsoft.Json;
using RuokalistaApp.Models;
using System.Net;
using System.Text;

namespace RuokalistaApp.Pages;

public partial class EditPage : ContentPage, IQueryAttributable
{

    int id = 0;
    private Ruokalista Ruokalista;
	public EditPage()
	{
		InitializeComponent();
	}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        var RuokaL = query["Ruokalista"] as Ruokalista;
        Ruokalista = RuokaL;
        if (RuokaL != null)
        {
            id = RuokaL.Id;
            ViikkoEntry.Text = RuokaL.WeekId.ToString();
            VuosiEntry.Text = RuokaL.Year.ToString();
            MaanantaiEntry.Text = RuokaL.Maanantai;
            TiistaiEntry.Text = RuokaL.Tiistai;
            KeskiviikkoEntry.Text = RuokaL.Keskiviikko;
            TorstaiEntry.Text = RuokaL.Torstai;
            PerjantaiEntry.Text = RuokaL.Perjantai;
        }
        else
        {
            DisplayAlert("Virhe", "virhe ladatessa tietoja", "Ok");
        }

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
        if (MaanantaiEntry.Text == null || TiistaiEntry.Text == null || KeskiviikkoEntry.Text == null || TorstaiEntry.Text == null || PerjantaiEntry.Text == null)
        {
            VaroitusLabel.Text += "\nTyhji‰ kentti‰";
            return;
        }
        if (MaanantaiEntry.Text.Length < 1 || TiistaiEntry.Text.Length < 1 || KeskiviikkoEntry.Text.Length < 1 || TorstaiEntry.Text.Length < 1 || PerjantaiEntry.Text.Length < 1)
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
                RequestUri = new Uri($"https://ruokalista.arttukuikka.fi/api/v1/Ruokalista/Edit/{id}"),
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

    private async void Poista_Clicked(object sender, EventArgs e)
    {
        var ruoka = Ruokalista;
        
        var result = "";
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync($"https://ruokalista.arttukuikka.fi/api/v1/Ruokalista/GetId/{ruoka.Year}/{ruoka.WeekId}");
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadAsStringAsync();
            }
            else
            {
                await DisplayAlert("Virhe " + response.StatusCode.ToString(), "Virhe ladatessa id t‰", "Ok");
                
                return;
            }
        }

        ruoka.Id = int.Parse(result);

        
        bool answer = await DisplayAlert("Oletko varma?", $"Oletko varma ett‰ haluat poistaa viikon {ruoka.WeekId} ruokalistan", "Kyll‰", "Ei");
        if (answer)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.Default.GetAsync("token"));
               

                //GET Method
                try
                {
                    HttpResponseMessage response = await client.PostAsync($"https://ruokalista.arttukuikka.fi/api/v1/Ruokalista/Delete/{ruoka.Id}", null);
                    if (response.IsSuccessStatusCode)
                    {


                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        {
                            await DisplayAlert("Virhe " + response.StatusCode.ToString(), "Kirjautumisesi on vanhentunut, kirjaudu uudelleen sis‰‰n asetukset v‰lilehdelt‰", "ok");
                            Preferences.Default.Set("IsAdmin", false);
                        }
                        else
                        {
                            await DisplayAlert("Virhe " + response.StatusCode.ToString(), "virhe suorittaessa komentoa\n\n" + await response.Content.ReadAsStringAsync(), "ok");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Virhe yhdist‰ess‰ palvelimeen", "virhe ladatessa sis‰ltˆ‰\n\n" + ex.Message, "ok");
                    return;
                }
            }

            await Shell.Current.GoToAsync("..", true);
        }


        
    }
}