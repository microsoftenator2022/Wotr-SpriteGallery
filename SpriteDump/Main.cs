using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

using Kingmaker;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.JsonSystem.Converters;
using Kingmaker.EntitySystem.Persistence;

using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;

using UnityModManagerNet;

namespace SpriteDump
{
    public static class Main
    {
        internal static UnityModManager.ModEntry ModEntry = null!;
        internal static UnityModManager.ModEntry.ModLogger Logger => ModEntry.Logger;

        internal static Harmony Harmony = null!;

        internal static string OutputDirectory { get; set; } =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Dump");
            //$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}{Path.DirectorySeparatorChar}SpriteDump";

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            ModEntry = modEntry;

            Harmony = new(ModEntry.Info.Id);

            OutputDirectory = Path.Combine(ModEntry.Path, "Dump");

            Harmony.PatchAll();

            return true;
        }

        static System.Collections.IEnumerator ExportSprites()
        {
            var displayName = ModEntry.Info.DisplayName;
            ModEntry.Info.DisplayName = $"{displayName} : Exporting sprites...";

            var logSb = new StringBuilder();

            var spritesByFileId = UnityObjectConverter.AssetList.m_Entries
                .Where(entry => entry.Asset is Sprite)
                .GroupBy(entry => entry.FileId)
                .Select(group => (group.Key, group.Select(entry => (assetId: entry.AssetId, sprite: (entry.Asset as Sprite)!)).ToArray()));

            foreach (var (fileId, spriteAssets) in spritesByFileId)
            {
                var dir = Path.Combine(OutputDirectory, fileId.ToString());
                    //$"{OutputDirectory}{Path.DirectorySeparatorChar}{fileId}";

                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                //File.WriteAllLines($"{dir}{Path.DirectorySeparatorChar}index.csv",
                File.WriteAllLines(Path.Combine(dir, "index.csv"),
                    spriteAssets.Select(asset => $"{fileId},{asset.assetId},{asset.sprite.name}"));

                var i = 0;
                foreach (var (assetId, sprite) in spriteAssets)
                {
                    var filePath = Path.Combine(dir, $"{assetId}.png");
                        //$"{dir}{Path.DirectorySeparatorChar}{assetId}.png";

                    if (File.Exists(filePath))
                    {
                        Logger.Log($"{filePath} exists. Skipping");
                    }    
                    else if (sprite.packed && sprite.packingMode == SpritePackingMode.Tight)
                    {
                        Logger.Warning($"Skipping tightly-packed sprite {assetId}");
                    }
                    else
                    {
                        var progress = $"FileID {fileId}, {i + 1}/{spriteAssets.Length} ({(double)i / spriteAssets.Length * 100}%)";

                        ModEntry.Info.DisplayName = $"(Taking a) {displayName} : {progress}";

                        logSb.AppendLine($"{progress} Exporting Sprite {sprite.name} to {filePath}");
                        logSb.AppendLine($"  AssetId: {assetId}");
                        logSb.Append($"  FileId: {fileId}");

                        Logger.Log(logSb.ToString());
                        logSb.Clear();

                        var copy = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.RGBA32, false);

                        Graphics.ConvertTexture(sprite.texture, copy);

                        yield return null;

                        if (sprite.packed)
                        {
                            var spriteTexture = new Texture2D(
                                (int)sprite.textureRect.width,
                                (int)sprite.textureRect.height,
                                TextureFormat.RGBA32,
                                false);

                            Graphics.CopyTexture(
                                copy, 0, 0,
                                (int)sprite.textureRect.x,
                                (int)sprite.textureRect.y,
                                (int)sprite.textureRect.width,
                                (int)sprite.textureRect.height,
                                spriteTexture, 0, 0, 0, 0);

                            var old = copy;

                            UnityEngine.Object.Destroy(old);

                            copy = spriteTexture;
                        }

                        var request = AsyncGPUReadback.Request(copy, 0, TextureFormat.RGBA32, r =>
                        {
                            var data = r.GetData<Color32>(0);
                        
                            var bytes = ImageConversion.EncodeNativeArrayToPNG(
                                data,
                                copy.graphicsFormat,
                                (uint)copy.width,
                                (uint)copy.height)
                                .ToArray();

                            File.WriteAllBytes(filePath, bytes);

                            UnityEngine.Object.Destroy(copy);
                        });

                        while (!request.done)
                            yield return null;
                    }
                    
                    i++;
                }
            }

            Logger.Log("Finished exporting sprites");

            ModEntry.Info.DisplayName = $"{displayName} : Finished";
        }

        [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
        static class BlueprintsCache_Init_Patch
        {
            [HarmonyPostfix]
            static void Postfix()
            {
                LoadingProcess.Instance.StartCoroutine(ExportSprites());
            }
        }

        //[HarmonyPatch(typeof(MainMenu), nameof(MainMenu.Start))]
        //static class MainMenu_Start_Patch
        //{
        //    [HarmonyPostfix]
        //    static void Postfix(MainMenu __instance)
        //    {
        //        __instance.StartCoroutine(ExportSprites());
        //    }
        //}
    }
}
