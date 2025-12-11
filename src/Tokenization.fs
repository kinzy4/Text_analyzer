namespace TextAnalyzer

module Tokenization =

    let splitIntoParagraphs (text: string) =
        text.Split([| "\n\n"; "\r\n\r\n" |], System.StringSplitOptions.RemoveEmptyEntries)

    let splitIntoSentences (text: string) =
        text.Split([| '.'; '!'; '?' |], System.StringSplitOptions.RemoveEmptyEntries)

    let splitIntoWords (text: string) =
        text.Split([| ' '; '\n'; '\r'; ','; '.'; '!'; '?' |], System.StringSplitOptions.RemoveEmptyEntries)
        |> Array.filter (fun w -> w <> "")
