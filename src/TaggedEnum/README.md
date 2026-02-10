# TaggedEnum

## Install
```bash
$ dotnet add package TaggedEnum
```

## Usage

```csharp
using TaggedEnum;

namespace Examples;

[Tagged]
public enum Book0 {
	[Data("Computer Systems: A Programmer's Perspective")]
	CSAPP,
	[Data("Advanced Programming in the UNIX Environment")]
	APUE,
	[Data("Structure and Interpretation of Computer Programs")]
	SICP
}

Console.WriteLine("Book0");
Console.WriteLine(Book0.CSAPP.Data); // Computer Systems: A Programmer's Perspective
Console.WriteLine(Book0.APUE.Data); // Advanced Programming in the UNIX Environment
Console.WriteLine(Book0.SICP.Data); // Structure and Interpretation of Computer Programs
Console.WriteLine(Book0.CSAPP.ToStringFast()); // CSAPP
Console.WriteLine(Book0.APUE.ToStringFast()); // APUE
Console.WriteLine(Book0.SICP.ToStringFast()); // SICP

Console.WriteLine("Book0Extension");
Console.WriteLine(ExamplesBook0Extension.TryGetDataByName("CSAPP", out var v)); // true, v is Computer Systems: A Programmer's Perspective
Console.WriteLine(ExamplesBook0Extension.TryGetValueByName("CSAPP", out var vv)); // true, vv is Book0.CSAPP
Console.WriteLine(ExamplesBook0Extension.TryGetValueByData("Computer Systems: A Programmer's Perspective", out var vvv)); // CSAPP

[Tagged<int>(Inline = false)] // Inline = true, will add [MethodImpl(MethodImplOptions.AggressiveInlining)] on Data, ToStringFast(), TryGetDataByName()
public enum Book1 {
	[Data(1)]
	CSAPP,
	[Data<int>(2)]
	APUE,
	[Data(3)]
	SICP
}

Console.WriteLine("Book1");
Console.WriteLine(Book1.CSAPP.Data); // 1
Console.WriteLine(Book1.APUE.Data); // 2
Console.WriteLine(Book1.SICP.Data); // 3
Console.WriteLine(Book1.CSAPP.ToStringFast()); // CSAPP
Console.WriteLine(Book1.APUE.ToStringFast()); // APUE
Console.WriteLine(Book1.SICP.ToStringFast()); // SICP

[Tagged(UseSwitch = false)] // use switch or dictionary
public enum Book2 {
	[Data("Computer Systems: A Programmer's Perspective")]
	CSAPP,
	[Data] // Data is "APUE"
	APUE,
	[Data("Structure and Interpretation of Computer Programs")]
	SICP
}

Console.WriteLine("Book2");
Console.WriteLine(Book2.CSAPP.Data); // Computer Systems: A Programmer's Perspective
Console.WriteLine(Book2.APUE.Data); // APUE
Console.WriteLine(Book2.SICP.Data); // Structure and Interpretation of Computer Programs
Console.WriteLine(Book2.CSAPP.ToStringFast()); // CSAPP
Console.WriteLine(Book2.APUE.ToStringFast()); // APUE
Console.WriteLine(Book2.SICP.ToStringFast()); // SICP

[Tagged(UseAll = true, Inline = false, UseSwitch = false)]
public enum Book3 {
	[Data("Computer Systems: A Programmer's Perspective")]
	CSAPP,
	APUE, // Data is "APUE"
	SICP // Data is "SICP"
}

Console.WriteLine("Book3");
Console.WriteLine(Book3.CSAPP.Data); // Computer Systems: A Programmer's Perspective
Console.WriteLine(Book3.APUE.Data); // APUE
Console.WriteLine(Book3.SICP.Data); // SICP
Console.WriteLine(Book3.CSAPP.ToStringFast()); // CSAPP
Console.WriteLine(Book3.APUE.ToStringFast()); // APUE
Console.WriteLine(Book3.SICP.ToStringFast()); // SICP

// for JSON serialize deserialize
var options = new JsonSerializerOptions() {
	IncludeFields = true
};

var str = $$"""
{
	"V0": "Computer Systems: A Programmer's Perspective",
	"V1": null,
	"V2": ["Advanced Programming in the UNIX Environment", "Structure and Interpretation of Computer Programs"],
	"V3": ["Advanced Programming in the UNIX Environment", null, "Structure and Interpretation of Computer Programs"],
	"V4": ["Advanced Programming in the UNIX Environment", "Structure and Interpretation of Computer Programs"],
	"V5": ["Advanced Programming in the UNIX Environment", null, "Structure and Interpretation of Computer Programs"],
	"V6": "CSAPP",
	"V7": null,
	"V8": ["CSAPP", "SICP"],
	"V9": ["CSAPP", null, "SICP"],
	"V10": ["CSAPP", "SICP"],
	"V11": ["CSAPP", null, "SICP"]
}
""";

var obj = JsonSerializer.Deserialize<JsonTest>(str, options);
Console.WriteLine(obj.V1);

var o = new JsonTest();
var s = JsonSerializer.Serialize(o, options);
Console.WriteLine(s);

public class JsonTest {
	[JsonConverter(typeof(ExamplesBook0ToDataConverter))]
	[JsonPropertyName("V0")]
	public Book0 V0 = Book0.CSAPP;

	[JsonConverter(typeof(ExamplesBook0ToDataConverter))]
	[JsonPropertyName("V1")]
	public Book0? V1 = default;

	[JsonConverter(typeof(ExamplesBook0ArrayToDataArrayConverter))]
	[JsonPropertyName("V2")]
	public Book0[] V2 = [Book0.CSAPP, Book0.SICP];

	[JsonConverter(typeof(NullableExamplesBook0ArrayToDataArrayConverter))]
	[JsonPropertyName("V3")]
	public Book0?[] V3 = [Book0.CSAPP, null, Book0.SICP];

	[JsonConverter(typeof(ExamplesBook0EnumerableToDataEnumerableConverter))]
	[JsonPropertyName("V4")]
	public IEnumerable<Book0> V4 = [Book0.CSAPP, Book0.SICP];

	[JsonConverter(typeof(NullableExamplesBook0EnumerableToDataEnumerableConverter))]
	[JsonPropertyName("V5")]
	public IEnumerable<Book0?> V5 = [Book0.CSAPP, null, Book0.SICP];

	[JsonConverter(typeof(ExamplesBook0ToNameConverter))]
	[JsonPropertyName("V6")]
	public Book0 V6 = Book0.CSAPP;

	[JsonConverter(typeof(ExamplesBook0ToNameConverter))]
	[JsonPropertyName("V7")]
	public Book0? V7 = default;

	[JsonConverter(typeof(ExamplesBook0ArrayToNameArrayConverter))]
	[JsonPropertyName("V8")]
	public Book0[] V8 = [Book0.CSAPP, Book0.SICP];

	[JsonConverter(typeof(NullableExamplesBook0ArrayToNameArrayConverter))]
	[JsonPropertyName("V9")]
	public Book0?[] V9 = [Book0.CSAPP, null, Book0.SICP];

	[JsonConverter(typeof(ExamplesBook0EnumerableToNameEnumerableConverter))]
	[JsonPropertyName("V10")]
	public IEnumerable<Book0> V10 = [Book0.CSAPP, Book0.SICP];

	[JsonConverter(typeof(NullableExamplesBook0EnumerableToNameEnumerableConverter))]
	[JsonPropertyName("V11")]
	public IEnumerable<Book0?> V11 = [Book0.CSAPP, null, Book0.SICP];
}
```
