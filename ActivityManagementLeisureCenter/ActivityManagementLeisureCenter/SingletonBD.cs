using Google.Protobuf.WellKnownTypes;
using Microsoft.UI.Xaml.Controls.Primitives;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManagementLeisureCenter
{
    class SingletonBD
    {
        private MySqlConnection con;
        private ObservableCollection<Activites> listeActivites;
        private ObservableCollection<Seances> listeSeancesActvite;
        private static SingletonBD instance = null;

        public SingletonBD()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420-345-ri_eq10;Uid=2361208;Pwd=2361208;");
            listeActivites = new ObservableCollection<Activites>();
            listeSeancesActvite = new ObservableCollection<Seances>();
        }

        public static SingletonBD getInstance()
        {
            if (instance == null)
                instance = new SingletonBD();

            return instance;
        }

        public ObservableCollection<Activites> getListeActivites()
        {
            return listeActivites;
        }

        public ObservableCollection<Seances> getListeSeances()
        {
            return listeSeancesActvite;
        }

        // Méthode pour aller chercher toutes les activitées
        public void chargerActivites()
        {
            listeActivites.Clear();
            var moyennesNotes = ChargerMoyenneNotesParActivite();

            string requete = "SELECT * FROM activite";
            con.Open();
            MySqlCommand commande = new MySqlCommand(requete, con);
            MySqlDataReader reader = commande.ExecuteReader();

            while (reader.Read())
            {
                int idActivite = reader.GetInt32("id_activite");
                string nom = reader.GetString("nom");
                string type = reader.GetString("type");
                decimal cout_organisation = reader.GetDecimal("cout_organisation");
                decimal prix_vente = reader.GetDecimal("prix_vente");
                string image = reader.GetString("image");

                Activites activite = new Activites(nom, type, cout_organisation, prix_vente, image, idActivite);

                // Associer la moyenne si elle existe
                if (moyennesNotes.ContainsKey(nom))
                {
                    activite.MoyenneNote = moyennesNotes[nom];
                }
                listeActivites.Add(activite);
            }
            reader.Close();
            con.Close();
        }

        // Méthode pour aller chercher tout les adherents
        public ObservableCollection<Adherents> getListeAdherents()
        {
            ObservableCollection<Adherents> adherentsList = new ObservableCollection<Adherents>();

            con.Open();

            string query = "SELECT Id_num, Nom, Prenom, Adresse, Date_naissance, Age FROM adherent";
            MySqlCommand command = new MySqlCommand(query, con);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Adherents adherent = new Adherents
                    {
                        Id_num = reader.GetString("Id_num"),
                        Nom = reader.GetString("Nom"),
                        Prenom = reader.GetString("Prenom"),
                        Adresse = reader.GetString("Adresse"),
                        Date_naissance = reader.GetDateTime("Date_naissance"),
                        Age = reader.GetInt32("Age")
                    };
                    adherentsList.Add(adherent);
                }
                reader.Close();
            }
            con.Close();

            return adherentsList;
        }

        // Méthode pour aller chercher tout les adherents
        public ObservableCollection<Activites> getListeActivitesAdmin()
        {
            ObservableCollection<Activites> activitesList = new ObservableCollection<Activites>();

            con.Open();

            string query = "SELECT Nom, Type, Cout_organisation, Prix_vente, Image, id_activite FROM activite";
            MySqlCommand command = new MySqlCommand(query, con);

            using (MySqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Activites activite = new Activites(
                        reader.GetString("Nom"),
                        reader.GetString("Type"),
                        reader.GetDecimal("Cout_organisation"),
                        reader.GetDecimal("Prix_vente"),
                        reader.GetString("Image"),
                        reader.GetInt32("id_activite")
                    );
                    activitesList.Add(activite);
                }
                reader.Close();
            }
            con.Close();

            return activitesList;
        }

        // Méthode pour aller chercher tout les séances pour l'affichage côté administrateur
        public ObservableCollection<Seances> getListeSeancesAdmin()
        {
            ObservableCollection<Seances> seancesList = new ObservableCollection<Seances>();

            con.Open();

            string query = "SELECT Date_debut, Date_fin, Heure_debut, Heure_fin, Nb_places, Nb_personnes, Id_activite, Id_seance FROM seance";
            MySqlCommand commande = new MySqlCommand(query, con);

            using(MySqlDataReader reader = commande.ExecuteReader())
            {
                while (reader.Read())
                {
                    Seances seance = new Seances(
                        reader.GetDateTime("Date_debut"),
                        reader.GetDateTime("Date_fin"),
                        reader.GetTimeSpan("Heure_debut"),
                        reader.GetTimeSpan("Heure_fin"),
                        reader.GetInt32("Nb_places"),
                        reader.GetInt32("Nb_personnes"),
                        reader.GetInt32("Id_activite"),
                        reader.GetInt32("Id_seance")
                    );
                    seancesList.Add(seance);
                }
                reader.Close();
            }
            con.Close();

            return seancesList;
        }

        // Méthode pour aller chercher toutes les séances correspondantes à l'activité choisit
        public void ChargerSeancesParActivite(int idActivite)
        {
            listeSeancesActvite.Clear();

            string requete = "SELECT * FROM seance WHERE id_activite = @idActivite";
            con.Open();
            MySqlCommand commande = new MySqlCommand(requete, con);
            commande.Parameters.AddWithValue("@idActivite", idActivite);

            MySqlDataReader reader = commande.ExecuteReader();

            while (reader.Read())
            {
                DateTime dateDebut = reader.GetDateTime("date_debut");
                TimeSpan heureDebut = reader.GetTimeSpan("heure_debut");
                DateTime dateFin = reader.GetDateTime("date_fin");
                TimeSpan heureFin = reader.GetTimeSpan("heure_fin");
                int nbPlaces = reader.GetInt32("nb_places");
                int nbPersonnes = reader.GetInt32("nb_personnes");
                int id_seance = reader.GetInt32("id_seance");

                Seances seance = new Seances(dateDebut, dateFin, heureDebut, heureFin, nbPlaces, nbPersonnes, idActivite, id_seance);
                listeSeancesActvite.Add(seance);
            }
            reader.Close();
            con.Close();
        }

        // Méthode pour aller chercher la moyenne des notes pour l'activité choisit
        public Dictionary<string, double> ChargerMoyenneNotesParActivite()
            {
                var moyennes = new Dictionary<string, double>();

                string requete = "SELECT activite, moyenne_note FROM moyenne_notes_par_activite";

                con.Open();
                MySqlCommand commande = new MySqlCommand(requete, con);
                MySqlDataReader reader = commande.ExecuteReader();

                while (reader.Read())
                {
                    string activite = reader.GetString("activite");
                    double moyenneNote = reader.GetDouble("moyenne_note");
                    moyennes[activite] = moyenneNote;
                }

                reader.Close();
                con.Close();

                return moyennes;
            }

        // Méthode pour aller chercher les informations de l'adhérent qui se connecte
        public Adherents AuthentifierAdherent(string idNum)
        {
            Adherents adherent = null;

            string requete = "SELECT * FROM adherent WHERE id_num = @idNum";
            con.Open();
            MySqlCommand commande = new MySqlCommand(requete, con);
            commande.Parameters.AddWithValue("@idNum", idNum);

            MySqlDataReader reader = commande.ExecuteReader();

            if (reader.Read())
            {
                string nom = reader.GetString("nom");
                string prenom = reader.GetString("prenom");
                string adresse = reader.GetString("adresse");
                DateTime dateNaissance = reader.GetDateTime("date_naissance");
                int age = reader.GetInt32("age");

                adherent = new Adherents(idNum, nom, prenom, adresse, dateNaissance, age);
            }

            reader.Close();
            con.Close();

            return adherent;
        }

        // Méthode pour aller chercher les informations de l'administrateur qui se connecte
        public Administrateur AuthentifierAdministrateur(string nom_utilisateur, string mdp)
        {
            Administrateur administrateur = null;

            string requete = "SELECT * FROM administrateur WHERE nom_utilisateur = @nom_utilisateur AND mdp = @mdp";
            con.Open();
            MySqlCommand commande = new MySqlCommand(requete, con);
            commande.Parameters.AddWithValue("@nom_utilisateur", nom_utilisateur);

            string mdpHache = Administrateur.HashPassword(mdp);
            commande.Parameters.AddWithValue("@mdp", mdpHache);

            MySqlDataReader reader = commande.ExecuteReader();

            if (reader.Read())
            {
                string nom_utilisateurDb = reader.GetString("nom_utilisateur");
                string mdpDb = reader.GetString("mdp");

                administrateur = new Administrateur(nom_utilisateurDb, mdpDb);
            }

            reader.Close();
            con.Close();

            return administrateur;
        }
        
        // Méthode pour obtenir le nombre total d'adhérents
        public int getTotalAdherents()
        {
            int totalAdherents = 0;
            string requete = "SELECT COUNT(*) FROM adherent";

            con.Open();
            MySqlCommand commande = new MySqlCommand(requete, con);
            totalAdherents = Convert.ToInt32(commande.ExecuteScalar());
            con.Close();

            return totalAdherents;
        }

        // Méthode pour obtenir le nombre total d'activités 
        public int getTotalActivites()
        {
            int totalActivites = 0;
            string requete = "SELECT COUNT(*) FROM activite";

            con.Open();
            MySqlCommand commande = new MySqlCommand(requete, con);
            totalActivites = Convert.ToInt32(commande.ExecuteScalar());
            con.Close();

            return totalActivites;
        }

        // Méthode pour obtenir le nombre d'adhérents pour chaque activité
        public Dictionary<string, int> chargerParticipantsParActivite()
        {
            var participantsParActivite = new Dictionary<string, int>();

            string requete = "SELECT nom_activite, nb_participants FROM participants_par_activite";

            con.Open();
            MySqlCommand commande = new MySqlCommand(requete, con);
            MySqlDataReader reader = commande.ExecuteReader();

            while (reader.Read())
            {
                string nomActivite = reader.GetString("nom_activite");
                int nbParticipants = reader.GetInt32("nb_participants");

                participantsParActivite[nomActivite] = nbParticipants;
            }
            reader.Close();
            con.Close();

            return participantsParActivite;
        }

        // Méthode pour obtenir le participant qui participe au plus grand nombre de séances
        public Dictionary<string, string> ChargerParticipantAvecPlusDeSeances()
        {
            var participants = new Dictionary<string, string>(); 
            
            string requete = "SELECT * FROM participant_avec_plus_seance";

            con.Open();
            MySqlCommand commande = new MySqlCommand(requete, con);
            MySqlDataReader reader = commande.ExecuteReader();

            while (reader.Read())
            {
                string nom = reader.GetString("nom");
                string prenom = reader.GetString("prenom");
                int nbSeances = reader.GetInt32("nb_seances");

                participants[$"{prenom} {nom}"] = $"{nbSeances} séances";
            }
            reader.Close();
            con.Close();

            return participants;
        }

        // Méthode pour obtenir le prix moyen que chaque participant rapporte ($)
        public Dictionary<string, double> ChargerPrixMoyenParParticipant()
        {
            var prixMoyens = new Dictionary<string, double>();

            string requete = "SELECT nom, prenom, prix_moyen FROM prix_moyen_par_participant";

            con.Open();
            MySqlCommand commande = new MySqlCommand(requete, con);
            MySqlDataReader reader = commande.ExecuteReader();

            while (reader.Read())
            {
                string participant = reader.GetString("prenom") + " " + reader.GetString("nom");
                double prixMoyen = reader.GetDouble("prix_moyen");
                prixMoyens[participant] = prixMoyen;
            }
            reader.Close();
            con.Close();

            return prixMoyens;
        }

        // Méthode pour obtenir le nombre de participant moyen par mois
        public Dictionary<string, double> ChargerParticipantsMoyenParMois()
        {
            var participantsMoyens = new Dictionary<string, double>();

            string requete = "SELECT * FROM participants_moyen_par_mois";

            con.Open();
            MySqlCommand commande = new MySqlCommand(requete, con);
            MySqlDataReader reader = commande.ExecuteReader();

            while (reader.Read())
            {
                string moisAnnee = $"{reader.GetInt32("mois")}/{reader.GetInt32("annee")}";
                double participantsMoyensParMois = reader.GetDouble("participants_moyens");

                participantsMoyens[moisAnnee] = participantsMoyensParMois;
            }
            reader.Close();
            con.Close();

            return participantsMoyens;
        }

        // Méthode pour supprimer un adhérent
        public bool SupprimerAdherent(string idAdherent)
        {
            try
            {
                string requete = "CALL supprimerAdherent(@idAdherent)";
                con.Open();
                MySqlCommand commande = new MySqlCommand(requete, con);
                commande.Parameters.AddWithValue("@idAdherent", idAdherent);
                commande.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
        }

        // Méthode pour supprimer une activité
        public bool SupprimerActivite(int idActivite)
        {
            try
            {
                string requete = "CALL supprimer_activite(@idActivite)";
                con.Open();
                MySqlCommand commande = new MySqlCommand(requete, con);
                commande.Parameters.AddWithValue("@idActivite", idActivite);
                commande.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
        }
  

        // Méthode pour supprimer une séance
        public bool SupprimerSeance(int idSeance)
        {
            try
            {
                string requete = "CALL supprimer_seance(@idSeance)";
                con.Open();
                MySqlCommand commande = new MySqlCommand(requete, con);
                commande.Parameters.AddWithValue("@idSeance", idSeance);
                commande.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
        }

        // Méthode pour ajouter un adhérent
        public bool AjouterAdherent(string nom, string prenom, string adresse, DateTime dateNaissance, int age)
        {
            try
            {
                string requete = "INSERT INTO adherent (Nom, Prenom, Adresse, Date_naissance, Age) VALUES (@nom, @prenom, @adresse, @dateNaissance, @age)";
                con.Open();
                MySqlCommand commande = new MySqlCommand(requete, con);
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@prenom", prenom);
                commande.Parameters.AddWithValue("@adresse", adresse);
                commande.Parameters.AddWithValue("@dateNaissance", dateNaissance);
                commande.Parameters.AddWithValue("@age", age);
                commande.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout de l'adhérent : " + ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        // Méthode pour ajouter une activité
        public bool AjouterActivite(string nom, string type, decimal cout_organisation, decimal prix_vente, string image)
        {
            try
            {
                string requete = "INSERT INTO activite (Nom, Type, Cout_organisation, Prix_vente, Image) VALUES (@nom, @type, @cout_organisation, @prix_vente, @image)";
                con.Open();
                MySqlCommand commande = new MySqlCommand(requete, con);
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@type", type);
                commande.Parameters.AddWithValue("@cout_organisation", cout_organisation);
                commande.Parameters.AddWithValue("@prix_vente", prix_vente);
                commande.Parameters.AddWithValue("@image", image);
                commande.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout de l'activité : " + ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        // Méthode pour ajouter une séance
        public bool AjouterSeance(DateTime dateDebut, TimeSpan heureDebut, DateTime dateFin, TimeSpan heureFin, int nbPlaces, int nbPersonnes, int idActivite)
        {
            try
            {
                string requete = "INSERT INTO seance (date_debut, heure_debut, date_fin, heure_fin, nb_places, nb_personnes, id_activite) VALUES (@dateDebut, @heureDebut, @dateFin, @heureFin, @nbPlaces, @nbPersonnes, @idActivite)";
                con.Open();
                MySqlCommand commande = new MySqlCommand(requete, con);
                commande.Parameters.AddWithValue("@dateDebut", dateDebut);
                commande.Parameters.AddWithValue("@heureDebut", heureDebut);
                commande.Parameters.AddWithValue("@dateFin", dateFin);
                commande.Parameters.AddWithValue("@heureFin", heureFin);
                commande.Parameters.AddWithValue("@nbPlaces", nbPlaces);
                commande.Parameters.AddWithValue("@nbPersonnes", nbPersonnes);
                commande.Parameters.AddWithValue("@idActivite", idActivite);
                commande.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'ajout de la séance : " + ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        // Méthode pour inscrire un adhérent à une séance
        public bool InscrireAdherent(string idNum, int idSeance)
        {
            try
            {
                con.Open();
                string query = "INSERT INTO inscrit (id_num, id_seance) VALUES (@idNum, @idSeance)";
                using (var commande = new MySqlCommand(query, con))
                {
                    commande.Parameters.AddWithValue("@idNum", idNum);
                    commande.Parameters.AddWithValue("@idSeance", idSeance);
                    commande.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                con.Close();
            }
        }

    }
}
