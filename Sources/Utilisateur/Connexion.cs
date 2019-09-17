using MySql.Data.MySqlClient;
using System;
using Orpea_WF.Cryptographie;

namespace Orpea_WF.Utilisateur
{

    /// <summary>
    ///  Cette classe gère l'accès (connexion) de l'utilisateur à l'application. La particularité de cette classe ainsi que de toutes les autres
    ///  est que la requête est divisée en deux : une partie requête + paramètres et une autre qui gère l'exécution de la requête
    /// </summary>

    public class Connexion
    {
        /// <summary>
        ///  Fonction principale : Stockage de l'Id utilisateur entré dans la textBox.
        ///  Fonction secondaire : Permet l'affichage de l'utilisateur connecté à l'application.
        /// </summary>
        public string   IdUtilisateur    { get; set; }

        /// <summary>
        ///  Fonction principale : Stockage du mot de passe utilisateur entré dans la textBox.
        /// </summary>
        public string   MdpUtilisateur { private get; set; }
        public string   MsgException   { get; private set; }

        private string _mdpUtilisateurCrypte;
        private bool   _compteExistant;

        /// <summary>
        ///  Fonction principale : Chaîne de connexion à la base de données réutilisée pour toutes les autres requêtes.
        /// </summary>
        public const string SqlConnexion = "server=***;Port=3306; userid=***; password=***; database=***";

        /// <summary>
        ///  Fonction principale : Cette méthode prépare la requête, tente de se connecter à la BDD, crée les paramètres puis exécute le return.
        /// </summary>
        /// <returns>Exécute la méthode "AuthentificationExection(MySqlCommand mySqlCommand)".</returns>
        /// <exception cref="Exception">L'exception peut être déclenchée si la requête plante, si la connexion à la BDD est impossible etc...</exception>
        public string Authentification()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = SqlConnexion;
                    mySqlConnection.Open();

                    const string query = "SELECT * FROM UTILISATEURS WHERE USER_ID=@U_ID";

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        mySqlCommand.Parameters.Add(new MySqlParameter("@U_ID", MySqlDbType.VarChar));
                        mySqlCommand.Parameters["@U_ID"].Value = IdUtilisateur;

                        return AuthentificationExection(mySqlCommand);
                    }
                }
            }
            catch (Exception exception)
            {
                MsgException = exception.Message;
                return "2";
            }
        }

        /// <summary>
        ///  Fonction principale : Cette méthode exécute la requête en fonction de ses paramètres puis retourne son résultat.
        ///  A savoir que les mots de passe sont chiffrés de base puis déchiffrés ci-dessous !
        /// </summary>
        /// <param name="mySqlCommand">Paramètres (Nom d'utilisateur, mot de passe) à exécuter pour la requête.</param>
        /// <returns>Retourne le résultat de la requête. Si le résultat est = 1 (Déchiffrage réussi) alors stockage de l'Id utilisateur puis autorisation de la connexion.</returns>
        private string AuthentificationExection(MySqlCommand mySqlCommand)
        {
            using (var mySqlDataReader = mySqlCommand.ExecuteReader())
            {
                if (mySqlDataReader.Read())
                {
                    // Récupération du mot de passe chiffré.
                    _mdpUtilisateurCrypte = mySqlDataReader.GetString(1);
                    _compteExistant       = true;
                }

                // Si le compte existe alors on procède à la tentative de déchiffrement du mot de passe
                // S'il échoue on retourne 0 le mot de passe est incorrect, sinon on retourne 1
                if (_compteExistant)
                {
                    return AesDecryption.Decrypt(_mdpUtilisateurCrypte).Equals(MdpUtilisateur) ? "1" : "0";
                }

                // Le compte n'existe pas.
                return "0";
            }
        }
    }
}
