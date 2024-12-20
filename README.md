# Extensions for ML.net DataFrame
This repository contains a set of extensions for [ML.net](https://dotnet.microsoft.com/en-us/apps/ai/ml-dotnet) [DataFrame](https://learn.microsoft.com/en-us/dotnet/machine-learning/how-to-guides/getting-started-dataframe). The extensions are designed to make it easier to work with ML.net DataFrame.

## Current extension methods

`ShowColumns()` returns the column names of the DataFrame.
```csharp
var df = DataFrame.LoadCsv(@"CSVs\HumanResources_Employee.csv");
IEnumerable<string> columnNames = df.ShowColumns();
```

`GetTextColumns()` returns the column names of all text columns of the DataFrame.
```csharp
var df = DataFrame.LoadCsv(@"CSVs\HumanResources_Employee.csv");
IEnumerable<string> textColumns = df.GetTextColumns();
```

`ValueCounts()` returns the count of unique values in descending order for all columns of the DataFrame as a DataFrame
(similiar to ValueCounts() that is available for DataFrameColumn).
```csharp
var df = DataFrame.LoadCsv(@"CSVs\HumanResources_Employee.csv");
var valueCounts = df.ValueCounts();
```

`EnumerableToDataframe()` converts an IEnumerable to a DataFrame.
```csharp
var data = new List<int> { 1, 2, 3, 4, 5 };
var df = data.EnumerableToDataframe();
```


## Installation
From the solution folder call `dotnet pack`
```powershell
dotnet pack -c RELEASE
```

Asuming that you want to use it in a Polyglot Notebook, you have to reference the `DataFrameExtensions.dll` like so
```csharp
#r "DataFrameExtensions\bin\Release\net6.0\publish\DataFrameExtensions.dll"
using Microsoft.Data.Analysis;
using DataFrameExtensions;

var df = DataFrame.LoadCsv(@"CSVs\Sales_SalesOrderHeader.csv");

df.ShowColumns()
```