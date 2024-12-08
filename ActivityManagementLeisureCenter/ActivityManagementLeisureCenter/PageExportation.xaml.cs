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
using Windows.Storage.Pickers;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ActivityManagementLeisureCenter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PageExportation : Page
    {
        public PageExportation()
        {
            this.InitializeComponent();
        }

        private async void ExporterAdherentsClick(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(HelperClass.mainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.SuggestedFileName = "Liste_Adherents";
            picker.FileTypeChoices.Add("Fichier texte", new List<string>() { ".csv" });

            // Crée le fichier
            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            // Réinitialise les messages de succès et d'erreur avant de commencer
            SuccessMessage.Text = "";
            ErrorMessage.Text = "";

            if (monFichier != null)
            {
                ObservableCollection<Adherents> adherentsList = SingletonBD.getInstance().getListeAdherents();

                // Vérifiez si la liste est vide ou nulle
                if (adherentsList == null || adherentsList.Count == 0)
                {
                    ErrorMessage.Text = "Aucun adhérent trouvé.";
                    return;
                }

                // Préparer les lignes à écrire dans le fichier CSV
                List<string> lignes = new List<string> { "Id_num;Nom;Prenom;Adresse;Date_naissance;Age" };

                // Boucle pour ajouter les données des adhérents
                foreach (var adherent in adherentsList)
                {
                    lignes.Add($"{adherent.Id_num};{adherent.Nom};{adherent.Prenom};{adherent.Adresse};{adherent.Date_naissance:yyyy-MM-dd};{adherent.Age}");
                }

                // Écriture des lignes dans le fichier CSV
                try
                {
                    await Windows.Storage.FileIO.WriteLinesAsync(monFichier, lignes, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    SuccessMessage.Text = "Exportation réussie !";
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = $"Erreur lors de l'exportation : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage.Text = "Aucun fichier sélectionné.";
            }
        }


        private async void ExporterActivitesClick(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(HelperClass.mainWindow);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.SuggestedFileName = "Liste_Activites";
            picker.FileTypeChoices.Add("Fichier texte", new List<string>() { ".csv" });

            // Crée le fichier
            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            // Réinitialise les messages de succès et d'erreur avant de commencer
            SuccessMessage.Text = "";
            ErrorMessage.Text = "";

            if (monFichier != null)
            {
                ObservableCollection<Activites> activitesList = SingletonBD.getInstance().getListeActivites();

                // Vérifiez si la liste est vide ou nulle
                if (activitesList == null || activitesList.Count == 0)
                {
                    ErrorMessage.Text = "Aucune activité trouvé.";
                    return;
                }

                // Préparer les lignes à écrire dans le fichier CSV
                List<string> lignes = new List<string> { "IdActivite;Nom;Type;Cout_organisation;PrixVenteFormatte;Image" };

                // Boucle pour ajouter les données des adhérents
                foreach (var activite in activitesList)
                {
                    lignes.Add($"{activite.IdActivite};{activite.Nom};{activite.Type};{activite.Cout_organisation};{activite.PrixVenteFormatte};{activite.Image}");
                }

                // Écriture des lignes dans le fichier CSV
                try
                {
                    await Windows.Storage.FileIO.WriteLinesAsync(monFichier, lignes, Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    SuccessMessage.Text = "Exportation réussie !";
                }
                catch (Exception ex)
                {
                    ErrorMessage.Text = $"Erreur lors de l'exportation : {ex.Message}";
                }
            }
            else
            {
                ErrorMessage.Text = "Aucun fichier sélectionné.";
            }
        }

    }
}
