﻿namespace SpriteGallery
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
            assetIDTextBox.BackColor = SystemColors.ControlLight;
            assetIDTextBox.BorderStyle = BorderStyle.FixedSingle;
            assetIDTextBox.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point);
            assetIDTextBox.Location = new Point(703, 175);
            assetIDTextBox.Name = "assetIDTextBox";
            assetIDTextBox.ReadOnly = true;
            assetIDTextBox.Size = new Size(231, 21);
            assetIDTextBox.TabIndex = 6;
            assetIDTextBox.Text = "0000000000000000f000000000000000";
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
            nameTextBox.BackColor = SystemColors.ControlLight;
            nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            nameTextBox.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point);
            nameTextBox.Location = new Point(703, 148);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.ReadOnly = true;
            nameTextBox.Size = new Size(231, 21);
            nameTextBox.TabIndex = 4;
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
            fileIDTextBox.BackColor = SystemColors.ControlLight;
            fileIDTextBox.BorderStyle = BorderStyle.FixedSingle;
            fileIDTextBox.Font = new Font("Cascadia Mono", 9F, FontStyle.Regular, GraphicsUnit.Point);
            fileIDTextBox.Location = new Point(703, 202);
            fileIDTextBox.Name = "fileIDTextBox";
            fileIDTextBox.ReadOnly = true;
            fileIDTextBox.Size = new Size(231, 21);
            fileIDTextBox.TabIndex = 7;
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
            progressBar1.Location = new Point(115, 11);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(527, 23);
            progressBar1.TabIndex = 9;
            // 
            // gridView
            // 
            gridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gridView.AutoScroll = true;
            gridView.BackColor = SystemColors.ControlDarkDark;
            gridView.ForeColor = SystemColors.Highlight;
            gridView.Location = new Point(12, 41);
            gridView.Name = "gridView";
            gridView.Padding = new Padding(0, 0, 17, 0);
            gridView.Size = new Size(630, 508);
            gridView.TabIndex = 10;
            gridView.TileHeight = 64;
            gridView.TileMargin = 1;
            gridView.TileWidth = 64;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(944, 561);
            Controls.Add(gridView);
            Controls.Add(progressBar1);
            Controls.Add(label3);
            Controls.Add(fileIDTextBox);
            Controls.Add(label2);
            Controls.Add(nameTextBox);
            Controls.Add(label1);
            Controls.Add(spriteTileBig);
            Controls.Add(assetIDTextBox);
            Controls.Add(OpenBundleButton);
            Name = "Form1";
            Text = "Form1";
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
    }
}