using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Models
{
    public enum SorteComptes { Aucun, Épargne, Chèque, intérêt }

    public enum Roles { Administrateur, Client }

    public class Client
    {
        
        // Champs
        private string m_motDePasse;
        private string m_nom;
        private string m_numClient;
        private Roles m_role;
        private int m_solde;
        private SorteComptes m_sorteCompte;
        public const int MAX_SOLDE = 1000000; // le nombre n'Est pas bon

        // Propriété

        public bool IsAdmin
        {
            get 
            {
                if (m_role == Roles.Administrateur)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }

        public string MotDePasse
        {
            get { return m_motDePasse; }
            set { m_motDePasse = value; }
        }

        public string Nom
        {
            get { return m_nom; }
            set 
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if ((value.Trim()).Length < 3)
                {
                    throw new ArgumentOutOfRangeException();
                }
                m_nom = value; 
            }
        }

        public string NumClient
        {
            get { return m_numClient; }
            set { 
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                bool reussi = int.TryParse(value.Trim(), out int nombre);
                if ((value.Trim()).Length != 6 || !reussi)
                {
                    throw new ArgumentException();
                }
                m_numClient = value.Trim(); 
            }
        }

        public Roles Role
        {
            get { return m_role; }
            set { m_role = value; }
        }

        public int Solde
        {
            get { return m_solde; }
            set 
            { 
                if (value < 0)
                {
                    throw new InvalidOperationException();
                }
                if (value > MAX_SOLDE)
                {
                    throw new InvalidOperationException();
                }
                m_solde = value; 
            }
        }

        public SorteComptes SorteCompte
        {
            get { return m_sorteCompte; }
            set { m_sorteCompte = value; }
        }

        public List<Transaction> Transactions
        {
            get ;
            set ;
        }

        // Constructeurs 

        public Client()
        { }

        public Client(string pNumClient, string pNom, string pMotDePasse,Roles pRole, SorteComptes pSorte, int pSolde)
        {
            if (pSolde < 0 || pSolde > MAX_SOLDE)
            {
                throw new ArgumentOutOfRangeException();
            }
            NumClient = pNumClient;
            Nom = pNom;
            MotDePasse = pMotDePasse;
            Role = pRole;
            SorteCompte = pSorte;
            Solde = pSolde;

            Transactions = new List<Transaction>();
        }

        public Client(string pChaineLue)
        {
            string[] séparation = pChaineLue.Split(',');

            if (int.Parse(séparation[5]) < 0 || int.Parse(séparation[5]) > MAX_SOLDE)
            {
                throw new ArgumentOutOfRangeException();
            }
            NumClient = séparation[0];
            Nom = séparation[1];
            MotDePasse = séparation[2];
            Role = (Roles)(int.Parse(séparation[3]));
            SorteCompte = (SorteComptes)(int.Parse(séparation[4]));
            Solde = int.Parse(séparation[5]);

            Transactions = new List<Transaction>();
        }

        // Méthodes
        public void Deposer(int pMontant)
        {
            if ( pMontant <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (pMontant > MAX_SOLDE)
            {
                throw new InvalidOperationException();
            }
            Solde += pMontant;
        }

        public void Retirer(int pMontant)
        {
            if (pMontant <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            if (pMontant > Solde)
            {
                throw new InvalidOperationException();
            }
            Solde -= pMontant;
        }
        public bool PeutRetirer(int pMontant)
        {
            if (Solde - pMontant > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void AjouterTransaction(Transaction pTransaction)
        {
            if (pTransaction == null)
            {
                throw new ArgumentNullException();
            }
            if (pTransaction.NumClient != NumClient)
            {
                throw new InvalidOperationException();
            }
            if (Transactions.Contains(pTransaction)) 
            {
                throw new InvalidOperationException();
            }
            
            Transactions.Add(pTransaction);
            
        }

        public string ToCsv()
        {
            
            string resultat = ($"{NumClient},{Nom},{MotDePasse},{Role},{SorteCompte},{Solde}");
            return resultat;
        }
    }
}
