using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManagementLeisureCenter
{
    internal class Adherents
    {
        string id_num, nom, prenom, adresse;
        DateTime date_naissance;
        int age;

        public Adherents()
        {

        }

        public Adherents(string id_num, string nom, string prenom, string adresse, DateTime date_naissance, int age)
        {
            Id_num = id_num;
            Nom = nom;
            Prenom = prenom;
            Adresse = adresse;
            Date_naissance = date_naissance;
            Age = age;
        }

        public string Id_num { get => id_num; set => id_num = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public string Adresse { get => adresse; set => adresse = value; }
        public DateTime Date_naissance { get => date_naissance; set => date_naissance = value; }
        public int Age { get => age; set => age = value; }

        public string Date_naissance_Formatee 
        {
            get
            {
                return Date_naissance.ToString("dd/MM/yyyy");
            }
        }

    }
}
