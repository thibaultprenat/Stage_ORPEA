using MySql.Data.MySqlClient;
using System;
using System.Collections;

namespace Orpea_WF.Activite
{
    public class ListeActivites
    {
        public ArrayList ListeActivitesAa  { get; private set; }
        public ArrayList ListeActivitesAm  { get; private set; }
        public ArrayList ListeActivitesAc  { get; private set; }
        public ArrayList ListeActivitesAl  { get; private set; }
        public ArrayList ListeActivitesAso { get; private set; }
        public ArrayList ListeActivitesAsp { get; private set; }

        public string RecupListeActivites()
        {
            try
            {
                using (var mySqlConnection = new MySqlConnection())
                {
                    mySqlConnection.ConnectionString = Utilisateur.Connexion.SqlConnexion;
                    mySqlConnection.Open();

                    const string query = "SELECT ACT_CODE_CATEG, ACT_CODE, ACT_LIBELLE FROM ACTIVITES";

                    ListeActivitesAa  = new ArrayList();
                    ListeActivitesAc  = new ArrayList();
                    ListeActivitesAl  = new ArrayList();
                    ListeActivitesAm  = new ArrayList();
                    ListeActivitesAso = new ArrayList();
                    ListeActivitesAsp = new ArrayList();

                    using (var mySqlCommand = new MySqlCommand(query, mySqlConnection))
                    {
                        using (var mySqlDataReader = mySqlCommand.ExecuteReader())
                        {
                            while (mySqlDataReader.Read())
                            {
                                switch (mySqlDataReader["ACT_CODE_CATEG"])
                                {
                                    case var _ when mySqlDataReader["ACT_CODE_CATEG"].Equals("AA"):
                                        ListeActivitesAa.Add(new ObjectItemId(mySqlDataReader.GetString(1),  mySqlDataReader.GetString(2)));
                                        break;
                                    case var _ when mySqlDataReader["ACT_CODE_CATEG"].Equals("AC"):
                                        ListeActivitesAc.Add(new ObjectItemId(mySqlDataReader.GetString(1),  mySqlDataReader.GetString(2)));
                                        break;
                                    case var _ when mySqlDataReader["ACT_CODE_CATEG"].Equals("AL"):
                                        ListeActivitesAl.Add(new ObjectItemId(mySqlDataReader.GetString(1),  mySqlDataReader.GetString(2)));
                                        break;
                                    case var _ when mySqlDataReader["ACT_CODE_CATEG"].Equals("AM"):
                                        ListeActivitesAm.Add(new ObjectItemId(mySqlDataReader.GetString(1),  mySqlDataReader.GetString(2)));
                                        break;
                                    case var _ when mySqlDataReader["ACT_CODE_CATEG"].Equals("ASO"):
                                        ListeActivitesAso.Add(new ObjectItemId(mySqlDataReader.GetString(1), mySqlDataReader.GetString(2)));
                                        break;
                                    case var _ when mySqlDataReader["ACT_CODE_CATEG"].Equals("ASP"):
                                        ListeActivitesAsp.Add(new ObjectItemId(mySqlDataReader.GetString(1), mySqlDataReader.GetString(2)));
                                        break;
                                }
                            }
                            return "1";
                        }
                    }
                }
            }
            catch (Exception)
            {
                return "0";
            }
        }
    }
}
