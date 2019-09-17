using System.Windows.Forms;

namespace Orpea_WF.Activite.Assignation
{
    public class InitialisationCombo
    {
        public static void Categories(ComboBox comboBox)
        {
            comboBox.Items.Add(new ObjectItemId("AA", "Activité artistique"));
            comboBox.Items.Add(new ObjectItemId("AC", "Activité culturelle"));
            comboBox.Items.Add(new ObjectItemId("AL", "Activité Ludique"));
            comboBox.Items.Add(new ObjectItemId("AM", "Activité manuelle"));
            comboBox.Items.Add(new ObjectItemId("ASO", "Activité sociale"));
            comboBox.Items.Add(new ObjectItemId("ASP", "Activité sportive"));
        }

        public static void Specifications(ComboBox comboBox)
        {
            comboBox.Items.Add(new ObjectItemId("++", "A déjà pratiqué"));
            comboBox.Items.Add(new ObjectItemId("+-", "N'a jamais pratiqué mais est intéressé"));
            comboBox.Items.Add(new ObjectItemId("--", "N'est pas du tout intéressé"));
        }

        public static void Autonomie(ComboBox comboBox)
        {
            comboBox.Items.Add(new ObjectItemId("C", "Chambre (incapacité)"));
            comboBox.Items.Add(new ObjectItemId("CD", "Canne/Déambulateur"));
            comboBox.Items.Add(new ObjectItemId("CS", "Chambre (choix)"));
            comboBox.Items.Add(new ObjectItemId("D", "Désorienté"));
            comboBox.Items.Add(new ObjectItemId("F", "Fauteuil"));
            comboBox.Items.Add(new ObjectItemId("HM", "Difficultés habileté manuelle"));
            comboBox.Items.Add(new ObjectItemId("MC", "Mal communicant"));
            comboBox.Items.Add(new ObjectItemId("ME", "Mal entendant"));
            comboBox.Items.Add(new ObjectItemId("MV", "Mal voyant"));
            comboBox.Items.Add(new ObjectItemId("TH", "Troubles de l'humeur"));
        }
        public static void Statut(ComboBox comboBox)
        {
            comboBox.Items.Add(new ObjectItemId("M", "Marié"));
            comboBox.Items.Add(new ObjectItemId("C", "Célibataire"));
            comboBox.Items.Add(new ObjectItemId("V", "Veuf"));
            comboBox.Items.Add(new ObjectItemId("E", "Enfant(s)"));
        }

        public static void Habitat(ComboBox comboBox)
        {
            comboBox.Items.Add(new ObjectItemId("V"  , "Ville"));
            comboBox.Items.Add(new ObjectItemId("VLG", "Village"));
            comboBox.Items.Add(new ObjectItemId("BDM", "Bord de mer"));
            comboBox.Items.Add(new ObjectItemId("C"  , "Campagne"));
            comboBox.Items.Add(new ObjectItemId("MTG", "Montagne"));
            comboBox.Items.Add(new ObjectItemId("M"  , "Maison"));
            comboBox.Items.Add(new ObjectItemId("A"  , "Appartement"));
        }
    }
}
