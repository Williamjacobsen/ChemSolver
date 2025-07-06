# ChemSolver

**ChemSolver** is a console-based chemistry problem solver developed in C#. It provides interactive tools to help with tasks such as exploring the periodic table, analyzing redox reactions, calculating oxidation numbers, and naming organic compounds.

## Features

- **Interactive Periodic Table**  
  Navigate a virtual periodic table and retrieve detailed element data from a JSON database.

- **Redox Reaction Balancer**  
  Input redox reactions and receive step-by-step assistance for balancing, including electron transfer, charge balancing (acidic or basic solutions), and final equation generation.

- **Oxidation Number Calculator**  
  Analyze chemical formulas to calculate the oxidation number of elements in compounds or ions.

- **Organic Compound Naming Tool**  
  Parses structural representations of organic molecules and provides IUPAC-compliant names, including support for branched chains and multiple bonds.

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (version 6 or later)

### Clone the Repository

```bash
git clone https://github.com/Williamjacobsen/ChemSolver.git
cd ChemSolver
```

### Build and Run

```bash
dotnet build
dotnet run
```

> On some systems, the application may attempt to resize the console window. If you experience issues, remove or comment out `Console.SetWindowSize()` in `Program.cs`.

## File Structure

- `Program.cs` — Entry point and main menu navigation.
- `Chemistry.cs` — Core chemical logic for:
  - Periodic Table
  - Redox Reactions
  - Oxidation Numbers
  - Organic Naming
- `UI.cs` — Terminal-based user interface and menu control.
- `JsonHandler.cs` — Utilities for parsing element data from JSON.
- `PeriodicTable.json` — JSON file with periodic element data (required).

## Example Inputs

### Redox Reaction

Input: MnO_4^-1 + NO_2^-1 -> MnO_2 + NO_3^-1
Solution type: s (acidic)

### Oxidation Number

Input: NO_3^-1
Output: Oxidation number of N is +5

### Organic Naming

Input: CH(CH)CH(cc)CH(ch)C(c)CC
Output: 3,4-diethyl-2-methylhexane

## Authors

- [William Jacobsen](https://github.com/Williamjacobsen)
- Søren Laursen
