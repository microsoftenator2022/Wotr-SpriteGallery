using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpriteGallery
{
    public partial class SpriteTile : Control
    {
        private BlueprintSprites.SpriteInfo? sprite;

        internal BlueprintSprites.SpriteInfo? Sprite
        {
            get => sprite;
            set
            {
                sprite = value;
                Image = sprite?.Image;
                this.Refresh();
            }
        }

        public SpriteTile()
        {
            InitializeComponent();
        }

        //protected override void OnMouseUp(MouseEventArgs e)
        //{
        //    base.OnMouseUp(e);

        //    if (Parent is SpriteGridView grid)
        //        grid.Selected = this;

        //    this.Refresh();
        //}

        private readonly object newImageLock = new();
        private bool hasNewImage = false;
        private Image? newImage;

        private Image? oldImage;
        internal Image? Image
        {
            get
            {
                if (hasNewImage)
                {
                    lock(newImageLock)
                    {
                        oldImage = newImage;
                        hasNewImage = false;
                        NeedsUpdate = true;
                    }
                }

                return oldImage;
            }
            private set
            {
                lock(newImageLock)
                {
                    newImage = value;
                    hasNewImage = true;
                }
            }
        }

        private bool NeedsUpdate;

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (Image is not null && NeedsUpdate)
            {
                var currentState = new Bitmap(this.Width, this.Height);

                var graphics = Graphics.FromImage(currentState);

                graphics.Clear(Color.Black);
                graphics.DrawImage(Image, 0, 0, this.Width, this.Height);

                oldImage = currentState;

                NeedsUpdate = false;
            }

            if (Image is null) pe.Graphics.Clear(BackColor);

            else pe.Graphics.DrawImage(Image, 0, 0, this.Width, this.Height);

            //if ((Parent as SpriteGridView)?.Selected == this)
            //{
            //    pe.Graphics.DrawRectangle(
            //        new Pen(Parent?.ForeColor ?? ForeColor, 8),
            //        new Rectangle(0, 0, this.Width, this.Height));
            //}
        }
    }
}
