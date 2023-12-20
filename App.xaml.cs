using CarCustomChecker.Services;

namespace CarCustomChecker
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new AppShell();
			DependencyService.Register<DataService>();
		}
	}
}
