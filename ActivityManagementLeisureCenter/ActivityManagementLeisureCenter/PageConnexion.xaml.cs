using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ActivityManagementLeisureCenter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageConnexion : Page
    {
        public PageConnexion()
        {
            this.InitializeComponent();
        }

        private async void SeConnecterAdherentButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Connexion Adhérent",
                Content = new StackPanel
                {
                    Children =
            {
                new TextBlock { Text = "Veuillez entrer votre matricule" },
                new TextBox { Name = "MatriculeTextBox", PlaceholderText = "Matricule" }
            }
                },
                PrimaryButtonText = "Se connecter",
                CloseButtonText = "Fermer"
            };
            dialog.XamlRoot = this.XamlRoot;
            await dialog.ShowAsync();
        }

        private async void SeConnecterAdministrateurButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = "Connexion Administrateur",
                Content = new StackPanel
                {
                    Children =
            {
                new TextBlock { Text = "Veuillez entrer votre nom d'utilisateur" },
                new TextBox { Name = "UtilisateurTextBox", PlaceholderText = "Nom d'utilisateur" },
                new TextBlock { Text = "" },
                new TextBlock { Text = "Veuillez entrer votre mot de passe" },
                new TextBox { Name = "MDPTextBox", PlaceholderText = "Mot de passe" }
            }
                },
                PrimaryButtonText = "Se connecter",
                CloseButtonText = "Fermer"
            };
            dialog.XamlRoot = this.XamlRoot;
            await dialog.ShowAsync();
        }

        private void RevenirButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageActivites));
        }
    }
}
