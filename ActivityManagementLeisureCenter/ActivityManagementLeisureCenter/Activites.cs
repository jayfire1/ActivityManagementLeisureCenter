using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManagementLeisureCenter
{
    class Activites
    {
        string nom, type, image;
        decimal cout_organisation, prix_vente;
        int id_activite;

        public Activites()
        {

        }

        public Activites(string nom, string type, decimal cout_organisation, decimal prix_vente, string image, int id_activite)
        {
            Nom = nom;
            Type = type;
            Cout_organisation = cout_organisation;
            Prix_vente = prix_vente;
            Image = image;
            IdActivite = id_activite;
        }

        public string Nom { get => nom; set => nom = value; }
        public string Type { get => type; set => type = value; }
        public decimal Cout_organisation { get => cout_organisation; set => cout_organisation = value; }
        public decimal Prix_vente { get => prix_vente; set => prix_vente = value; }
        public string Image { get => image; set => image = value; }
        public int IdActivite { get => id_activite; set => id_activite = value; }
        public double MoyenneNote { get; set; }

        public string PrixVenteFormatte => $"{Prix_vente:0.00} $";
    }
}
