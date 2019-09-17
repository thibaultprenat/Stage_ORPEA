using MySql.Data.MySqlClient;
using Orpea_WF.Activite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Orpea_WF.Resident
{
    public class ListesResidents
    {
        private string Nom                { get; set; }
        private string Prenom             { get; set; }
        private int IdResident            { get; set; }
        private string NomDesaffect       { get; set; }
        private string PrenomDesaffect    { get; set; }

        public readonly AutoCompleteStringCollection ResidCollection = new AutoCompleteStringCollection();
        public readonly List<int> IdResidents                        = new List<int>();
        public readonly ArrayList ResidentsDesaffect                 = new ArrayList();

        public string RecupListeResidents()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = "SELECT RESID_ID, RESID_NOM, RESID_PRENOM " +
                                         "FROM RESIDENTS";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        return RlrExecution(mySqlCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                return MessageBox.Show($@"Erreur : {ex.Message}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }

        private string RlrExecution(MySqlCommand mySqlCommand)
        {
            using (var mySqlDataReader = mySqlCommand.ExecuteReader())
            {
                if (!mySqlDataReader.HasRows) return "0";

                while (mySqlDataReader.Read())
                {
                    IdResident = mySqlDataReader.GetInt16(0);
                    Nom        = mySqlDataReader.GetString(1);
                    Prenom     = mySqlDataReader.GetString(2);

                    ResidCollection.Add(Nom + " " + Prenom);
                    IdResidents.Add(IdResident);
                }

                return "1";
            }
        }

        public string RecupListeResidentsDesaffect()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = "SELECT RESID_ID, RESID_NOM, RESID_PRENOM " +
                                         "FROM DESAFFECTATIONS B " +
                                         "INNER JOIN RESIDENTS A " +
                                         "ON B.DESAFECT_RESIDENT=A.RESID_ID";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        return RlrdExecution(mySqlCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                return MessageBox.Show($@"Erreur : {ex.Message}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }

        private string RlrdExecution(MySqlCommand mySqlCommand)
        {
            using (var mySqlDataReader = mySqlCommand.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    IdResident      = mySqlDataReader.GetInt16(0);
                    NomDesaffect    = mySqlDataReader.GetString(1);
                    PrenomDesaffect = mySqlDataReader.GetString(2);

                    ResidentsDesaffect.Add(new ObjectItemId(IdResident.ToString(), NomDesaffect + " " + PrenomDesaffect));
                    ResidCollection.Remove(NomDesaffect + " " + PrenomDesaffect);
                    IdResidents.Remove(IdResident);
                }

                return "1";
            }
        }
    }
}
