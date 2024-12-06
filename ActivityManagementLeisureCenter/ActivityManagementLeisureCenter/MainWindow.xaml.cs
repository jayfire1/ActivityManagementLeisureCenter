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
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            HelperClass.mainWindow = this;

            mainFrame.Navigate(typeof(PageActivites));

            UpdateMenuItemsVisibility();
        }

        private void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavigationViewItem;

            switch (item.Name)
            {
                case "iActivite":
                    mainFrame.Navigate(typeof(PageActivites));
                    break;
                case "iConnexion":
                    mainFrame.Navigate(typeof(PageConnexion));
                    break;
                case "iDeconnexion":
                    DeconnecterUtilisateur();
                    break;
                case "iAdmin":
                    mainFrame.Navigate(typeof(PageAdmin));
                    break;
                case "iStats":
                    mainFrame.Navigate(typeof(PageStatistiques));
                    break;
                case "iExportation":
                    mainFrame.Navigate(typeof(PageExportation));
                    break;
                default:
                    break;
            }
        }

        private void DeconnecterUtilisateur()
        {
            SessionManager.EstConnecte = false;
            SessionManager.UtilisateurConnecte = string.Empty;
            SessionManager.EstAdministrateur = false;

            // Mettre à jour les boutons
            UpdateMenuItemsVisibility();

            // Rediriger vers la page des activités
            mainFrame.Navigate(typeof(PageActivites));
        }

        public void UpdateMenuItemsVisibility()
        {
            var connexionItem = navView.FooterMenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Name == "iConnexion");
            var deconnexionItem = navView.FooterMenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Name == "iDeconnexion");

            var adminItem = navView.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Name == "iAdmin");

            var StatsItem = navView.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Name == "iStats");

            var ExportationItem = navView.MenuItems
                .OfType<NavigationViewItem>()
                .FirstOrDefault(item => item.Name == "iExportation");

            if (connexionItem != null && deconnexionItem != null)
            {
                connexionItem.Visibility = SessionManager.EstConnecte ? Visibility.Collapsed : Visibility.Visible;
                deconnexionItem.Visibility = SessionManager.EstConnecte ? Visibility.Visible : Visibility.Collapsed;
            }

            if (adminItem != null)
            {
                adminItem.Visibility = (SessionManager.EstConnecte && SessionManager.EstAdministrateur) ? Visibility.Visible : Visibility.Collapsed;
            }
            
            if (StatsItem != null)
            {
                StatsItem.Visibility = (SessionManager.EstConnecte && SessionManager.EstAdministrateur) ? Visibility.Visible : Visibility.Collapsed;
            }

            if (ExportationItem != null)
            {
                ExportationItem.Visibility = (SessionManager.EstConnecte && SessionManager.EstAdministrateur) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

    }
}
