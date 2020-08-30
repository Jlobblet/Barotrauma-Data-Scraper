// Learn more about F# at http://fsharp.org
open System.IO
open Barotrauma_Data_Scraper.Io
open Barotrauma_Data_Scraper.Constants
open Barotrauma_Data_Scraper.ItemInfoBox

[<EntryPoint>]
let main argv =
    let allFiles =
        Directory.EnumerateFiles(ContentDirectory, "*.xml", SearchOption.AllDirectories)
        |> Seq.toList

    [ @"C:\Program Files (x86)\Steam\steamapps\common\Barotrauma\Content\Items\Weapons\weapons.xml" ]
    |> List.choose (TryParseItemFile)
    |> List.collect (fun i -> i.Items |> Array.toList)
    |> List.map (fun i -> ProduceItemBox i)
    |> List.head
    |> printfn "%s"
    0 // return an integer exit code
