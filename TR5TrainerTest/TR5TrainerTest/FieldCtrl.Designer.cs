namespace TR5TrainerTest
{
    partial class FieldCtrl
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
            this.txtVal = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.chkLock = new System.Windows.Forms.CheckBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtVal
            // 
            this.txtVal.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtVal.Location = new System.Drawing.Point(161, 4);
            this.txtVal.Name = "txtVal";
            this.txtVal.Size = new System.Drawing.Size(94, 23);
            this.txtVal.TabIndex = 1;
            this.txtVal.TextChanged += new System.EventHandler(this.txtVal_TextChanged);
            // 
            // lblText
            // 
            this.lblText.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(3, 8);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(38, 15);
            this.lblText.TabIndex = 2;
            this.lblText.Text = "label1";
            // 
            // chkLock
            // 
            this.chkLock.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkLock.AutoSize = true;
            this.chkLock.Location = new System.Drawing.Point(140, 9);
            this.chkLock.Name = "chkLock";
            this.chkLock.Size = new System.Drawing.Size(15, 14);
            this.chkLock.TabIndex = 3;
            this.chkLock.UseVisualStyleBackColor = true;
            this.chkLock.CheckedChanged += new System.EventHandler(this.chkLock_CheckedChanged);
            // 
            // btnSet
            // 
            this.btnSet.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSet.Image = global::TR5TrainerTest.Properties.Resources.accept_button;
            this.btnSet.Location = new System.Drawing.Point(261, 3);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(25, 25);
            this.btnSet.TabIndex = 0;
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // FieldCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkLock);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.txtVal);
            this.Controls.Add(this.btnSet);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FieldCtrl";
            this.Size = new System.Drawing.Size(289, 31);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.TextBox txtVal;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.CheckBox chkLock;
    }
}
