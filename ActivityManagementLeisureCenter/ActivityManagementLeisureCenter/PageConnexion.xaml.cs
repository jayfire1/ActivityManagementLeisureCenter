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
            try
            {
                var matriculeTextBox = new TextBox { PlaceholderText = "Matricule" };

                var dialog = new ContentDialog
                {
                    Title = "Connexion Adhérent",
                    Content = new StackPanel
                    {
                        Children =
                {
                    new TextBlock { Text = "Veuillez entrer votre matricule" },
                    matriculeTextBox
                }
                    },
                    PrimaryButtonText = "Se connecter",
                    CloseButtonText = "Fermer"
                };

                dialog.XamlRoot = this.XamlRoot;
                ContentDialogResult result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    string matricule = matriculeTextBox.Text;

                    // Authentification
                    Adherents adherent = SingletonBD.getInstance().AuthentifierAdherent(matricule);

                    if (adherent != null)
                    {
                        // Mettre à jour l'état de connexion
                        SessionManager.EstConnecte = true;
                        SessionManager.UtilisateurConnecte = $"{adherent.Prenom} {adherent.Nom}";
                        SessionManager.UtilisateurId = adherent.Id_num;

                        var mainWindow = App.MainWindowInstance;
                        mainWindow.UpdateMenuItemsVisibility();

                        // Rediriger vers la page des activités
                        Frame.Navigate(typeof(PageActivites));

                        // Message de succès
                        var successDialog = new ContentDialog
                        {
                            Title = "Connexion réussie",
                            Content = new TextBlock
                            {
                                Text = $"Bienvenue {adherent.Prenom} {adherent.Nom}"
                            },
                            PrimaryButtonText = "OK"
                        };
                        successDialog.XamlRoot = this.XamlRoot;
                        await successDialog.ShowAsync();
                    }
                    else
                    {
                        // Message d'erreur
                        var errorDialog = new ContentDialog
                        {
                            Title = "Erreur",
                            Content = new TextBlock
                            {
                                Text = "Matricule incorrect. Veuillez réessayer."
                            },
                            PrimaryButtonText = "OK"
                        };
                        errorDialog.XamlRoot = this.XamlRoot;
                        await errorDialog.ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                var exceptionDialog = new ContentDialog
                {
                    Title = "Erreur inattendue",
                    Content = new TextBlock { Text = $"Une erreur est survenue : {ex.Message}" },
                    PrimaryButtonText = "OK"
                };
                exceptionDialog.XamlRoot = this.XamlRoot;
                await exceptionDialog.ShowAsync();
            }
        }


        private async void SeConnecterAdministrateurButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Créer les TextBox pour le nom d'utilisateur et le mot de passe
                var utilisateurTextBox = new TextBox { Name = "UtilisateurTextBox", PlaceholderText = "Nom d'utilisateur" };
                var mdpTextBox = new PasswordBox { Name = "MDPTextBox", PlaceholderText = "Mot de passe" };

                var dialog = new ContentDialog
                {
                    Title = "Connexion Administrateur",
                    Content = new StackPanel
                    {
                        Children =
                        {
                            new TextBlock { Text = "Veuillez entrer votre nom d'utilisateur" },
                            utilisateurTextBox,
                            new TextBlock { Text = "" },
                            new TextBlock { Text = "Veuillez entrer votre mot de passe" },
                            mdpTextBox
                        }
                    },
                    PrimaryButtonText = "Se connecter",
                    CloseButtonText = "Fermer"
                };

                dialog.XamlRoot = this.XamlRoot;
                ContentDialogResult result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    string nomUtilisateur = utilisateurTextBox.Text;
                    string motDePasse = mdpTextBox.Password;

                    // Authentification
                    Administrateur administrateur = SingletonBD.getInstance().AuthentifierAdministrateur(nomUtilisateur, motDePasse);

                    if (administrateur != null)
                    {
                        // Mettre à jour l'état de connexion
                        SessionManager.EstConnecte = true;
                        SessionManager.UtilisateurConnecte = $"Administrateur : {administrateur.Nom_utilisateur}";
                        SessionManager.EstAdministrateur = true;

                        var mainWindow = App.MainWindowInstance;
                        mainWindow.UpdateMenuItemsVisibility();

                        // Rediriger vers la page d'administration ou autre page spécifique
                        Frame.Navigate(typeof(PageActivites));

                        // Message de succès
                        var successDialog = new ContentDialog
                        {
                            Title = "Connexion réussie",
                            Content = new TextBlock
                            {
                                Text = $"Bienvenue {administrateur.Nom_utilisateur}"
                            },
                            PrimaryButtonText = "OK"
                        };
                        successDialog.XamlRoot = this.XamlRoot;
                        await successDialog.ShowAsync();
                    }
                    else
                    {
                        // Message d'erreur
                        var errorDialog = new ContentDialog
                        {
                            Title = "Erreur",
                            Content = new TextBlock
                            {
                                Text = "Nom d'utilisateur ou mot de passe incorrect. Veuillez réessayer."
                            },
                            PrimaryButtonText = "OK"
                        };
                        errorDialog.XamlRoot = this.XamlRoot;
                        await errorDialog.ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                var exceptionDialog = new ContentDialog
                {
                    Title = "Erreur inattendue",
                    Content = new TextBlock { Text = $"Une erreur est survenue : {ex.Message}" },
                    PrimaryButtonText = "OK"
                };
                exceptionDialog.XamlRoot = this.XamlRoot;
                await exceptionDialog.ShowAsync();
            }
        }

        private void RevenirButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageActivites));
        }
    }
}
