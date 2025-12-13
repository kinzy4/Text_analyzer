open System
open TextAnalyzer.Tokenization
open TextAnalyzer.Metrics
open TextAnalyzer.Frequency

module ManualTests =

    let runTests () =
        printfn "Running Text Analyzer Tests..."

        // Test word count
        let words = splitIntoWords "Hello world!"
        if wordCount words = 2 then printfn "Word count test passed"
        else printfn "Word count test FAILED"

        // Test sentence count
        let sentences = splitIntoSentences "Hello world! How are you?"
        if sentenceCount sentences = 2 then printfn "Sentence count test passed"
        else printfn "Sentence count test FAILED"

        // Test paragraph count
        let paragraphs = splitIntoParagraphs "Para1\n\nPara2\n\nPara3"
        if paragraphCount paragraphs = 3 then printfn "Paragraph count test passed"
        else printfn "Paragraph count test FAILED"

        // Test average sentence length
        let avgLen = averageSentenceLength (Array.length words) (Array.length sentences)
        if abs(avgLen - 2.5) < 0.0001 then printfn "Average sentence length test passed"
        else printfn "Average sentence length test FAILED"

        // Test top words
        let words2 = splitIntoWords "apple banana apple orange banana apple"
        let top = topWords words2 2
        if top = [|("apple",3);("banana",2)|] then printfn "Top words test passed"
        else printfn "Top words test FAILED"
