namespace SpriteGallery
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            OpenBundleButton = new Button();
            assetIDTextBox = new TextBox();
            spriteTileBig = new SpriteTile();
            label1 = new Label();
            nameTextBox = new TextBox();
            label2 = new Label();
            fileIDTextBox = new TextBox();
            label3 = new Label();
            progressBar1 = new ProgressBar();
            gridView = new SpriteGridView();
            label10 = new Label();
            viewOptionsButton = new Button();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label8 = new Label();
            label9 = new Label();
            label7 = new Label();
            OpenDumpButton = new Button();
            spritesViewPanel = new Panel();
            SpritesViewPanelInner = new Panel();
            OptionsPanel = new Panel();
            NameFilterTextBox = new TextBox();
            NameFilterLabel = new Label();
            SpritesViewModeLabel = new Label();
            viewModeDropDown = new ComboBox();
            gridView.SuspendLayout();
            spritesViewPanel.SuspendLayout();
            SpritesViewPanelInner.SuspendLayout();
            OptionsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // OpenBundleButton
            // 
            OpenBundleButton.BackColor = SystemColors.ButtonFace;
            OpenBundleButton.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            OpenBundleButton.FlatAppearance.MouseDownBackColor = SystemColors.ButtonShadow;
            OpenBundleButton.FlatAppearance.MouseOverBackColor = SystemColors.ButtonHighlight;
            OpenBundleButton.FlatStyle = FlatStyle.Flat;
            OpenBundleButton.Location = new Point(12, 10);
            OpenBundleButton.Name = "OpenBundleButton";
            OpenBundleButton.Size = new Size(97, 25);
            OpenBundleButton.TabIndex = 0;
            OpenBundleButton.Text = "Open bundle...";
            OpenBundleButton.UseVisualStyleBackColor = false;
            OpenBundleButton.Click += OpenBundleButton_Click;
            // 
            // assetIDTextBox
            // 
            assetIDTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            assetIDTextBox.BorderStyle = BorderStyle.FixedSingle;
            assetIDTextBox.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point);
            assetIDTextBox.Location = new Point(703, 175);
            assetIDTextBox.Name = "assetIDTextBox";
            assetIDTextBox.Size = new Size(231, 21);
            assetIDTextBox.TabIndex = 3;
            assetIDTextBox.Text = "0000000000000000f000000000000000";
            assetIDTextBox.KeyDown += assetIDTextBox_KeyDown;
            // 
            // spriteTileBig
            // 
            spriteTileBig.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            spriteTileBig.BackColor = SystemColors.ControlDarkDark;
            spriteTileBig.Location = new Point(806, 10);
            spriteTileBig.Margin = new Padding(1);
            spriteTileBig.MinimumSize = new Size(64, 64);
            spriteTileBig.Name = "spriteTileBig";
            spriteTileBig.Size = new Size(128, 128);
            spriteTileBig.TabIndex = 2;
            spriteTileBig.TabStop = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(648, 178);
            label1.Name = "label1";
            label1.Size = new Size(49, 15);
            label1.TabIndex = 5;
            label1.Text = "AssetID:";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // nameTextBox
            // 
            nameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            nameTextBox.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point);
            nameTextBox.Location = new Point(703, 148);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(231, 21);
            nameTextBox.TabIndex = 2;
            nameTextBox.KeyDown += nameTextBox_KeyDown;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(655, 151);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 3;
            label2.Text = "Name:";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // fileIDTextBox
            // 
            fileIDTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            fileIDTextBox.BorderStyle = BorderStyle.FixedSingle;
            fileIDTextBox.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point);
            fileIDTextBox.Location = new Point(703, 202);
            fileIDTextBox.Name = "fileIDTextBox";
            fileIDTextBox.Size = new Size(231, 21);
            fileIDTextBox.TabIndex = 4;
            fileIDTextBox.KeyDown += fileIDTextBox_KeyDown;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(658, 205);
            label3.Name = "label3";
            label3.Size = new Size(39, 15);
            label3.TabIndex = 8;
            label3.Text = "FileID:";
            label3.TextAlign = ContentAlignment.TopRight;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.Location = new Point(218, 11);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(424, 23);
            progressBar1.TabIndex = 9;
            // 
            // gridView
            // 
            gridView.AutoScroll = true;
            gridView.AutoScrollMinSize = new Size(1, 462);
            gridView.BackColor = SystemColors.ControlDarkDark;
            gridView.Controls.Add(label10);
            gridView.Dock = DockStyle.Fill;
            gridView.ForeColor = SystemColors.Highlight;
            gridView.Location = new Point(0, 0);
            gridView.Margin = new Padding(0);
            gridView.Name = "gridView";
            gridView.Padding = new Padding(0, 0, 17, 0);
            gridView.Size = new Size(630, 508);
            gridView.TabIndex = 1;
            gridView.TileHeight = 64;
            gridView.TileMargin = 1;
            gridView.TileWidth = 64;
            gridView.KeyUp += gridView_KeyPress;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);
            label10.ForeColor = Color.DarkRed;
            label10.Location = new Point(60, 131);
            label10.Name = "label10";
            label10.Size = new Size(437, 74);
            label10.TabIndex = 19;
            label10.Text = "Mouse over top-left of grid view\r\nto see options dropdown";
            // 
            // viewOptionsButton
            // 
            viewOptionsButton.BackColor = SystemColors.ButtonFace;
            viewOptionsButton.FlatAppearance.BorderSize = 0;
            viewOptionsButton.FlatStyle = FlatStyle.Flat;
            viewOptionsButton.Font = new Font("Segoe UI Emoji", 9F, FontStyle.Regular, GraphicsUnit.Point);
            viewOptionsButton.Location = new Point(0, 0);
            viewOptionsButton.Margin = new Padding(0);
            viewOptionsButton.Name = "viewOptionsButton";
            viewOptionsButton.Size = new Size(33, 33);
            viewOptionsButton.TabIndex = 2;
            viewOptionsButton.Text = "⬇️";
            viewOptionsButton.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label4.Location = new Point(808, 365);
            label4.Margin = new Padding(0);
            label4.Name = "label4";
            label4.Padding = new Padding(8, 8, 0, 8);
            label4.Size = new Size(128, 46);
            label4.TabIndex = 10;
            label4.Text = "Copy selected sprite to clipboard\r\n";
            label4.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            label5.Location = new Point(648, 350);
            label5.MinimumSize = new Size(286, 0);
            label5.Name = "label5";
            label5.Size = new Size(286, 15);
            label5.TabIndex = 11;
            label5.Text = "Keyboard shortcuts";
            label5.TextAlign = ContentAlignment.BottomCenter;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label6.Location = new Point(647, 365);
            label6.Margin = new Padding(0);
            label6.Name = "label6";
            label6.Padding = new Padding(0, 8, 0, 8);
            label6.Size = new Size(161, 46);
            label6.TabIndex = 12;
            label6.Text = "Ctrl+C";
            label6.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            label8.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label8.Location = new Point(647, 413);
            label8.Margin = new Padding(0);
            label8.Name = "label8";
            label8.Padding = new Padding(0, 8, 0, 8);
            label8.Size = new Size(161, 46);
            label8.TabIndex = 14;
            label8.Text = "Arrows\r\nPgUp/PgDown";
            label8.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            label9.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label9.Location = new Point(808, 413);
            label9.Margin = new Padding(0);
            label9.Name = "label9";
            label9.Padding = new Padding(8, 8, 0, 8);
            label9.Size = new Size(128, 46);
            label9.TabIndex = 15;
            label9.Text = "Move Selection";
            label9.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            label7.Location = new Point(648, 459);
            label7.Margin = new Padding(0);
            label7.Name = "label7";
            label7.Padding = new Padding(8, 8, 0, 8);
            label7.Size = new Size(286, 90);
            label7.TabIndex = 16;
            label7.Text = "Lookup By ID:\r\nEnter AssetID and FileID (optional) and press Enter\r\n\r\nSearch by name:\r\nEnter Name and press Enter";
            label7.TextAlign = ContentAlignment.BottomLeft;
            // 
            // OpenDumpButton
            // 
            OpenDumpButton.BackColor = SystemColors.ButtonFace;
            OpenDumpButton.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            OpenDumpButton.FlatAppearance.MouseDownBackColor = SystemColors.ButtonShadow;
            OpenDumpButton.FlatAppearance.MouseOverBackColor = SystemColors.ButtonHighlight;
            OpenDumpButton.FlatStyle = FlatStyle.Flat;
            OpenDumpButton.Location = new Point(115, 10);
            OpenDumpButton.Name = "OpenDumpButton";
            OpenDumpButton.Size = new Size(97, 25);
            OpenDumpButton.TabIndex = 17;
            OpenDumpButton.Text = "From dump...";
            OpenDumpButton.UseVisualStyleBackColor = false;
            OpenDumpButton.Click += OpenDumpButton_Click;
            // 
            // spritesViewPanel
            // 
            spritesViewPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            spritesViewPanel.BackColor = Color.Transparent;
            spritesViewPanel.Controls.Add(SpritesViewPanelInner);
            spritesViewPanel.Controls.Add(OptionsPanel);
            spritesViewPanel.Controls.Add(viewOptionsButton);
            spritesViewPanel.Location = new Point(12, 41);
            spritesViewPanel.Name = "spritesViewPanel";
            spritesViewPanel.Size = new Size(630, 508);
            spritesViewPanel.TabIndex = 18;
            // 
            // SpritesViewPanelInner
            // 
            SpritesViewPanelInner.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SpritesViewPanelInner.Controls.Add(gridView);
            SpritesViewPanelInner.Location = new Point(0, 33);
            SpritesViewPanelInner.Margin = new Padding(0);
            SpritesViewPanelInner.Name = "SpritesViewPanelInner";
            SpritesViewPanelInner.Size = new Size(630, 508);
            SpritesViewPanelInner.TabIndex = 3;
            // 
            // OptionsPanel
            // 
            OptionsPanel.BackColor = SystemColors.Control;
            OptionsPanel.Controls.Add(NameFilterTextBox);
            OptionsPanel.Controls.Add(NameFilterLabel);
            OptionsPanel.Controls.Add(SpritesViewModeLabel);
            OptionsPanel.Controls.Add(viewModeDropDown);
            OptionsPanel.Location = new Point(33, 0);
            OptionsPanel.Margin = new Padding(0);
            OptionsPanel.Name = "OptionsPanel";
            OptionsPanel.Size = new Size(597, 33);
            OptionsPanel.TabIndex = 0;
            // 
            // NameFilterTextBox
            // 
            NameFilterTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            NameFilterTextBox.Location = new Point(78, 5);
            NameFilterTextBox.Name = "NameFilterTextBox";
            NameFilterTextBox.Size = new Size(200, 23);
            NameFilterTextBox.TabIndex = 3;
            // 
            // NameFilterLabel
            // 
            NameFilterLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            NameFilterLabel.AutoSize = true;
            NameFilterLabel.Location = new Point(3, 8);
            NameFilterLabel.Name = "NameFilterLabel";
            NameFilterLabel.Size = new Size(69, 15);
            NameFilterLabel.TabIndex = 2;
            NameFilterLabel.Text = "Name filter:";
            // 
            // SpritesViewModeLabel
            // 
            SpritesViewModeLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SpritesViewModeLabel.AutoSize = true;
            SpritesViewModeLabel.Location = new Point(401, 8);
            SpritesViewModeLabel.Name = "SpritesViewModeLabel";
            SpritesViewModeLabel.Size = new Size(66, 15);
            SpritesViewModeLabel.TabIndex = 1;
            SpritesViewModeLabel.Text = "View Mode";
            // 
            // viewModeDropDown
            // 
            viewModeDropDown.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            viewModeDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
            viewModeDropDown.FlatStyle = FlatStyle.Flat;
            viewModeDropDown.FormattingEnabled = true;
            viewModeDropDown.Items.AddRange(new object[] { "Grid", "List" });
            viewModeDropDown.Location = new Point(473, 5);
            viewModeDropDown.Name = "viewModeDropDown";
            viewModeDropDown.Size = new Size(121, 23);
            viewModeDropDown.TabIndex = 0;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(944, 561);
            Controls.Add(spritesViewPanel);
            Controls.Add(OpenDumpButton);
            Controls.Add(label7);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(progressBar1);
            Controls.Add(label3);
            Controls.Add(fileIDTextBox);
            Controls.Add(label2);
            Controls.Add(nameTextBox);
            Controls.Add(label1);
            Controls.Add(spriteTileBig);
            Controls.Add(assetIDTextBox);
            Controls.Add(OpenBundleButton);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Form1";
            gridView.ResumeLayout(false);
            gridView.PerformLayout();
            spritesViewPanel.ResumeLayout(false);
            SpritesViewPanelInner.ResumeLayout(false);
            OptionsPanel.ResumeLayout(false);
            OptionsPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button OpenBundleButton;
        private TextBox assetIDTextBox;
        private SpriteTile spriteTileBig;
        private Label label1;
        private TextBox nameTextBox;
        private Label label2;
        private TextBox fileIDTextBox;
        private Label label3;
        private ProgressBar progressBar1;
        private SpriteGridView gridView;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label8;
        private Label label9;
        private Label label7;
        private Button OpenDumpButton;
        private Panel spritesViewPanel;
        private Button viewOptionsButton;
        private Panel OptionsPanel;
        private ComboBox viewModeDropDown;
        private Label SpritesViewModeLabel;
        private Panel SpritesViewPanelInner;
        private TextBox NameFilterTextBox;
        private Label NameFilterLabel;
        private Label label10;
    }
}