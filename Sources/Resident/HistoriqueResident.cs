using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace Orpea_WF.Resident
{
    public class HistoriqueResident
    {
        public int IdResident          { private get; set; }
        public string Nom              { get; private set; }
        public string Prenom           { get; private set; }
        public string Autonomie        { get; private set; }
        public DateTime DateNaissance  { get; private set; }
        public string LieuNaissance    { get; private set; }
        public int NumChambre          { get; private set; }
        public string StatutSocial     { get; private set; }
        public string HabitatAnt       { get; private set; }
        public int NbrEnfants          { get; private set; }
        public string MetierAnt        { get; private set; }
        public string MsgException     { get; private set; }

        private DataTable dtEntreesResident { get; set; }
        private DataTable dtSortiesResident { get; set; }
        private DataTable dtCiResident      { get; set; }
        private DataTable dtPartResident    { get; set; }

        private static string SafeGetString(MySqlDataReader reader, int colIndex, string text)
        {
            return !reader.IsDBNull(colIndex) ? reader.GetString(colIndex) : text;
        }

        public string RecupInfosResident()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = @"SELECT RESID_NOM, RESID_PRENOM, AUTON_LIBELLE, RESID_NAISSANCE,
                                           RESID_LIEU_NAISSANCE, RESID_CHAMBRE, STATUT_LIBELLE, HABIT_LIBELLE, RESID_ENFANT, RESID_METIER
                                           FROM RESIDENTS RE
                                           LEFT JOIN AUTONOMIES A ON RE.RESID_AUTONOMIE = A.AUTON_ID
                                           LEFT JOIN STATUT S ON RE.RESID_STATUT        = S.STATUT_CODE
                                           LEFT JOIN HABITAT H ON RE.RESID_HABITAT      = H.HABIT_CODE
                                           WHERE RE.RESID_ID                            = @RESID_ID";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_ID", MySqlDbType.Int16));
                        mySqlCommand.Parameters["@RESID_ID"].Value = IdResident;

                        using (var mySqlDataReader = mySqlCommand.ExecuteReader())
                        {
                            return RpaExecution(mySqlDataReader);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MsgException = exception.Message;
                return "2";
            }
        }

        private string RpaExecution(MySqlDataReader mySqlDataReader)
        {
            if (!mySqlDataReader.HasRows) return "0";

            if (mySqlDataReader.Read())
            {
                Nom       = SafeGetString(mySqlDataReader, 0, "Non renseigné");
                Prenom    = SafeGetString(mySqlDataReader, 1, "Non renseigné");
                Autonomie = SafeGetString(mySqlDataReader, 2, "Non renseigné");

                var x = mySqlDataReader.GetOrdinal("RESID_NAISSANCE");

                if (!mySqlDataReader.IsDBNull(x))
                {
                    DateNaissance = mySqlDataReader.GetDateTime(x);
                }

                LieuNaissance = SafeGetString(mySqlDataReader, 4, "Non renseigné");
                NumChambre    = mySqlDataReader[5] as int? ??  0;
                StatutSocial  = SafeGetString(mySqlDataReader, 6, "Non renseigné");
                HabitatAnt    = SafeGetString(mySqlDataReader, 7, "Non renseigné");
                NbrEnfants    = mySqlDataReader[8] as int? ??  0;
                MetierAnt     = SafeGetString(mySqlDataReader, 9, "Non renseigné");
            }
            return "1";
        }

        public DataTable[] RecupEntreesResident()
        {
            dtEntreesResident = new DataTable();
            dtSortiesResident = new DataTable();

            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = @"server=mysql-orpeafauriel.alwaysdata.net;Port=3306; userid=180881; password=Rdc3ycww**; database=orpeafauriel_bdd";
                    mySqlConnection.Open();

                    const string query = @"SELECT ENTREE_DATE 
                                           FROM ENTREES e
                                           INNER JOIN RESIDENTS r ON e.ENTREE_RESID = r.RESID_ID
                                           WHERE e.ENTREE_RESID = @RESID_ID ORDER BY ENTREE_DATE DESC;
                                           
                                           SELECT SORTIE_DATE, SORTIE_CAUSE
                                           FROM SORTIES s
                                           INNER JOIN RESIDENTS r ON s.SORTIE_RESID = r.RESID_ID
                                           WHERE s.SORTIE_RESID = @RESID_ID ORDER BY SORTIE_DATE DESC;";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_ID", MySqlDbType.Int16));
                        mySqlCommand.Parameters["@RESID_ID"].Value = IdResident;

                        using (var sda = new MySqlDataAdapter())
                        {
                            RerExecution(sda, mySqlCommand);
                        }

                        return new[] {dtEntreesResident, dtSortiesResident};
                    }
                }
            }
            catch (Exception exception)
            {
                MsgException = exception.Message;
                return null;
            }
        }

        private void RerExecution(MySqlDataAdapter sda, MySqlCommand mySqlCommand)
        {
            sda.SelectCommand = mySqlCommand;

            using (var ds = new DataSet())
            {
                sda.Fill(ds);

                dtEntreesResident = ds.Tables[0];
                dtSortiesResident = ds.Tables[1];

                //dtEntreesResident.Columns["RESID_NOM"].ColumnName    = "Nom";
                //dtEntreesResident.Columns["RESID_PRENOM"].ColumnName = "Prénom";
                dtEntreesResident.Columns["ENTREE_DATE"].ColumnName = "Date";
                dtEntreesResident.AcceptChanges();

                //dtSortiesResident.Columns["RESID_NOM"].ColumnName    = "Nom";
                //dtSortiesResident.Columns["RESID_PRENOM"].ColumnName = "Prénom";
                dtSortiesResident.Columns["SORTIE_DATE"].ColumnName  = "Date";
                dtSortiesResident.Columns["SORTIE_CAUSE"].ColumnName = "Cause";
                dtSortiesResident.AcceptChanges();
            }
        }

        public DataTable[] RecupPartCIResident()
        {
            dtCiResident   = new DataTable();
            dtPartResident = new DataTable();

            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = @"server=mysql-orpeafauriel.alwaysdata.net;Port=3306; userid=180881; password=Rdc3ycww**; database=orpeafauriel_bdd";
                    mySqlConnection.Open();

                    const string query = @"SELECT CAT_LIBELLE, ACT_LIBELLE, COTA_LIBELLE
                                           FROM CENTRES_INTERET ci
                                           INNER JOIN RESIDENTS r ON ci.CI_RESID = r.RESID_ID
                                           INNER JOIN CATEGORIES c ON ci.CI_CATEG = c.CAT_CODE
                                           INNER JOIN ACTIVITES a ON ci.CI_ACT = a.ACT_CODE
                                           INNER JOIN COTATIONS co ON ci.CI_COTATION = co.COTA_ID
                                           WHERE ci.CI_RESID = @RESID_ID;

                                           SELECT ACT_LIBELLE, PART_PLAN_DATE_D
                                           FROM PARTICIPATIONS p
                                           INNER JOIN RESIDENTS r ON p.PART_RESID_ID = r.RESID_ID
                                           INNER JOIN ACTIVITES a ON p.PART_PLAN_ACTIVITE = a.ACT_CODE
                                           WHERE p.PART_RESID_ID = @RESID_ID;";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_ID", MySqlDbType.Int16));
                        mySqlCommand.Parameters["@RESID_ID"].Value = IdResident;

                        using (var sda = new MySqlDataAdapter())
                        {
                            RpciExecution(sda, mySqlCommand);
                        }

                        return new[] { dtCiResident, dtPartResident };
                    }
                }
            }
            catch (Exception exception)
            {
                MsgException = exception.Message;
                return null;
            }
        }

        private void RpciExecution(MySqlDataAdapter sda, MySqlCommand mySqlCommand)
        {
            sda.SelectCommand = mySqlCommand;

            using (var ds = new DataSet())
            {
                sda.Fill(ds);

                dtCiResident   = ds.Tables[0];
                dtPartResident = ds.Tables[1];

                //dtCiResident.Columns["RESID_NOM"].ColumnName    = "Nom";
                //dtCiResident.Columns["RESID_PRENOM"].ColumnName = "Prénom";
                dtCiResident.Columns["CAT_LIBELLE"].ColumnName  = "Catégorie";
                dtCiResident.Columns["ACT_LIBELLE"].ColumnName  = "Activité";
                dtCiResident.Columns["COTA_LIBELLE"].ColumnName = "Statut";
                dtCiResident.AcceptChanges();

                //dtPartResident.Columns["RESID_NOM"].ColumnName         = "Nom";
                //dtPartResident.Columns["RESID_PRENOM"].ColumnName      = "Prénom";
                dtPartResident.Columns["ACT_LIBELLE"].ColumnName      = "Activité";
                dtPartResident.Columns["PART_PLAN_DATE_D"].ColumnName = "Date";
                dtPartResident.AcceptChanges();
            }
        }
    }
}
