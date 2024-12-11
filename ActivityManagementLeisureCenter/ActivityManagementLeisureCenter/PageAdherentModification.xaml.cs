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
    public sealed partial class PageAdherentModification : Page
    {
        private string IdAdherent;

        public PageAdherentModification()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Adherents adherent)
            {
                IdAdherent = adherent.Id_num.ToString();
                NomTextBox.Text = adherent.Nom;
                PrenomTextBox.Text = adherent.Prenom;
                AdresseTextBox.Text = adherent.Adresse;
                DateNaissancePicker.Date = adherent.Date_naissance;
                int age = CalculerAge(adherent.Date_naissance);
                AgeTextBlock.Text = age.ToString();
            }
        }

        private int CalculerAge(DateTime dateNaissance)
        {
            var today = DateTime.Today;
            var age = today.Year - dateNaissance.Year;

            if (dateNaissance > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }

        private void EnregistrerAdherent_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            NomErrorText.Text = "";
            PrenomErrorText.Text = "";
            AdresseErrorText.Text = "";
            DateNaissanceErrorText.Text = "";

            if (string.IsNullOrWhiteSpace(PrenomTextBox.Text))
            {
                PrenomErrorText.Text = "Le prénom est obligatoire.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(NomTextBox.Text))
            {
                NomErrorText.Text = "Le nom est obligatoire.";
                isValid = false;
            }

            string adressePattern = @"^\d{1,4}\s[\w\s-]+,\s[\w\s-]+$";
            if (!Regex.IsMatch(AdresseTextBox.Text, adressePattern))
            {
                AdresseErrorText.Text = "L'adresse doit respecter le format : 'Numéro(maximum 4 chiffres), Rue, Ville'.";
                isValid = false;
            }

            if (DateNaissancePicker.Date.DateTime > DateTime.Today)
            {
                DateNaissanceErrorText.Text = "La date de naissance ne peut pas être dans le futur.";
                isValid = false;
            }

            if (isValid)
            {
                int age = CalculerAge(DateNaissancePicker.Date.DateTime);

                bool isDeleted = SingletonBD.getInstance().SupprimerAdherent(IdAdherent);

                if (isDeleted)
                {
                    SingletonBD.getInstance().AjouterAdherent(
                        NomTextBox.Text,
                        PrenomTextBox.Text,
                        AdresseTextBox.Text,
                        DateNaissancePicker.Date.DateTime,
                        age
                    );

                    Frame.Navigate(typeof(PageAdminAdherents));
                }
            }
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageAdminAdherents));
        }


    }
}
