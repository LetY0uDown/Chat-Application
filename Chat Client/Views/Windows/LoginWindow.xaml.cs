﻿using System.Windows;

namespace Chat_Client.Views.Windows;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    internal string Password => passwordBox.SecurePassword.ToString();
}