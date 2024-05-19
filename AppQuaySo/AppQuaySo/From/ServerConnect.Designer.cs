using System.Data;

namespace AppQuaySo.From
{
    partial class ServerConnect
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.txtPassWord = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cbDataBaseName = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "ServerName";
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(142, 6);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(356, 23);
            this.txtServerName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(84, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "User:";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(142, 41);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(356, 23);
            this.txtUser.TabIndex = 3;
            // 
            // txtPassWord
            // 
            this.txtPassWord.Location = new System.Drawing.Point(142, 80);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(356, 23);
            this.txtPassWord.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(55, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "PassWord:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Database Name:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(142, 173);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(352, 31);
            this.button1.TabIndex = 7;
            this.button1.Text = "Conect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbDataBaseName
            // 
            this.cbDataBaseName.FormattingEnabled = true;
            this.cbDataBaseName.Location = new System.Drawing.Point(142, 123);
            this.cbDataBaseName.Name = "cbDataBaseName";
            this.cbDataBaseName.Size = new System.Drawing.Size(356, 23);
            this.cbDataBaseName.TabIndex = 8;
            this.cbDataBaseName.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.cbDataBaseName.Click += new System.EventHandler(this.cbClickCheck);
            // 
            // ServerConnect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 224);
            this.Controls.Add(this.cbDataBaseName);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtPassWord);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtServerName);
            this.Controls.Add(this.label1);
            this.Name = "ServerConnect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ServerConnect";
            this.ResumeLayout(false);
            this.PerformLayout();
            this.GetServer();

        }

        private void GetServer()
        {
            DataTable data = Core.DataContext.DataAccess.GetDataTableConnectStr();
            if (data != null && data.Rows.Count > 0)
            {
                txtServerName.Text = data.Rows[0]["ServerName"]?.ToString();
                txtPassWord.Text = data.Rows[0]["Password"]?.ToString();
                txtUser.Text = data.Rows[0]["User"]?.ToString();
            }
        }

        #endregion

        private Label label1;
        private TextBox txtServerName;
        private Label label2;
        private TextBox txtUser;
        private TextBox txtPassWord;
        private Label label3;
        private Label label4;
        private Button button1;
        private ComboBox cbDataBaseName;
    }
}