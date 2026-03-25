using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    internal class Client
    {
        public enum SorteComptes { Aucun, Épargne, Chèque, intérêt }

        public enum Roles { Administrateur, Client }

        private string m_motDePasse;
        private string m_nom;
        private string m_numClient;
        private Roles m_role;
        private int m_solde;
        private SorteComptes m_sorteCompte;
        public const int MAX_SOLDE = 100; // le nombre n'Est pas bon

    }
}
