#QuantityMeasurementApp
📏Quantity Measurement Application (C# .NET)
📌 Project Overview

The Quantity Measurement Application is a console-based system developed using C# and .NET, designed to handle real-world unit measurement operations such as comparison, conversion, and arithmetic calculations.

This project is structured to reflect industry-level development practices, focusing on:
Clean and maintainable architecture
Incremental feature development using Use Cases (UC1 → UC6)
Strong emphasis on Test-Driven Development (TDD)
Accurate and reliable mathematical computations

Each feature is implemented step-by-step, ensuring backward compatibility, code reusability, and scalability.

---
🎯 Problem Statement

In real-world systems, handling different measurement units (feet, inches, yards, centimeters, etc.) can lead to:
Inconsistent comparisons
Conversion errors
Loss of precision
Complex and repetitive logic

This application solves these problems by introducing a standardized and extensible measurement framework.

---

🎯 Key Capabilities
✔ Measurement Comparison
Compare values within the same unit
Compare values across different units
Ensures logical equality using base unit normalization

--
✔ Unit Conversion
Convert any supported unit to another
Centralized conversion logic
Eliminates redundancy and inconsistency

--

✔ Arithmetic Operations
Add measurements of different units
Maintains correctness by converting to a base unit first
Returns results in a predictable format

--

✔ Precision Handling
Uses floating-point tolerance for equality checks
Avoids rounding errors during conversions

--

🧠 Core Design Principles

🔹 Object-Oriented Programming (OOP)
The project strictly follows OOP principles:
Encapsulation → Data is protected within classes
Abstraction → Only relevant functionality is exposed
Value Objects → Units like Feet and Inches behave as immutable objects
Method Overriding → Custom equality logic implemented

--

🔹 Clean Architecture

The application is divided into logical layers:
Layer	              Responsibility
Models              Represent data structures and units
Services	          Business logic (conversion, comparison, operations)
UI (Program.cs)	    User interaction

--

👉 This separation ensures:
Easy maintenance
Better testability
Clear code structure

--

🔹 Enum-Driven Design
The LengthUnit enum acts as a central registry for all supported units.
Advantages:
Easy to add new units
No need to modify core logic
Improves readability and maintainability

--

🔹 Base Unit Normalization
All calculations are internally converted to a single base unit (Feet).
👉 Why this matters:
Ensures consistent comparisons
Simplifies arithmetic operations
Reduces conversion complexity

--

🔹 Test-Driven Development (TDD)
Features are developed alongside tests using MSTest.
Workflow followed:
Write test case
Implement minimal code
Refactor while keeping tests passing

--


🚀 Detailed Use Case Breakdown
✅ UC1 — Feet Equality
Objective: Compare two values in Feet.
Implementation Details:
Created Feet class as a value object
Overrode Equals() method
Added null and reference checks

--
✅ UC2 — Inches Equality
Objective: Compare two values in Inches.
Implementation Details:
Separate Inches class introduced
Same equality logic reused
Ensured no impact on UC1

--
✅ UC3 — Generic Length Equality
Objective: Compare values across different units.
Implementation Details:
Introduced QuantityLength class
Added LengthUnit enum
Implemented conversion to base unit
Example:
1 foot == 12 inches → TRUE

--

✅ UC4 — Extended Units
Objective: Add more measurement units.
Units Added:
Yards
Centimeters

Key Design Decision:
Only enum updated
No change in core logic

--
✅ UC5 — Unit Conversion
Objective: Convert between any two units.
Implementation Details:
Created centralized conversion method
Used base unit internally
Added validation checks

--
✅ UC6 — Addition of Quantities
Objective: Add two measurements with different units.
Implementation Details:
Convert both values to base unit
Perform addition
Convert result back to desired unit

--

🧪 Testing Strategy
The project includes comprehensive unit tests:

✔ Test Coverage
Same unit comparison
Cross-unit comparison
Conversion validation
Arithmetic operations
Edge cases

✔ Edge Cases Covered
Zero values
Negative values
Large numbers
Floating-point precision

--

▶️ Run Tests
dotnet test

--
▶️ Running the Application
Build
dotnet build
Run
dotnet run

--
🎯 Key Design Advantages
✅ Scalability
New units can be added with minimal effort (only enum update).

✅ Maintainability
Clear separation of concerns makes code easy to update.

✅ Reusability
Centralized logic avoids duplication.

✅ Accuracy
All operations are normalized to a base unit.

✅ Testability
Independent components allow easy unit testing.

--

👩‍💻 Technologies Used
C# (.NET)
MSTest Framework
Visual Studio / VS Code
Git & GitHub
