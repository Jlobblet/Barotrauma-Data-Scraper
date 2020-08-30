module Barotrauma_Data_Scraper.ItemInfoBox

open Barotrauma_Data_Scraper.Constants
open Barotrauma_Data_Scraper.Providers

// key -> value -> | key = value
let private InfoBoxAttribute = sprintf "| %s = %s"

// item -> {{Hyperlink|item|}}
let private Hyperlink = sprintf "{{Hyperlink|%s}}"

// Information about an item's fabrication recipe.
let private FabricationInfo (item: ItemFile.Item) =
    item.Fabricate
    |> Option.map (fun f ->
        [
          // Item can be fabricated
          // | fabricate = yes
          Some(InfoBoxAttribute "fabricate" "yes")
          // Item's required skill to fabricate, if any
          // | fabricatorskill = mechanical
          f.RequiredSkill
          |> Option.map (fun s -> InfoBoxAttribute "fabricatorskill" s.Identifier)
          // The required skill level
          // | fabricatorskilllevel = 30
          f.RequiredSkill
          |> Option.map (fun s -> InfoBoxAttribute "fabricatorskilllevel" (s.Level |> string))
          // The required time, if any
          // | fabricatortime = 20
          Some(InfoBoxAttribute "fabricatortime" (f.Requiredtime |> string))
          // The required materials to fabricate
          Some
              (InfoBoxAttribute
                  "fabricatormaterials"
                   // Some have items, some have requireditems...
                   (Array.concat [ (f.Items |> Array.map (fun i -> i.Identifier))
                                   (f.RequiredItems
                                    |> Array.map (fun i -> i.Identifier |> tryGetNameFromIdentifier |> Hyperlink)) ]
                    |> String.concat "\n")) ]
        |> List.choose (fun s -> s)
        |> String.concat "\n")

let ProduceItemBox (item: ItemFile.Item) =
    [ Some "{{Items infobox"
      FabricationInfo item
      Some "}}" ]
    |> List.choose (fun s -> s)
    |> String.concat "\n"
