namespace TextAnalyzer

open System
open System.Windows.Forms
open TextAnalyzer
open TextAnalyzer.DB
open TextAnalyzer.FileIO
open System.ComponentModel
open System.Data
open System.IO
open System.Net
open System.Threading

module UI =

    // ---------------------------
    // HTML CONTENT GENERATION
    // ---------------------------
    let private getMainPage() : string =
        """
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Text Analyzer </title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            min-height: 100vh;
            padding: 20px;
        }

        .container {
            max-width: 1200px;
            margin: 0 auto;
        }

        .header {
            text-align: center;
            color: white;
            margin-bottom: 30px;
            padding: 20px;
        }

        .header h1 {
            font-size: 2.5rem;
            margin-bottom: 10px;
            text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
        }

        .header p {
            font-size: 1.1rem;
            opacity: 0.9;
        }

        .card {
            background: white;
            border-radius: 15px;
            padding: 30px;
            margin-bottom: 30px;
            box-shadow: 0 20px 40px rgba(0,0,0,0.1);
        }

        .input-section {
            margin-bottom: 30px;
        }

        .text-area {
            width: 100%;
            min-height: 200px;
            padding: 20px;
            border: 2px solid #e0e0e0;
            border-radius: 10px;
            font-size: 16px;
            resize: vertical;
            transition: border-color 0.3s;
        }

        .text-area:focus {
            outline: none;
            border-color: #667eea;
        }

        .button-group {
            display: flex;
            gap: 15px;
            margin-top: 20px;
            flex-wrap: wrap;
        }

        .btn {
            padding: 12px 24px;
            border: none;
            border-radius: 8px;
            font-size: 16px;
            font-weight: 600;
            cursor: pointer;
            transition: all 0.3s;
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .btn-primary {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }

        .btn-secondary {
            background: #f0f0f0;
            color: #333;
        }

        .btn-success {
            background: linear-gradient(135deg, #11998e 0%, #38ef7d 100%);
            color: white;
        }

        .btn-danger {
            background: linear-gradient(135deg, #ff416c 0%, #ff4b2b 100%);
            color: white;
        }

        .btn:hover {
            transform: translateY(-2px);
            box-shadow: 0 10px 20px rgba(0,0,0,0.1);
        }

        .results-section {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 20px;
            margin-bottom: 30px;
        }

        .metric-card {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 20px;
            border-radius: 10px;
            text-align: center;
        }

        .metric-value {
            font-size: 2.5rem;
            font-weight: bold;
            margin: 10px 0;
        }

        .metric-label {
            font-size: 0.9rem;
            opacity: 0.9;
        }

        .top-words {
            display: flex;
            flex-wrap: wrap;
            gap: 10px;
            margin-top: 20px;
        }

        .word-badge {
            background: rgba(255,255,255,0.2);
            padding: 8px 16px;
            border-radius: 20px;
            font-size: 14px;
        }

        .history-section {
            max-height: 400px;
            overflow-y: auto;
        }

        .history-table {
            width: 100%;
            border-collapse: collapse;
        }

        .history-table th {
            background: #f5f5f5;
            padding: 15px;
            text-align: left;
            font-weight: 600;
            border-bottom: 2px solid #e0e0e0;
        }

        .history-table td {
            padding: 15px;
            border-bottom: 1px solid #e0e0e0;
        }

        .history-table tr:hover {
            background: #f9f9f9;
        }

        .modal {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            justify-content: center;
            align-items: center;
            z-index: 1000;
        }

        .modal-content {
            background: white;
            padding: 30px;
            border-radius: 15px;
            max-width: 400px;
            width: 90%;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: 600;
        }

        .form-control {
            width: 100%;
            padding: 10px;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 16px;
        }

        .loading {
            display: none;
            text-align: center;
            padding: 20px;
        }

        .spinner {
            width: 40px;
            height: 40px;
            border: 4px solid #f3f3f3;
            border-top: 4px solid #667eea;
            border-radius: 50%;
            animation: spin 1s linear infinite;
            margin: 0 auto;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }

        @media (max-width: 768px) {
            .button-group {
                flex-direction: column;
            }
            
            .btn {
                width: 100%;
                justify-content: center;
            }
            
            .results-section {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 480px) {
            .results-section {
                grid-template-columns: 1fr;
            }
            
            .header h1 {
                font-size: 2rem;
            }
        }
    </style>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css">
</head>
<body>
    <div class="container">
        <div class="header">
            <h1><i class="fas fa-chart-bar"></i> Text Analyzer </h1>
        </div>

        <div class="card">
            <div class="input-section">
                <h2><i class="fas fa-pencil-alt"></i> Enter Your Text</h2>
                <textarea id="textInput" class="text-area" placeholder="Paste or type your text here..."></textarea>
                
                <div class="button-group">
                    <button onclick="analyzeText()" class="btn btn-primary">
                        <i class="fas fa-chart-line"></i> Analyze Text
                    </button>
                    <button onclick="loadFile()" class="btn btn-secondary">
                        <i class="fas fa-file-upload"></i> Load File
                    </button>
                    <button onclick="exportJson()" class="btn btn-success">
                        <i class="fas fa-download"></i> Export JSON
                    </button>
                    <button onclick="showHistory()" class="btn btn-danger">
                        <i class="fas fa-history"></i> View History
                    </button>
                    <button onclick="clearText()" class="btn btn-secondary">
                        <i class="fas fa-trash"></i> Clear
                    </button>
                </div>
            </div>

            <div class="loading" id="loading">
                <div class="spinner"></div>
                <p>Processing your text...</p>
            </div>

            <div id="results" style="display: none;">
                <h2><i class="fas fa-chart-pie"></i> Analysis Results</h2>
                
                <div class="results-section" id="metrics">
                    <!-- Metrics will be inserted here -->
                </div>

                <h3><i class="fas fa-star"></i> Top Words</h3>
                <div class="top-words" id="topWords">
                    <!-- Top words will be inserted here -->
                </div>
            </div>
        </div>

        <div class="card history-section" id="historyCard" style="display: none;">
            <h2><i class="fas fa-database"></i> Analysis History</h2>
            <table class="history-table" id="historyTable">
                <thead>
                    <tr>
                        <th>File Name</th>
                        <th>Words</th>
                        <th>Sentences</th>
                        <th>Paragraphs</th>
                        <th>Top Word</th>
                        <th>Date</th>
                    </tr>
                </thead>
                <tbody id="historyBody">
                    <!-- History rows will be inserted here -->
                </tbody>
            </table>
        </div>
    </div>

    <!-- Login Modal -->
    <div class="modal" id="loginModal">
        <div class="modal-content">
            <h2><i class="fas fa-lock"></i> Admin Login</h2>
            <div class="form-group">
                <label for="username">Username</label>
                <input type="text" id="username" class="form-control" placeholder="Enter username">
            </div>
            <div class="form-group">
                <label for="password">Password</label>
                <input type="password" id="password" class="form-control" placeholder="Enter password">
            </div>
            <div class="button-group">
                <button onclick="performLogin()" class="btn btn-primary">
                    <i class="fas fa-sign-in-alt"></i> Login
                </button>
                <button onclick="hideLoginModal()" class="btn btn-secondary">Cancel</button>
            </div>
        </div>
    </div>

    <script>
        function analyzeText() {
            const text = document.getElementById('textInput').value;
            if (!text.trim()) {
                alert('Please enter some text to analyze.');
                return;
            }

            showLoading(true);
            
            fetch('/analyze', {
                method: 'POST',
                body: text,
                headers: {
                    'Content-Type': 'text/plain'
                }
            })
            .then(response => response.json())
            .then(data => {
                displayResults(data);
                showLoading(false);
            })
            .catch(error => {
                console.error('Error:', error);
                showLoading(false);
                alert('Error analyzing text. Please try again.');
            });
        }

        function displayResults(data) {
            const metricsDiv = document.getElementById('metrics');
            const topWordsDiv = document.getElementById('topWords');
            
            // Clear previous results
            metricsDiv.innerHTML = '';
            topWordsDiv.innerHTML = '';
            
            // Display metrics
            const metrics = [
                { label: 'Words', value: data.wordCount, icon: 'fas fa-font' },
                { label: 'Sentences', value: data.sentenceCount, icon: 'fas fa-paragraph' },
                { label: 'Paragraphs', value: data.paragraphCount, icon: 'fas fa-align-left' },
                { label: 'Avg Sentence Length', value: data.averageSentenceLength.toFixed(1), icon: 'fas fa-ruler' }
            ];
            
            metrics.forEach(metric => {
                metricsDiv.innerHTML += `
                    <div class="metric-card">
                        <i class="${metric.icon}"></i>
                        <div class="metric-value">${metric.value}</div>
                        <div class="metric-label">${metric.label}</div>
                    </div>
                `;
            });
            
            // Display top words
            if (data.topWords && data.topWords.length > 0) {
                data.topWords.forEach(word => {
                    topWordsDiv.innerHTML += `
                        <div class="word-badge">
                            <strong>${word.word}</strong> (${word.count})
                        </div>
                    `;
                });
            }
            
            // Show results section
            document.getElementById('results').style.display = 'block';
        }

        function loadFile() {
            const input = document.createElement('input');
            input.type = 'file';
            input.accept = '.txt';
            
            input.onchange = function(event) {
                const file = event.target.files[0];
                if (file) {
                    const reader = new FileReader();
                    reader.onload = function(e) {
                        document.getElementById('textInput').value = e.target.result;
                    };
                    reader.readAsText(file);
                }
            };
            
            input.click();
        }

        function exportJson() {
            const text = document.getElementById('textInput').value;
            if (!text.trim()) {
                alert('Please enter some text to export.');
                return;
            }

            fetch('/export', {
                method: 'POST',
                body: text,
                headers: {
                    'Content-Type': 'text/plain'
                }
            })
            .then(response => response.text())
            .then(message => {
                alert(message);
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Error exporting JSON.');
            });
        }

        function showHistory() {
            document.getElementById('loginModal').style.display = 'flex';
        }

        function hideLoginModal() {
            document.getElementById('loginModal').style.display = 'none';
        }

        function performLogin() {
            const username = document.getElementById('username').value;
            const password = document.getElementById('password').value;
            
            if (!username || !password) {
                alert('Please enter both username and password.');
                return;
            }

            fetch('/login', {
                method: 'POST',
                body: `username=${encodeURIComponent(username)}&password=${encodeURIComponent(password)}`,
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            })
            .then(response => response.text())
            .then(result => {
                if (result === 'success') {
                    hideLoginModal();
                    loadHistory();
                } else {
                    alert('Invalid admin credentials.');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Error during login.');
            });
        }

        function loadHistory() {
            fetch('/history')
            .then(response => response.json())
            .then(data => {
                const historyBody = document.getElementById('historyBody');
                historyBody.innerHTML = '';
                
                if (data.length === 0) {
                    historyBody.innerHTML = '<tr><td colspan="6" style="text-align: center;">No history found</td></tr>';
                } else {
                    data.forEach(item => {
                        const date = new Date(item.createdAt);
                        historyBody.innerHTML += `
                            <tr>
                                <td>${item.fileName}</td>
                                <td>${item.wordCount}</td>
                                <td>${item.sentenceCount}</td>
                                <td>${item.paragraphCount}</td>
                                <td>${item.topWord}</td>
                                <td>${date.toLocaleDateString()} ${date.toLocaleTimeString()}</td>
                            </tr>
                        `;
                    });
                }
                
                document.getElementById('historyCard').style.display = 'block';
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Error loading history.');
            });
        }

        function clearText() {
            if (confirm('Are you sure you want to clear the text?')) {
                document.getElementById('textInput').value = '';
                document.getElementById('results').style.display = 'none';
                document.getElementById('historyCard').style.display = 'none';
            }
        }

        function showLoading(show) {
            document.getElementById('loading').style.display = show ? 'block' : 'none';
        }

        // Add some sample text for demo purposes
        window.onload = function() {
            const sampleText = `Text Analyzer  is a  tool that helps you understand your writing.
The tool is perfect for writers, students, and professionals who want to improve their writing skills.`;
            
            document.getElementById('textInput').value = sampleText;
        };
    </script>
</body>
</html>
        """

    // ---------------------------
    // ADMIN LOGIN DIALOG (For desktop version)
    // ---------------------------
    let private showAdminLoginDialog () =
        let loginForm = new Form(Text="Admin Login", Width=300, Height=200, FormBorderStyle=FormBorderStyle.FixedDialog)
        loginForm.StartPosition <- FormStartPosition.CenterScreen

        let userLabel = new Label(Text="Username:", Top=20, Left=20)
        let userBox = new TextBox(Top=40, Left=20, Width=240)

        let passLabel = new Label(Text="Password:", Top=80, Left=20)
        let passBox = new TextBox(Top=100, Left=20, Width=240, UseSystemPasswordChar=true)

        let loginButton = new Button(Text="Login", Top=130, Left=20, Width=240)

        let mutable result = false

        loginButton.Click.Add(fun _ ->
            if DB.validateAdmin userBox.Text passBox.Text then
                result <- true
                loginForm.Close()
            else
                MessageBox.Show("Invalid admin credentials.") |> ignore
        )

        loginForm.Controls.AddRange [| userLabel; userBox; passLabel; passBox; loginButton |]
        loginForm.ShowDialog() |> ignore

        result

    // ---------------------------
    // HISTORY DASHBOARD (For desktop version)
    // ---------------------------
    let private showHistoryDashboard() =
        let data = DB.getAllAnalysis()

        if data |> List.isEmpty then
            // If the table is empty, show a message instead of an empty dashboard
            MessageBox.Show("No analysis history found in the database.") |> ignore
        else
            // Only show the dashboard if there is data
            let form = new Form(Text="Analysis Dashboard", Width=800, Height=400)
            let grid = new DataGridView(Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill)

            let dt = new DataTable()
            dt.Columns.Add("Id", typeof<int>) |> ignore
            dt.Columns.Add("FileName", typeof<string>) |> ignore
            dt.Columns.Add("WordCount", typeof<int>) |> ignore
            dt.Columns.Add("SentenceCount", typeof<int>) |> ignore
            dt.Columns.Add("ParagraphCount", typeof<int>) |> ignore
            dt.Columns.Add("TopWord", typeof<string>) |> ignore
            dt.Columns.Add("CreatedAt", typeof<string>) |> ignore

            data |> List.iter (fun r ->
                dt.Rows.Add([| box r.Id; box r.FileName; box r.WordCount; box r.SentenceCount; box r.ParagraphCount; box r.TopWord; box r.CreatedAt |]) |> ignore
            )
            grid.DataSource <- dt

            form.Controls.Add(grid)
            form.ShowDialog() |> ignore

    // ---------------------------
    // MODERN WEB SERVER
    // ---------------------------
    let private startWebServer (analyzeFunc: string -> AnalysisReport) (port: int) =
        let listener = new HttpListener()
        listener.Prefixes.Add($"http://localhost:{port}/")
        listener.Start()
        printfn $"Web server started at http://localhost:{port}/"

        let handleRequest (context: HttpListenerContext) =
            async {
                try
                    let request = context.Request
                    let response = context.Response

                    match request.HttpMethod, request.Url.LocalPath with
                    | "GET", "/" ->
                        // Serve main page
                        let html = getMainPage()
                        let buffer = System.Text.Encoding.UTF8.GetBytes(html : string)
                        response.ContentType <- "text/html"
                        response.ContentLength64 <- int64 buffer.Length
                        do! response.OutputStream.WriteAsync(buffer, 0, buffer.Length) |> Async.AwaitTask
                    
                    | "POST", "/analyze" ->
                        // Handle text analysis
                        use reader = new StreamReader(request.InputStream)
                        let text = reader.ReadToEnd()
                        let report = analyzeFunc text
                        
                        // Save to database
                        let fileName =
                            match text.Split([|' '; '\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries) |> Array.tryHead with
                            | Some w -> w
                            | None -> "UserInput"
                        let topWord = if report.TopWords.Length > 0 then fst report.TopWords.[0] else ""
                        
                        DB.saveAnalysis fileName report.WordCount report.SentenceCount report.ParagraphCount topWord |> ignore
                        
                        // Return JSON response
                        let jsonResponse = 
                            sprintf """{
                                "wordCount": %d,
                                "sentenceCount": %d,
                                "paragraphCount": %d,
                                "averageSentenceLength": %.2f,
                                "topWords": [%s]
                            }""" 
                                report.WordCount 
                                report.SentenceCount 
                                report.ParagraphCount 
                                report.AverageSentenceLength
                                (report.TopWords |> List.map (fun (w,c) -> sprintf """{"word": "%s", "count": %d}""" w c) |> String.concat ",")
                        
                        let buffer = System.Text.Encoding.UTF8.GetBytes(jsonResponse : string)
                        response.ContentType <- "application/json"
                        response.ContentLength64 <- int64 buffer.Length
                        do! response.OutputStream.WriteAsync(buffer, 0, buffer.Length) |> Async.AwaitTask
                    
                    | "POST", "/export" ->
                        // Handle export to JSON
                        use reader = new StreamReader(request.InputStream)
                        let text = reader.ReadToEnd()
                        let report = analyzeFunc text
                        
                        let fileName =
                            match text.Split([|' '; '\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries) |> Array.tryHead with
                            | Some w -> w + ".json"
                            | None -> "report.json"
                        
                        FileIO.exportToJson report fileName
                        
                        let responseText = sprintf "Report exported to %s" fileName
                        let buffer = System.Text.Encoding.UTF8.GetBytes(responseText : string)
                        response.ContentType <- "text/plain"
                        response.ContentLength64 <- int64 buffer.Length
                        do! response.OutputStream.WriteAsync(buffer, 0, buffer.Length) |> Async.AwaitTask
                    
                    | "POST", "/login" ->
                        // Handle admin login
                        use reader = new StreamReader(request.InputStream)
                        let data = reader.ReadToEnd()
                        let parts = data.Split('&')
                        let username = if parts.Length > 0 then parts.[0].Split('=').[1] else ""
                        let password = if parts.Length > 1 then parts.[1].Split('=').[1] else ""
                        
                        let isValid = DB.validateAdmin username password
                        
                        let responseText = if isValid then "success" else "failure"
                        let buffer = System.Text.Encoding.UTF8.GetBytes(responseText : string)
                        response.ContentType <- "text/plain"
                        response.ContentLength64 <- int64 buffer.Length
                        do! response.OutputStream.WriteAsync(buffer, 0, buffer.Length) |> Async.AwaitTask
                    
                    | "GET", "/history" ->
                        // Get analysis history
                        let data = DB.getAllAnalysis()
                        
                        let jsonData = 
                            data 
                            |> List.map (fun r ->
                                sprintf """{
                                    "id": %d,
                                    "fileName": "%s",
                                    "wordCount": %d,
                                    "sentenceCount": %d,
                                    "paragraphCount": %d,
                                    "topWord": "%s",
                                    "createdAt": "%s"
                                }""" r.Id r.FileName r.WordCount r.SentenceCount r.ParagraphCount r.TopWord r.CreatedAt)
                            |> String.concat ","
                        
                        let jsonResponse = sprintf "[%s]" jsonData
                        let buffer = System.Text.Encoding.UTF8.GetBytes(jsonResponse : string)
                        response.ContentType <- "application/json"
                        response.ContentLength64 <- int64 buffer.Length
                        do! response.OutputStream.WriteAsync(buffer, 0, buffer.Length) |> Async.AwaitTask
                    
                    | _ ->
                        // 404 for unknown routes
                        response.StatusCode <- 404
                        let buffer = System.Text.Encoding.UTF8.GetBytes("Not Found" : string)
                        response.ContentLength64 <- int64 buffer.Length
                        do! response.OutputStream.WriteAsync(buffer, 0, buffer.Length) |> Async.AwaitTask
                    
                    response.OutputStream.Close()
                with ex ->
                    printfn "Error handling request: %s" ex.Message
            }
        
        // Start async listener
        let rec listen() =
            async {
                let! context = listener.GetContextAsync() |> Async.AwaitTask
                do! handleRequest context
                return! listen()
            }
        
        Async.Start(listen())
        
        // Open browser
        try
            System.Diagnostics.Process.Start($"http://localhost:{port}/") |> ignore
        with ex ->
            printfn "Could not open browser automatically. Please navigate to http://localhost:%d/ manually." port
        
        // Return the listener so it can be stopped later
        listener

    // ---------------------------
    // MAIN UI FUNCTION
    // ---------------------------
    let startUI analyzeFunc =
        DB.initializeDatabase()
        
        // Ask user if they want web or desktop version
        let result = MessageBox.Show(
            "Would you like to use the modern web interface? (Yes) or classic desktop interface? (No)",
            "Text Analyzer",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        )
        
        if result = DialogResult.Yes then
            // Start web server on port 8080
            let listener = startWebServer analyzeFunc 8080
            
            MessageBox.Show(
                "Web server started at http://localhost:8080/\n\n" +
                "Your browser should open automatically. If not, please visit the URL manually.\n\n" +
                "Click OK to stop the server when finished.",
                "Text Analyzer Web Interface",
                MessageBoxButtons.OK
            ) |> ignore
            
            // Stop the server
            listener.Stop()
            listener.Close()
        else
            // Fall back to original desktop interface
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
                    let fileName =
                        match textBox.Text.Split([|' '; '\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries) |> Array.tryHead with
                        | Some w -> w
                        | None -> "UserInput"
                    let topWord = if report.TopWords.Length > 0 then fst report.TopWords.[0] else ""

                    (DB.saveAnalysis fileName report.WordCount report.SentenceCount report.ParagraphCount topWord) |> ignore

                    resultsBox.Text <-
                        "Words: " + report.WordCount.ToString() + Environment.NewLine +
                        "Sentences: " + report.SentenceCount.ToString() + Environment.NewLine +
                        "Paragraphs: " + report.ParagraphCount.ToString() + Environment.NewLine +
                        "Top Words: " + (report.TopWords |> List.map (fun (w,c) -> w + " (" + c.ToString() + ")") |> String.concat ", ") + Environment.NewLine +
                        "Avg Sentence Length: " + report.AverageSentenceLength.ToString("F2")
                with ex ->
                    MessageBox.Show("Error: " + ex.Message) |> ignore
            )

            // Export JSON button
            let exportButton = new Button(Text="Export JSON", Top=220, Left=120)
            exportButton.Click.Add(fun _ ->
                try
                    let report = analyzeFunc textBox.Text
                    let fileName =
                        match textBox.Text.Split([|' '; '\n'; '\r'|], StringSplitOptions.RemoveEmptyEntries) |> Array.tryHead with
                        | Some w -> w + ".json"
                        | None -> "report.json"

                    FileIO.exportToJson report fileName
                    MessageBox.Show($"Report saved as {fileName}") |> ignore
                with ex ->
                    MessageBox.Show("Error: " + ex.Message) |> ignore
            )

            // Load file button
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

            // HISTORY button (requires admin login)
            let historyButton = new Button(Text="History", Top=220, Left=340)
            historyButton.Click.Add(fun _ ->
                try
                    let ok = showAdminLoginDialog()
                    if ok then
                       showHistoryDashboard() |> ignore
                    else
                       ()
                with ex ->
                    MessageBox.Show("Error: " + ex.Message) |> ignore
            )

            form.Controls.AddRange([|
                textBox; analyzeButton; exportButton; loadButton; historyButton; resultsBox
            |])

            Application.Run(form)