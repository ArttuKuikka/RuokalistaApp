using RuokalistaApp.Pages;

namespace RuokalistaApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("LoginPage", typeof(LoginPage));
        Routing.RegisterRoute("LuoUusi", typeof(CreateNewPage));

        if (Preferences.Default.Get("IsAdmin", false))
		{
            tabbar.Items.Add(new ShellContent()
            {
                Title = "Admin",
                Route = "AdminPage",
                Icon = "admin.png",
                ContentTemplate = new DataTemplate(() => new AdminPage())
            });
        }
		
        
    }
}
