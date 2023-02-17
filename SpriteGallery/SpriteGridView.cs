using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SpriteGallery.Util;

using WikiGen.Assets;

namespace SpriteGallery
{
    
    public partial class SpriteGridView : ScrollableControl
    {
        public readonly record struct Tile(int Column, int Row);

        internal readonly List<BlueprintSprites.SpriteInfo> Sprites = new();

        public int TileWidth { get; set; } = 64;
        public int TileHeight { get; set; } = 64;
        public int TileMargin { get; set; } = 1;

        public int ColumnWidth => TileWidth + (TileMargin * 2);
        public int RowHeight => TileHeight + (TileMargin * 2);

        public int Columns => Math.Max(Width / ColumnWidth, 1);
        public int VisibleRows => Height / RowHeight;
        public int Rows => Math.Max((Sprites.Count / Columns) + 1, VisibleRows);
        public int InternalHeight => Rows * RowHeight;

        private int IndexForTile(Tile tile) => ((tile.Row) * Columns) + tile.Column;

        private Tile TileForIndex(int index) => new(index % Columns, index / Columns);

        private Point TilePosition(Tile tile, bool includeAutoScrollY = true)
        {
            var x = TileMargin + (tile.Column * ColumnWidth);
            var y = TileMargin + (tile.Row * RowHeight);

            if (includeAutoScrollY) y += this.AutoScrollPosition.Y;

            return new(x, y);
        }

        internal void ApplyLayout()
        {
            this.AutoScrollMinSize = new Size(1, InternalHeight);
            this.Refresh();
        }

        private void DrawSprite(int index, BlueprintSprites.SpriteInfo sprite, Graphics g)
        {
            var rect = TileRect(TileForIndex(index));

            g.FillRectangle(new SolidBrush(Color.Black), rect);

            g.DrawImage(sprite.Image, rect);
            
        }

        protected override void OnResize(EventArgs e)
        {
            ApplyLayout();

            base.OnResize(e);
        }

        private Rectangle TileRect(Tile tile)
        {
            var point = TilePosition(tile);
            return new Rectangle(point.X, point.Y, TileWidth, TileHeight);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;

            foreach ((var index, var sprite) in Sprites.Indexed())
            {
                DrawSprite(index, sprite, g);
            }

            if (Selected is Tile tile)
            {
                var clip = g.Clip;
                
                var tileRect = TileRect(tile);
                
                g.SetClip(tileRect);

                g.DrawRectangle(new Pen(ForeColor, 8), tileRect);

                g.Clip = clip;
            }
        }

        internal BlueprintSprites.SpriteInfo? GetSprite(Tile? tile)
        {
            if(tile is not Tile t) return null;

            var index = IndexForTile(t);

            return Sprites.Count > index ? Sprites[index] : null;
        }

        internal BlueprintSprites.SpriteInfo? SelectedSprite => GetSprite(Selected);

        internal Tile? Selected { get; private set; }
        
        internal void SetSelected(Tile? tile)
        {
            var previous = Selected;
            Selected = tile;

            OnSelectedChanged(Selected, previous);
        }

        public event Action<Tile?, Tile?>? SelectedChanged;
        
        protected void OnSelectedChanged(Tile? tile, Tile? previous)
        {
            SelectedChanged?.Invoke(tile, previous);

            if (tile is Tile selectedTile)
            {
                this.Invalidate(TileRect(selectedTile));
            }

            if (previous is Tile previousTile)
            {
                this.Invalidate(TileRect(previousTile));
            }

            this.Update();
        }

        internal Tile? TileAtPoint(Point location)
        {
            var column = location.X / ColumnWidth;
            var row = (location.Y - this.AutoScrollPosition.Y) / RowHeight;

            if (column >= Columns || row >= Rows) return null;

            var tile = new Tile(column, row);

            var index = IndexForTile(tile);

            return Sprites.Count > index ? tile : null;
        }

        protected void SelectTileAtPosition(Point point) => SetSelected(TileAtPoint(point));

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button != MouseButtons.Left) return;

            this.Focus();
            this.Select();

            SelectTileAtPosition(e.Location);
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }


        //public event EventHandler? GotFocus;
        protected override void OnGotFocus(EventArgs e)
        {
            
        }

        public SpriteGridView()
        {
            this.SetStyle(ControlStyles.ContainerControl, false);
            this.SetStyle(ControlStyles.Selectable, true);

            this.DoubleBuffered = true;

            InitializeComponent();

            this.VerticalScroll.Enabled = true;
            this.VerticalScroll.Visible = true;

            Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);

            //SelectedChanged += sprite => { };
        }
    }
}
