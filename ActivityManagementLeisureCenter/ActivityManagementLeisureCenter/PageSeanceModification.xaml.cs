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
    public sealed partial class PageSeanceModification : Page
    {
        private int seanceId;

        public PageSeanceModification()
        {
            this.InitializeComponent();
            ChargerActivites();
        }

        private void ChargerActivites()
        {
            var activites = SingletonBD.getInstance().getListeActivites();
            foreach (var activite in activites)
            {
                ActiviteComboBox.Items.Add(new ComboBoxItem { Content = $"{activite.IdActivite} - {activite.Nom} ({activite.Type})", Tag = activite.IdActivite });
            }
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.Parameter is Seances seance)
            {
                seanceId = seance.Id_seance;

                DateDebutTextBox.Text = seance.Date_debut.ToString("yyyy/MM/dd");
                DateFinTextBox.Text = seance.Date_fin.ToString("yyyy/MM/dd");

                HeureDebutTextBox.Text = seance.Heure_debut != TimeSpan.MinValue ? seance.Heure_debut.ToString(@"hh\:mm\:ss") : "00:00:00";
                HeureFinTextBox.Text = seance.Heure_fin != TimeSpan.MinValue ? seance.Heure_fin.ToString(@"hh\:mm\:ss") : "00:00:00";

                foreach (ComboBoxItem item in ActiviteComboBox.Items)
                {
                    if ((int)item.Tag == seance.Id_activite)
                    {
                        ActiviteComboBox.SelectedItem = item;
                        break;
                    }
                }

                NbPlacesTextBox.Text = seance.Nb_places.ToString();
                NbPersonnesTextBox.Text = seance.Nb_personnes.ToString();
            }
        }

        private void EnregistrerSeance_Click(object sender, RoutedEventArgs e)
        {
            DateDebutErrorText.Text = string.Empty;
            DateFinErrorText.Text = string.Empty;
            HeureDebutErrorText.Text = string.Empty;
            HeureFinErrorText.Text = string.Empty;
            ActiviteErrorText.Text = string.Empty;
            NbPlacesErrorText.Text = string.Empty;
            NbPersonnesErrorText.Text = string.Empty;

            bool isValid = true;

            int nbPlaces = 0;
            int nbPersonnes = 0;

            DateTime dateDebut = DateTime.MinValue;
            DateTime dateFin = DateTime.MinValue;
            TimeSpan heureDebut = TimeSpan.Zero;
            TimeSpan heureFin = TimeSpan.Zero;

            if (string.IsNullOrWhiteSpace(DateDebutTextBox.Text) || !DateTime.TryParseExact(DateDebutTextBox.Text, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out dateDebut))
            {
                DateDebutErrorText.Text = "Veuillez entrer une date de début valide.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(DateFinTextBox.Text) || !DateTime.TryParseExact(DateFinTextBox.Text, "yyyy/MM/dd", null, System.Globalization.DateTimeStyles.None, out dateFin))
            {
                DateFinErrorText.Text = "Veuillez entrer une date de fin valide.";
                isValid = false;
            }

            if (isValid && dateFin < dateDebut)
            {
                DateFinErrorText.Text = "La date de fin ne peut pas être avant la date de début.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(HeureDebutTextBox.Text) || !TimeSpan.TryParseExact(HeureDebutTextBox.Text, @"hh\:mm\:ss", null, out heureDebut))
            {
                HeureDebutErrorText.Text = "Veuillez entrer une heure de début valide (HH:mm:ss).";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(HeureFinTextBox.Text) || !TimeSpan.TryParseExact(HeureFinTextBox.Text, @"hh\:mm\:ss", null, out heureFin))
            {
                HeureFinErrorText.Text = "Veuillez entrer une heure de fin valide (HH:mm:ss).";
                isValid = false;
            }

            if (isValid && heureFin < heureDebut)
            {
                HeureFinErrorText.Text = "L'heure de fin ne peut pas être avant l'heure de début.";
                isValid = false;
            }

            if (ActiviteComboBox.SelectedIndex == -1)
            {
                ActiviteErrorText.Text = "Veuillez choisir une activité.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(NbPlacesTextBox.Text) || !int.TryParse(NbPlacesTextBox.Text, out nbPlaces) || nbPlaces <= 0)
            {
                NbPlacesErrorText.Text = "Veuillez entrer un nombre de places valide.";
                isValid = false;
            }

            if (string.IsNullOrWhiteSpace(NbPersonnesTextBox.Text) || !int.TryParse(NbPersonnesTextBox.Text, out nbPersonnes) || nbPersonnes <= 0)
            {
                NbPersonnesErrorText.Text = "Veuillez entrer un nombre de personnes valide.";
                isValid = false;
            }

            if (nbPlaces < nbPersonnes)
            {
                NbPlacesErrorText.Text = "Le nombre de places ne peut pas être inférieur au nombre de personnes.";
                isValid = false;
            }

            if (isValid)
            {
                bool deleteResult = SingletonBD.getInstance().SupprimerSeance(seanceId);

                if (deleteResult)
                {
                    var seance = new Seances
                    {
                        Id_seance = seanceId,
                        Date_debut = dateDebut,
                        Date_fin = dateFin,
                        Heure_debut = heureDebut,
                        Heure_fin = heureFin,
                        Nb_places = nbPlaces,
                        Nb_personnes = nbPersonnes,
                        Id_activite = (int)((ComboBoxItem)ActiviteComboBox.SelectedItem).Tag
                    };

                    bool result = SingletonBD.getInstance().AjouterSeance(seance.Date_debut, seance.Heure_debut, seance.Date_fin, seance.Heure_fin, seance.Nb_places, seance.Nb_personnes, seance.Id_activite);

                    if (result)
                    {
                        Frame.Navigate(typeof(PageAdminSeances));
                    }
                }
            }
        }

        private void Revenir_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PageAdminSeances));
        }

    }
}
