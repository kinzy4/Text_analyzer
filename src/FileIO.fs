namespace TextAnalyzer

open System.IO
open Newtonsoft.Json

module FileIO =

    let exportToJson (report: AnalysisReport) (path: string) =
        let json = JsonConvert.SerializeObject(report, Formatting.Indented)
        File.WriteAllText(path, json)
