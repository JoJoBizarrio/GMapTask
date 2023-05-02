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
            this.ScriptActionsRadioButtons_GroupBox = new System.Windows.Forms.GroupBox();
            this.NewMarker_RadioButton = new System.Windows.Forms.RadioButton();
            this.MarkerColor_RadioButton = new System.Windows.Forms.RadioButton();
            this.DialogBox_RadioButton = new System.Windows.Forms.RadioButton();
            this.ScriptActionsRadioButtons_GroupBox.SuspendLayout();
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
            // ScriptActionsRadioButtons_GroupBox
            // 
            this.ScriptActionsRadioButtons_GroupBox.AutoSize = true;
            this.ScriptActionsRadioButtons_GroupBox.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ScriptActionsRadioButtons_GroupBox.Controls.Add(this.NewMarker_RadioButton);
            this.ScriptActionsRadioButtons_GroupBox.Controls.Add(this.MarkerColor_RadioButton);
            this.ScriptActionsRadioButtons_GroupBox.Controls.Add(this.DialogBox_RadioButton);
            this.ScriptActionsRadioButtons_GroupBox.Location = new System.Drawing.Point(12, 12);
            this.ScriptActionsRadioButtons_GroupBox.MinimumSize = new System.Drawing.Size(160, 0);
            this.ScriptActionsRadioButtons_GroupBox.Name = "ScriptActionsRadioButtons_GroupBox";
            this.ScriptActionsRadioButtons_GroupBox.Size = new System.Drawing.Size(160, 115);
            this.ScriptActionsRadioButtons_GroupBox.TabIndex = 2;
            this.ScriptActionsRadioButtons_GroupBox.TabStop = false;
            this.ScriptActionsRadioButtons_GroupBox.Text = "Choose script action:";
            // 
            // NewMarker_RadioButton
            // 
            this.NewMarker_RadioButton.AutoSize = true;
            this.NewMarker_RadioButton.Location = new System.Drawing.Point(6, 74);
            this.NewMarker_RadioButton.Name = "NewMarker_RadioButton";
            this.NewMarker_RadioButton.Size = new System.Drawing.Size(100, 20);
            this.NewMarker_RadioButton.TabIndex = 2;
            this.NewMarker_RadioButton.Text = "New marker";
            this.NewMarker_RadioButton.UseVisualStyleBackColor = true;
            this.NewMarker_RadioButton.CheckedChanged += new System.EventHandler(this.ScriptActions_CheckedChanged);
            // 
            // MarkerColor_RadioButton
            // 
            this.MarkerColor_RadioButton.AutoSize = true;
            this.MarkerColor_RadioButton.Location = new System.Drawing.Point(6, 48);
            this.MarkerColor_RadioButton.Name = "MarkerColor_RadioButton";
            this.MarkerColor_RadioButton.Size = new System.Drawing.Size(103, 20);
            this.MarkerColor_RadioButton.TabIndex = 1;
            this.MarkerColor_RadioButton.Text = "Marker color";
            this.MarkerColor_RadioButton.UseVisualStyleBackColor = true;
            this.MarkerColor_RadioButton.CheckedChanged += new System.EventHandler(this.ScriptActions_CheckedChanged);
            // 
            // DialogBox_RadioButton
            // 
            this.DialogBox_RadioButton.AutoSize = true;
            this.DialogBox_RadioButton.Checked = true;
            this.DialogBox_RadioButton.Location = new System.Drawing.Point(6, 21);
            this.DialogBox_RadioButton.Name = "DialogBox_RadioButton";
            this.DialogBox_RadioButton.Size = new System.Drawing.Size(93, 20);
            this.DialogBox_RadioButton.TabIndex = 0;
            this.DialogBox_RadioButton.TabStop = true;
            this.DialogBox_RadioButton.Text = "Dialog box";
            this.DialogBox_RadioButton.UseVisualStyleBackColor = true;
            this.DialogBox_RadioButton.CheckedChanged += new System.EventHandler(this.ScriptActions_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ScriptActionsRadioButtons_GroupBox);
            this.Controls.Add(this.MyGMapControl);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_Closed);
            this.ScriptActionsRadioButtons_GroupBox.ResumeLayout(false);
            this.ScriptActionsRadioButtons_GroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl MyGMapControl;
        private System.Windows.Forms.GroupBox ScriptActionsRadioButtons_GroupBox;
        private System.Windows.Forms.RadioButton NewMarker_RadioButton;
        private System.Windows.Forms.RadioButton MarkerColor_RadioButton;
        private System.Windows.Forms.RadioButton DialogBox_RadioButton;
    }
}
