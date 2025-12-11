# Text Analyzer

A **Text Analyzer** application written in F# that allows users to analyze text for various metrics such as word count, sentence count, paragraph count, top words, and average sentence length. The application features a **Windows Forms GUI**, stores analysis history in a **SQLite database**, and supports exporting results to **JSON** files.

---

## Features

- **Text input:** Manually enter text or load from `.txt` files.  
- **Text analysis metrics:**
  - Word count
  - Sentence count
  - Paragraph count
  - Top words with frequency
  - Average sentence length
- **GUI interface:** Interactive Windows Forms interface for easy usage.  
- **History tracking:** All analyses are saved in a SQLite database.  
- **Export reports:** Save analysis results as JSON files.  
- **Simple modular design:** Easily extendable with additional metrics or analysis features.

---

## Project Structure
TextAnalyzer/
│
├─ src/ # Source code
│ ├─ DB.fs # Database management
│ ├─ FileIO.fs # JSON export
│ ├─ Frequency.fs # Top word calculation
│ ├─ InputHandling.fs # Load text from file or manual input
│ ├─ Metrics.fs # Text metrics calculation
│ ├─ Tokenization.fs # Split text into words, sentences, paragraphs
│ ├─ Types.fs # Data types (AnalysisReport)
│ ├─ UI.fs # Windows Forms GUI
│ ├─ Program.fs # Entry point
│ └─ TextAnalyzer.fsproj
│
├─ test/ # Test project
│ ├─ ManualTests.fs # Manual test cases
│ └─ TextAnalyzerTests.fsproj
│
├─ bin/ # Build output (ignored in Git)
├─ obj/ # Build artifacts (ignored in Git)
└─ README.md # Project overview and instructions

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- Windows OS (for Windows Forms GUI)  
- Optional: Visual Studio or VS Code for F# development  

---

### Running the Application
Running the Application
1. Clone the repository:

bash
git clone https://github.com/kinzy4/TextAnalyzer.git
cd TextAnalyzer/src

2.Build and run the project:

bash
dotnet build
dotnet run

3.The GUI window will open. You can:

Type or paste text into the input box

Click Analyze to view metrics

Click Export JSON to save results

Click Load File to import a .txt file

Click View History to see previous analyses

## Running Tests
Navigate to the test folder:

bash
cd ../test
dotnet run
The console will show which tests passed or failed.

Usage Example
Input Text:

text
Hello world! This is a test.
Text Analyzer should count words, sentences, paragraphs, and top words.
Text Analyzer should count words, sentences, paragraphs, and top words.

Analysis Output:

Words: 15

Sentences: 3

Paragraphs: 1

Top Words: "text" (2), "analyzer" (2), "hello" (1), ...

Average Sentence Length: 5.0


## Contributing
Fork the repository

Create a feature branch:

bash
git checkout -b feature/my-feature
Commit your changes:

bash
git commit -m "feat: add new token
Open a Pull Request

 
License

This project is licensed under the MIT License.

Acknowledgements

Microsoft.Data.Sqlite for database integration

Newtonsoft.Json for JSON serialization

F# and .NET community for support and resources
