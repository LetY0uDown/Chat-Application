using System.Windows;

namespace Chat_Client.Views.Windows;

public partial class MessageWindow : Window
{
    public MessageWindow()
    {
        InitializeComponent();
    }

    internal static void Show(string title, string message)
    {
        var window = new MessageWindow()
        {
            Title = title
        };
        window.text.Text = message;

        window.ShowDialog();
    }
}