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

            //crée le fichier
            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            if (monFichier != null)
            {
                ObservableCollection<Adherents> adherents = SingletonBD.getInstance().getListeAdherents();

                List<string> lignes = adherents.Select(a => $"{a.Id_num};{a.Nom};{a.Prenom};{a.Adresse};{a.Date_naissance};{a.Age}").ToList();
                await Windows.Storage.FileIO.WriteLinesAsync(monFichier, lignes, Windows.Storage.Streams.UnicodeEncoding.Utf8);
            }
        }

            private void ExporterActivitesClick(object sender, RoutedEventArgs e)
            {

            }

    }
}
