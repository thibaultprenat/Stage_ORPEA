using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Orpea_WF.Utilisateur;

namespace Orpea_WF.Resident
{
    /// <summary>
    ///  Cette classe enregistre toutes les infos du nouveau résident pour être appelé lors
    /// de l'exécution de la requête.
    /// </summary>

    public class Résident
    {
        public string NomResident             { get; }
        public string PrenomResident          { get; }
        public string AutonomieResident       { get; }
        public DateTime DateNaissanceResident { get; }
        public string LieuNaissanceResident   { get; }
        public int ChambreResident            { get; }
        public string StatutResident          { get; }
        public string HabitatResident         { get; }
        public int EnfantResident             { get; }
        public string MetierResident          { get; }
        public DateTime DateEntreeResident    { get; }

        // Constructeur de la classe résident
        public Résident(string nomResident, string prenomResident, string autonomieResident,
            DateTime dateNaissanceResident, string lieuNaissanceResident, int chambreResident, string statutResident, string habitatResident,
            int enfantResident, string metierResident, DateTime dateEntreeResident)
        {
            NomResident           = nomResident;
            PrenomResident        = prenomResident;
            AutonomieResident     = autonomieResident;
            DateNaissanceResident = dateNaissanceResident;
            LieuNaissanceResident = lieuNaissanceResident;
            ChambreResident       = chambreResident;
            StatutResident        = statutResident;
            HabitatResident       = habitatResident;
            EnfantResident        = enfantResident;
            MetierResident        = metierResident;
            DateEntreeResident    = dateEntreeResident;
        }
    }

    public class FicheResident
    {
        public static int IdResident                { get; private set; }

        /// <summary>
        ///  Cette liste récupère tous les centres d'intérêt
        /// </summary>
        public static List<string> ActivitesCochees { get; set; }
        public static DataTable DtCentresInterets   { get; set; }

        private readonly Résident _Résident;
        
