#r "bin/Debug/net6.0/BubbleAssets.dll"
#r "System.Drawing.Common"

open BubbleAssets
open BubbleAssets.Assets
open WikiGen.Assets
open System.Drawing
open System.Drawing.Imaging

let context = AssetContext()

context.AddBundle @"D:\SteamLibrary\steamapps\common\Pathfinder Second Adventure\Bundles\blueprint.assets"

let bundle = context.assetsByBundle |> Seq.head |> fun element -> element.Value |> Seq.head

let bpr =
    bundle.ObjectIndex
    |> Seq.where (fun o -> o.ClassType = ClassIDType.MonoBehaviour)
    |> Seq.map (fun o ->
        use reader = new AssetFileReader(o)
        MonoBehaviour(reader, o), o)
    |> Seq.pick(fun (mb, o) -> if mb.m_Name = "BlueprintReferencedAssets" then Some o else None)

let dumpObject o =
    use reader = new AssetFileReader(o)
    TreeDumper.ReadType(o.serializedType.TypeTree, reader)

let bprTree = dumpObject bpr

bprTree["m_Name"].ToString() |> printfn "Name: %s"

let bpContext = BlueprintAssetsContext(bprTree)

printfn "%A" bpContext.AssetRefs.Count

// printfn "Entries:"

// bprTree["m_Entries"] :?> seq<System.Collections.Specialized.OrderedDictionary>
// |> Seq.iter (fun o ->printfn "%A" o)

// let typeStrings =
//     bundle.ObjectLookup
//     |> Seq.map (fun o ->
//         use reader = new AssetFileReader(o.Value)

//         let tree = o.Value.serializedType.TypeTree

//         TreeDumper.ReadTypeString(tree, reader))

// typeStrings
// |> Seq.head
// |> printfn "%s"


// let blueprintAssets = BlueprintAssetsContext("BlueprintReferencedAssets.json")

// let sprites =
//     blueprintAssets.refToAsset
//     |> Seq.choose (fun x ->
//         let path = x.Value.m_PathID
//         if bundle.ObjectLookup.Keys |> Seq.contains path then
//             (x.Key, bundle.LookupObject path) |> Some
//         else None)
//     |> Seq.where (fun (_, o) -> o.ClassType = ClassIDType.Sprite)
//     |> Seq.map (fun (id, o) ->
//         use reader = new AssetFileReader(o)
//         let sprite = Sprite reader
//         id, sprite)

// let tryGetSprite sprite =
//     let mutable (maybeBitmap : Bitmap) = null

//     if (BlueprintAssetsContext.TryRenderSprite(sprite, &maybeBitmap)) then
//         Some maybeBitmap
//     else None

// for (id, sprite) in sprites do
//     printfn "%s (%s)" id sprite.Name 
//     tryGetSprite sprite
//     |> function
//     | Some s -> s.Save($"sprites/{id}.png", ImageFormat.Png)
//     | None -> ()
