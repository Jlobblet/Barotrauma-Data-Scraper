module Barotrauma_Data_Scraper.ItemInfoBox

open Barotrauma_Data_Scraper.Constants
open Barotrauma_Data_Scraper.Providers

/// key -> value -> "| key = value"
let private InfoBoxAttribute = sprintf "| %s = %s"

/// item -> "{{Hyperlink|item}}"
let private Hyperlink = sprintf "{{Hyperlink|%s}}"
/// item -> "{{Hyperlink|item name}}"
let private IdHyperlink = tryGetNameFromIdentifier >> Hyperlink

/// Information about an item's fabrication recipe.
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
                                    |> Array.map (fun i ->
                                        i.Identifier
                                        |> IdHyperlink)) ]
                    |> String.concat "\n")) ]
        |> List.choose (fun s -> s)
        |> String.concat "\n")

/// Information about an item's deconstructor recipe.
let private DeconstructInfo (item: ItemFile.Item) =
    item.Deconstruct
    |> Option.map (fun d ->
        [
          // Item can be deconstructed
          // | deconstructor = yes
          Some(InfoBoxAttribute "deconstructor" "yes")
          // Time to deconstruct
          // | deconstructortime = 10
          Some(InfoBoxAttribute "deconstructortime" (d.Time |> string))
          // Items deconstructed into
          Some
              (InfoBoxAttribute
                  "deconstructormaterials"
                   (d.Items
                    |> Array.map (fun i ->
                        i.Identifier
                        |> IdHyperlink)
                    |> String.concat "\n")) ]
        |> List.choose (fun s -> s)
        |> String.concat "\n")

/// Information about an item's price.
let private PriceInfo (item: ItemFile.Item) =
    item.Price
    |> Option.map (fun p ->
        InfoBoxAttribute
            "price"
            (p.Prices
             // Multiple base price by multiplier for price
             |> Array.map (fun p2 -> (p2.Locationtype, p2.Multiplier * (p.Baseprice |> decimal)))
             // Sort in the order research/military/city/outpost/mine
             |> Array.sortBy (fun (s, _) -> LocationTypeOrder.Item s)
             // Get just the price instead of the location name and price
             |> Array.map (fun (_, p) -> p |> round |> string)
             |> String.concat "/"))

/// Produce an item WikiBox for a given item
let ProduceItemBox (item: ItemFile.Item) =
    [ Some "{{Items infobox"
      Some (InfoBoxAttribute "name" (tryGetNameFromIdentifier item.Identifier))
      PriceInfo item
      FabricationInfo item
      DeconstructInfo item
      Some "}}" ]
    |> List.choose (fun s -> s)
    |> String.concat "\n"
