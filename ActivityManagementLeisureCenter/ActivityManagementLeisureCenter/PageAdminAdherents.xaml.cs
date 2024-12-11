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
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ActivityManagementLeisureCenter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageAdminAdherents : Page
    {
        public PageAdminAdherents()
        {
            this.InitializeComponent();
            chargerAdherents();
        }

        private void chargerAdherents()
        {
            lvAdherents.ItemsSource = SingletonBD.getInstance().getListeAdherents();
        }

        private async void SupprimerAdherent_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var idAdherent = button.Tag.ToString();

            var dialog = new ContentDialog
            {
                Title = "Confirmation de suppression",
                Content = new TextBlock { Text = "Êtes-vous sûr de vouloir supprimer cet adhérent ?" },
                PrimaryButtonText = "Oui",
                CloseButtonText = "Non"
            };

            dialog.XamlRoot = this.XamlRoot;

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    var success = SingletonBD.getInstance().SupprimerAdherent(idAdherent);

                    if (success)
                    {
                        var successDialog = new ContentDialog
                        {
                            Title = "Suppression réussie",
                            Content = new TextBlock { Text = "L'adhérent a été supprimé avec succès." },
                            PrimaryButtonText = "OK"
                        };
                        successDialog.XamlRoot = this.XamlRoot;
                        await successDialog.ShowAsync();

                        chargerAdherents();
                    }
                    else
                    {
                        var errorDialog = new ContentDialog
                        {
                            Title = "Erreur",
                            Content = new TextBlock { Text = "Une erreur est survenue lors de la suppression de l'adhérent." },
                            PrimaryButtonText = "OK"
                        };
                        errorDialog.XamlRoot = this.XamlRoot;
                        await errorDialog.ShowAsync();
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
        }

        private void ModifierAdherent_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var adherent = (Adherents)button.DataContext;

            Frame.Navigate(typeof(PageAdherentModification), adherent);
        }

        private void AjouterAdherent_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageAdherentFormulaire));
        }












    }
}
