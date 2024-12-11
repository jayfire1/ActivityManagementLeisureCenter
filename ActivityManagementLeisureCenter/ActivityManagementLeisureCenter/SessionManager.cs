using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActivityManagementLeisureCenter
{
    public static class SessionManager
    {
        public static bool EstConnecte { get; set; } = false;

        public static string UtilisateurConnecte { get; set; } = String.Empty;

        public static bool EstAdministrateur { get; set; } = false;

        public static string UtilisateurId { get; set; } = String.Empty;
    }
}
