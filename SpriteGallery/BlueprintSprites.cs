using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BubbleAssets;

using WikiGen.Assets;

namespace SpriteGallery
{
    internal interface ISpriteCollection
    {
        int Count { get; }
        IEnumerable<SpriteInfo> Sprites { get; }
    }

    internal class DumpedSprites : ISpriteCollection
    {
        public readonly string DumpLocation;

        public DumpedSprites(string dumpLocation)
        {
            DumpLocation = dumpLocation;
        }

        private IEnumerable<Lazy<SpriteInfo>> GetGetters()
        {
            Program.Log.WriteLine($"Dump: {DumpLocation}");
            foreach (var ((fileId, assetId, name), path) in 
                Directory.EnumerateFiles(DumpLocation, "index.csv", SearchOption.AllDirectories)
                .SelectMany(f =>
                {
                    Program.Log.WriteLine($"Loading from {f}");

                    var lines = File.ReadAllLines(f);

                    Program.Log.WriteLine($"  {lines.Length} entries");

                    return lines
                        .Select(l => l.Split(','))
                        .Where(fields => fields.Length == 3)
                        .Select(fields => (fileId: fields[0], assetId: fields[1], name: fields[2]));
                })
                .Select(entry => (entry, path: Path.Join(DumpLocation, entry.fileId, $"{entry.assetId}.png")))
                .Where(entryAndPath => File.Exists(entryAndPath.path)))
            {
                yield return new(() => new SpriteInfo(assetId, long.Parse(fileId), name, Image.FromFile(path)));
            }
        }

        private Lazy<SpriteInfo>[]? spriteGetters;
        private Lazy<SpriteInfo>[] SpriteGetters => spriteGetters ??= GetGetters().ToArray();

        public int Count => SpriteGetters.Length;
        public IEnumerable<SpriteInfo> Sprites => SpriteGetters.Select(s => s.Value);
    }

    public readonly record struct SpriteInfo(string AssetId, long FileId, string Name, Image Image);

    internal class BlueprintSprites : ISpriteCollection
    {
        //public readonly record struct SpriteInfo(string AssetId, long FileId, Sprite Sprite, Image Image);

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
            Program.Log.WriteLine($"{nameof(AddBundle)} {bundlePath}");
            try
            {
                AssetContext.AddBundle(bundlePath);

                BundleFiles = AssetContext.assetsByBundle.First().Value;

                Program.Log.WriteLine($"{BundleFiles.Count()} bundle files");

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

                    Program.Log.WriteLine($"Owning bundle: {bundle.OwningBundle} {refAssets.Count} assets");

                    ReferencedBundles[bundle] = new(refAssets);
                }
            }
            catch (Exception ex)
            {
                Program.Log.WriteLine($"EXCEPTION loading bundle from {bundlePath}");
                Program.Log.WriteLine(ex.ToString());
#if DEBUG
                throw;
#endif
            }
        }

        private IEnumerable<(string, BlueprintAssetReference, ObjectInfo)> GetSpriteAssets() =>
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
        
        public int Count => GetSpriteAssets().Count();

        private IEnumerable<SpriteInfo> LoadSprites()
        {
            if(ReferencedBundles.Count == 0) return Enumerable.Empty<SpriteInfo>();
            
            return GetSpriteAssets()
                .SelectMany(asset =>
                {
                    var (id, assetRef, o) = asset;
                    using var reader = new AssetFileReader(o);
                    var sprite = new Sprite(reader);

                    if (!BlueprintAssetsContext.TryRenderSprite(sprite, out Bitmap? bitmap))
                        return Enumerable.Empty<SpriteInfo>();

                    return new[] { new SpriteInfo(id, assetRef.FileId, sprite.Name, bitmap) };
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
