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

        public int IndexForTile(Tile tile) => ((tile.Row) * Columns) + tile.Column;

        public Tile TileForIndex(int index) => new(index % Columns, index / Columns);

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

            if (Focused && Selected is Tile tile)
            {
                var clip = g.Clip;
                
                var tileRect = TileRect(tile);
                
                g.SetClip(tileRect);

                g.DrawRectangle(new Pen(this.ForeColor, 8), tileRect);

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
            if (tile == Selected) return;

            var previous = Selected;
            Selected = tile;

            OnSelectedChanged(Selected, previous);
        }

        public event Action<Tile?, Tile?> SelectedChanged;
        
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

        private Tile? TileAtPoint(Point location)
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
            
            SelectTileAtPosition(e.Location);
            this.Focus();
        }
        
        protected override void OnGotFocus(EventArgs e)
        {
            if (Selected is Tile tile)
            {
                this.Invalidate(TileRect(tile));
            }

            base.OnGotFocus(e);
            this.Select();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (Selected is Tile tile)
            {
                this.Invalidate(TileRect(tile));
            }

            base.OnLostFocus(e);
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

            SelectedChanged += (selected, _) =>
            {

                Rectangle? tileRect = selected is Tile t ? TileRect(t) : null;

                if (tileRect is Rectangle rect)
                {
                    var dummy = new Control { Top = rect.Y, Left = rect.X, Height = rect.Height, Width = 0 };
                    Controls.Add(dummy);
                    this.ScrollControlIntoView(dummy);
                    Controls.Remove(dummy);
                }
            };
        }

        protected override void OnPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case var k when k.HasFlag(Keys.Right):
                    MoveSelection(Direction.Right);
                    break;
                case var k when k.HasFlag(Keys.Up):
                    MoveSelection(Direction.Up);
                    break;
                case var k when k.HasFlag(Keys.Left):
                    MoveSelection(Direction.Left);
                    break;
                case var k when k.HasFlag(Keys.Down):
                    MoveSelection(Direction.Down);
                    break;
                case var k when k.HasFlag(Keys.PageUp):
                    MoveSelection(Direction.PageUp);
                    break;
                case var k when k.HasFlag(Keys.PageDown):
                    MoveSelection(Direction.PageDown);
                    break;
                default:
                    base.OnPreviewKeyDown(e);
                    return;
            }

            e.IsInputKey = true;
        }

        private enum Direction
        {
            None,
            Up,
            Right,
            Down,
            Left,
            PageUp,
            PageDown,
        }

        private Direction KeyToDirection(Keys key)
        {
            if (key == Keys.Right) return Direction.Right;
            if (key == Keys.Up) return Direction.Up;
            if (key == Keys.Left) return Direction.Left;
            if (key == Keys.Down) return Direction.Down;
            if (key == Keys.PageUp) return Direction.PageUp;
            if (key == Keys.PageDown) return Direction.PageDown;

            return Direction.None;
        }

        private Direction movedOne = Direction.None;

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (KeyToDirection(e.KeyData) == movedOne)
                movedOne = Direction.None;

            base.OnKeyUp(e);
        }

        private void MoveSelection(Direction d)
        {
            if (d is Direction.None) return;

            if (Selected is not Tile selected)
            {
                switch (d)
                {
                    case Direction.Right:
                    case Direction.Down:
                        SetSelected(new Tile(0, 0));
                        break;
                    case Direction.Left:
                        SetSelected(TileForIndex(Sprites.Count - 1));
                        break;
                    case Direction.Up:
                        SetSelected(new Tile(0,
                            TileForIndex(Sprites.Count - 1).Row));
                        break;
                }
            }
            else
            { 
                var index = IndexForTile(selected);

                switch (d)
                {
                    case Direction.Right:
                        if (index + 1 < Sprites.Count)
                            SetSelected(TileForIndex(index + 1));
                        else
                            if (movedOne != d)
                                SetSelected(TileForIndex(0));
                        break;
                    case Direction.Down:
                        if (index + Columns < Sprites.Count)
                            SetSelected(TileForIndex(index + Columns));
                        else
                            if(movedOne != d)
                                SetSelected(new Tile(selected.Column, 0));
                        break;
                    case Direction.Left:
                        if (index > 0)
                            SetSelected(TileForIndex(index - 1));
                        else
                            if (movedOne != d)
                                SetSelected(TileForIndex(Sprites.Count - 1));
                        break;
                    case Direction.Up:
                        if (index >= Columns)
                            SetSelected(TileForIndex(index - Columns));
                        else if (movedOne != d)
                        { 
                            if (index > 0)
                                SetSelected(TileForIndex(0));
                            else
                            {
                                var tileUp = new Tile(selected.Column, Rows - 1);;
                                if(IndexForTile(tileUp) >= Sprites.Count)
                                    SetSelected(TileForIndex(Sprites.Count - 1));
                                else SetSelected(tileUp);
                            }
                        }
                        break;
                    case Direction.PageUp:
                        if (selected.Row - VisibleRows > 0)
                            SetSelected(new Tile(selected.Column, selected.Row - VisibleRows));
                        else SetSelected(new Tile(selected.Column, 0));
                        break;
                    case Direction.PageDown:
                        var tilePageDown = new Tile(selected.Column, selected.Row + VisibleRows);

                        if (IndexForTile(tilePageDown) >= Sprites.Count) SetSelected(TileForIndex(Sprites.Count - 1));
                        else SetSelected(tilePageDown);
                        break;
                }
            }
            movedOne = d;
        }
    }
}
