using Chat_Client.Views.Windows;
using Chat_Client.Core;
using MVVM_Classes;
using System.Windows;
using ChatLibrary.User;
using System;

namespace Chat_Client.ViewModels;

internal sealed class LoginWindowViewModel : ObservableObject
{
    private readonly LoginWindow _loginWindow = Application.Current.MainWindow as LoginWindow;

    public LoginWindowViewModel()
    {
        LoginCommand = new(o =>
        {
            PrivateUserData userData = new(Guid.NewGuid(), Username, _loginWindow.Password);
            var request = MessageFactory.CreateLoginRequest(userData, DontHaveAcconut);

            App.CurrentClient.SendMessageToServer(request);

        }, b => !string.IsNullOrEmpty(Username) 
                && !string.IsNullOrEmpty(_loginWindow.Password));
    }

    public string Username { get; set; }

    public bool DontHaveAcconut { get; set; }

    public Command LoginCommand { get; init; }
}