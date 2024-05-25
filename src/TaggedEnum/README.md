# TaggedEnum

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
Console.WriteLine(Book0.CSAPP.Data()); // Computer Systems: A Programmer's Perspective
Console.WriteLine(Book0.APUE.Data()); // Advanced Programming in the UNIX Environment
Console.WriteLine(Book0.SICP.Data()); // Structure and Interpretation of Computer Programs
Console.WriteLine(Book0.CSAPP.ToStringFast()); // CSAPP
Console.WriteLine(Book0.APUE.ToStringFast()); // APUE
Console.WriteLine(Book0.SICP.ToStringFast()); // SICP

Console.WriteLine("Book0Extension");
Console.WriteLine(ExamplesBook0Extension.GetDataByName("CSAPP")); // Computer Systems: A Programmer's Perspective
Console.WriteLine(ExamplesBook0Extension.GetValueByName("CSAPP")); // Book0.CSAPP

[Tagged<int>]
public enum Book1 {

	[Data(1)]
	CSAPP,
	[Data<int>(2)]
	APUE,
	[Data(3)]
	SICP
}

Console.WriteLine("Book1");
Console.WriteLine(Book1.CSAPP.Data()); // 1
Console.WriteLine(Book1.APUE.Data()); // 2
Console.WriteLine(Book1.SICP.Data()); // 3
Console.WriteLine(Book1.CSAPP.ToStringFast()); // CSAPP
Console.WriteLine(Book1.APUE.ToStringFast()); // APUE
Console.WriteLine(Book1.SICP.ToStringFast()); // SICP

[Tagged]
public enum Book2 {

	[Data("Computer Systems: A Programmer's Perspective")]
	CSAPP,
	[Data] // Data is "APUE"
	APUE,
	[Data("Structure and Interpretation of Computer Programs")]
	SICP
}

Console.WriteLine("Book2");
Console.WriteLine(Book2.CSAPP.Data()); // Computer Systems: A Programmer's Perspective
Console.WriteLine(Book2.APUE.Data()); // APUE
Console.WriteLine(Book2.SICP.Data()); // Structure and Interpretation of Computer Programs
Console.WriteLine(Book2.CSAPP.ToStringFast()); // CSAPP
Console.WriteLine(Book2.APUE.ToStringFast()); // APUE
Console.WriteLine(Book2.SICP.ToStringFast()); // SICP

[Tagged(UseAll = true)]
public enum Book3 {

	[Data("Computer Systems: A Programmer's Perspective")]
	CSAPP,
	APUE, // Data is "APUE"
	SICP // Data is "SICP"
}

Console.WriteLine("Book3");
Console.WriteLine(Book3.CSAPP.Data()); // Computer Systems: A Programmer's Perspective
Console.WriteLine(Book3.APUE.Data()); // APUE
Console.WriteLine(Book3.SICP.Data()); // SICP
Console.WriteLine(Book3.CSAPP.ToStringFast()); // CSAPP
Console.WriteLine(Book3.APUE.ToStringFast()); // APUE
Console.WriteLine(Book3.SICP.ToStringFast()); // SICP
```
