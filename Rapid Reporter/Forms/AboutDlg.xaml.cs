using System.Diagnostics;
using System.Windows;

namespace Rapid_Reporter.Forms
{
    /// <summary>
    /// Interaction logic for AboutDlg.xaml
    /// The dialog shows basic information about the application, as well as an expandable credits pane.
    /// Two buttons are provided: A help button that shows the help dialog, and an Ok button to close the dialog.
    /// </summary>
    public partial class AboutDlg
    {
        // Constructor
        //  We set the application name, version, and dialog title.
        public AboutDlg()
        {
            Logger.Record("[AboutDlg]: Starting About Dialog. Initializing component.", "AboutDlg", "info");
            InitializeComponent();
            Title = System.Windows.Forms.Application.ProductName + " - Help";
            appName.Content = System.Windows.Forms.Application.ProductName;
            appVers.Content = System.Windows.Forms.Application.ProductVersion;
            Logger.Record("[AboutDlg]: Window Ready, setting focus.", "AboutDlg", "info");
            Ok.Focus();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.Record("[Window_Loaded]: loading dialog....", "AboutDlg", "info");
            Ok.Focus();
        }

        // There's a link line in the dialog pointing to the Rapid Reporter page: http://testing.gershon.info/reporter/
        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Logger.Record("[RequestNavigate]: Link Pressed:" + e.Uri.AbsoluteUri, "AboutDlg", "info");
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            Logger.Record("[RequestNavigate]: started browser process", "AboutDlg", "info");
            e.Handled = true; // Can dismiss the event now that we dealt with it
        }
    }
}
