module Barotrauma_Data_Scraper.Io

open Barotrauma_Data_Scraper.Providers
open System.Xml

let TryParseItemFile (filepath: string) =
    try
        Some(ItemFile.Load(filepath))
    with :? XmlException -> None
