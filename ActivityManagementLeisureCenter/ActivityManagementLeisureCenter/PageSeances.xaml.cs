using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class PageSeances : Page
    {
        public PageSeances()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Activites activite)
            {
                ChargerSeancesParActivite(activite);
            }
        }

        private void ChargerSeancesParActivite(Activites activite)
        {
            SingletonBD singletonBD = SingletonBD.getInstance();
            singletonBD.ChargerSeancesParActivite(activite.IdActivite);

            SeancesList.ItemsSource = singletonBD.getListeSeances();
        }

        private async void OnParticiperClick(object sender, RoutedEventArgs e)
        {
            // Vérifier si l'utilisateur est connecté
            if (!SessionManager.EstConnecte)
            {
                var dialog = new ContentDialog
                {
                    Title = "Non connecté",
                    Content = "Vous devez être connecté pour participer à une séance.",
                    CloseButtonText = "Ok"
                };
                await dialog.ShowAsync();
                return;
            }

            var seance = (Seances)((Button)sender).DataContext;
            string idAdherent = SessionManager.UtilisateurId;

            SingletonBD singletonBD = SingletonBD.getInstance();
            bool inscriptionReussie = singletonBD.InscrireAdherent(idAdherent, seance.Id_seance);

            if (inscriptionReussie)
            {
                Frame.Navigate(typeof(PageActivites));
            }
            else
            {
                var dialog = new ContentDialog
                {
                    Title = "Erreur",
                    Content = "L'inscription a échoué. Veuillez réessayer.",
                    CloseButtonText = "Ok"
                };
                await dialog.ShowAsync();
            }
        }

    }
}
