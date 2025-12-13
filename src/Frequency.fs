namespace TextAnalyzer

module Frequency =

    let topWords (words: string[]) (topN: int) : (string * int)[] =
        words
        |> Array.map (fun (w: string) -> w.ToLower())
        |> Array.groupBy id
        |> Array.map (fun (k,v) -> (k, Array.length v))
        |> Array.sortByDescending snd
        |> Array.truncate topN

