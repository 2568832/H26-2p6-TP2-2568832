using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using Models;


namespace AppGuichet
{
    /// =====================================================================================================================
    /// <summary>
    /// Représente l’utilisation d’un guichet automatique. Un client peut se connecter avec son numéro et son mot de passe.
    /// Il peut retirer de l’argent si son solde le permet. L’administrateur du guichet peut demander la liste des clients
    /// où la liste des transactions effectuées sur le guichet.
    /// </summary>
    /// ======================================================================================================================
    public partial class FrmPrincipal : Form
    {
        public const string APP_INFO = "(Démo)";

        #region Constantes
        //--- CHAMPS: constantes ----------------------------------------------------------
        public const string CHEMIN_FICHIER_CLIENTS = "../../../Fichiers/Clients.csv";
        public const string CHEMIN_FICHIER_TRANSACTIONS = "../../../Fichiers/Transactions.csv";
        #endregion

        #region Champs et Propriétés
        public ServiceGuichet ServiceGuichets = new ServiceGuichet(CHEMIN_FICHIER_CLIENTS,CHEMIN_FICHIER_TRANSACTIONS);

        #endregion

        #region Constructeur
        //---------------------------------------------------------------------------------
        public FrmPrincipal()
        {
            InitializeComponent();
            ServiceGuichets.ChargerClients();
            ServiceGuichets.ChargerTransactions();

            this.Text += APP_INFO;


        }
        #endregion

        public List<Client> clients;
        public Client utilisateur;



        #region Menu Administrateur
        //---------------------------------------------------------------------------------
        private void mnuAdminListeClients_Click(object sender, EventArgs e)
        {
            FrmListeClients frm = new FrmListeClients();
            frm.ShowDialog();
        }
        //---------------------------------------------------------------------------------
        private void mnuAdminListeTransactions_Click(object sender, EventArgs e)
        {
            FrmListeTransactions frmListeTransactions = new FrmListeTransactions();
            frmListeTransactions.ShowDialog();
        }


        #endregion

        private void FrmPrincipal_FormClosing(object sender, EventArgs e)
        {


        }
        private void mnuFichierQuitter_Click(object sender, EventArgs e)
        {



        }



        #region Bouton Connexion/Déconnexion 
        //---------------------------------------------------------------------------------
        public bool Connexion = false;
        public void btnConnexion_Click(object sender, EventArgs e)
        {
            if (Connexion == true)
            {
                txtMotDePasse.Clear();
                txtNumClient.Clear();
                txtNom.Clear();
                txtSolde.Clear();
                txtSorteCompte.Clear();
            }
            

            try
            {
                ServiceGuichets.Connexion(txtNumClient.Text.Trim(), txtMotDePasse.Text.Trim());



                if (ServiceGuichets.Connexion(txtNumClient.Text.Trim(), txtMotDePasse.Text.Trim()) == true)
                {

                    Connexion = true;

                    txtMotDePasse.Enabled = false;
                    txtNumClient.Enabled = false;

                    txtNom.Text = ServiceGuichets.ClientCourant.Nom;
                    txtSorteCompte.Text = ServiceGuichets.ClientCourant.SorteCompte.ToString();
                    txtSolde.Text = ServiceGuichets.ClientCourant.Solde.ToString();

                    if (txtNumClient.Text != "000000")
                    {
                        grpInfosClient.Enabled = true;
                        mnuAdministrateur.Enabled = false;
                        mnuAdminListeClients.Enabled = false;
                        mnuAdminListeTransactions.Enabled = false;
                        
                    }
                    else
                    {
                        mnuAdministrateur.Enabled = true;
                        mnuAdminListeClients.Enabled = true;
                        mnuAdminListeTransactions.Enabled = true;
                    }
                }
                else 
                {
                    Connexion = false;

                    grpInfosClient.Enabled = false;
                    txtMotDePasse.Enabled = true;
                    txtNumClient.Enabled = true;

                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            if (Connexion == false)
            {
                btnConnexion.Text = "Se connecter";
            }
            else
            {
                btnConnexion.Text = "Se déconnecter";

            }

        }
        
        #endregion

        #region Bouton Retirer et Événement Combo Montant à retirer
        //---------------------------------------------------------------------------------
        //Retire le montant choisi
        public void btnDeposer_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboMontant.Text != null || cboMontant.Text != "")
                {
                    
                    try
                    {
                        ServiceGuichets.ClientCourant.Deposer(int.Parse(cboMontant.Text));
                    }
                    catch
                    {
                        MessageBox.Show("Erreur");
                    }

                }
            }
            catch (InvalidOperationException  ex) 
            {
                MessageBox.Show(ex.Message);
            }
            
            txtSolde.Text = ServiceGuichets.ClientCourant.Solde.ToString();
            


        }
        //---------------------------------------------------------------------------------
        //Choix du montant à retirer
        private void cboMontant_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ServiceGuichets.ClientCourant.PeutRetirer(int.Parse(cboMontant.Text)))
            {
                btnRetirer.Enabled = false;
            }
            else { btnRetirer.Enabled = true; }
        }

        #endregion

        private void btnRetirer_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboMontant.Text != null)
                {
                    
                    try
                    {
                        ServiceGuichets.ClientCourant.Retirer(int.Parse(cboMontant.Text));
                    }
                    catch
                    {
                        MessageBox.Show("Erreur");
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
            txtSolde.Text = ServiceGuichets.ClientCourant.Solde.ToString();
            try // ---------------------------------------------------------------------------------Les Try ne sont pas tous bon
            {
                if (!ServiceGuichets.ClientCourant.PeutRetirer(int.Parse(cboMontant.Text)))
                {
                    btnRetirer.Enabled = false;
                }
                else { btnRetirer.Enabled = true; }
            }
            catch
            {
                MessageBox.Show("Erreur dans le if");

            }

        }



        private void mnuAdministrateur_Click(object sender, EventArgs e)
        {

        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
           
        }
    }
}