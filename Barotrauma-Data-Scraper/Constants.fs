module Barotrauma_Data_Scraper.Constants

open System.IO

/// Shame this can't be a compile time constant.
let (+/) path1 path2 = Path.Combine(path1, path2)

/// Directory where barotrauma is installed.
[<Literal>]
let BaroDirectory =
    @"C:\Program Files (x86)\Steam\steamapps\common\Barotrauma"

/// Content directory.
[<Literal>]
let ContentDirectory = BaroDirectory + @"\Content"

/// Vanilla 0.9.xml content package.
[<Literal>]
let ContentPackage =
    BaroDirectory
    + @"\Data"
    + @"\ContentPackages"
    + @"\Vanilla 0.9.xml"

/// Order to sort item prices by
let LocationTypeOrder =
    [ "research", 0
      "military", 1
      "city", 2
      "outpost", 3
      "mine", 4 ]
    |> Map.ofList
