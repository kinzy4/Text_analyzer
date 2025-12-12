open System
open System.Windows.Forms
open TextAnalyzer
open TextAnalyzer.InputHandling
open TextAnalyzer.Tokenization
open TextAnalyzer.Metrics
open TextAnalyzer.Frequency
open TextAnalyzer.FileIO
open TextAnalyzer.DB
open TextAnalyzer.UI

[<STAThread>]
[<EntryPoint>]
let main argv =
    // Initialize the database
    DB.initializeDatabase()

    // Analysis pipeline
    let analyze text =
        let paragraphs = splitIntoParagraphs text
        let sentences = splitIntoSentences text
        let words = splitIntoWords text

        {
            WordCount = wordCount words
            SentenceCount = sentenceCount sentences
            ParagraphCount = paragraphCount paragraphs
            TopWords = topWords words 10 |> Array.toList
            AverageSentenceLength = averageSentenceLength (Array.length words) (Array.length sentences)
        }

    // Start UI with web option
    UI.startUI analyze

    0
