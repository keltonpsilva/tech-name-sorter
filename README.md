# 🧠 Name Sorter

A console application built in **.NET 9** that sorts a list of names by **last name**, then by **given name(s)**.

---

## 🎯 Goal

The **Name Sorter** reads a file of unsorted names, validates and parses them, then outputs a new file with the names sorted alphabetically by:

1. **Last name**
2. **Given name(s)** (in ascending order)

A valid name must:

- Contain **at least 1 given name** and **1 last name**.
- Contain **no more than 3 given names**.

Example input:

```json
Janet Parsons
Vaughn Lewis
Adonis Julius Archer
Shelby Nathan Yoder
```

Example output:

```josn
Adonis Julius Archer
Vaughn Lewis
Janet Parsons
Shelby Nathan Yoder
```

---

## 🏗️ Tech Stack

- **.NET 9 Console Application**
- **Vertical Slice Architecture**
- **xUnit** for unit testing
- **FluentAssertions** for expressive test validation
- **Bogus** for realistic test data generation
- **Moq** for mocking dependencies
- **TDD** (Test-Driven Development)
- **GitHub Actions** for continuous integration

---

## 🧩 Architecture Overview

The project is designed around **Vertical Slices**, meaning each feature encapsulates its:

- Command/Query logic
- Domain rules
- Interfaces
- Tests

### Core Components

| Component                | Responsibility                                            |
| ------------------------ | --------------------------------------------------------- |
| `Fullname`               | Domain record representing a validated person’s full name |
| `NameParser`             | Parses raw strings into `Fullname` objects                |
| `NameSorterService`      | Sorts names alphabetically by last name, then given names |
| `SortNameCommandHandler` | Coordinates the process: reads → parses → sorts → writes  |
| `IFileManager`           | Abstracts file system operations                          |
| `Program.cs`             | Entry point for the console application                   |

---

## ⚙️ Running the Application

### 🧾 Command Syntax

```bash
name-sorter ./unsorted-names-list.txt
```

## 🚀 Future Improvements

- Implement Dependecy Injection Container
- Add validation and error logging middleware.
- Extend CLI for specifying output file path.
- Add integration tests for file-based workflows.
- Support asynchronous file operations.
