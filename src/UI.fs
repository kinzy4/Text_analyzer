namespace TextAnalyzer

open System
open System.Windows.Forms
open TextAnalyzer
open TextAnalyzer.FileIO
open TextAnalyzer.DB

module UI =

    let startUI analyzeFunc =
        let form = new Form(Text = "Text Analyzer", Width=600, Height=500)

        // Input textbox
        let textBox = new TextBox(Multiline=true, Width=550, Height=200, Top=10, Left=10)

        // Results textbox
        let resultsBox = new TextBox(Multiline=true, Width=550, Height=120, Top=260, Left=10, ReadOnly=true)

        // Analyze button
        let analyzeButton = new Button(Text="Analyze", Top=220, Left=10)
        analyzeButton.Click.Add(fun _ ->
            try
                let report = analyzeFunc textBox.Text

                // Save to database
                let topWord = if report.TopWords.Length > 0 then fst report.TopWords.[0] else ""
                DB.saveAnalysis("UserInput", report.WordCount, report.SentenceCount, report.ParagraphCount, topWord)

                // Show results
                resultsBox.Text <-
                    "Words: " + report.WordCount.ToString() + Environment.NewLine +
                    "Sentences: " + report.SentenceCount.ToString() + Environment.NewLine +
                    "Paragraphs: " + report.ParagraphCount.ToString() + Environment.NewLine +
                    "Top Words: " + (report.TopWords |> List.map (fun (w,c) -> w + " (" + c.ToString() + ")") |> String.concat ", ") + Environment.NewLine +
                    "Avg Sentence Length: " + report.AverageSentenceLength.ToString("F2")
                let firstTopWord = if report.TopWords.Length > 0 then fst report.TopWords.Head else "none"
                DB.saveAnalysis "manual_input" textBox.Text report.WordCount report.SentenceCount report.ParagraphCount firstTopWord report.AverageSentenceLength
            with ex ->
                MessageBox.Show("Error: " + ex.Message) |> ignore
        )

        // Export JSON button
        let exportButton = new Button(Text="Export JSON", Top=220, Left=120)
        exportButton.Click.Add(fun _ ->
            try
                let report = analyzeFunc textBox.Text
                let fileName =
                    let firstWord = textBox.Text.Split([|' '; '\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries)
                                    |> Array.tryHead
                    match firstWord with
                    | Some w -> w + ".json"
                    | None -> "report.json"
                FileIO.exportToJson report fileName
                MessageBox.Show($"Report saved as {fileName}") |> ignore
            with ex ->
                MessageBox.Show("Error: " + ex.Message) |> ignore
        )

        // Load File button
        let loadButton = new Button(Text="Load File", Top=220, Left=230)
        loadButton.Click.Add(fun _ ->
            try
                use dialog = new OpenFileDialog()
                dialog.Filter <- "Text Files|*.txt"
                if dialog.ShowDialog() = DialogResult.OK then
                    textBox.Text <- System.IO.File.ReadAllText(dialog.FileName)
            with ex ->
                MessageBox.Show("Error: " + ex.Message) |> ignore
        )

        // View History button
        let historyButton = new Button(Text="View History", Top=220, Left=340)
        historyButton.Click.Add(fun _ ->
            try
                let records = DB.getAllAnalysis()
                if records.Length = 0 then
                    MessageBox.Show("No analysis history found.") |> ignore
                else
                    let text =
                        records
                        |> List.map (fun r ->
                            sprintf "Id=%d, Words=%d, Sentences=%d, Paragraphs=%d, AvgLen=%.2f, Text=%s"
                                r.Id r.WordCount r.SentenceCount r.ParagraphCount r.AverageSentenceLength r.Text
                        )
                        |> String.concat Environment.NewLine
                    MessageBox.Show(text, "Analysis History") |> ignore
            with ex ->
                MessageBox.Show("Error: " + ex.Message) |> ignore
        )

        form.Controls.AddRange([| textBox; analyzeButton; exportButton; loadButton; historyButton; resultsBox |])
        Application.Run(form)
