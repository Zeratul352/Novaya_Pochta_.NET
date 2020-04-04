namespace Novaya_Pochta_.NET
{
    partial class Interface
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
            this.gmap = new GMap.NET.WindowsForms.GMapControl();
            this.NextRoute = new System.Windows.Forms.Button();
            this.NextCarRoute = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // gmap
            // 
            this.gmap.AutoSize = true;
            this.gmap.Bearing = 0F;
            this.gmap.CanDragMap = true;
            this.gmap.EmptyTileColor = System.Drawing.Color.Navy;
            this.gmap.GrayScaleMode = false;
            this.gmap.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gmap.LevelsKeepInMemory = 5;
            this.gmap.Location = new System.Drawing.Point(12, 12);
            this.gmap.MarkersEnabled = true;
            this.gmap.MaxZoom = 2;
            this.gmap.MinZoom = 2;
            this.gmap.MouseWheelZoomEnabled = true;
            this.gmap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gmap.Name = "gmap";
            this.gmap.NegativeMode = false;
            this.gmap.PolygonsEnabled = true;
            this.gmap.RetryLoadTile = 0;
            this.gmap.RoutesEnabled = true;
            this.gmap.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gmap.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gmap.ShowTileGridLines = false;
            this.gmap.Size = new System.Drawing.Size(1302, 705);
            this.gmap.TabIndex = 0;
            this.gmap.Zoom = 0D;
            this.gmap.Load += new System.EventHandler(this.gMapControl1_Load);
            // 
            // NextRoute
            // 
            this.NextRoute.BackColor = System.Drawing.Color.Red;
            this.NextRoute.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NextRoute.Location = new System.Drawing.Point(1320, 12);
            this.NextRoute.Name = "NextRoute";
            this.NextRoute.Size = new System.Drawing.Size(157, 114);
            this.NextRoute.TabIndex = 1;
            this.NextRoute.Text = "NextBike";
            this.NextRoute.UseVisualStyleBackColor = false;
            this.NextRoute.Click += new System.EventHandler(this.NextRoute_Click);
            // 
            // NextCarRoute
            // 
            this.NextCarRoute.BackColor = System.Drawing.SystemColors.HotTrack;
            this.NextCarRoute.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NextCarRoute.Location = new System.Drawing.Point(1320, 173);
            this.NextCarRoute.Name = "NextCarRoute";
            this.NextCarRoute.Size = new System.Drawing.Size(157, 117);
            this.NextCarRoute.TabIndex = 2;
            this.NextCarRoute.Text = "NextCar";
            this.NextCarRoute.UseVisualStyleBackColor = false;
            this.NextCarRoute.Click += new System.EventHandler(this.NextCarRoute_Click);
            this.NextCarRoute.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NextCarRoute_KeyDown);
            // 
            // Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1489, 729);
            this.Controls.Add(this.NextCarRoute);
            this.Controls.Add(this.NextRoute);
            this.Controls.Add(this.gmap);
            this.Name = "Interface";
            this.Text = "Interface";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gmap;
        private System.Windows.Forms.Button NextRoute;
        private System.Windows.Forms.Button NextCarRoute;
    }
}