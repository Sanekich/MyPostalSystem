namespace Server
{
    partial class Server
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lbLog = new ListBox();
            bStart = new Button();
            bStop = new Button();
            nudX = new NumericUpDown();
            nudY = new NumericUpDown();
            bAddBuilding = new Button();
            tbParcelType = new TextBox();
            bAddParcelType = new Button();
            nudCostMultiplier = new NumericUpDown();
            bRemoveBuilding = new Button();
            bRemoveParcelType = new Button();
            cbSelect = new ComboBox();
            bSelect = new Button();
            ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudCostMultiplier).BeginInit();
            SuspendLayout();
            // 
            // lbLog
            // 
            lbLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lbLog.Enabled = false;
            lbLog.FormattingEnabled = true;
            lbLog.Location = new Point(292, 12);
            lbLog.Name = "lbLog";
            lbLog.Size = new Size(496, 424);
            lbLog.TabIndex = 0;
            // 
            // bStart
            // 
            bStart.Location = new Point(12, 12);
            bStart.Name = "bStart";
            bStart.Size = new Size(137, 82);
            bStart.TabIndex = 2;
            bStart.Text = "Start server";
            bStart.UseVisualStyleBackColor = true;
            bStart.Click += bStart_Click;
            // 
            // bStop
            // 
            bStop.Enabled = false;
            bStop.Location = new Point(149, 12);
            bStop.Name = "bStop";
            bStop.Size = new Size(137, 82);
            bStop.TabIndex = 3;
            bStop.Text = "Stop server";
            bStop.UseVisualStyleBackColor = true;
            bStop.Click += bStop_Click;
            // 
            // nudX
            // 
            nudX.Enabled = false;
            nudX.Location = new Point(12, 100);
            nudX.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudX.Name = "nudX";
            nudX.Size = new Size(137, 27);
            nudX.TabIndex = 4;
            // 
            // nudY
            // 
            nudY.Enabled = false;
            nudY.Location = new Point(149, 100);
            nudY.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
            nudY.Name = "nudY";
            nudY.Size = new Size(137, 27);
            nudY.TabIndex = 5;
            // 
            // bAddBuilding
            // 
            bAddBuilding.Enabled = false;
            bAddBuilding.Location = new Point(12, 133);
            bAddBuilding.Name = "bAddBuilding";
            bAddBuilding.Size = new Size(137, 82);
            bAddBuilding.TabIndex = 6;
            bAddBuilding.Text = "Add building";
            bAddBuilding.UseVisualStyleBackColor = true;
            bAddBuilding.Click += bAddBuilding_Click;
            // 
            // tbParcelType
            // 
            tbParcelType.Enabled = false;
            tbParcelType.Location = new Point(12, 221);
            tbParcelType.Name = "tbParcelType";
            tbParcelType.PlaceholderText = "Parcel type";
            tbParcelType.Size = new Size(207, 27);
            tbParcelType.TabIndex = 7;
            // 
            // bAddParcelType
            // 
            bAddParcelType.Enabled = false;
            bAddParcelType.Location = new Point(12, 254);
            bAddParcelType.Name = "bAddParcelType";
            bAddParcelType.Size = new Size(137, 82);
            bAddParcelType.TabIndex = 8;
            bAddParcelType.Text = "Add parcel type";
            bAddParcelType.UseVisualStyleBackColor = true;
            bAddParcelType.Click += bAddParcelType_Click;
            // 
            // nudCostMultiplier
            // 
            nudCostMultiplier.Enabled = false;
            nudCostMultiplier.Location = new Point(225, 222);
            nudCostMultiplier.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            nudCostMultiplier.Name = "nudCostMultiplier";
            nudCostMultiplier.Size = new Size(61, 27);
            nudCostMultiplier.TabIndex = 9;
            nudCostMultiplier.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // bRemoveBuilding
            // 
            bRemoveBuilding.Enabled = false;
            bRemoveBuilding.Location = new Point(149, 133);
            bRemoveBuilding.Name = "bRemoveBuilding";
            bRemoveBuilding.Size = new Size(137, 82);
            bRemoveBuilding.TabIndex = 10;
            bRemoveBuilding.Text = "Remove building";
            bRemoveBuilding.UseVisualStyleBackColor = true;
            bRemoveBuilding.Click += bRemoveBuilding_Click;
            // 
            // bRemoveParcelType
            // 
            bRemoveParcelType.Enabled = false;
            bRemoveParcelType.Location = new Point(149, 254);
            bRemoveParcelType.Name = "bRemoveParcelType";
            bRemoveParcelType.Size = new Size(137, 82);
            bRemoveParcelType.TabIndex = 11;
            bRemoveParcelType.Text = "Remove parcel type";
            bRemoveParcelType.UseVisualStyleBackColor = true;
            bRemoveParcelType.Click += bRemoveParcelType_Click;
            // 
            // cbSelect
            // 
            cbSelect.Enabled = false;
            cbSelect.FormattingEnabled = true;
            cbSelect.Location = new Point(12, 342);
            cbSelect.Name = "cbSelect";
            cbSelect.Size = new Size(274, 28);
            cbSelect.TabIndex = 12;
            cbSelect.Text = "Select table";
            // 
            // bSelect
            // 
            bSelect.Enabled = false;
            bSelect.Location = new Point(12, 376);
            bSelect.Name = "bSelect";
            bSelect.Size = new Size(274, 60);
            bSelect.TabIndex = 13;
            bSelect.Text = "Select";
            bSelect.UseVisualStyleBackColor = true;
            bSelect.Click += bSelect_Click;
            // 
            // Server
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(bSelect);
            Controls.Add(cbSelect);
            Controls.Add(bRemoveParcelType);
            Controls.Add(bRemoveBuilding);
            Controls.Add(nudCostMultiplier);
            Controls.Add(bAddParcelType);
            Controls.Add(tbParcelType);
            Controls.Add(bAddBuilding);
            Controls.Add(nudY);
            Controls.Add(nudX);
            Controls.Add(bStop);
            Controls.Add(bStart);
            Controls.Add(lbLog);
            Name = "Server";
            Text = "Server";
            ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudCostMultiplier).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lbLog;
        private Button bStart;
        private Button bStop;
        private NumericUpDown nudX;
        private NumericUpDown nudY;
        private Button bAddBuilding;
        private TextBox tbParcelType;
        private Button bAddParcelType;
        private NumericUpDown nudCostMultiplier;
        private Button bRemoveBuilding;
        private Button bRemoveParcelType;
        private ComboBox cbSelect;
        private Button bSelect;
    }
}
