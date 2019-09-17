using MySql.Data.MySqlClient;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Orpea_WF.Activite.Planification
{
    /// <summary>
    ///  Stockage des infos de l'activité
    /// </summary>

    public class Activité
    {
        public int IdActivite             { get; }
        public DateTime DateDebutActivite { get; }

        // Constructeur de la classe Activité
        public Activité(int idActivite, DateTime dateDebutActivite)
        {
            IdActivite        = idActivite;
            DateDebutActivite = dateDebutActivite;
        }
    }

    public class Planification
    {
        /// <summary>
        ///  Accès aux propriétés de la classe Activité
        /// </summary>
        private Activité _Activité { get; }

        // Constructeur
        public Planification(Activité activité)
        {
            _Activité = activité;
        }

        /// <summary>
        /// Exécution de la requête.
        /// Cette requête paramétrée ajoute une activité au planning. 
        /// </summary>
        public string AjoutActivitePlanning()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = "INSERT INTO PLANNING (PLAN_ACTIVITE, PLAN_DATE_DEBUT) " +
                                         "VALUES (@PLAN_ACTIVITE, @PLAN_DATE_DEBUT)";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter("@PLAN_ACTIVITE",   MySqlDbType.Int16));
                        mySqlCommand.Parameters["@PLAN_ACTIVITE"].Value   = Convert.ToInt16(_Activité.IdActivite);
                        mySqlCommand.Parameters.Add(new MySqlParameter("@PLAN_DATE_DEBUT", MySqlDbType.DateTime));
                        mySqlCommand.Parameters["@PLAN_DATE_DEBUT"].Value = _Activité.DateDebutActivite.ToString("G", CultureInfo.CreateSpecificCulture("zh-CN"));

                        var result = mySqlCommand.ExecuteNonQuery();

                        return result > 0 ? "1" : "0";
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

