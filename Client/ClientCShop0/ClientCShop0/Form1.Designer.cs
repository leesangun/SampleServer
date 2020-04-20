namespace ClientCShop0
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.buttonLogin = new System.Windows.Forms.Button();
            this.buttonRoomAreaJoin = new System.Windows.Forms.Button();
            this.buttonRoomAreaMessage = new System.Windows.Forms.Button();
            this.labelLog = new System.Windows.Forms.Label();
            this.testUdp = new System.Windows.Forms.Button();
            this.labelLogUdp = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(588, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonLogin
            // 
            this.buttonLogin.Location = new System.Drawing.Point(386, 171);
            this.buttonLogin.Name = "buttonLogin";
            this.buttonLogin.Size = new System.Drawing.Size(75, 23);
            this.buttonLogin.TabIndex = 1;
            this.buttonLogin.Text = "login";
            this.buttonLogin.UseVisualStyleBackColor = true;
            this.buttonLogin.Click += new System.EventHandler(this.buttonLogin_Click);
            // 
            // buttonRoomAreaJoin
            // 
            this.buttonRoomAreaJoin.Location = new System.Drawing.Point(386, 211);
            this.buttonRoomAreaJoin.Name = "buttonRoomAreaJoin";
            this.buttonRoomAreaJoin.Size = new System.Drawing.Size(159, 23);
            this.buttonRoomAreaJoin.TabIndex = 2;
            this.buttonRoomAreaJoin.Text = "roomAreaJoin";
            this.buttonRoomAreaJoin.UseVisualStyleBackColor = true;
            this.buttonRoomAreaJoin.Click += new System.EventHandler(this.buttonRoomAreaJoin_Click);
            // 
            // buttonRoomAreaMessage
            // 
            this.buttonRoomAreaMessage.Location = new System.Drawing.Point(386, 254);
            this.buttonRoomAreaMessage.Name = "buttonRoomAreaMessage";
            this.buttonRoomAreaMessage.Size = new System.Drawing.Size(159, 23);
            this.buttonRoomAreaMessage.TabIndex = 3;
            this.buttonRoomAreaMessage.Text = "roomAreaMessage";
            this.buttonRoomAreaMessage.UseVisualStyleBackColor = true;
            this.buttonRoomAreaMessage.Click += new System.EventHandler(this.buttonRoomAreaMessage_Click);
            // 
            // labelLog
            // 
            this.labelLog.AutoSize = true;
            this.labelLog.Location = new System.Drawing.Point(111, 49);
            this.labelLog.Name = "labelLog";
            this.labelLog.Size = new System.Drawing.Size(62, 15);
            this.labelLog.TabIndex = 4;
            this.labelLog.Text = "labelLog";
            // 
            // testUdp
            // 
            this.testUdp.Location = new System.Drawing.Point(653, 171);
            this.testUdp.Name = "testUdp";
            this.testUdp.Size = new System.Drawing.Size(75, 23);
            this.testUdp.TabIndex = 5;
            this.testUdp.Text = "udp";
            this.testUdp.UseVisualStyleBackColor = true;
            this.testUdp.Click += new System.EventHandler(this.testUdp_Click);
            // 
            // labelLogUdp
            // 
            this.labelLogUdp.AutoSize = true;
            this.labelLogUdp.Location = new System.Drawing.Point(114, 129);
            this.labelLogUdp.Name = "labelLogUdp";
            this.labelLogUdp.Size = new System.Drawing.Size(88, 15);
            this.labelLogUdp.TabIndex = 6;
            this.labelLogUdp.Text = "labelLogUdp";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelLogUdp);
            this.Controls.Add(this.testUdp);
            this.Controls.Add(this.labelLog);
            this.Controls.Add(this.buttonRoomAreaMessage);
            this.Controls.Add(this.buttonRoomAreaJoin);
            this.Controls.Add(this.buttonLogin);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonLogin;
        private System.Windows.Forms.Button buttonRoomAreaJoin;
        private System.Windows.Forms.Button buttonRoomAreaMessage;
        private System.Windows.Forms.Label labelLog;
        private System.Windows.Forms.Button testUdp;
        private System.Windows.Forms.Label labelLogUdp;
    }
}

