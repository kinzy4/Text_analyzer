namespace TextAnalyzer

open System.IO

module InputHandling =

    let loadTextFromFile (path: string) =
        if File.Exists path then
            File.ReadAllText path
        else
            failwith "File not found"

    let getManualInput () =
        printfn "Enter text (finish with an empty line):"
        let rec loop acc =
            let line = System.Console.ReadLine()
            if System.String.IsNullOrWhiteSpace(line) then
                String.concat "\n" (List.rev acc)
            else
                loop (line::acc)
        loop []
