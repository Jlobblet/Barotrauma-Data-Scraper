module Barotrauma_Data_Scraper.ItemInfoBox

open Barotrauma_Data_Scraper.Constants
open Barotrauma_Data_Scraper.Providers

// key -> value -> | key = value
let private InfoBoxAttribute = sprintf "| %s = %s"

// item -> {{Hyperlink|item|}}
let private Hyperlink = sprintf "{{Hyperlink|%s}}"

let ProduceItemBox (item: ItemFile.Item) =
    [ Some "{{Items infobox"
      Some "}}" ]
    |> List.choose (fun s -> s)
    |> String.concat "\n"
