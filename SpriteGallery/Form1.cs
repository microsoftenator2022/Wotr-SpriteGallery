using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using SpriteGallery.Util;

using WikiGen.Assets;

using static SpriteGallery.SpriteGridView;

namespace SpriteGallery
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            gridView.SelectedChanged += (tile, _) =>
            {
                var sprite = gridView.GetSprite(tile);

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

            };
        }

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
