using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManagementLeisureCenter
{
    internal class Administrateur
    {
        string nom_utilisateur, mdp;

        public Administrateur ()
        {

        }

        public Administrateur(string nom_utilisateur, string mdp)
        {
            Nom_utilisateur = nom_utilisateur;
            Mdp = HashPassword(mdp);
        }

        public string Nom_utilisateur { get => nom_utilisateur; set => nom_utilisateur = value; }
        public string Mdp { get; private set; }

        // Méthode pour hacher le mot de passe avec SHA-1
        public static string HashPassword(string password)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public bool VerifierMotDePasse(string motDePasse)
        {
            // Hacher le mot de passe donné et le comparer avec le mot de passe haché stocké
            return Mdp == HashPassword(motDePasse);
        }

    }
}
