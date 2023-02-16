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

        private Point TilePosition(int column, int row, bool includeAutoScrollY = true)
        {
            var x = TileMargin + (column * ColumnWidth);
            var y = TileMargin + (row * RowHeight);

            if (includeAutoScrollY) y += this.AutoScrollPosition.Y;

            return new(x, y);
        }

        private Point TilePosition(int index)
        {
            var row = index / Columns;
            var column = index % Columns;

            return TilePosition(column, row);
        }

        internal void ApplyLayout()
        {
            this.AutoScrollMinSize = new Size(1, InternalHeight);
            this.Refresh();
        }

        internal void DrawSprite(int index, BlueprintSprites.SpriteInfo sprite, Graphics g)
        {
            var position = TilePosition(index);

            g.DrawRectangle(
                new Pen(this.BackColor, 1),
                position.X - TileMargin,
                position.Y - TileMargin,
                ColumnWidth,
                RowHeight);

            g.FillRectangle(new SolidBrush(Color.Black), position.X, position.Y, TileWidth, TileHeight);

            g.DrawImage(sprite.Image, position.X, position.Y, TileWidth, TileHeight);
            
        }

        protected override void OnResize(EventArgs e)
        {
            ApplyLayout();

            base.OnResize(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;

            foreach ((var index, var sprite) in Sprites.Indexed())
            {
                DrawSprite(index, sprite, g);
            }

            if (selectedIndex >= 0)
            {
                var point = TilePosition(selectedIndex);
                var rect = new Rectangle(point.X + 2, point.Y + 2, TileWidth - 4, TileHeight - 4);

                g.DrawRectangle(new Pen(ForeColor, 4), rect);
            }
        }

        private int selectedIndex = -1;
        internal BlueprintSprites.SpriteInfo? Selected =>
            selectedIndex < 0 ? null : Sprites[selectedIndex];

        internal event Action<BlueprintSprites.SpriteInfo?> SelectedChanged;

        internal (int, BlueprintSprites.SpriteInfo?) SpriteAtPoint(int x, int y)
        {
            var column = x / ColumnWidth;
            var row = (y - this.AutoScrollPosition.Y) / RowHeight;

            var index = IndexForTile(column, row);

            return Sprites.Count > index ? (index, Sprites[index]) : (-1, null);
        }

        internal (int, BlueprintSprites.SpriteInfo?) SpriteAtPoint(Point location) =>
            SpriteAtPoint(location.X, location.Y);

        private int IndexForTile(int column, int row) =>
            ((row) * Columns) + column;

        protected void UpdateSelected(Point location)
        {
            var previous = selectedIndex;
            
            var (index, sprite) = SpriteAtPoint(location);

            selectedIndex = index;

            SelectedChanged(sprite);

            if(selectedIndex >= 0)
            {
                var point = TilePosition(index);
                var rect = new Rectangle(point.X, point.Y, TileWidth, TileHeight);

                this.Invalidate(rect);
            }

            if (previous >= 0)
            {
                var previousLocation = TilePosition(previous);
                var previousRect = new Rectangle(previousLocation.X, previousLocation.Y, TileWidth, TileHeight);

                this.Invalidate(previousRect);
            }

            this.Update();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button != MouseButtons.Left) return;

            UpdateSelected(e.Location);
        }

        public override Color ForeColor
        {
            get => base.ForeColor;
            set => base.ForeColor = value;
        }

        public SpriteGridView()
        {
            this.DoubleBuffered = true;

            InitializeComponent();

            this.VerticalScroll.Enabled = true;
            this.VerticalScroll.Visible = true;

            Padding = new Padding(0, 0, SystemInformation.VerticalScrollBarWidth, 0);

            SelectedChanged += sprite => { };
        }
    }
}
