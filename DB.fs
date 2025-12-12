namespace TextAnalyzer

open System
open Microsoft.Data.Sqlite

module DB =

    let connectionString = "Data Source=text_analyzer.db"
    [<CLIMutable>]
    type AnalysisRecord = {
        mutable Id: int
        mutable FileName: string
        mutable WordCount: int
        mutable SentenceCount: int
        mutable ParagraphCount: int
        mutable TopWord: string
        mutable CreatedAt: string
    }

    let initializeDatabase () =
        use conn = new SqliteConnection(connectionString)
        conn.Open()
        let sqlUsers = """
        CREATE TABLE IF NOT EXISTS Users (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Username TEXT UNIQUE,
            Password TEXT,
            IsAdmin INTEGER
        );"""
        use cmdUsers = new SqliteCommand(sqlUsers, conn)
        cmdUsers.ExecuteNonQuery() |> ignore

        let sqlHistory = """
        CREATE TABLE IF NOT EXISTS History (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            FileName TEXT,
            WordCount INT,
            SentenceCount INT,
            ParagraphCount INT,
            TopWord TEXT,
            CreatedAt TEXT
        );"""
        use cmdHistory = new SqliteCommand(sqlHistory, conn)
        cmdHistory.ExecuteNonQuery() |> ignore

        // default admin
        let sqlAdmin = "INSERT OR IGNORE INTO Users (Username, Password, IsAdmin) VALUES ('doaa', '123', 1);"
        use cmdAdmin = new SqliteCommand(sqlAdmin, conn)
        cmdAdmin.ExecuteNonQuery() |> ignore

    let saveAnalysis fileName wordCount sentenceCount paragraphCount topWord =
        use conn = new SqliteConnection(connectionString)
        conn.Open()

        // Add a debug print to confirm function execution
        printfn "Attempting to save analysis for: %s" fileName

        let sql = """
        INSERT INTO History (FileName, WordCount, SentenceCount, ParagraphCount, TopWord, CreatedAt)
        VALUES (@file, @w, @s, @p, @top, @date)"""  // Use standard '@' parameters

        try
            use cmd = new SqliteCommand(sql, conn)

            // Use standard names and AddWithValue
            cmd.Parameters.AddWithValue("@file", fileName) |> ignore
            cmd.Parameters.AddWithValue("@w", wordCount) |> ignore
            cmd.Parameters.AddWithValue("@s", sentenceCount) |> ignore
            cmd.Parameters.AddWithValue("@p", paragraphCount) |> ignore
            cmd.Parameters.AddWithValue("@top", topWord) |> ignore
            cmd.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")) |> ignore

            // Execute the command and get the number of rows inserted
            let rowsAffected = cmd.ExecuteNonQuery()

            // Add a debug print to confirm rows were inserted
            printfn "Successfully inserted %d row(s) into History." rowsAffected

        with ex ->
            // Catch any exception during the insert process
            printfn "DB Save Error: %s" ex.Message

        // Note: SQLite often operates in autocommit mode, so an explicit commit isn't always needed,
        // but the use of 'use conn' ensures the connection is closed immediately, which is good practice.

    let getAllAnalysis () =
        let results = ResizeArray<AnalysisRecord>()
        use conn = new SqliteConnection(connectionString)
        conn.Open()
        let cmd = conn.CreateCommand()
        cmd.CommandText <- "SELECT Id, FileName, WordCount, SentenceCount, ParagraphCount, TopWord, CreatedAt FROM History"
        use reader = cmd.ExecuteReader()
        while reader.Read() do
            results.Add({
                Id = reader.GetInt32(0)
                FileName = reader.GetString(1)
                WordCount = reader.GetInt32(2)
                SentenceCount = reader.GetInt32(3)
                ParagraphCount = reader.GetInt32(4)
                TopWord = reader.GetString(5)
                CreatedAt = reader.GetString(6)
            })
        results |> Seq.toList

    let validateAdmin username password =
        use conn = new SqliteConnection(connectionString)
        conn.Open()
        let cmd = conn.CreateCommand()
        cmd.CommandText <- "SELECT COUNT(*) FROM Users WHERE Username=$u AND Password=$p AND IsAdmin=1"
        cmd.Parameters.AddWithValue("$u", username) |> ignore
        cmd.Parameters.AddWithValue("$p", password) |> ignore
        let count = cmd.ExecuteScalar() :?> int64
        count > 0
