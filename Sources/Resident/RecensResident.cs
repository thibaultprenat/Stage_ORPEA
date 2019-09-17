using System;
using System.Globalization;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Orpea_WF.Resident
{
    public class RésidentARecenser
    {
        /// <summary>
        ///  Fonction principale : La textBox est associée à un "AutoCompleteCustomSource". De ce fait, lorsque l'utilisateur sélectionne une résident,
        ///  son ID est stocké ici. 
        /// </summary>
        public string IdResident    { get; }

        /// <summary>
        ///  Fonction principale : Récupère la date de départ du résident indiqué par l'utilisateur dans la textBox. 
        /// </summary>
        public DateTime DateDepart  { get; }

        /// <summary>
        ///  Fonction principale : Récupère la date d'arrivée du résident indiqué par l'utilisateur dans la textBox. 
        /// </summary>
        public DateTime DateArrivee { get; }

        /// <summary>
        ///  Fonction principale : Récupère la cause du départ du résident indiqué par l'utilisateur dans le comboBox. 
        /// </summary>
        public string CauseDepart   { get; }

        public RésidentARecenser(string idResident, DateTime? dateDepart, DateTime? dateArrivee, string causeDepart)
        {
            IdResident  = idResident;
            if (dateDepart  != null) DateDepart  = (DateTime) dateDepart;
            if (dateArrivee != null) DateArrivee = (DateTime)dateArrivee;
            CauseDepart = causeDepart;
        }
    }

    /// <summary>
    ///  Cette classe regroupe deux principales méthodes divisées chacune en deux. La première permet de désaffecter un résident
    ///  de l'enseigne ORPEA avec une cause (Hospitalisation, décès, changement d'établissement). La seconde permet de réaffecter un résident.
    ///  La particularité de cette classe ainsi que de toutes les autres est que les requêtes sont divisées en deux : une partie requête + paramètres
    ///  et une autre qui gère l'exécution de la requête.
    ///  Un historique est gardé (Sortie = Désaffectation, Entrée = Réaffectation).
    /// </summary>

    public class Recensement
    {
        private RésidentARecenser _résidentADesaffecter { get; }

        public Recensement(RésidentARecenser résidentADesaffecter)
        {
            _résidentADesaffecter = résidentADesaffecter;
        }

        public string DesaffectResident()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = @"INSERT INTO DESAFFECTATIONS (DESAFECT_RESIDENT, DESAFECT_DATE, DESAFECT_CAUSE) 
                                           VALUES (@DESAFECT_RESIDENT, @DESAFECT_DATE, @DESAFECT_CAUSE);

                                           INSERT INTO SORTIES (SORTIE_RESID, SORTIE_DATE, SORTIE_CAUSE) 
                                           VALUES (@SORTIE_RESID, @SORTIE_DATE, @SORTIE_CAUSE);
                                           
                                           DELETE FROM PARTICIPATIONS WHERE PART_RESID_ID=@PART_RESID_ID";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        return DrExecution(mySqlCommand);
                    }
                }
            }
            catch (MySqlException ex)
            {
                return MessageBox.Show($@"Erreur : {ex.Message} {ex.ErrorCode}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }

        private string DrExecution(MySqlCommand mySqlCommand)
        {
            mySqlCommand.Parameters.Add(new MySqlParameter("@DESAFECT_RESIDENT", MySqlDbType.Int16));
            mySqlCommand.Parameters["@DESAFECT_RESIDENT"].Value = _résidentADesaffecter.IdResident;
            mySqlCommand.Parameters.Add(new MySqlParameter("@PART_RESID_ID", MySqlDbType.Int16));
            mySqlCommand.Parameters["@PART_RESID_ID"].Value     = _résidentADesaffecter.IdResident;
            mySqlCommand.Parameters.Add(new MySqlParameter("@DESAFECT_DATE"    , MySqlDbType.DateTime));
            mySqlCommand.Parameters["@DESAFECT_DATE"].Value     = _résidentADesaffecter.DateDepart.ToString("G", CultureInfo.CreateSpecificCulture("zh-CN"));
            mySqlCommand.Parameters.Add(new MySqlParameter("@DESAFECT_CAUSE"   , MySqlDbType.VarChar));
            mySqlCommand.Parameters["@DESAFECT_CAUSE"].Value    = _résidentADesaffecter.CauseDepart;
            mySqlCommand.Parameters.Add(new MySqlParameter("@SORTIE_RESID"     , MySqlDbType.Int16));
            mySqlCommand.Parameters["@SORTIE_RESID"].Value      = _résidentADesaffecter.IdResident;
            mySqlCommand.Parameters.Add(new MySqlParameter("@SORTIE_DATE"      , MySqlDbType.DateTime));
            mySqlCommand.Parameters["@SORTIE_DATE"].Value       = _résidentADesaffecter.DateDepart.ToString("G", CultureInfo.CreateSpecificCulture("zh-CN"));
            mySqlCommand.Parameters.Add(new MySqlParameter("@SORTIE_CAUSE"     , MySqlDbType.VarChar));
            mySqlCommand.Parameters["@SORTIE_CAUSE"].Value      = _résidentADesaffecter.CauseDepart;

            var result = mySqlCommand.ExecuteNonQuery();

            return result > 0 ? "1" : "0";
        }

        public string ReaffectResident()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = @"DELETE FROM DESAFFECTATIONS 
                                           WHERE DESAFECT_RESIDENT=@DESAFECT_RESIDENT;

                                           INSERT INTO ENTREES (ENTREE_RESID, ENTREE_DATE) 
                                           VALUES (@ENTREE_RESID, @ENTREE_DATE);";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        return RrExecution(mySqlCommand);
                    }
                }
            }
            catch (MySqlException ex)
            {
                return MessageBox.Show($@"Erreur : {ex.Message} {ex.ErrorCode}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }

        private string RrExecution(MySqlCommand mySqlCommand)
        {
            mySqlCommand.Parameters.Add(new MySqlParameter("@DESAFECT_RESIDENT", MySqlDbType.Int16));
            mySqlCommand.Parameters["@DESAFECT_RESIDENT"].Value = _résidentADesaffecter.IdResident;
            mySqlCommand.Parameters.Add(new MySqlParameter("@ENTREE_RESID"     , MySqlDbType.Int16));
            mySqlCommand.Parameters["@ENTREE_RESID"].Value      = _résidentADesaffecter.IdResident;
            mySqlCommand.Parameters.Add(new MySqlParameter("@ENTREE_DATE"      , MySqlDbType.DateTime));
            mySqlCommand.Parameters["@ENTREE_DATE"].Value       = _résidentADesaffecter.DateArrivee.ToString("G", CultureInfo.CreateSpecificCulture("zh-CN"));

            var result = mySqlCommand.ExecuteNonQuery();

            return result > 0 ? "1" : "0";
        }

        public string SupprimerResident()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = @"DELETE FROM CENTRES_INTERET WHERE CI_RESID = @RESIDENT;
                                           DELETE FROM PARTICIPATIONS WHERE PART_RESID_ID = @RESIDENT;
                                           DELETE FROM ENTREES WHERE ENTREE_RESID = @RESIDENT;
                                           DELETE FROM SORTIES WHERE SORTIE_RESID = @RESIDENT;
                                           DELETE FROM DESAFFECTATIONS WHERE DESAFECT_RESIDENT = @RESIDENT;
                                           DELETE FROM RESIDENTS WHERE RESID_ID = @RESIDENT;";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        return SrExecution(mySqlCommand);
                    }
                }
            }
            catch (MySqlException ex)
            {
                return MessageBox.Show($@"Erreur : {ex.Message} {ex.ErrorCode}", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation).ToString();
            }
        }

        private string SrExecution(MySqlCommand mySqlCommand)
        {
            mySqlCommand.Parameters.Add(new MySqlParameter("@RESIDENT", MySqlDbType.Int16));
            mySqlCommand.Parameters["@RESIDENT"].Value = _résidentADesaffecter.IdResident;

            var result = mySqlCommand.ExecuteNonQuery();

            return result > 0 ? "1" : "0";
        }
    }
}
