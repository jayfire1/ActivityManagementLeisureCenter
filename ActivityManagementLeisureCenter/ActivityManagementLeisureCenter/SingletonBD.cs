using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManagementLeisureCenter
{
    class SingletonBD
    {
        private MySqlConnection con;
        private ObservableCollection<Activites> listeActivites;
        private static SingletonBD instance = null;

        public SingletonBD()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420-345-ri_eq10;Uid=2361208;Pwd=2361208;");
            listeActivites = new ObservableCollection<Activites>();
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

        // Méthode pour aller chercher toutes les activitées
        public void chargerActivites()
        {
            listeActivites.Clear();
            MySqlCommand commande = new MySqlCommand("SELECT * FROM activite", con);

            con.Open();
            MySqlDataReader reader = commande.ExecuteReader();

            while (reader.Read())
            {
                string nom = reader.GetString("nom");
                string type = reader.GetString("type");
                decimal cout_organisation = reader.GetDecimal("cout_organisation");
                decimal prix_vente = reader.GetDecimal("prix_vente");
                string image = reader.GetString("image");

                Activites activite = new Activites(nom, type, cout_organisation, prix_vente, image);
                listeActivites.Add(activite);
            }
            reader.Close();
            con.Close();
        }



    }
}
