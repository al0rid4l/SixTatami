using System.Text.Json;
using System.Text.Json.Serialization;

using Examples;

Console.WriteLine("Book0");
Console.WriteLine(Book0.CSAPP.Data());
Console.WriteLine(Book0.APUE.Data());
Console.WriteLine(Book0.SICP.Data());
Console.WriteLine(Book0.CSAPP.ToStringFast());
Console.WriteLine(Book0.APUE.ToStringFast());
Console.WriteLine(Book0.SICP.ToStringFast());
Console.WriteLine("");

Console.WriteLine("Book1");
Console.WriteLine(Book1.CSAPP.Data());
Console.WriteLine(Book1.APUE.Data());
Console.WriteLine(Book1.SICP.Data());
Console.WriteLine(Book1.CSAPP.ToStringFast());
Console.WriteLine(Book1.APUE.ToStringFast());
Console.WriteLine(Book1.SICP.ToStringFast());
Console.WriteLine("");

Console.WriteLine("Book2");
Console.WriteLine(Book2.CSAPP.Data());
Console.WriteLine(Book2.APUE.Data());
Console.WriteLine(Book2.SICP.Data());
Console.WriteLine(Book2.CSAPP.ToStringFast());
Console.WriteLine(Book2.APUE.ToStringFast());
Console.WriteLine(Book2.SICP.ToStringFast());
Console.WriteLine("");

Console.WriteLine("Book3");
Console.WriteLine(Book3.CSAPP.Data());
Console.WriteLine(Book3.APUE.Data());
Console.WriteLine(Book3.SICP.Data());
Console.WriteLine(Book3.CSAPP.ToStringFast());
Console.WriteLine(Book3.APUE.ToStringFast());
Console.WriteLine(Book3.SICP.ToStringFast());
Console.WriteLine("");

Console.WriteLine("Book0Extension");
Console.WriteLine(ExamplesBook0Extension.TryGetDataByName("CSAPP", out var v));
Console.WriteLine(v);
Console.WriteLine(ExamplesBook0Extension.TryGetValueByName("CSAPP", out var vv));
Console.WriteLine((int)vv);
Console.WriteLine(ExamplesBook0Extension.TryGetValueByData("Computer Systems: A Programmer's Perspective", out var vvv));
Console.WriteLine((int)vvv);

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