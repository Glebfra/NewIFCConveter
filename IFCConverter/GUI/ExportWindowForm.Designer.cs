using System.ComponentModel;

namespace IFCConverter.GUI
{
    internal partial class ExportWindowForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportWindowForm));
            this.exportButton = new System.Windows.Forms.Button();
            this.outputFilePathPanel = new System.Windows.Forms.Panel();
            this.outputFilePathTextbox = new System.Windows.Forms.TextBox();
            this.selectOutputFilePathButton = new System.Windows.Forms.Button();
            this.outputFilePathLabel = new System.Windows.Forms.Label();
            this.exportTypePanel = new System.Windows.Forms.Panel();
            this.exportTypeCombobox = new System.Windows.Forms.ComboBox();
            this.exportTypeLabel = new System.Windows.Forms.Label();
            this.vertexSegmentsPanel = new System.Windows.Forms.Panel();
            this.vertexSegmentsTextbox = new System.Windows.Forms.TextBox();
            this.vertexSegmentsLabel = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.outputFilePathPanel.SuspendLayout();
            this.exportTypePanel.SuspendLayout();
            this.vertexSegmentsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // exportButton
            // 
            resources.ApplyResources(this.exportButton, "exportButton");
            this.exportButton.Name = "exportButton";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // outputFilePathPanel
            // 
            this.outputFilePathPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.outputFilePathPanel.Controls.Add(this.outputFilePathTextbox);
            this.outputFilePathPanel.Controls.Add(this.selectOutputFilePathButton);
            resources.ApplyResources(this.outputFilePathPanel, "outputFilePathPanel");
            this.outputFilePathPanel.Name = "outputFilePathPanel";
            // 
            // outputFilePathTextbox
            // 
            resources.ApplyResources(this.outputFilePathTextbox, "outputFilePathTextbox");
            this.outputFilePathTextbox.Name = "outputFilePathTextbox";
            // 
            // selectOutputFilePathButton
            // 
            resources.ApplyResources(this.selectOutputFilePathButton, "selectOutputFilePathButton");
            this.selectOutputFilePathButton.Name = "selectOutputFilePathButton";
            this.selectOutputFilePathButton.UseVisualStyleBackColor = true;
            this.selectOutputFilePathButton.Click += new System.EventHandler(this.selectOutputFilePathButton_Click);
            // 
            // outputFilePathLabel
            // 
            resources.ApplyResources(this.outputFilePathLabel, "outputFilePathLabel");
            this.outputFilePathLabel.Name = "outputFilePathLabel";
            // 
            // exportTypePanel
            // 
            this.exportTypePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exportTypePanel.Controls.Add(this.exportTypeCombobox);
            resources.ApplyResources(this.exportTypePanel, "exportTypePanel");
            this.exportTypePanel.Name = "exportTypePanel";
            // 
            // exportTypeCombobox
            // 
            resources.ApplyResources(this.exportTypeCombobox, "exportTypeCombobox");
            this.exportTypeCombobox.Name = "exportTypeCombobox";
            // 
            // exportTypeLabel
            // 
            resources.ApplyResources(this.exportTypeLabel, "exportTypeLabel");
            this.exportTypeLabel.Name = "exportTypeLabel";
            // 
            // vertexSegmentsPanel
            // 
            this.vertexSegmentsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.vertexSegmentsPanel.Controls.Add(this.vertexSegmentsTextbox);
            resources.ApplyResources(this.vertexSegmentsPanel, "vertexSegmentsPanel");
            this.vertexSegmentsPanel.Name = "vertexSegmentsPanel";
            // 
            // vertexSegmentsTextbox
            // 
            resources.ApplyResources(this.vertexSegmentsTextbox, "vertexSegmentsTextbox");
            this.vertexSegmentsTextbox.Name = "vertexSegmentsTextbox";
            this.vertexSegmentsTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.vertexSegmentsTextbox_KeyPress);
            // 
            // vertexSegmentsLabel
            // 
            resources.ApplyResources(this.vertexSegmentsLabel, "vertexSegmentsLabel");
            this.vertexSegmentsLabel.Name = "vertexSegmentsLabel";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            resources.ApplyResources(this.comboBox1, "comboBox1");
            this.comboBox1.Name = "comboBox1";
            // 
            // ExportWindowForm
            // 
            this.AcceptButton = this.exportButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.vertexSegmentsLabel);
            this.Controls.Add(this.vertexSegmentsPanel);
            this.Controls.Add(this.exportTypeLabel);
            this.Controls.Add(this.exportTypePanel);
            this.Controls.Add(this.outputFilePathLabel);
            this.Controls.Add(this.outputFilePathPanel);
            this.Controls.Add(this.exportButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportWindowForm";
            this.outputFilePathPanel.ResumeLayout(false);
            this.outputFilePathPanel.PerformLayout();
            this.exportTypePanel.ResumeLayout(false);
            this.vertexSegmentsPanel.ResumeLayout(false);
            this.vertexSegmentsPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.ComboBox comboBox1;

        private System.Windows.Forms.Label vertexSegmentsLabel;

        private System.Windows.Forms.Panel vertexSegmentsPanel;

        private System.Windows.Forms.TextBox vertexSegmentsTextbox;

        private System.Windows.Forms.ComboBox exportTypeCombobox;

        private System.Windows.Forms.Label exportTypeLabel;

        private System.Windows.Forms.Panel exportTypePanel;

        private System.Windows.Forms.Label outputFilePathLabel;

        private System.Windows.Forms.TextBox outputFilePathTextbox;

        private System.Windows.Forms.Button selectOutputFilePathButton;

        private System.Windows.Forms.Panel outputFilePathPanel;

        private System.Windows.Forms.Button exportButton;

        #endregion
    }
}