using Uno.ServerDrivenUI.Services;

namespace SampleApp;

public sealed partial class MainPage : Page
{
    App Application => (App)App.Current;
    public MainPage()
    {
        this.InitializeComponent();


        //Loaded += (_, __) => ServiceUI.Current.ProcessXaml(mainContent.Name);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        Application.WindowContent.Navigate(typeof(SecondPage));
    }
}
