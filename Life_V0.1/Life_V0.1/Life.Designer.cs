namespace Life_V0._1
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.RefTimer = new System.Windows.Forms.Timer(this.components);
            this.labelFPS = new System.Windows.Forms.Label();
            this.labelRender = new System.Windows.Forms.Label();
            this.labelClaculations = new System.Windows.Forms.Label();
            this.backgroundWorkerCalculations = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // RefTimer
            // 
            this.RefTimer.Interval = 1;
            this.RefTimer.Tick += new System.EventHandler(this.RefTimer_Tick);
            // 
            // labelFPS
            // 
            this.labelFPS.AutoSize = true;
            this.labelFPS.Location = new System.Drawing.Point(12, 9);
            this.labelFPS.Name = "labelFPS";
            this.labelFPS.Size = new System.Drawing.Size(31, 17);
            this.labelFPS.TabIndex = 1;
            this.labelFPS.Text = "FPS:";
            // 
            // labelRender
            // 
            this.labelRender.AutoSize = true;
            this.labelRender.Location = new System.Drawing.Point(12, 43);
            this.labelRender.Name = "labelRender";
            this.labelRender.Size = new System.Drawing.Size(19, 17);
            this.labelRender.TabIndex = 2;
            this.labelRender.Text = "R:";
            // 
            // labelClaculations
            // 
            this.labelClaculations.AutoSize = true;
            this.labelClaculations.Location = new System.Drawing.Point(12, 26);
            this.labelClaculations.Name = "labelClaculations";
            this.labelClaculations.Size = new System.Drawing.Size(23, 17);
            this.labelClaculations.TabIndex = 3;
            this.labelClaculations.Text = "C: ";
            // 
            // backgroundWorkerCalculations
            // 
            this.backgroundWorkerCalculations.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerCalculations_DoWork);
            this.backgroundWorkerCalculations.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerCalculations_RunWorkerCompleted);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1084, 611);
            this.Controls.Add(this.labelClaculations);
            this.Controls.Add(this.labelRender);
            this.Controls.Add(this.labelFPS);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Life";
            this.SizeChanged += new System.EventHandler(this.Main_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Main_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Main_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer RefTimer;
        private System.Windows.Forms.Label labelFPS;
        private System.Windows.Forms.Label labelRender;
        private System.Windows.Forms.Label labelClaculations;
        private System.ComponentModel.BackgroundWorker backgroundWorkerCalculations;
    }
}

