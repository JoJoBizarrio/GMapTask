namespace GMapTask
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.MyGMapControl = new GMap.NET.WindowsForms.GMapControl();
            this.SuspendLayout();
            // 
            // MyGMapControl
            // 
            this.MyGMapControl.Bearing = 0F;
            this.MyGMapControl.CanDragMap = true;
            this.MyGMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MyGMapControl.EmptyTileColor = System.Drawing.Color.Navy;
            this.MyGMapControl.GrayScaleMode = false;
            this.MyGMapControl.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.MyGMapControl.LevelsKeepInMemory = 5;
            this.MyGMapControl.Location = new System.Drawing.Point(0, 0);
            this.MyGMapControl.Margin = new System.Windows.Forms.Padding(10);
            this.MyGMapControl.MarkersEnabled = true;
            this.MyGMapControl.MaxZoom = 2;
            this.MyGMapControl.MinZoom = 2;
            this.MyGMapControl.MouseWheelZoomEnabled = true;
            this.MyGMapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.MyGMapControl.Name = "MyGMapControl";
            this.MyGMapControl.NegativeMode = false;
            this.MyGMapControl.PolygonsEnabled = true;
            this.MyGMapControl.RetryLoadTile = 0;
            this.MyGMapControl.RoutesEnabled = true;
            this.MyGMapControl.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.MyGMapControl.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.MyGMapControl.ShowTileGridLines = false;
            this.MyGMapControl.Size = new System.Drawing.Size(800, 450);
            this.MyGMapControl.TabIndex = 0;
            this.MyGMapControl.Zoom = 0D;
            this.MyGMapControl.OnMarkerEnter += new GMap.NET.WindowsForms.MarkerEnter(this.MyGMapControl_OnMarkerEnter);
            this.MyGMapControl.OnMarkerLeave += new GMap.NET.WindowsForms.MarkerLeave(this.MyGMapControl_OnMarkerLeave);
            this.MyGMapControl.Load += new System.EventHandler(this.MyGMapControl_Load);
            this.MyGMapControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MyGMapMarker_MouseMove);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.MyGMapControl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_Closed);
            this.ResumeLayout(false);

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl MyGMapControl;
    }
}
