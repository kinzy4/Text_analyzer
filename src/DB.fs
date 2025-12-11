namespace TextAnalyzer

open System

module DB =
    open Microsoft.Data.Sqlite

    let connectionString = "Data Source=text_analyzer.db"

    let initializeDatabase () =
        use conn = new SqliteConnection(connectionString)
        conn.Open()

        let sql = """
        CREATE TABLE IF NOT EXISTS AnalysisHistoryy (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            FileName TEXT,
            Text TEXT,
            WordCount INT,
            SentenceCount INT,
            ParagraphCount INT,
            TopWord TEXT,
            AverageSentenceLength REAL,
            CreatedAt TEXT
        );
        """

        use cmd = new SqliteCommand(sql, conn)
        cmd.ExecuteNonQuery() |> ignore



    let saveAnalysis fileName text wordCount sentenceCount paragraphCount topWord avgSentenceLength =
        use conn = new SqliteConnection(connectionString)
        conn.Open()

        let sql = """
        INSERT INTO AnalysisHistoryy (FileName, Text, WordCount, SentenceCount, ParagraphCount, TopWord, AverageSentenceLength, CreatedAt)
        VALUES ($file, $text, $w, $s, $p, $top, $avg, $date)
        """

        use cmd = new SqliteCommand(sql, conn)
        cmd.Parameters.AddWithValue("$file", fileName) |> ignore
        cmd.Parameters.AddWithValue("$text", text) |> ignore
        cmd.Parameters.AddWithValue("$w", wordCount) |> ignore
        cmd.Parameters.AddWithValue("$s", sentenceCount) |> ignore
        cmd.Parameters.AddWithValue("$p", paragraphCount) |> ignore
        cmd.Parameters.AddWithValue("$top", topWord) |> ignore
        cmd.Parameters.AddWithValue("$avg", avgSentenceLength) |> ignore
        cmd.Parameters.AddWithValue("$date", DateTime.Now.ToString()) |> ignore

        cmd.ExecuteNonQuery() |> ignore

    type AnalysisRecord = {
        Id: int
        FileName: string
        Text: string
        WordCount: int
        SentenceCount: int
        ParagraphCount: int
        TopWord: string
        AverageSentenceLength: float
        CreatedAt: string
    }


    let getAllAnalysis () =
        let results = ResizeArray<AnalysisRecord>()
        use conn = new SqliteConnection(connectionString)
        conn.Open()
        let cmd = conn.CreateCommand()
        cmd.CommandText <- "SELECT Id, FileName, Text, WordCount, SentenceCount, ParagraphCount, TopWord, AverageSentenceLength, CreatedAt FROM AnalysisHistoryy"
        use reader = cmd.ExecuteReader()
        while reader.Read() do
            results.Add({
                Id = reader.GetInt32(0)
                FileName = reader.GetString(1)
                Text = reader.GetString(2)
                WordCount = reader.GetInt32(3)
                SentenceCount = reader.GetInt32(4)
                ParagraphCount = reader.GetInt32(5)
                TopWord = reader.GetString(6)
                AverageSentenceLength = reader.GetDouble(7)
                CreatedAt = reader.GetString(8)
            })
        conn.Close()
        results |> Seq.toList



