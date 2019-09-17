using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Orpea_WF.Activite.Planning
{
    partial class UcPlanning
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UcPlanning));
            this.lblFactActivite = new System.Windows.Forms.Label();
            this.lblFactDate = new System.Windows.Forms.Label();
            this.lblLibelleActivite = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboParticipants = new System.Windows.Forms.ComboBox();
            this.btnSupprAct = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // lblFactActivite
            // 
            this.lblFactActivite.AutoSize = true;
            this.lblFactActivite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.lblFactActivite.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFactActivite.ForeColor = System.Drawing.Color.Gray;
            this.lblFactActivite.Location = new System.Drawing.Point(12, 46);
            this.lblFactActivite.Name = "lblFactActivite";
            this.lblFactActivite.Size = new System.Drawing.Size(56, 17);
            this.lblFactActivite.TabIndex = 0;
            this.lblFactActivite.Text = "Activité :";
            // 
            // lblFactDate
            // 
            this.lblFactDate.AutoSize = true;
            this.lblFactDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.lblFactDate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFactDate.ForeColor = System.Drawing.Color.Gray;
            this.lblFactDate.Location = new System.Drawing.Point(222, 46);
            this.lblFactDate.Name = "lblFactDate";
            this.lblFactDate.Size = new System.Drawing.Size(42, 17);
            this.lblFactDate.TabIndex = 1;
            this.lblFactDate.Text = "Date :";
            // 
            // lblLibelleActivite
            // 
            this.lblLibelleActivite.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.lblLibelleActivite.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblLibelleActivite.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblLibelleActivite.Location = new System.Drawing.Point(68, 45);
            this.lblLibelleActivite.Name = "lblLibelleActivite";
            this.lblLibelleActivite.Size = new System.Drawing.Size(145, 22);
            this.lblLibelleActivite.TabIndex = 2;
            this.lblLibelleActivite.Text = "Lecture journaux";
            // 
            // lblDate
            // 
            this.lblDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.lblDate.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblDate.Location = new System.Drawing.Point(264, 45);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(127, 22);
            this.lblDate.TabIndex = 3;
            this.lblDate.Text = "11/05/2019 15:35";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(397, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Participant(s) :";
            // 
            // comboParticipants
            // 
            this.comboParticipants.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.comboParticipants.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboParticipants.FormattingEnabled = true;
            this.comboParticipants.Location = new System.Drawing.Point(495, 43);
            this.comboParticipants.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboParticipants.Name = "comboParticipants";
            this.comboParticipants.Size = new System.Drawing.Size(161, 25);
            this.comboParticipants.TabIndex = 4;
            // 
            // btnSupprAct
            // 
            this.btnSupprAct.ActiveLinkColor = System.Drawing.Color.Black;
            this.btnSupprAct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.btnSupprAct.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.btnSupprAct.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.btnSupprAct.LinkColor = System.Drawing.Color.Red;
            this.btnSupprAct.Location = new System.Drawing.Point(575, 81);
            this.btnSupprAct.Name = "btnSupprAct";
            this.btnSupprAct.Size = new System.Drawing.Size(81, 15);
            this.btnSupprAct.TabIndex = 7;
            this.btnSupprAct.TabStop = true;
            this.btnSupprAct.Text = "Supprimer";
            this.btnSupprAct.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSupprAct.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.btnSupprAct_LinkClicked);
            // 
            // UcPlanning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.btnSupprAct);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboParticipants);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblFactDate);
            this.Controls.Add(this.lblFactActivite);
            this.Controls.Add(this.lblLibelleActivite);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UcPlanning";
            this.Size = new System.Drawing.Size(668, 111);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFactActivite;
        private System.Windows.Forms.Label lblFactDate;
        private System.Windows.Forms.Label lblLibelleActivite;
        private System.Windows.Forms.Label lblDate;
        private Label label1;
        private ComboBox comboParticipants;
        private LinkLabel btnSupprAct;
    }
}
