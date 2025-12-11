namespace TextAnalyzer

type AnalysisReport = {
    WordCount: int
    SentenceCount: int
    ParagraphCount: int
    TopWords: (string * int) list
    AverageSentenceLength: float
}
