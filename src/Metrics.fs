namespace TextAnalyzer

module Metrics =

    let wordCount words = Array.length words
    let sentenceCount sentences = Array.length sentences
    let paragraphCount paragraphs = Array.length paragraphs

    let averageSentenceLength words sentences =
        if sentences = 0 then 0.0 else float words / float sentences

    // Placeholder for readability formula
    let fleschReadingEase words sentences syllables =
        206.835 - 1.015 * (float words / float sentences) - 84.6 * (float syllables / float words)
