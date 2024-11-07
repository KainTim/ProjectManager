namespace ProjectManager;

public partial class MainWindow : Window
{

  public MainWindow() => InitializeComponent();

  private void Window_Loaded(object sender, RoutedEventArgs e)
  {
    var cultureInfo = new System.Globalization.CultureInfo("en-US");
    cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
    Thread.CurrentThread.CurrentUICulture = cultureInfo;
  }

}
