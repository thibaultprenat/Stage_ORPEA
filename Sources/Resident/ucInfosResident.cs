using System;
using System.Windows.Forms;

namespace Orpea_WF.Resident
{
    public partial class ucInfosResident : UserControl
    {
        public ucInfosResident()
        {
            InitializeComponent();
        }

        public Label LblNomM           => lblNom;
        public Label LblPrenomM        => lblPrenom;
        public Label LblAutonomieM     => lblAutonomie;
        public Label LblLieuNaissanceM => lblLieuNaissance;
        public Label LblDateNaissance  => lblDateNaissance;
        public Label LblNumChambreM    => lblNumChambre;
        public Label LblStatutSocialM  => lblStatutSocial;
        public Label LblHabitatAntM    => lblHabitatAnt;
        public Label LblMetierAntM     => lblMetierAnt;
        public Label LblNbrEnfants     => lblNbrEnfants;

        public void ucInfosResident_Load(object sender, EventArgs e)
        {
            foreach (Control x in Controls)
            {
                if (x is Label label && label.Text == @"0")
                {
                    label.Text = @"Non renseigné";
                }
            }
        }
    }
}
