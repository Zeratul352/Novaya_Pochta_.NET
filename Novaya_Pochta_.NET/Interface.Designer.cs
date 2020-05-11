namespace Novaya_Pochta_.NET
{
    partial class Nova_pochta
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Nova_pochta));
            this.gmap = new GMap.NET.WindowsForms.GMapControl();
            this.NextStep = new System.Windows.Forms.Button();
            this.NextRoute = new System.Windows.Forms.Button();
            this.Current_courier = new System.Windows.Forms.Label();
            this.curier_text = new System.Windows.Forms.Label();
            this.NextCurier = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.help_tooltip = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.source_selection = new System.Windows.Forms.Button();
            this.dialog = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.progressStrip = new System.Windows.Forms.ToolStripProgressBar();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
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
            this.gmap.Location = new System.Drawing.Point(0, 2);
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
            this.gmap.Size = new System.Drawing.Size(1302, 720);
            this.gmap.TabIndex = 0;
            this.gmap.Zoom = 0D;
            this.gmap.Load += new System.EventHandler(this.gMapControl1_Load);
            // 
            // NextStep
            // 
            this.NextStep.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.NextStep.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NextStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NextStep.Location = new System.Drawing.Point(1320, 12);
            this.NextStep.Name = "NextStep";
            this.NextStep.Size = new System.Drawing.Size(157, 117);
            this.NextStep.TabIndex = 1;
            this.NextStep.Text = "Next step";
            this.NextStep.UseVisualStyleBackColor = false;
            this.NextStep.Click += new System.EventHandler(this.Next_step_click);
            // 
            // NextRoute
            // 
            this.NextRoute.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.NextRoute.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NextRoute.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NextRoute.Location = new System.Drawing.Point(1320, 157);
            this.NextRoute.Name = "NextRoute";
            this.NextRoute.Size = new System.Drawing.Size(157, 117);
            this.NextRoute.TabIndex = 2;
            this.NextRoute.Text = "Next route";
            this.NextRoute.UseVisualStyleBackColor = false;
            this.NextRoute.Click += new System.EventHandler(this.Next_route_click);
            // 
            // Current_courier
            // 
            this.Current_courier.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Current_courier.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Current_courier.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.Current_courier.Location = new System.Drawing.Point(341, -3);
            this.Current_courier.Name = "Current_courier";
            this.Current_courier.Size = new System.Drawing.Size(218, 39);
            this.Current_courier.TabIndex = 3;
            this.Current_courier.Text = "Current courier is:";
            this.Current_courier.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // curier_text
            // 
            this.curier_text.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.curier_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.curier_text.ForeColor = System.Drawing.Color.RoyalBlue;
            this.curier_text.Location = new System.Drawing.Point(556, -3);
            this.curier_text.Name = "curier_text";
            this.curier_text.Size = new System.Drawing.Size(342, 39);
            this.curier_text.TabIndex = 4;
            this.curier_text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NextCurier
            // 
            this.NextCurier.BackColor = System.Drawing.SystemColors.HotTrack;
            this.NextCurier.Cursor = System.Windows.Forms.Cursors.Hand;
            this.NextCurier.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.NextCurier.Location = new System.Drawing.Point(1320, 302);
            this.NextCurier.Name = "NextCurier";
            this.NextCurier.Size = new System.Drawing.Size(157, 117);
            this.NextCurier.TabIndex = 5;
            this.NextCurier.Text = "Next courier";
            this.NextCurier.UseVisualStyleBackColor = false;
            this.NextCurier.Click += new System.EventHandler(this.NextCurier_Click);
            // 
            // help_tooltip
            // 
            this.help_tooltip.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.help_tooltip.Cursor = System.Windows.Forms.Cursors.Help;
            this.help_tooltip.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.help_tooltip.Location = new System.Drawing.Point(-4, -3);
            this.help_tooltip.Name = "help_tooltip";
            this.help_tooltip.Size = new System.Drawing.Size(48, 23);
            this.help_tooltip.TabIndex = 6;
            this.help_tooltip.Text = "help";
            this.toolTip1.SetToolTip(this.help_tooltip, resources.GetString("help_tooltip.ToolTip"));
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "TestInput.csv";
            this.openFileDialog1.Filter = "\"csv files(*.csv)|*.csv";
            this.openFileDialog1.Title = "Open .csv file with packages";
            // 
            // source_selection
            // 
            this.source_selection.BackColor = System.Drawing.Color.MediumPurple;
            this.source_selection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.source_selection.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.source_selection.Location = new System.Drawing.Point(1320, 447);
            this.source_selection.Name = "source_selection";
            this.source_selection.Size = new System.Drawing.Size(157, 117);
            this.source_selection.TabIndex = 7;
            this.source_selection.Text = "Select source";
            this.source_selection.UseVisualStyleBackColor = false;
            this.source_selection.Click += new System.EventHandler(this.Select_source_Click);
            // 
            // dialog
            // 
            this.dialog.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.dialog.ForeColor = System.Drawing.SystemColors.ControlText;
            this.dialog.Location = new System.Drawing.Point(6, 699);
            this.dialog.Name = "dialog";
            this.dialog.Size = new System.Drawing.Size(1289, 23);
            this.dialog.TabIndex = 8;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressStrip,
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 678);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1482, 25);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // progressStrip
            // 
            this.progressStrip.Name = "progressStrip";
            this.progressStrip.Size = new System.Drawing.Size(100, 19);
            // 
            // StatusLabel
            // 
            this.StatusLabel.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(121, 20);
            this.StatusLabel.Text = "Select source file";
            // 
            // Nova_pochta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(1482, 703);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.dialog);
            this.Controls.Add(this.source_selection);
            this.Controls.Add(this.help_tooltip);
            this.Controls.Add(this.NextCurier);
            this.Controls.Add(this.curier_text);
            this.Controls.Add(this.Current_courier);
            this.Controls.Add(this.NextRoute);
            this.Controls.Add(this.NextStep);
            this.Controls.Add(this.gmap);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1500, 750);
            this.MinimumSize = new System.Drawing.Size(1500, 750);
            this.Name = "Nova_pochta";
            this.Text = "Nova pochta";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gmap;
        private System.Windows.Forms.Button NextStep;
        private System.Windows.Forms.Button NextRoute;
        private System.Windows.Forms.Label Current_courier;
        private System.Windows.Forms.Label curier_text;
        private System.Windows.Forms.Button NextCurier;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label help_tooltip;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button source_selection;
        private System.Windows.Forms.Label dialog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar progressStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
    }
}