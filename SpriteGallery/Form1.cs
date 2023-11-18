using System.Diagnostics;
using System.Drawing.Imaging;

using DotNet.Globbing;

using SpriteGallery.Util;

using WikiGen.Assets;

namespace SpriteGallery
{
    public partial class Form1 : Form
    {
        void OnSelectedChanged(SpriteInfo? sprite)
        {
            spriteTileBig.Sprite = sprite;

            if (sprite is null)
            {
                nameTextBox.Text = "Nothing selected";
                assetIDTextBox.Text = fileIDTextBox.Text = "";
                return;
            }

            assetIDTextBox.Text = $"{sprite.Value.AssetId}";
            nameTextBox.Text = $"{sprite.Value.Name}";
            fileIDTextBox.Text = $"{sprite.Value.FileId}";
        }

        ISpritesView currentView;

        bool optionsPanelVisible = false;
        bool OptionsPanelVisible {
            get => optionsPanelVisible;
            set
            {
                optionsPanelVisible = value;
                
                spritesViewPanel.SuspendLayout();
                SpritesViewPanelInner.SuspendLayout();
                OptionsPanel.SuspendLayout();

                OptionsPanel.Enabled = optionsPanelVisible;
                OptionsPanel.Visible = optionsPanelVisible;

                if (optionsPanelVisible)
                {
                    SpritesViewPanelInner.Location = new(0, OptionsPanel.Height);
                    SpritesViewPanelInner.Size = SpritesViewPanelInner.Size with { Height = SpritesViewPanelInner.Size.Height - OptionsPanel.Height };
                }
                else
                {
                    SpritesViewPanelInner.Location = new(0, 0);
                    SpritesViewPanelInner.Size = SpritesViewPanelInner.Size with { Height = SpritesViewPanelInner.Size.Height + OptionsPanel.Height };
                }

                spritesViewPanel.ResumeLayout();
                SpritesViewPanelInner.ResumeLayout();
                OptionsPanel.ResumeLayout();
            }
        }

        public Form1()
        {
            InitializeComponent();

            viewModeDropDown.Enabled = false;
            viewModeDropDown.Visible = false;
            SpritesViewModeLabel.Enabled = false;
            SpritesViewModeLabel.Visible = false;

            currentView = gridView;

            gridView.SelectedChanged += OnSelectedChanged;

            viewModeDropDown.SelectedIndexChanged += (_, _) =>
            {
                currentView = viewModeDropDown.SelectedIndex switch
                {
                    _ => gridView
                };
            };

            viewModeDropDown.SelectedIndex = 0;

            gridView.MouseMove += (_, args) =>
            {
                var xMin = viewOptionsButton.Location.X;
                var xMax = xMin + viewOptionsButton.Width;
                var yMin = viewOptionsButton.Location.Y;
                var yMax = yMin + viewOptionsButton.Height;

                if (args.X > xMin && args.X < xMax && args.Y > yMin && args.Y < yMax)
                {
                    viewOptionsButton.BringToFront();
                }
            };

            OptionsPanelVisible = false;

            viewOptionsButton.MouseLeave += (_, _) => viewOptionsButton.SendToBack();

            viewOptionsButton.Click += (_, _) =>
            {
                OptionsPanelVisible = !OptionsPanelVisible;

                OptionsPanel.Visible = OptionsPanelVisible;

                if (OptionsPanelVisible)
                {
                    viewOptionsButton.Text = "⬆️";
                }
                else
                {
                    viewOptionsButton.Text = "⬇️";
                }

                gridView.ApplyLayout();
            };

            spriteFilterGlob = Glob.Parse("*");

            var globOptions = GlobOptions.Default;
            globOptions.Evaluation.CaseInsensitive = true;

            NameFilterTextBox.TextChanged += (_, args) =>
            {
                if (string.IsNullOrWhiteSpace(NameFilterTextBox.Text))
                {
                    spriteFilterGlob = Glob.Parse("*");
                }
                else
                {
                    spriteFilterGlob = Glob.Parse("*" + NameFilterTextBox.Text + "*", globOptions);
                }

                gridView.Sprites.Clear();
                gridView.Sprites.AddRange(filteredSprites);
                gridView.ApplyLayout();
            };
        }

        Glob spriteFilterGlob;
        IEnumerable<SpriteInfo> filteredSprites =>
            Sprites?.Sprites?.Where(sprite => spriteFilterGlob.IsMatch(sprite.Name)) ??
            System.Linq.Enumerable.Empty<SpriteInfo>();

