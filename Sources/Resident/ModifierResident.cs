using System;
using MySql.Data.MySqlClient;
using Orpea_WF.Utilisateur;

namespace Orpea_WF.Resident
{
    public class ModifierResident
    {
        public int IdResident          { get; set; }
        public string Nom              { get; set; }
        public string Prenom           { get; set; }
        public string Autonomie        { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string LieuNaissance    { get; set; }
        public int? NumChambre         { get; set; }
        public string StatutSocial     { get; set; }
        public string HabitatAnt       { get; set; }
        public int? NbrEnfants         { get; set; }
        public string MetierAnt        { get; set; }
        public string MsgException     { get; private set; }

        public string Informations()
        {
            try
            {
                using (var connection = new MySqlConnection(Connexion.SqlConnexion))
                {
                    connection.Open();

                    const string query = @"UPDATE RESIDENTS SET RESID_NOM=@RESID_NOM, RESID_PRENOM=@RESID_PRENOM, RESID_AUTONOMIE=@RESID_AUTONOMIE,
                                           RESID_NAISSANCE=@RESID_NAISSANCE, RESID_LIEU_NAISSANCE=@RESID_LIEU_NAISSANCE, RESID_CHAMBRE=@RESID_CHAMBRE,
                                           RESID_STATUT=@RESID_STATUT, RESID_HABITAT=@RESID_HABITAT, RESID_ENFANT=@RESID_ENFANT, RESID_METIER=@RESID_METIER
                                           WHERE RESID_ID=@RESID_ID;";

                    using (var mySqlCommand = new MySqlCommand(query, connection))
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_ID", MySqlDbType.Int16));
                        mySqlCommand.Parameters["@RESID_ID"].Value = IdResident;

                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_NOM", MySqlDbType.VarChar));
                        mySqlCommand.Parameters["@RESID_NOM"].Value = Nom;

                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_PRENOM", MySqlDbType.VarChar));
                        mySqlCommand.Parameters["@RESID_PRENOM"].Value = Prenom;

                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_AUTONOMIE", MySqlDbType.VarChar));
                        mySqlCommand.Parameters["@RESID_AUTONOMIE"].Value = !string.IsNullOrEmpty(Autonomie) ? (object)Autonomie : DBNull.Value;

                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_NAISSANCE", MySqlDbType.Date));
                        mySqlCommand.Parameters["@RESID_NAISSANCE"].Value = DateNaissance;

                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_LIEU_NAISSANCE", MySqlDbType.VarChar));
                        mySqlCommand.Parameters["@RESID_LIEU_NAISSANCE"].Value = !string.IsNullOrEmpty(LieuNaissance) ? (object)LieuNaissance : DBNull.Value;

                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_CHAMBRE", MySqlDbType.Int16));
                        mySqlCommand.Parameters["@RESID_CHAMBRE"].Value = NumChambre <= 0 ? null : NumChambre;

                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_STATUT", MySqlDbType.VarChar));
                        mySqlCommand.Parameters["@RESID_STATUT"].Value = !string.IsNullOrEmpty(StatutSocial) ? (object)StatutSocial : DBNull.Value;

                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_HABITAT", MySqlDbType.VarChar));
                        mySqlCommand.Parameters["@RESID_HABITAT"].Value = !string.IsNullOrEmpty(HabitatAnt) ? (object)HabitatAnt : DBNull.Value;

                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_ENFANT", MySqlDbType.Int16));
                        mySqlCommand.Parameters["@RESID_ENFANT"].Value = NbrEnfants <= 0 ? null : NbrEnfants;

                        mySqlCommand.Parameters.Add(new MySqlParameter("@RESID_METIER", MySqlDbType.VarChar));
                        mySqlCommand.Parameters["@RESID_METIER"].Value = !string.IsNullOrEmpty(MetierAnt) ? (object)MetierAnt : DBNull.Value;

                        var resultat = mySqlCommand.ExecuteNonQuery();

                        return resultat > 0 ? "1" : "0";
                    }
                }
            }
            catch (Exception exception)
            {
                MsgException = exception.Message;
                return "2";
            }
        }
    }
}