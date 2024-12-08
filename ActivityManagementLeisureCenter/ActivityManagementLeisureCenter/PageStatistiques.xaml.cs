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
    public sealed partial class PageStatistiques : Page
    {
        public PageStatistiques()
        {
            this.InitializeComponent();
            ChargerStatistiques();
        }

        private void ChargerStatistiques()
        {
            var BD = SingletonBD.getInstance();

            int totalAdherents = BD.getTotalAdherents();
            TotalAdherentsText.Text = $"Nombre total d'adhérents : {totalAdherents}";

            int totalActivites = BD.getTotalActivites();
            TotalActivitesText.Text = $"Nombre total d'activités : {totalActivites}";

            Dictionary<string, int> participantsParActivite = BD.chargerParticipantsParActivite();
            Dictionary<string, double> moyennesParActivite = BD.ChargerMoyenneNotesParActivite();
            Dictionary<string, double> prixMoyensParParticipant = BD.ChargerPrixMoyenParParticipant();
            Dictionary<string, double> participantsMoyensParMois = BD.ChargerParticipantsMoyenParMois();

            foreach (var activite in participantsParActivite)
            {
                var stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };

                var textBlock = new TextBlock
                {
                    Text = $"{activite.Key} : {activite.Value} participants |",
                    VerticalAlignment = VerticalAlignment.Center
                };

                var moyenneNote = moyennesParActivite.ContainsKey(activite.Key) ? moyennesParActivite[activite.Key] : 0;
                var ratingControl = new RatingControl
                {
                    Value = moyenneNote,
                    MaxRating = 5,
                    IsReadOnly = true,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = (SolidColorBrush)Resources["RatingControlSelectedForeground"]
                };

                stackPanel.Children.Add(textBlock);
                stackPanel.Children.Add(ratingControl);

                ParticipantsParActiviteList.Items.Add(stackPanel);
            }

            foreach (var participant in prixMoyensParParticipant)
            {
                var stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };

                var textBlock = new TextBlock
                {
                    Text = $"{participant.Key} | Prix moyen : {participant.Value} $",
                    VerticalAlignment = VerticalAlignment.Center
                };

                stackPanel.Children.Add(textBlock);

                PrixMoyenList.Items.Add(stackPanel);
            }

            foreach (var mois in participantsMoyensParMois)
            {
                var stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };

                var textBlock = new TextBlock
                {
                    Text = $"{mois.Key} | Nombre moyen de participants : {mois.Value} participants",
                    VerticalAlignment = VerticalAlignment.Center
                };

                stackPanel.Children.Add(textBlock);

                ParticipantsMoyenList.Items.Add(stackPanel);
            }
        }


    }
}
