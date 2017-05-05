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
            this.labelSpeed = new System.Windows.Forms.Label();
            this.labelSize = new System.Windows.Forms.Label();
            this.labelHealth = new System.Windows.Forms.Label();
            this.labelEnergy = new System.Windows.Forms.Label();
            this.labelPopulation = new System.Windows.Forms.Label();
            this.labelAge = new System.Windows.Forms.Label();
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
            this.backgroundWorkerCalculations.WorkerSupportsCancellation = true;
            this.backgroundWorkerCalculations.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerCalculations_DoWork);
            this.backgroundWorkerCalculations.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerCalculations_RunWorkerCompleted);
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(352, 9);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(55, 17);
            this.labelSpeed.TabIndex = 4;
            this.labelSpeed.Text = "Fastest: ";
            this.labelSpeed.Visible = false;
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Location = new System.Drawing.Point(207, 9);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(58, 17);
            this.labelSize.TabIndex = 5;
            this.labelSize.Text = "Largest: ";
            this.labelSize.Visible = false;
            // 
            // labelHealth
            // 
            this.labelHealth.AutoSize = true;
            this.labelHealth.Location = new System.Drawing.Point(494, 9);
            this.labelHealth.Name = "labelHealth";
            this.labelHealth.Size = new System.Drawing.Size(72, 17);
            this.labelHealth.TabIndex = 6;
            this.labelHealth.Text = "Healthiest: ";
            this.labelHealth.Visible = false;
            // 
            // labelEnergy
            // 
            this.labelEnergy.AutoSize = true;
            this.labelEnergy.Location = new System.Drawing.Point(653, 9);
            this.labelEnergy.Name = "labelEnergy";
            this.labelEnergy.Size = new System.Drawing.Size(60, 17);
            this.labelEnergy.TabIndex = 7;
            this.labelEnergy.Text = "Most fit: ";
            this.labelEnergy.Visible = false;
            // 
            // labelPopulation
            // 
            this.labelPopulation.AutoSize = true;
            this.labelPopulation.Location = new System.Drawing.Point(12, 60);
            this.labelPopulation.Name = "labelPopulation";
            this.labelPopulation.Size = new System.Drawing.Size(77, 17);
            this.labelPopulation.TabIndex = 8;
            this.labelPopulation.Text = "Population: ";
            // 
            // labelAge
            // 
            this.labelAge.AutoSize = true;
            this.labelAge.Location = new System.Drawing.Point(800, 9);
            this.labelAge.Name = "labelAge";
            this.labelAge.Size = new System.Drawing.Size(53, 17);
            this.labelAge.TabIndex = 9;
            this.labelAge.Text = "Oldest: ";
            this.labelAge.Visible = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1484, 961);
            this.Controls.Add(this.labelAge);
            this.Controls.Add(this.labelPopulation);
            this.Controls.Add(this.labelEnergy);
            this.Controls.Add(this.labelHealth);
            this.Controls.Add(this.labelSize);
            this.Controls.Add(this.labelSpeed);
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
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Main_KeyUp);
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
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.Label labelHealth;
        private System.Windows.Forms.Label labelEnergy;
        private System.Windows.Forms.Label labelPopulation;
        private System.Windows.Forms.Label labelAge;
    }
}

