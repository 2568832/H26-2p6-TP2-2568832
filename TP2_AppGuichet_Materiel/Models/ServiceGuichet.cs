using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ServiceGuichet
    {


        // Champs
        public Client ClientCourant;
        private string m_cheminFichierClients;
        private string m_cheminFichierTransactions;
        private List<Client> m_clients;
        private List<Transaction> m_transactions;

        // Propriétés
        public string CheminFichierClients
        {
            get;
        }

        public string CheminFichierTransactions
        {
            get;
        }

        public List<Client> Clients { get; }

        public List<Transaction> Transactions { get; }


        // Constructeur
        ServiceGuichet(string pCheminFichierClients, string pCheminFichierTransactions)
        {
            if (File.Exists(pCheminFichierClients) || File.Exists(pCheminFichierTransactions))
            {
                CheminFichierClients = pCheminFichierClients;
                CheminFichierTransactions = pCheminFichierTransactions;
            }
            

        }

        // Méthodes

        public int ChargerClients()
        {
            int TentativeEchouées = 0;
            using (StreamReader file = new StreamReader(CheminFichierClients))
            {
                

                while (file.EndOfStream)
                {
                    try
                    {
                        string line = file.ReadLine();
                        Client c = new Client(line);
                        Clients.Add(c);
                    }
                    catch { TentativeEchouées++; }
                }
            }
            return TentativeEchouées;
        }

        public int ChargerTransactions()
        {
            int TentativeEchouées = 0;
            using (StreamReader file = new StreamReader(CheminFichierTransactions))
            {


                while (file.EndOfStream)
                {
                    try
                    {
                        string line = file.ReadLine();
                        string[] séparation = line.Split(",");
                        CreerTransaction(((SorteTransactions)(int.Parse(séparation[0]))), (séparation[1]), (DateTime.Parse(séparation[2])), (int.Parse(séparation[3])) );
                    }
                    catch { TentativeEchouées++; }
                }
            }
            return TentativeEchouées;
        }

        public void CreerTransaction(SorteTransactions pSorte, string pNumClient, DateTime pDate, int pMontant)
        {
            try
            {
                Transaction T = new Transaction(pSorte, pNumClient, pDate, pMontant);
                Transactions.Add(T);
            }
            catch (ArgumentException ex)
            {
            }
            
        }

    }



}
