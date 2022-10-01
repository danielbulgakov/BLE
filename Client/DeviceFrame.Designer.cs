namespace Client
{
    partial class DeviceFrame
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.sinLabel = new System.Windows.Forms.Label();
            this.cosLabel = new System.Windows.Forms.Label();
            this.sintrackBar = new System.Windows.Forms.TrackBar();
            this.costrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sintrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea4.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chart1.Legends.Add(legend4);
            this.chart1.Location = new System.Drawing.Point(12, 12);
            this.chart1.Name = "chart1";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chart1.Series.Add(series4);
            this.chart1.Size = new System.Drawing.Size(516, 473);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // sinLabel
            // 
            this.sinLabel.AutoSize = true;
            this.sinLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sinLabel.Location = new System.Drawing.Point(558, 54);
            this.sinLabel.Name = "sinLabel";
            this.sinLabel.Size = new System.Drawing.Size(125, 23);
            this.sinLabel.TabIndex = 3;
            this.sinLabel.Text = "Sin Step = 1.0";
            // 
            // cosLabel
            // 
            this.cosLabel.AutoSize = true;
            this.cosLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cosLabel.Location = new System.Drawing.Point(558, 158);
            this.cosLabel.Name = "cosLabel";
            this.cosLabel.Size = new System.Drawing.Size(130, 23);
            this.cosLabel.TabIndex = 4;
            this.cosLabel.Text = "Cos Step = 1.0";
            // 
            // sintrackBar
            // 
            this.sintrackBar.LargeChange = 1;
            this.sintrackBar.Location = new System.Drawing.Point(562, 91);
            this.sintrackBar.Minimum = 1;
            this.sintrackBar.Name = "sintrackBar";
            this.sintrackBar.Size = new System.Drawing.Size(290, 45);
            this.sintrackBar.TabIndex = 5;
            this.sintrackBar.Value = 1;
            this.sintrackBar.ValueChanged += new System.EventHandler(this.sintrackBarChanged);
            // 
            // costrackBar
            // 
            this.costrackBar.LargeChange = 1;
            this.costrackBar.Location = new System.Drawing.Point(562, 202);
            this.costrackBar.Minimum = 1;
            this.costrackBar.Name = "costrackBar";
            this.costrackBar.Size = new System.Drawing.Size(290, 45);
            this.costrackBar.TabIndex = 6;
            this.costrackBar.Value = 1;
            this.costrackBar.ValueChanged += new System.EventHandler(this.costrackBarChanged);
            // 
            // DeviceFrame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 497);
            this.Controls.Add(this.costrackBar);
            this.Controls.Add(this.sintrackBar);
            this.Controls.Add(this.cosLabel);
            this.Controls.Add(this.sinLabel);
            this.Controls.Add(this.chart1);
            this.Name = "DeviceFrame";
            this.Text = "DeviceFrame";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.onClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.onClosed);
            this.Load += new System.EventHandler(this.DeviceFrame_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sintrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label sinLabel;
        private System.Windows.Forms.Label cosLabel;
        private System.Windows.Forms.TrackBar sintrackBar;
        private System.Windows.Forms.TrackBar costrackBar;
    }
}