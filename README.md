# Extensions for ML.net DataFrame
This repository contains a set of extensions for [ML.net](https://dotnet.microsoft.com/en-us/apps/ai/ml-dotnet) [DataFrame](https://learn.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/getting-started-dataframe). The extensions are designed to make it easier to work with ML.net DataFrame.

## Current extension methods

`EnumerableToDataframe()` converts an `IEnumerable<T>` to a DataFrame.
```csharp
List<Person> _personList =
    [
        new("John", 25, "New York"),
        new("Jane", 30, "Los Angeles"),
        new("Doe", 35, "Chicago"),
        new("Smith", 40, "Houston"),
        new("Alex", 45, "Phoenix"),
        new("Alice", 50, "Philadelphia"),
        new("Bob", 55, "San Antonio"),
        new("Charlie", 60, "San Diego"),
        new("David", 65, "Dallas"),
        new("Eve", 70, null)
    ];
var df = _personList.EnumerableToDataframe();
```

`ShowColumns()` returns the column names of the DataFrame.
```csharp

var df = _personList.EnumerableToDataframe();
IEnumerable<string> columnNames = df.ShowColumns();
```

`GetTextColumns()` returns the column names of all text columns of the DataFrame.
```csharp
var df = _personList.EnumerableToDataframe();
IEnumerable<string> textColumns = df.GetTextColumns();
```

`ValueCounts()` returns the count of unique values in descending order for all columns of the DataFrame as a DataFrame
(similiar to ValueCounts() that is available for DataFrameColumn).
```csharp
var df = _personList.EnumerableToDataframe();
var valueCounts = df.ValueCounts();
```

`CreateFilterColumn()` creates a new column with a boolean value based on a condition.
```csharp
var df = _personList.EnumerableToDataframe();

var filter = df.CreateFilterColumn("FilterCol", row => ((string)row["Name"] ).StartsWith('J') && (int) row["Age"] <= 30);
var filtedDataFrame = df.Filter(filter);

Assert.That(filtedDataFrame.Rows.Count(), Is.EqualTo(2));
```

`Filter()` filters the DataFrame based on a condition.
```csharp
var df = _personList.EnumerableToDataframe();
var filter = df.Filter(row => ((string)row["Name"] ).StartsWith('J') && (int) row["Age"] <= 30);
Assert.That(filter.Rows.Count(), Is.EqualTo(2));
```

`AddColumn()` adds a new column to the DataFrame. The new column is based on a function that is applied to each row. 
Thus gives more flexibility than the default DataFrame-way of computing a new column, especially when the computation
contains data from more than one column per row
```csharp
var df = _personList.EnumerableToDataframe();
//this
df.AddColumn("BirthYear", row => DateTime.Now.Year - (int)row["Age"]);

//instead of this
df["BirthYear"] = DateTime.Now.Year - df.Columns["Age"];
```

## Installation
From the solution folder call `dotnet pack`
```powershell
dotnet pack -c RELEASE
```

Asuming that you want to use it in a Polyglot Notebook, you have to reference the `DataFrameExtensions.dll` like so
```csharp
#r "DataFrameExtensions\bin\Release\net8.0\publish\DataFrameExtensions.dll"
using Microsoft.Data.Analysis;
using DataFrameExtensions;

var df = DataFrame.LoadCsv(@"CSVs\Sales_SalesOrderHeader.csv");

df.ShowColumns()
```