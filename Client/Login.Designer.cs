namespace Client
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            textBox_login = new TextBox();
            textBox_password = new TextBox();
            textBox_port = new TextBox();
            button_signIn = new Button();
            button_signUp = new Button();
            SuspendLayout();
            // 
            // textBox_login
            // 
            textBox_login.Location = new Point(79, 38);
            textBox_login.Multiline = true;
            textBox_login.Name = "textBox_login";
            textBox_login.PlaceholderText = "Login";
            textBox_login.Size = new Size(494, 42);
            textBox_login.TabIndex = 0;
            // 
            // textBox_password
            // 
            textBox_password.Location = new Point(79, 101);
            textBox_password.Multiline = true;
            textBox_password.Name = "textBox_password";
            textBox_password.PlaceholderText = "Password";
            textBox_password.Size = new Size(494, 42);
            textBox_password.TabIndex = 1;
            // 
            // textBox_port
            // 
            textBox_port.Location = new Point(79, 179);
            textBox_port.Multiline = true;
            textBox_port.Name = "textBox_port";
            textBox_port.PlaceholderText = "Local port";
            textBox_port.Size = new Size(494, 42);
            textBox_port.TabIndex = 2;
            // 
            // button_signIn
            // 
            button_signIn.Location = new Point(79, 287);
            button_signIn.Name = "button_signIn";
            button_signIn.Size = new Size(158, 59);
            button_signIn.TabIndex = 3;
            button_signIn.Text = "Sign in";
            button_signIn.UseVisualStyleBackColor = true;
            button_signIn.Click += button_signIn_Click;
            // 
            // button_signUp
            // 
            button_signUp.Location = new Point(415, 287);
            button_signUp.Name = "button_signUp";
            button_signUp.Size = new Size(158, 59);
            button_signUp.TabIndex = 4;
            button_signUp.Text = "Sign up";
            button_signUp.UseVisualStyleBackColor = true;
            button_signUp.Click += button_signUp_Click;
            // 
            // Login
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(648, 398);
            Controls.Add(button_signUp);
            Controls.Add(button_signIn);
            Controls.Add(textBox_port);
            Controls.Add(textBox_password);
            Controls.Add(textBox_login);
            Name = "Login";
            Text = "Login";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox_login;
        private TextBox textBox_password;
        private TextBox textBox_port;
        private Button button_signIn;
        private Button button_signUp;
    }
}