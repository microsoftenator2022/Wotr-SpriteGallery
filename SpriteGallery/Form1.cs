using System.Diagnostics;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

using SpriteGallery.Util;

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
                nameTextBox.Text = $"{sprite.Value.Sprite.Name}";
                fileIDTextBox.Text = $"{sprite.Value.FileId}";

            };
        }

        internal BlueprintSprites? Sprites { get; set; } = null;

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

            Sprites = new();

            var loadTask = new Task(() =>
            {
                Sprites.AddBundle(bundleFilePath);

                this.Invoke(() =>
                {
                    lock (this)
                    {
                        progressBar1.Maximum = Sprites.GetSpriteAssets().Count();
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

        private void CopySelectedSpriteToClipboard()
        {
            if (gridView.SelectedSprite is not BlueprintSprites.SpriteInfo sprite) return;

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
    }
}
