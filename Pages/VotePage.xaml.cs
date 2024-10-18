namespace RuokalistaApp.Pages;

public partial class VotePage : ContentPage
{
	public VotePage()
	{
		InitializeComponent();

		var isDev = Preferences.Default.Get("IsDeveloper", false);
		if (isDev)
		{
			webview1.Source = "https://ruokalistadev.arttukuikka.fi/Aanestys/Tulokset?isApp=true";
		}
		else
		{
			webview1.Source = "https://ruokalista.arttukuikka.fi/Aanestys/Tulokset?isApp=true";
		}
	}

    private void RefreshView_Refreshing(object sender, EventArgs e)
    {
		webview1.Reload();
		RefreshView1.IsRefreshing = false;
    }
}