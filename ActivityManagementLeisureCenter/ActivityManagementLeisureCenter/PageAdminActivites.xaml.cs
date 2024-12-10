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
    public sealed partial class PageAdminActivites : Page
    {
        public PageAdminActivites()
        {
            this.InitializeComponent();
            chargerActivites();
        }

        private void chargerActivites()
        {
            lvActivites.ItemsSource = SingletonBD.getInstance().getListeActivitesAdmin();
        }

        private async void SupprimerActivite_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int idActivite = (int)button.Tag;

            var dialog = new ContentDialog
            {
                Title = "Confirmation de suppression",
                Content = new TextBlock { Text = "Êtes-vous sûr de vouloir supprimer cette activité ?" },
                PrimaryButtonText = "Oui",
                CloseButtonText = "Non"
            };

            dialog.XamlRoot = this.XamlRoot;

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                try
                {
                    var success = SingletonBD.getInstance().SupprimerActivite(idActivite);

                    if (success)
                    {
                        var successDialog = new ContentDialog
                        {
                            Title = "Suppression réussie",
                            Content = new TextBlock { Text = "L'activité a été supprimée avec succès." },
                            PrimaryButtonText = "OK"
                        };
                        successDialog.XamlRoot = this.XamlRoot;
                        await successDialog.ShowAsync();

                        chargerActivites();
                    }
                    else
                    {
                        var errorDialog = new ContentDialog
                        {
                            Title = "Erreur",
                            Content = new TextBlock { Text = "Une erreur est survenue lors de la suppression de l'activité." },
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

        private void ModifierActivite_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AjouterActivite_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
