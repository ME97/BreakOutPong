namespace BreakOutPong
{
    partial class frmBreakout
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
            this.tmr_Animation = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmr_Animation
            // 
            this.tmr_Animation.Enabled = true;
            this.tmr_Animation.Interval = 33;
            this.tmr_Animation.Tick += new System.EventHandler(this.tmr_Animation_Tick);
            // 
            // frmBreakout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 561);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmBreakout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Breakout";
            this.Load += new System.EventHandler(this.frmBreakout_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmBreakout_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmBreakout_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmBreakout_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmr_Animation;
    }
}

