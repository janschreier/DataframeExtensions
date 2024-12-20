using DataFrameExtensions;

namespace Tests;

public class DataFrameExtensionsTests
{
    record Person(string Name, int Age, string? City);


    [Test]
    public void EnumerableToDataframe_Is_Created()
    {
        var personList = new List<Person>
        {
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
        };

        var df = personList.EnumerableToDataframe();
        Assert.That(df.Rows.Count(), Is.EqualTo(10));
        Assert.That(df.Rows[0]["Name"], Is.EqualTo("John"));
        Assert.That(df.Rows[0]["Age"], Is.EqualTo(25));
        Assert.That(df.Rows[0]["City"], Is.EqualTo("New York"));
    }

    record Address(string Street);

    record PersonWithAddress(string Name, Address Address);


    [Test]
    public void EnumerableToDataframe_Throws_On_Class_Property()
    {
        var personList = new List<PersonWithAddress>
        {
            new("John",  new Address("123 Main St")),
        };

        Assert.Throws<ArgumentException>(() => personList.EnumerableToDataframe());
    }

    struct AddressStruct
    {
        public string Street { get; set; }
    }

    record PersonWithAddressStruct(string Name, AddressStruct Address);

    [Test]
    public void EnumerableToDataframe_Is_Created_With_Struct()
    {
        var personList = new List<PersonWithAddressStruct>
        {
            new("John", new AddressStruct { Street = "123 Main St" }),
        };

        Assert.Throws<ArgumentException>(() => personList.EnumerableToDataframe());
    }

    enum DummyEnum
    {
        One,
    }

    record PersonWithEnum(string Name, DummyEnum Enum);

    [Test]
    public void EnumerableToDataframe_Is_Created_With_Enum()
    {
        var personList = new List<PersonWithEnum>
        {
            new("John", DummyEnum.One),
        };

        Assert.Throws<ArgumentException>(() => personList.EnumerableToDataframe());
    }
}