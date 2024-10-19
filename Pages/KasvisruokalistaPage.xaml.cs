namespace RuokalistaApp
{
	public partial class KasvisruokalistaPage: ContentPage
	{
		public KasvisruokalistaPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			//only slightly cursed
			var mp = new MainPage();
			mp.Initialize(MainStack, true);
		}
	}
}
