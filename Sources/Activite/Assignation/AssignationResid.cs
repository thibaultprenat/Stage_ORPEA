using MySql.Data.MySqlClient;
using Orpea_WF.Resident;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Orpea_WF.Activite.Assignation
{
    public class AssignationResid
    {
        public int CodeActivite                { private get; set; }
        public DateTime DateActivite           { private get; set; }
        private string LibelleActivite         { get; set; }
        private string Nom                     { get; set; }
        private string Prenom                  { get; set; }
        private int IdResident                 { get; set; }
        public List<DateTime> ListeDates       { get; private set; }
        public ListesResidents ListesResidents { get; private set; }

        public string RecupPlanningActivites(ComboBox comboActivite)
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = "SELECT PLAN_ACTIVITE, PLAN_DATE_DEBUT, ACT_LIBELLE " +
                                         "FROM PLANNING P " +
                                         "INNER JOIN  ACTIVITES A " +
                                         "ON P.PLAN_ACTIVITE = A.ACT_CODE";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        return RpaExecution(comboActivite, mySqlCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                return MessageBox.Show($@"Erreur : {ex.Message}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }

        private string RpaExecution(ComboBox comboActivite, MySqlCommand mySqlCommand)
        {
            using (var mySqlDataReader = mySqlCommand.ExecuteReader())
            {
                if (!mySqlDataReader.HasRows)
                    return "0";

                ListeDates = new List<DateTime>();

                while (mySqlDataReader.Read())
                {
                    CodeActivite    = mySqlDataReader.GetInt16(0);
                    DateActivite    = mySqlDataReader.GetDateTime(1);
                    LibelleActivite = mySqlDataReader.GetString(2);

                    comboActivite.Invoke((MethodInvoker)delegate
                    {
                        comboActivite.Items.Add(new ObjectItemId(CodeActivite.ToString(), LibelleActivite));
                    });
                    ListeDates.Add(DateActivite);
                }

                return "1";
            }
        }

        public void RecupPlanningResidents(ListBox listBoxResidents)
        {
            ListesResidents = new ListesResidents();
            ListesResidents.RecupListeResidents();
            ListesResidents.RecupListeResidentsDesaffect();

            foreach (string item in ListesResidents.ResidCollection)
            {
                listBoxResidents.Invoke((MethodInvoker)delegate { listBoxResidents.Items.Add(item); });
            }
        }

        public string RecupPlanningParticipants(ListBox listBoxParticipants)
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = "SELECT PART_PLAN_ACTIVITE, RESID_ID, RESID_NOM, RESID_PRENOM " +
                                         "FROM PARTICIPATIONS B " +
                                         "INNER JOIN RESIDENTS A " +
                                         "ON B.PART_RESID_ID=A.RESID_ID " +
                                         "WHERE PART_PLAN_ACTIVITE=@PART_PLAN_ACTIVITE AND PART_PLAN_DATE_D=@PART_PLAN_DATE_D";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter("@PART_PLAN_ACTIVITE", MySqlDbType.Int16));
                        mySqlCommand.Parameters["@PART_PLAN_ACTIVITE"].Value = CodeActivite;
                        mySqlCommand.Parameters.Add(new MySqlParameter("@PART_PLAN_DATE_D", MySqlDbType.DateTime));
                        mySqlCommand.Parameters["@PART_PLAN_DATE_D"].Value   = DateActivite;

                        return RpExecution(listBoxParticipants, mySqlCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                return MessageBox.Show(@"Erreur : " + ex.Message, @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }

        private string RpExecution(ListBox listBoxParticipants, MySqlCommand mySqlCommand)
        {
            using (var mySqlDataReader = mySqlCommand.ExecuteReader())
            {
                while (mySqlDataReader.Read())
                {
                    IdResident = mySqlDataReader.GetInt16(1);
                    Nom        = mySqlDataReader.GetString(2);
                    Prenom     = mySqlDataReader.GetString(3);

                    listBoxParticipants.Invoke((MethodInvoker)delegate
                    {
                        listBoxParticipants.Items.Add(new ObjectItemId(IdResident.ToString(), Nom + " " + Prenom));
                    });
                    frmAssignation.ListeIdParticipants.Add(IdResident.ToString());
                }

                return "1";
            }
        }

        public string AssignerResidentsPlanning()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = @"INSERT IGNORE INTO PARTICIPATIONS (PART_RESID_ID, PART_PLAN_ACTIVITE, PART_PLAN_DATE_D) 
                                           VALUES (@PART_RESID_ID, @PART_PLAN_ACTIVITE, @PART_PLAN_DATE_D);

                                           DELETE IGNORE FROM PARTICIPATIONS WHERE PART_RESID_ID=@PART_RESID_ID_SUPPR AND PART_PLAN_DATE_D=@PART_PLAN_DATE_D;";

                    const int result = 0;

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter("@PART_RESID_ID", MySqlDbType.Int16));
                        mySqlCommand.Parameters.Add(new MySqlParameter("@PART_RESID_ID_SUPPR", MySqlDbType.Int16));
                        mySqlCommand.Parameters.Add(new MySqlParameter("@PART_PLAN_ACTIVITE", MySqlDbType.Int16));
                        mySqlCommand.Parameters.Add(new MySqlParameter("@PART_PLAN_DATE_D", MySqlDbType.DateTime));

                        return ArExecution(mySqlCommand, result);
                    }
                }
            }
            catch (Exception ex)
            {
                return MessageBox.Show($@"Erreur : {ex.Message}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }

        private string ArExecution(MySqlCommand mySqlCommand, int result)
        {
            foreach (var ajoutParticipants in frmAssignation.ListeIdParticipants)
            {
                mySqlCommand.Parameters["@PART_RESID_ID"].Value      = ajoutParticipants;
                mySqlCommand.Parameters["@PART_PLAN_ACTIVITE"].Value = CodeActivite;
                mySqlCommand.Parameters["@PART_PLAN_DATE_D"].Value   = DateActivite;
                result = mySqlCommand.ExecuteNonQuery();
            }

            if (frmAssignation.ListeIdParticipantsASupprimer.Count <= 0) return result < 0 ? "0" : "1";

            foreach (var supprParticipants in frmAssignation.ListeIdParticipantsASupprimer)
            {
                mySqlCommand.Parameters["@PART_RESID_ID_SUPPR"].Value = supprParticipants;
                result = mySqlCommand.ExecuteNonQuery();
            }


            return result < 0 ? "0" : "1";
        }
    }
}