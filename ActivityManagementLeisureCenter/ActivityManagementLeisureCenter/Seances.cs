﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManagementLeisureCenter
{
    internal class Seances
    {
        DateTime date_debut, date_fin;
        TimeSpan heure_debut, heure_fin;
        int nb_places, nb_personnes, id_activite;

        public Seances ()
        {

        }

        public Seances(DateTime date_debut, DateTime date_fin, TimeSpan heure_debut, TimeSpan heure_fin, int nb_places, int nb_personnes, int id_activite)
        {
            Date_debut = date_debut;
            Date_fin = date_fin;
            Heure_debut = heure_debut;
            Heure_fin = heure_fin;
            Nb_places = nb_places;
            Nb_personnes = nb_personnes;
            Id_activite = id_activite;
        }

        public DateTime Date_debut { get => date_debut; set => date_debut = value; }
        public DateTime Date_fin { get => date_fin; set => date_fin = value; }
        public TimeSpan Heure_debut { get => heure_debut; set => heure_debut = value; }
        public TimeSpan Heure_fin { get => heure_fin; set => heure_fin = value; }
        public int Nb_places { get => nb_places; set => nb_places = value; }
        public int Nb_personnes { get => nb_personnes; set => nb_personnes = value; }
        public int Id_activite { get => id_activite; set => id_activite = value; }
    }
}