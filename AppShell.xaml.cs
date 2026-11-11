namespace CarCustomChecker
{
	public partial class AppShell : Shell
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(Views.RptoDetails), typeof(Views.RptoDetails));
			Routing.RegisterRoute(nameof(Views.ZoneDetails), typeof(Views.ZoneDetails));
			Routing.RegisterRoute(nameof(Views.SvhDetails), typeof(Views.SvhDetails));
		}
	}
}
