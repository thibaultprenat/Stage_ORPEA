using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace Orpea_WF.Activite.Planning
{
    public class PlanningAct
    {
        private int CodeActivite         { get; set; }
        private string NomParticipant    { get; set; }
        private string PrenomParticipant { get; set; }
        private string LibelleActivite   { get; set; }
        private DateTime DateActivite    { get; set; }
        private UcPlanning UcPlanning    { get; set; }
        public DataTable DataTablePrint  { get; private set; }

        public string RecupPlanningActivites(FlowLayoutPanel flPlanning, frmConsultPlanning frmConsultPlanning)
        {
            using (var mySqlConnection = new MySqlConnection())
            {
                mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                mySqlConnection.Open();

                const string query = "SELECT PLAN_ACTIVITE, PLAN_DATE_DEBUT, ACT_LIBELLE " +
                                     "FROM PLANNING B " +
                                     "INNER JOIN ACTIVITES A " +
                                     "ON B.PLAN_ACTIVITE = A.ACT_CODE " +
                                     "ORDER BY PLAN_DATE_DEBUT DESC LIMIT 10";

                using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                {
                    using (var mySqlDataReader = mySqlCommand.ExecuteReader())
                    {
                        return RpaExecution(flPlanning, frmConsultPlanning, mySqlDataReader);
                    }
                }
            }
        }

        private string RpaExecution(FlowLayoutPanel flPlanning, frmConsultPlanning frmConsultPlanning,
            MySqlDataReader mySqlDataReader)
        {
            if (!mySqlDataReader.HasRows) return "0";

            DataTablePrint = new DataTable();
            DataTablePrint.Columns.Add("Activité"    , typeof(string));
            DataTablePrint.Columns.Add("Date"        , typeof(DateTime));
            DataTablePrint.Columns.Add("Participants", typeof(int));

            while (mySqlDataReader.Read())
            {
                CodeActivite    = mySqlDataReader.GetInt16(0);
                DateActivite    = mySqlDataReader.GetDateTime(1);
                LibelleActivite = mySqlDataReader.GetString(2);

                UcPlanning = new UcPlanning
                {
                    LblDateM     = {Text = DateActivite.ToString("g", CultureInfo.CreateSpecificCulture("fr-FR")), Tag = DateActivite},
                    BtnSupprActM = {Tag = CodeActivite},
                };

                RecupIdParticipants();

                UcPlanning.LblLibelleActiviteM.Text = LibelleActivite;

                DataTablePrint.Rows.Add(LibelleActivite, DateActivite.ToString("dd/MM/yyyy HH:mm"), UcPlanning.ComboParticipantsM.Items.Count - 1);

                if (frmConsultPlanning.IsHandleCreated)
                {
                    flPlanning.BeginInvoke((MethodInvoker) delegate { flPlanning.Controls.Add(UcPlanning); });
                }
            }

            return "1";
        }

        private void RecupIdParticipants()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = "SELECT PART_PLAN_ACTIVITE, RESID_NOM, RESID_PRENOM " +
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

                        RipExecution(mySqlCommand);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Erreur : " + ex.Message, @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void RipExecution(MySqlCommand mySqlCommand)
        {
            using (var mySqlDataReader = mySqlCommand.ExecuteReader())
            {
                if (!mySqlDataReader.HasRows)
                {
                    UcPlanning.ComboParticipantsM.Items.Add("Aucun");
                    UcPlanning.ComboParticipantsM.SelectedIndex = 0;
                }
                else
                {
                    UcPlanning.ComboParticipantsM.Items.Add("Participant(s)");
                    UcPlanning.ComboParticipantsM.SelectedIndex = 0;

                    while (mySqlDataReader.Read())
                    {
                        NomParticipant    = mySqlDataReader.GetString(1);
                        PrenomParticipant = mySqlDataReader.GetString(2);
                        UcPlanning.ComboParticipantsM.Items.Add(NomParticipant + " " + PrenomParticipant);
                    }
                }
            }
        }

        public static string SupprActivite(int idActivite, DateTime dateActivite)
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = "DELETE FROM PLANNING WHERE PLAN_ACTIVITE = @PLAN_ACTIVITE AND PLAN_DATE_DEBUT = @PLAN_DATE_DEBUT";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter("@PLAN_ACTIVITE", MySqlDbType.Int16));
                        mySqlCommand.Parameters["@PLAN_ACTIVITE"].Value   = idActivite;
                        mySqlCommand.Parameters.Add(new MySqlParameter("@PLAN_DATE_DEBUT", MySqlDbType.DateTime));
                        mySqlCommand.Parameters["@PLAN_DATE_DEBUT"].Value = dateActivite;

                        var resultat = Convert.ToInt32(mySqlCommand.ExecuteNonQuery());

                        if (resultat > 0)
                        {
                            return "1";
                        }

                        return MessageBox.Show(@"Une erreur est survenue durant la suppression de l'activité.", @"Erreur", MessageBoxButtons.OK,
                            MessageBoxIcon.Information).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return MessageBox.Show($@"Exception levée : {ex.Message}").ToString();
            }
        }
    }
}
