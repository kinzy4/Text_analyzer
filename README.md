# 📊 Text Analyzer

A **Text Analyzer** application written in **F#** that analyzes text and reports useful metrics such as **word count**, **sentence count**, **paragraph count**, **top words**, and **average sentence length**. The application features a **Windows Forms GUI**, stores analysis history in a **SQLite database**, and supports exporting results to **JSON** files.

---

## ✨ Features

* **Text Input**

  * Manually enter text
  * Load text from `.txt` files

* **Text Analysis Metrics**

  * Word count
  * Sentence count
  * Paragraph count
  * Top words with frequency
  * Average sentence length

* **GUI Interface**

  * Interactive Windows Forms interface for easy usage

* **History Tracking**

  * All analyses are saved in a SQLite database

* **Export Reports**

  * Save analysis results as JSON files

* **Modular Design**

  * Easily extendable with additional metrics or analysis features

---

## 🗂 Project Structure

```text
TextAnalyzer/
│
├─ src/                     # Source code
│  ├─ DB.fs                  # Database management
│  ├─ FileIO.fs              # JSON export
│  ├─ Frequency.fs           # Top word calculation
│  ├─ InputHandling.fs       # Load text from file or manual input
│  ├─ Metrics.fs             # Text metrics calculation
│  ├─ Tokenization.fs        # Split text into words, sentences, paragraphs
│  ├─ Types.fs               # Data types (AnalysisReport)
│  ├─ UI.fs                  # Windows Forms GUI
│  ├─ Program.fs             # Entry point
│  └─ TextAnalyzer.fsproj
│
├─ test/                    # Test project
│  ├─ ManualTests.fs         # Manual test cases
│  └─ TextAnalyzerTests.fsproj
│
├─ bin/                     # Build output (ignored in Git)
├─ obj/                     # Build artifacts (ignored in Git)
└─ README.md                # Project overview and instructions
```

---

## 🚀 Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* Windows OS (required for Windows Forms GUI)
* Optional: Visual Studio or VS Code for F# development

---

## ▶️ Running the Application

1. **Clone the repository**

```bash
git clone https://github.com/kinzy4/TextAnalyzer.git
cd TextAnalyzer/src
```

2. **Build and run the project**

```bash
dotnet build
dotnet run
```

3. **Use the GUI to:**

* Type or paste text into the input box
* Click **Analyze** to view metrics
* Click **Export JSON** to save results
* Click **Load File** to import a `.txt` file
* Click **View History** to see previous analyses

---

## 🧪 Running Tests

Navigate to the test folder:

```bash
cd ../test
dotnet run
```

The console will display which tests passed or failed.

---

## 📝 Usage Example

**Input Text:**

```text
Hello world! This is a test.
Text Analyzer should count words, sentences, paragraphs, and top words.
Text Analyzer should count words, sentences, paragraphs, and top words.
```

**Analysis Output:**

* **Words:** 15
* **Sentences:** 3
* **Paragraphs:** 1
* **Top Words:** `text (2)`, `analyzer (2)`, `hello (1)`, ...
* **Average Sentence Length:** 5.0

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch:

```bash
git checkout -b feature/my-feature
```

3. Commit your changes:

```bash
git commit -m "feat: add new feature"
```

4. Open a Pull Request

---

## 📄 License

This project is licensed under the **MIT License**.

---

## 🙏 Acknowledgements

* **Microsoft.Data.Sqlite** for database integration
* **Newtonsoft.Json** for JSON serialization
* **F# and .NET community** for support and resources
