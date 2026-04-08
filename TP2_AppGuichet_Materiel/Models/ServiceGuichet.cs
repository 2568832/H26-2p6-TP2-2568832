using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;

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
            get {  return m_cheminFichierClients;}
        }

        public string CheminFichierTransactions
        {
            get { return m_cheminFichierTransactions;}
        }

        public List<Client> Clients { get { return m_clients; } }

        public List<Transaction> Transactions { get { return m_transactions; } }


        // Constructeur
        public ServiceGuichet(string pCheminFichierClients, string pCheminFichierTransactions)
        {
            if (File.Exists(pCheminFichierClients) && File.Exists(pCheminFichierTransactions))
            {
                m_cheminFichierClients = pCheminFichierClients;
                m_cheminFichierTransactions = pCheminFichierTransactions;
                m_clients = new List<Client>();
                m_transactions = new List<Transaction>();
                
            }
            else
            {
                throw new ArgumentException();
            }
            

        }

        // Méthodes

        public int ChargerClients()
        {
            if (File.Exists(CheminFichierClients))
            {
                using (StreamReader file = new StreamReader(CheminFichierClients))
                {
                    int TentativeEchouées = 0;

                    while (!file.EndOfStream)
                    {
                        try
                        {
                            string line = file.ReadLine();
                            Client c = new Client(line);
                            Clients.Add(c);

                        }
                        catch
                        {
                            if (TentativeEchouées < 2)
                            {
                                TentativeEchouées++;

                            }
                        }
                    }
                    return TentativeEchouées;
                }
            }
            else { throw new ArgumentException(); }
        }

        public int ChargerTransactions()
        {
            
            using (StreamReader file = new StreamReader(CheminFichierTransactions))
            {
                int tentativeEchouées = 0;

                while (!file.EndOfStream)
                {
                    try
                    {
                        string line = file.ReadLine();
                        string[] séparation = line.Split(',');
                        CreerTransaction(((SorteTransactions)( int.Parse(séparation[0]))), (séparation[1]), (DateTime.Parse(séparation[2])), (int.Parse(séparation[3])) );
                    }
                    catch { tentativeEchouées++; }
                }
                return tentativeEchouées;
            }
            
        }

        public void CreerTransaction(SorteTransactions pSorte, string pNumClient, DateTime pDate, int pMontant)
        {
            if (TrouverClient(pNumClient) != null)
            {
                Transaction T = new Transaction(pSorte, pNumClient, pDate, pMontant);
                Transactions.Add(T);
                TrouverClient(pNumClient).AjouterTransaction(T);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public bool Sauvegarde()
        {
            try {
                SauvegarderClients(CheminFichierClients);
                SauvegarderTransactions(CheminFichierTransactions);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void SauvegarderTransactions(string pFichier)
        {
            
            using (StreamWriter writ = new StreamWriter(pFichier))
            {

                for (int i = 0; i < Transactions.Count; i++)
                {
                    Transaction t = new Transaction(Transactions[i].SorteTransaction, Transactions[i].NumClient, Transactions[i].Date, Transactions[i].Montant);
                    string T = t.ToCsv();
                    writ.WriteLine(T);
                    
                }

            }
            
            
        }
        public void SauvegarderClients(string pFichier)
        {
            using (StreamWriter writ = new StreamWriter(pFichier))
            {

                for (int i = 0; i < Clients.Count; i++)
                {
                    Client client = new Client(Clients[i].NumClient, Clients[i].Nom, Clients[i].MotDePasse, Clients[i].Role, Clients[i].SorteCompte, Clients[i].Solde);
                    string C = client.ToCsv();
                    writ.WriteLine(C);

                }

            }
        }
        public bool Connexion(string numClient, string motDePasse)
        {
            bool rep = false;
            foreach (Client C in Clients)
            {
                if (C.NumClient == numClient && C.MotDePasse == motDePasse)
                {
                    ClientCourant = C;
                    rep = true;
                    break;
                }
                else if (rep == false)
                {
                    rep = false;

                }
                

            }
            return rep;
        }

        public bool Deconnexion()
        {
            ClientCourant = null;
            return true;
        }

        public Client TrouverClient(string pNumClient)
        {
            Client Client = null;
            foreach (Client C in Clients)
            {
                if (C.NumClient == pNumClient)
                {
                    Client = C;
                    break;
                }
                else
                {
                    Client = null;

                }
            }
            return Client;
        }
    }
}
