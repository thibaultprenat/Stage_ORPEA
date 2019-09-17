using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Orpea_WF.Activite.Planning
{
    public partial class UcPlanning : UserControl
    {
        public UcPlanning()
        {
            InitializeComponent();
        }

        public Label LblLibelleActiviteM   => lblLibelleActivite;
        public Label LblDateM              => lblDate;
        public ComboBox ComboParticipantsM => comboParticipants;
        public LinkLabel BtnSupprActM      => btnSupprAct;

        private BackgroundWorker _bgwSupprAct;

        private void btnSupprAct_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var msg = MessageBox.Show(
                $@"L'activité : {lblLibelleActivite.Text}, programmée le : {lblDate.Text}, est sur le point d'être définitivement supprimée. Souhaitez-vous poursuivre ?",
                @"Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (msg == DialogResult.Yes)
            {
                _bgwSupprAct = new BackgroundWorker();

                if (!_bgwSupprAct.IsBusy)
                {
                    btnSupprAct.Enabled = false;
                    btnSupprAct.Text    = @"Suppression...";

                    _bgwSupprAct.DoWork             += _bgwSupprAct_DoWork;
                    _bgwSupprAct.RunWorkerCompleted += _bgwSupprAct_RunWorkerCompleted;
                    _bgwSupprAct.RunWorkerAsync();
                }
            }
        }

        private void _bgwSupprAct_DoWork(object sender, DoWorkEventArgs e)
        {
            if (PlanningAct.SupprActivite(Convert.ToInt32(btnSupprAct.Tag), Convert.ToDateTime(lblDate.Tag)) == "1")
            {
                MessageBox.Show($@"L'activité : {lblLibelleActivite.Text}, programmée le : {lblDate.Text}, a correctement été supprimée.", @"Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (Application.OpenForms[1].Name == "frmConsultPlanning")
                {
                    Invoke((MethodInvoker) delegate
                    {
                        Application.OpenForms[1].Close();
                    });
                }
            }
        }

        private void _bgwSupprAct_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                MessageBox.Show(@"L'opération a été annulée par l'utilisateur.", @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (e.Error != null)
            {
                MessageBox.Show(@"Erreur : " + e.Error.Message, @"Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
