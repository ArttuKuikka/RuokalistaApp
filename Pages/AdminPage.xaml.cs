using Java.Time.Temporal;

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

		var viikko = 52;
		var vuosi = 2022;

		for(int i = 0; i < 15; i++)
		{
            var Ruoka = new HorizontalStackLayout() { Padding = new Thickness(0, 5), Spacing = 15 };
            var tekstit = new VerticalStackLayout();
            tekstit.Children.Add(new Label() { FontSize = 20, Text = "Viikko " + viikko.ToString() });
            tekstit.Children.Add(new Label() { Text = vuosi.ToString() });
            Ruoka.Children.Add(tekstit);
            Ruoka.Children.Add(new Button() { Text = "Muokkaa" });
            Ruoka.Children.Add(new Button() { Text = "Tiedot" });
            RuokaView.Children.Add(Ruoka);
        }

	}
}