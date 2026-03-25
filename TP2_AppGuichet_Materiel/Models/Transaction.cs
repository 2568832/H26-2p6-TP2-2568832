using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public enum SorteTransactions { Dépôt, Retrait }
    public enum FiltreOperation { Toutes, Dépôt, Retrait }


    public class Transaction // c'est la première de mes claaaaaaasssssssss
    {
        // champ
        private DateTime m_date;
        private int m_montant;
        private string m_numClient;
        private SorteTransactions m_sorteTransaction;

        // Propriétés

        public DateTime Date
        {
            get { return m_date; }
            set { m_date = value; } 
        }

        public int Montant
        {
            get { return m_montant; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                
                m_montant = value;
                
            }
        }

        public string NumClient
        {
            get { return m_numClient; }
            set
            {
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

        public SorteTransactions SorteTransaction
        {
            get { return m_sorteTransaction; }
            set { m_sorteTransaction = value; }
        }

        // Constructeur

        public Transaction() // N'EST PAS BON !!!!!!!!!!!!!!!!!
        {
            
        }
        public Transaction(SorteTransactions pSorte, string pNumClient, DateTime pDate, int pMontant)
        {
            SorteTransaction = pSorte;
            NumClient = pNumClient;
            Date = pDate;
            Montant = pMontant;

        }

        // Méthodes

        public string ToCsv()
        {
            string sorte;
            if (m_sorteTransaction == SorteTransactions.Dépôt)
            {
                sorte = "0";
            }
            else {
                sorte = "1";
            }
            string resultat = ($"{sorte},{NumClient.ToString()},{m_date.ToString()},{m_montant.ToString()}");
            return resultat;
        }


    }
}
