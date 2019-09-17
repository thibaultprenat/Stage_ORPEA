using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;

namespace Orpea_WF.Activite.Creation
{
    public class Activité
    {
        public string CodeActivite { get; }
        public string Libelle      { get; }

        public Activité(string codeActivite, string libelle)
        {
            CodeActivite = codeActivite;
            Libelle      = libelle;
        }
    }
    public class AjoutActiviteCatalogue
    {
        private Activité _Activité { get; }

        public AjoutActiviteCatalogue(Activité activité)
        {
            _Activité = activité;
        }

        public string AjoutActivite()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = "INSERT INTO ACTIVITES (ACT_CODE_CATEG, ACT_LIBELLE) " +
                                         "VALUES (@ACT_CODE_CATEG, @ACT_LIBELLE)";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter("@ACT_CODE_CATEG", MySqlDbType.VarChar));
                        mySqlCommand.Parameters["@ACT_CODE_CATEG"].Value = _Activité.CodeActivite;
                        mySqlCommand.Parameters.Add(new MySqlParameter("@ACT_LIBELLE",    MySqlDbType.VarChar));
                        mySqlCommand.Parameters["@ACT_LIBELLE"].Value    = _Activité.Libelle;

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
