using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BubbleAssets;

using WikiGen.Assets;

namespace SpriteGallery
{
    internal class BlueprintSprites
    {
        public readonly record struct SpriteInfo(string AssetId, long FileId, Sprite Sprite, Image Image);
        
        internal AssetContext AssetContext { get; init; }
        internal List<AssetFile> BundleFiles { get; set; } = new();
        //internal BlueprintAssetsContext? BlueprintContext { get; set; }

        internal Dictionary<AssetFile, BlueprintAssetsContext> ReferencedBundles { get; set; } = new();

        public BlueprintSprites()
        {
            AssetContext = new();
        }

        //public void LoadBlueprintReferencedAssets(string path) =>
        //    BlueprintContext = new BlueprintAssetsContext(path);

        public void AddBundle(string bundlePath)
        {
            AssetContext.AddBundle(bundlePath);

            BundleFiles = AssetContext.assetsByBundle.First().Value;

            foreach(var bundle in BundleFiles)
            {
                // BlueprintReferencedAssets is an index of sorts.
                // Maps "AssetId" and "FileId" from blueprints to assets like icons
                var refAssets = bundle.ObjectIndex
                    .Where(o => o.ClassType == ClassIDType.MonoBehaviour)
                    .Select(o =>
                    {
                        using var reader = new AssetFileReader(o);
                        return TreeDumper.ReadType(o.serializedType.TypeTree, reader);
                    })
                    .First(asset => asset["m_Name"] as string == "BlueprintReferencedAssets");
                
                ReferencedBundles[bundle] = new(refAssets);
            }

        }

        internal IEnumerable<(string, BlueprintAssetReference, ObjectInfo)> GetSpriteAssets() =>
            ReferencedBundles.SelectMany(b => b.Value.AssetRefs)
                .SelectMany(assetRef =>
                {
                    var pathID = assetRef.Asset.m_PathID;

                    var bundle = BundleFiles.First();

                    if (!bundle.ObjectLookup.ContainsKey(pathID))
                        return Enumerable.Empty<(string, BlueprintAssetReference, ObjectInfo)>();
                    return new[] { (assetRef.AssetId, assetRef, bundle.LookupObject(pathID)) };
                })
                .Where(asset => { var (_, _, o) = asset; return o.ClassType == ClassIDType.Sprite; });
        
        internal IEnumerable<SpriteInfo> LoadSprites()
        {
            if(ReferencedBundles.Count == 0) return Enumerable.Empty<SpriteInfo>().ToList();
            
            return GetSpriteAssets()
                .SelectMany(asset =>
                {
                    var (id, assetRef, o) = asset;
                    using var reader = new AssetFileReader(o);
                    var sprite = new Sprite(reader);

                    if (!BlueprintAssetsContext.TryRenderSprite(sprite, out Bitmap? bitmap))
                        return Enumerable.Empty<SpriteInfo>();

                    return new[] { new SpriteInfo(id, assetRef.FileId, sprite, bitmap) };
                });
        }

        private List<SpriteInfo>? sprites;

        public IEnumerable<SpriteInfo> Sprites
        {
            get
            {
                if (sprites is not null && sprites.Count > 0)
                {
                    foreach(var sprite in sprites) yield return sprite;
                }
                else
                {
                    var spritesLoaded = new List<SpriteInfo>();

                    foreach(var sprite in LoadSprites())
                    {
                        spritesLoaded.Add(sprite);

                        yield return sprite;
                    }

                    sprites = spritesLoaded;
                }
            }
        }
    }
}
