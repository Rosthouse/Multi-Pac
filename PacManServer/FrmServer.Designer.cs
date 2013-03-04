namespace PacManServer
{
    partial class FrmServer
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
            this.components = new System.ComponentModel.Container();
            this.txtLog = new System.Windows.Forms.RichTextBox();
            this.msgTicker = new System.Windows.Forms.Timer(this.components);
            this.gameTicker = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Location = new System.Drawing.Point(12, 34);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(469, 216);
            this.txtLog.TabIndex = 0;
            this.txtLog.Text = "";
            // 
            // msgTicker
            // 
            this.msgTicker.Enabled = true;
            this.msgTicker.Tick += new System.EventHandler(this.msgTicker_Tick);
            // 
            // gameTicker
            // 
            this.gameTicker.Enabled = true;
            this.gameTicker.Interval = 10;
            this.gameTicker.Tick += new System.EventHandler(this.gameTicker_Tick);
            // 
            // FrmServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 262);
            this.Controls.Add(this.txtLog);
            this.Name = "FrmServer";
            this.Text = "Server-Startwindow";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtLog;
        private System.Windows.Forms.Timer msgTicker;
        private System.Windows.Forms.Timer gameTicker;


    }
}

