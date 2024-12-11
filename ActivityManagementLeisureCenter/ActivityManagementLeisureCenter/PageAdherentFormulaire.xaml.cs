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
    public sealed partial class PageAdherentFormulaire : Page
    {
        public PageAdherentFormulaire()
        {
            this.InitializeComponent();
        }

        private void AjouterAdherent_Click(object sender, RoutedEventArgs e)
        {
            // Réinitialiser les erreurs
            NomErrorText.Text = "";
            PrenomErrorText.Text = "";
            AdresseErrorText.Text = "";
            DateNaissanceErrorText.Text = "";

            bool isValid = true;

            // Validation du Nom
            if (string.IsNullOrWhiteSpace(NomTextBox.Text))
            {
                NomErrorText.Text = "Le nom ne peut pas être vide.";
                isValid = false;
            }

            // Validation du Prénom
            if (string.IsNullOrWhiteSpace(PrenomTextBox.Text))
            {
                PrenomErrorText.Text = "Le prénom ne peut pas être vide.";
                isValid = false;
            }

            // Validation de l'Adresse
            string adressePattern = @"^\d{1,4}\s[\w\s-]+,\s[\w\s-]+$";
            if (!Regex.IsMatch(AdresseTextBox.Text, adressePattern))
            {
                AdresseErrorText.Text = "L'adresse doit être dans un format valide (ex : 1234 Rue Exemple, Ville).";
                isValid = false;
            }

            // Validation de la Date de naissance
            DateTime? dateNaissance = DateNaissancePicker.SelectedDate?.DateTime;
            if (!dateNaissance.HasValue)
            {
                DateNaissanceErrorText.Text = "Veuillez sélectionner une date de naissance.";
                isValid = false;
            }

            // Calcul de l'âge à partir de la date de naissance
            int age = 0;
            if (dateNaissance.HasValue)
            {
                DateTime birthDate = dateNaissance.Value;
                age = DateTime.Now.Year - birthDate.Year;
                if (DateTime.Now < birthDate.AddYears(age)) age--; // Ajuster si l'anniversaire n'est pas encore passé cette année
                                                                   
                // Validation que l'âge est positif et supérieur à 0
                if (age <= 0)
                {
                    DateNaissanceErrorText.Text = "La date de naissance est invalide. L'âge doit être supérieur à 0.";
                    isValid = false;
                }
                else
                {
                    AgeTextBlock.Text = $"Âge : {age} ans";
                }
            }

            // Si tout est valide, ajouter l'adhérent
            if (isValid)
            {
                // Récupérer les valeurs du formulaire
                string nom = NomTextBox.Text.Trim();
                string prenom = PrenomTextBox.Text.Trim();
                string adresse = AdresseTextBox.Text.Trim();

                SingletonBD.getInstance().AjouterAdherent(nom, prenom, adresse, dateNaissance.Value, age);

                Frame.Navigate(typeof(PageAdminAdherents));
            }
        }

        private void DateNaissancePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            DateNaissanceErrorText.Text = "";

            DateTime? dateNaissance = e.NewDate.DateTime;
            if (dateNaissance.HasValue)
            {
                DateTime birthDate = dateNaissance.Value;
                int age = DateTime.Now.Year - birthDate.Year;
                if (DateTime.Now < birthDate.AddYears(age)) age--;

                if (age <= 0)
                {
                    DateNaissanceErrorText.Text = "La date de naissance est invalide. L'âge doit être supérieur à 0.";
                    AgeTextBlock.Text = "";
                }
                else
                {
                    AgeTextBlock.Text = $"Âge : {age} ans";
                }
            }
            else
            {
                AgeTextBlock.Text = "";
            }
        }


        private void Revenir_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageAdminAdherents));
        }
    }
}
