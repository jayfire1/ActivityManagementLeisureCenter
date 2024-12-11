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
    public sealed partial class PageActivites : Page
    {
        public PageActivites()
        {
            this.InitializeComponent();
            chargerActivites();
        }

        private void chargerActivites()
        {
            SingletonBD singletonBD = SingletonBD.getInstance();
            singletonBD.chargerActivites();

            ActivitesList.ItemsSource = singletonBD.getListeActivites();
        }

        private void OnInscrireClick(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var activite = button.DataContext as Activites;

            Frame.Navigate(typeof(PageSeances), activite);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (!string.IsNullOrEmpty(SessionManager.UtilisateurConnecte) && SessionManager.EstAdministrateur == false)
            {
                BienvenueTextBlock.Text = $"Bienvenue {SessionManager.UtilisateurConnecte}";
            }
        }

    }
}
