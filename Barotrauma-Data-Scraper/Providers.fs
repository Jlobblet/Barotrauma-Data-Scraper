module Barotrauma_Data_Scraper.Providers

open System.IO
open System.Xml.Linq
open FSharp.Data
open Barotrauma_Data_Scraper.Constants

// Type provider for the Vanilla 0.9.xml
// Used to retrieve list of items etc.
type VanillaPackage = XmlProvider<ContentPackage>

// The English texts file
// Used to look up names for items from identifiers
[<Literal>]
let EnglishTextFile =
    ContentDirectory
    + @"\Texts"
    + @"\English"
    + @"\EnglishVanilla.xml"

let private TextDoc = XDocument.Load(EnglishTextFile)

let tryGetNameFromIdentifier (identifier: string) =
    match (TextDoc.Root.Elements()
           |> Seq.tryFind (fun e -> (e.Name.LocalName.Contains(identifier)))
           |> Option.map (fun e -> e.Value)) with
    | Some v -> v
    | None -> identifier

// A file that contains a bunch of items for the type provider.
[<Literal>]
let ExampleItemFile =
    ContentDirectory
    + @"\Items"
    + @"\Weapons"
    + @"\weapons.xml"

// Type provider for a file containing items.
type ItemFile = XmlProvider<ExampleItemFile>
