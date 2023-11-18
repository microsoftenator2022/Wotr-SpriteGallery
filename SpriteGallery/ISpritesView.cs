using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpriteGallery
{
    internal interface ISpritesView
    {
        IList<SpriteInfo> Sprites { get; }
        SpriteInfo? SelectedSprite { get; }
        event Action<SpriteInfo?> SelectedChanged;
    }
}
