# 🧪 TestVismaTask1

Unit tests for the console application [VismaTask1](https://github.com/Enysmen/VismaTask1), designed to manage resource shortages in the Visma company.

The project covers the business layer logic and validation checks of CLI command parameters.

---

## 📋 Content

- [📦 Structure](#-structure)
- [🧪 What is covered by tests](#-what-is-covered-by-tests)
- [🔧 Installation and launch](#-installation-and-launch)
- [✅ Technologies Used](#-technologies-used)

---

## 📦 Structure

The project is organized into classes:

| Test class                      | What is testing                                                  |
|-------------------------------  |------------------------------------------------------------------|
| `ShortageServiceRegisterTests`  | Checking the logic of registration of applications               |
| `FactoryOptionsCommandTests`    | Validation and correctness of CLI option parameters              |
| `ShortageServiceDeleteTests`    | Checking for deletion of applications (rights, errors)           |
| `ShortageServiceFilterTests`    | Check filtering by date, name, category                          |
| `JsonShortageRepositoryTests`   | Tests for correct operation of a JSON file (reading and writing) |

---

## 🧪 What is covered by the tests

- **Successful registration of new applications**
- **Refusal to register a duplicate with lower priority**
- **Replacing duplicate with higher priority**
- **Filtration** By:
  - Name (partial match, case insensitive)
  - Date of creation
  - categories
  - Rooms
- **Deleting applications**:
  - Only available to creator or administrator
- **Validating CLI parameters**:
  - Checking the correctness of `--priority`, `--room`, `--category` and other parameters

---

## 🔧 Installation and launch

### 1️⃣ Clone the repositoryй

```bash
git clone https://github.com/Enysmen/TestVismaTask1.git
cd TestVismaTask1
```

### 2️⃣ Make sure the link to the main project is added

```bash
dotnet add reference ../VismaTask1/VismaTask1.csproj
```

### 3️⃣ Run the tests

```bash
dotnet TestVismaTask1
```

---

## ✅ Technologies used

- **.NET 9** — development and launch platform
- **xUnit** — unit testing framework
- **Moq** — to create mock dependencies
- **Microsoft.Extensions.Logging.Abstractions** — for logging
- **System.CommandLine** - To generate and validate command line parameters