        public FicheResident(Résident résident)
        {
            _Résident = résident;
        }
        public string CreationResident()
        {
            try
            {
                using (var cn = new MySqlConnection(Connexion.SqlConnexion))
                {
                    cn.Open();

                    const string query = @"INSERT INTO RESIDENTS(RESID_NOM, RESID_PRENOM, RESID_AUTONOMIE, RESID_NAISSANCE,
                                           RESID_LIEU_NAISSANCE, RESID_CHAMBRE, RESID_STATUT, RESID_HABITAT, RESID_ENFANT, RESID_METIER) 
                                           VALUES (@RESID_NOM, @RESID_PRENOM, @RESID_AUTONOMIE, @RESID_NAISSANCE, @RESID_LIEU_NAISSANCE,
                                           @RESID_CHAMBRE, @RESID_STATUT, @RESID_HABITAT, @RESID_ENFANT, @RESID_METIER);
                                           SELECT LAST_INSERT_ID();";

                    using (var cmd = new MySqlCommand(query, cn))
                    {
                        cmd.UpdatedRowSource = UpdateRowSource.None;

                        cmd.Parameters.Add(new MySqlParameter("@RESID_NOM"           , MySqlDbType.VarChar));
                        cmd.Parameters["@RESID_NOM"].Value            = _Résident.NomResident;
                        cmd.Parameters.Add(new MySqlParameter("@RESID_PRENOM"        , MySqlDbType.VarChar));
                        cmd.Parameters["@RESID_PRENOM"].Value         = _Résident.PrenomResident;
                        cmd.Parameters.Add(new MySqlParameter("@RESID_AUTONOMIE"     , MySqlDbType.VarChar));
                        cmd.Parameters["@RESID_AUTONOMIE"].Value      = _Résident.AutonomieResident;
                        cmd.Parameters.Add(new MySqlParameter("@RESID_NAISSANCE"     , MySqlDbType.Date));
                        cmd.Parameters["@RESID_NAISSANCE"].Value      = _Résident.DateNaissanceResident;
                        cmd.Parameters.Add(new MySqlParameter("@RESID_LIEU_NAISSANCE", MySqlDbType.VarChar));
                        cmd.Parameters["@RESID_LIEU_NAISSANCE"].Value = _Résident.LieuNaissanceResident;
                        cmd.Parameters.Add(new MySqlParameter("@RESID_CHAMBRE"       , MySqlDbType.Int16));
                        cmd.Parameters["@RESID_CHAMBRE"].Value        = _Résident.ChambreResident;
                        cmd.Parameters.Add(new MySqlParameter("@RESID_STATUT"        , MySqlDbType.VarChar));
                        cmd.Parameters["@RESID_STATUT"].Value         = _Résident.StatutResident;
                        cmd.Parameters.Add(new MySqlParameter("@RESID_HABITAT"       , MySqlDbType.VarChar));
                        cmd.Parameters["@RESID_HABITAT"].Value        = _Résident.HabitatResident;
                        cmd.Parameters.Add(new MySqlParameter("@RESID_METIER", MySqlDbType.VarChar));
                        cmd.Parameters["@RESID_METIER"].Value         = _Résident.MetierResident;
                        cmd.Parameters.Add(new MySqlParameter("@RESID_ENFANT"        , MySqlDbType.Int16));
                        cmd.Parameters["@RESID_ENFANT"].Value         = _Résident.EnfantResident;

                        var result = cmd.ExecuteNonQuery();

                        if (result <= 0) return "0";

                        foreach (DataRow dr in DtCentresInterets.Rows)
                        {
                            if ((int) dr["CI_RESID"] == 0)
                            {
                                dr["CI_RESID"] = cmd.LastInsertedId;
                                IdResident     = Convert.ToInt32(cmd.LastInsertedId);
                            }
                        }

                        return "1";
                    }
                }
            }
            catch (Exception ex)
            {
                return MessageBox.Show($@"Erreur : {ex.Message}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }

        public string AjoutCentresInteret()
        {
            try
            {
                using (var cn = new MySqlConnection(Connexion.SqlConnexion))
                {
                    cn.Open();
                
                    const string query = @"INSERT INTO CENTRES_INTERET(CI_RESID, CI_CATEG, CI_ACT, CI_COTATION) VALUES (@CI_RESID, @CI_CATEG, @CI_ACT, @CI_COTATION);";
                
                    using (var cmd = new MySqlCommand(query, cn))
                    {
                        cmd.UpdatedRowSource = UpdateRowSource.None;
                
                        cmd.Parameters.Add("@CI_RESID", MySqlDbType.Int16).SourceColumn      = "CI_RESID";
                        cmd.Parameters.Add("@CI_CATEG", MySqlDbType.VarChar).SourceColumn    = "CI_CATEG";
                        cmd.Parameters.Add("@CI_ACT", MySqlDbType.Int16).SourceColumn        = "CI_ACT";
                        cmd.Parameters.Add("@CI_COTATION", MySqlDbType.VarChar).SourceColumn = "CI_COTATION";
                
                        var da      = new MySqlDataAdapter { InsertCommand = cmd };
                        var records = da.Update(DtCentresInterets);

                        return records <= 0 ? "0" : "1";
                    }
                }
            }
            catch (Exception ex)
            {
                return MessageBox.Show($@"Erreur : {ex.Message}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }

        public string AjoutEntree()
        {
            try
            {
                using (var cn = new MySqlConnection(Connexion.SqlConnexion))
                {
                    cn.Open();

                    const string query = @"INSERT INTO ENTREES (ENTREE_RESID, ENTREE_DATE)
                                           SELECT RESID_ID, @ENTREE_RESID FROM RESIDENTS 
                                           WHERE RESID_NOM = @RESID_NOM AND RESID_PRENOM = @RESID_PRENOM;";

                    using (var cmd = new MySqlCommand(query, cn))
                    {
                        cmd.Parameters.Add(new MySqlParameter("@ENTREE_RESID", MySqlDbType.DateTime));
                        cmd.Parameters["@ENTREE_RESID"].Value = _Résident.DateEntreeResident.ToString("G", CultureInfo.CreateSpecificCulture("zh-CN"));
                        cmd.Parameters.Add(new MySqlParameter("@RESID_NOM", MySqlDbType.VarChar));
                        cmd.Parameters["@RESID_NOM"].Value    = _Résident.NomResident;
                        cmd.Parameters.Add(new MySqlParameter("@RESID_PRENOM", MySqlDbType.VarChar));
                        cmd.Parameters["@RESID_PRENOM"].Value = _Résident.PrenomResident;

                        var records = Convert.ToInt32(cmd.ExecuteNonQuery());
                        return records <= 0 ? "0" : "1";
                    }
                }
            }
            catch (Exception ex)
            {
                return MessageBox.Show($@"Erreur : {ex.Message}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }
    }
}
