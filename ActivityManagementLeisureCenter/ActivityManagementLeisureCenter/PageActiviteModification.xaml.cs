using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ActivityManagementLeisureCenter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageActiviteModification : Page
    {
        private string IdActivite;

        public PageActiviteModification()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Activites activite)
            {
                IdActivite = activite.IdActivite.ToString();
                NomTextBox.Text = activite.Nom;
                TypeTextBox.Text = activite.Type;
                CoutOrganisationTextBox.Text = activite.Cout_organisation.ToString();
                PrixVenteTextBox.Text = activite.Prix_vente.ToString();
                ImageLienTextBox.Text = activite.Image;
            }
        }

        // Méthode de validation et d'enregistrement de l'activité
        private async void EnregistrerActivite_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(NomTextBox.Text))
            {
                NomErrorText.Text = "Le nom est obligatoire.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(TypeTextBox.Text))
            {
                TypeErrorText.Text = "Le type est obligatoire.";
                isValid = false;
            }

            if (!decimal.TryParse(CoutOrganisationTextBox.Text, out decimal coutOrganisation))
            {
                CoutOrganisationErrorText.Text = "Le coût d'organisation doit être un nombre valide.";
                isValid = false;
            }

            if (!decimal.TryParse(PrixVenteTextBox.Text, out decimal prixVente))
            {
                PrixVenteErrorText.Text = "Le prix de vente doit être un nombre valide.";
                isValid = false;
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
                string nom = NomTextBox.Text;
                string type = TypeTextBox.Text;
                decimal cout = coutOrganisation;
                decimal prix = prixVente;
                string image = ImageLienTextBox.Text;

                bool isDeleted = SingletonBD.getInstance().SupprimerActivite(Convert.ToInt32(IdActivite));

                if (isDeleted)
                {
                    bool isAdded = SingletonBD.getInstance().AjouterActivite(nom, type, cout, prix, image);

                    if (isAdded)
                    {
                        Frame.Navigate(typeof(PageAdminActivites));
                    }
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
