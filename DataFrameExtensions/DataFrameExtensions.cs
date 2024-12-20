using Microsoft.Data.Analysis;

namespace DataFrameExtensions;

public static class DataFrameExtensions
{
    /// <summary>
    /// Returns the column names of a DataFrame as a list of strings.
    /// </summary>
    /// <param name="dataframe"></param>
    /// <returns></returns>
    public static IEnumerable<string> ShowColumns(this DataFrame dataframe) =>
        dataframe.Columns.Select(c => c.Name);


    /// <summary>
    /// Returns the names of columns in the DataFrame that have a data type of string.
    /// </summary>
    /// <param name="dataframe">The input DataFrame.</param>
    /// <returns>An IEnumerable of strings containing the names of the text columns.</returns>
    public static IEnumerable<string> GetTextColumns(this DataFrame dataframe) =>
        dataframe.Columns.Where(c => c.DataType == typeof(string)).Select(c => c.Name);


    /// <summary>
    /// Computes the value counts for each column in the DataFrame and returns a new DataFrame
    /// with the counts sorted in descending order.
    /// </summary>
    /// <param name="dataframe">The input DataFrame.</param>
    /// <param name="maxLength">The maximum length of the returned DataFrame.</param>
    /// <param name="columnNames">The column names to include in the returned DataFrame.</param>
    /// <returns>A new DataFrame containing the value counts for each column.</returns>
    public static DataFrame ValueCounts(this DataFrame dataframe, int maxLength = 10, string[]? columnNames = null)
    {
        var columns = columnNames ?? dataframe.ShowColumns();
        var newDataFrame = new DataFrame();
        var maxLengthInternal = Math.Min(dataframe.Rows.Count, maxLength);
        long realMaxLength = 0;
        foreach (var column in columns)
        {
            var valueCounts = dataframe[column].ValueCounts().OrderByDescending("Counts");
            realMaxLength = Math.Max(valueCounts.Rows.Count, realMaxLength);

            var missingRows = maxLengthInternal - valueCounts.Rows.Count;

            if (missingRows > 0)
            {
                for (var i = 0; i < missingRows; i++)
                {
                    valueCounts.Append((IEnumerable<object>)null!, true);
                }
            }
            else
            {
                valueCounts = valueCounts.Head((int)maxLengthInternal);
            }

            valueCounts.Columns[0].SetName(column);
            valueCounts.Columns[1].SetName($"{column}Count");

            newDataFrame.Columns.Add(valueCounts.Columns[0]);
            newDataFrame.Columns.Add(valueCounts.Columns[1]);
        }

        return newDataFrame;
    }

    /// <summary>
    /// Converts an enumerable collection of objects to a DataFrame. Works only for value types (except structs) and strings
    /// Otherwise, an ArgumentException is thrown.
    /// </summary>
    /// <typeparam name="T">The type of objects in the enumerable collection.</typeparam>
    /// <param name="data">The enumerable collection of objects.</param>
    /// <returns>A DataFrame representing the enumerable collection.</returns>
    public static DataFrame EnumerableToDataframe<T>(this IEnumerable<T> data)
    {
        var properties = typeof(T).GetProperties();

        List<(string, Type)> stringTypeList = [];
        foreach (var prop in properties)
        {
            if ((prop.PropertyType.IsClass && prop.PropertyType != typeof(string)) || prop.PropertyType.IsInterface
                || prop.PropertyType.IsArray || IsStruct(prop.PropertyType) || prop.PropertyType.IsEnum )
            {
                throw new ArgumentException(
                    $"Only value types are supported. Property {prop.Name} is of type {prop.PropertyType.Name}");
            }

            stringTypeList.Add((prop.Name, prop.PropertyType));
        }

        IEnumerable<IList<object>> resultList = new List<IList<object>>();

        foreach (var item in data)
        {
            List<object> row = properties.Select(prop => prop.GetValue(item)).ToList();
            resultList = resultList.Append(row);
        }

        return DataFrame.LoadFrom(resultList, stringTypeList);
    }

    private static bool IsStruct(Type type)
    {
        return type is { IsValueType: true, IsPrimitive: false, IsEnum: false };
    }
}