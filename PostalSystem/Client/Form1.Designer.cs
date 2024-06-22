namespace Client
{
    partial class Client
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
            listBox_listParcel = new ListBox();
            textBox_receiverLogin = new TextBox();
            textBox_description = new TextBox();
            comboBox_from = new ComboBox();
            comboBox_to = new ComboBox();
            button_createParcel = new Button();
            cbParcelTypes = new ComboBox();
            SuspendLayout();
            // 
            // listBox_listParcel
            // 
            listBox_listParcel.FormattingEnabled = true;
            listBox_listParcel.Location = new Point(12, 12);
            listBox_listParcel.Name = "listBox_listParcel";
            listBox_listParcel.Size = new Size(289, 424);
            listBox_listParcel.TabIndex = 0;
            listBox_listParcel.SelectedIndexChanged += listBox_listParcel_SelectedIndexChanged;
            // 
            // textBox_receiverLogin
            // 
            textBox_receiverLogin.Location = new Point(354, 12);
            textBox_receiverLogin.Multiline = true;
            textBox_receiverLogin.Name = "textBox_receiverLogin";
            textBox_receiverLogin.PlaceholderText = "Receiver's login";
            textBox_receiverLogin.Size = new Size(421, 65);
            textBox_receiverLogin.TabIndex = 1;
            // 
            // textBox_description
            // 
            textBox_description.Location = new Point(354, 194);
            textBox_description.Multiline = true;
            textBox_description.Name = "textBox_description";
            textBox_description.PlaceholderText = "Description";
            textBox_description.Size = new Size(421, 65);
            textBox_description.TabIndex = 3;
            // 
            // comboBox_from
            // 
            comboBox_from.FormattingEnabled = true;
            comboBox_from.Location = new Point(354, 286);
            comboBox_from.Name = "comboBox_from";
            comboBox_from.Size = new Size(208, 28);
            comboBox_from.TabIndex = 4;
            comboBox_from.Text = "From";
            // 
            // comboBox_to
            // 
            comboBox_to.FormattingEnabled = true;
            comboBox_to.Location = new Point(567, 286);
            comboBox_to.Name = "comboBox_to";
            comboBox_to.Size = new Size(208, 28);
            comboBox_to.TabIndex = 5;
            comboBox_to.Text = "To";
            // 
            // button_createParcel
            // 
            button_createParcel.Location = new Point(354, 385);
            button_createParcel.Name = "button_createParcel";
            button_createParcel.Size = new Size(421, 51);
            button_createParcel.TabIndex = 6;
            button_createParcel.Text = "Create parcel";
            button_createParcel.UseVisualStyleBackColor = true;
            button_createParcel.Click += button_createParcel_Click;
            // 
            // cbParcelTypes
            // 
            cbParcelTypes.FormattingEnabled = true;
            cbParcelTypes.Location = new Point(354, 101);
            cbParcelTypes.Name = "cbParcelTypes";
            cbParcelTypes.Size = new Size(421, 28);
            cbParcelTypes.TabIndex = 7;
            // 
            // Client
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cbParcelTypes);
            Controls.Add(button_createParcel);
            Controls.Add(comboBox_to);
            Controls.Add(comboBox_from);
            Controls.Add(textBox_description);
            Controls.Add(textBox_receiverLogin);
            Controls.Add(listBox_listParcel);
            Name = "Client";
            Text = "Client";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox_listParcel;
        private TextBox textBox_receiverLogin;
        private TextBox textBox_description;
        private ComboBox comboBox_from;
        private ComboBox comboBox_to;
        private Button button_createParcel;
        private ComboBox cbParcelTypes;
    }
}
