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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Net.Http;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ActivityManagementLeisureCenter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageActiviteFormulaire : Page
    {
        public PageActiviteFormulaire()
        {
            this.InitializeComponent();
        }

        private async void AjouterActivite_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            // Validation du Nom
            if (string.IsNullOrWhiteSpace(NomTextBox.Text))
            {
                NomErrorText.Text = "Le nom ne peut pas être vide.";
                isValid = false;
            }
            else
            {
                NomErrorText.Text = string.Empty;
            }

            // Validation du Type
            if (string.IsNullOrWhiteSpace(TypeTextBox.Text))
            {
                TypeErrorText.Text = "Le type ne peut pas être vide.";
                isValid = false;
            }
            else
            {
                TypeErrorText.Text = string.Empty;
            }

            // Validation du Coût d'organisation
            if (!decimal.TryParse(CoutOrganisationTextBox.Text, out decimal coutOrganisation) || coutOrganisation <= 0)
            {
                CoutOrganisationErrorText.Text = "Le coût d'organisation doit être un nombre positif.";
                isValid = false;
            }
            else
            {
                CoutOrganisationErrorText.Text = string.Empty;
            }

            // Validation du Prix de vente
            if (!decimal.TryParse(PrixVenteTextBox.Text, out decimal prixVente) || prixVente <= 0)
            {
                PrixVenteErrorText.Text = "Le prix de vente doit être un nombre positif.";
                isValid = false;
            }
            else
            {
                PrixVenteErrorText.Text = string.Empty;
            }

            // Validation du lien vers l'image
            bool isImageValid = await IsImageUrlValid(ImageLienTextBox.Text);
            if (!isImageValid)
            {
                ImageLienErrorText.Text = "Le lien vers l'image ne correspond pas à une image valide.";
                isValid = false;
            }
            else
            {
                ImageLienErrorText.Text = string.Empty;
            }

            if (isValid)
            {
                bool result = SingletonBD.getInstance().AjouterActivite(
                    NomTextBox.Text,
                    TypeTextBox.Text,
                    coutOrganisation,
                    prixVente,
                    ImageLienTextBox.Text
                );

                if (result)
                {
                    Frame.Navigate(typeof(PageAdminActivites));
                }
            }
        }

        private void Revenir_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageAdminActivites));
        }

        public async Task<bool> IsImageUrlValid(string url)
        {
            try
            {
                // Créer une instance de HttpClient pour envoyer une requête HTTP
                using (var client = new HttpClient())
                {
                    // Définir un délai d'attente raisonnable pour la requête
                    client.Timeout = TimeSpan.FromSeconds(5);

                    // Envoyer une requête GET au lien de l'image
                    HttpResponseMessage response = await client.GetAsync(url);

                    // Vérifier si le statut de la réponse est OK (status code 200)
                    if (response.IsSuccessStatusCode && response.Content.Headers.ContentType.ToString().StartsWith("image"))
                    {
                        // Le lien est valide et c'est une image
                        return true;
                    }
                }
            }
            catch
            {
                // En cas d'exception (par exemple, temps d'attente dépassé, URL invalide, etc.)
                return false;
            }

            // Si la requête échoue ou ce n'est pas une image, retourner false
            return false;
        }

    }
}