        internal ISpriteCollection? Sprites { get; set; } = null;

        protected override void OnLoad(EventArgs e)
        {
            assetIDTextBox.Text = "";
            base.OnLoad(e);
        }

        private void OpenBundleButton_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "|*.assets|All files (*.*)|*.*"
            };

            var result = dialog.ShowDialog();

            if (result != DialogResult.OK) return;

            var bundleFilePath = dialog.FileName;

            var sprites = new BlueprintSprites();

            Sprites = sprites;

            gridView.Sprites.Clear();

            var loadTask = new Task(() =>
            {
                sprites.AddBundle(bundleFilePath);

                this.Invoke(() =>
                {
                    label10.Visible = false;
                    label10.Enabled = false;

                    lock (this)
                    {
                        progressBar1.Maximum = Sprites.Count;
                        progressBar1.Value = 0;
                    }
                });


                var chunkSize = 1;

                foreach (var sprites in Sprites.Sprites.Chunk(chunkSize))
                {
                    this.Invoke(() =>
                    {
                        lock (this)
                        {
                            gridView.Sprites.AddRange(sprites);
                            progressBar1.Value += sprites.Length;
                        }
                    });
                }

                this.Invoke(() => gridView.ApplyLayout());

            });

            loadTask.Start();
        }

        private void OpenDumpButton_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();

            var result = dialog.ShowDialog();

            if (result is not DialogResult.OK) return;

            var sprites = new DumpedSprites(dialog.SelectedPath);

            Sprites = sprites;

            gridView.Sprites.Clear();

            Task.Run(() =>
            {
                this.Invoke(() =>
                {
                    label10.Visible = false;
                    label10.Enabled = false;

                    lock (this)
                    {
                        progressBar1.Maximum = Sprites.Count;
                        progressBar1.Value = 0;
                    }
                });

                foreach (var sprite in Sprites.Sprites)
                {
                    this.Invoke(() =>
                    {
                        lock (this)
                        {
                            gridView.Sprites.Add(sprite);
                            progressBar1.Value++;
                        }
                    });
                }

                this.Invoke(() => gridView.ApplyLayout());
            });
        }

        private void CopySelectedSpriteToClipboard()
        {
            if (gridView.SelectedSprite is not SpriteInfo sprite) return;

            Utils.Retry(() => Clipboard.SetImage(sprite.Image), 3, 100);

            this.Text = "Copied";
        }

        private void gridView_KeyPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case var k when k.HasFlag(Keys.Control) && k.HasFlag(Keys.C):
                    CopySelectedSpriteToClipboard();
                    break;
            }
        }

        internal bool SelectTileFromIds(string assetId, string fileIdStr)
        {
            var tile = gridView.Tiles
                .Where(t => t.sprite.AssetId == assetId &&
                    long.TryParse(fileIdStr, out var fileId) &&
                    t.sprite.FileId == fileId)
                .FirstOrDefault();

            if (tile == default) return false;

            if (tile.sprite != gridView.SelectedSprite)
                gridView.SetSelected(tile.tile);

            return true;
        }

        internal bool FindTileByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            var maybeMatch = gridView.Tiles
                .Select(tile =>
                {
                    var intersection =
                        tile.sprite.Name
                            .ToLowerInvariant()
                            .OrderedIntersect(name.ToLowerInvariant());

                    return (intersectSize: intersection.Count(), tile);
                })
                .Where(t => t.intersectSize > 0)
                .OrderByDescending(t => t.intersectSize)
                .Select(t => t.tile)
                .FirstOrDefault();

            if (maybeMatch == default)
                return false;

            if (maybeMatch.sprite != gridView.SelectedSprite)
                gridView.SetSelected(maybeMatch.tile);

            return true;
        }

        private void assetIDTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (!SelectTileFromIds(assetIDTextBox.Text, fileIDTextBox.Text)) return;

            gridView.Focus();

            e.SuppressKeyPress = true;
        }

        private void fileIDTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (!SelectTileFromIds(assetIDTextBox.Text, fileIDTextBox.Text)) return;

            gridView.Focus();

            e.SuppressKeyPress = true;
        }

        private void nameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            if (!FindTileByName(nameTextBox.Text)) return;

            gridView.Focus();

            e.SuppressKeyPress = true;
        }

    }
}
