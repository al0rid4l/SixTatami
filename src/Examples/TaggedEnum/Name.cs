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

[Tagged<int>(Inline = false)]
public enum Book1 {
	[Data(1)]
	CSAPP,
	[Data<int>(2)]
	APUE,
	[Data(3)]
	SICP
}

[Tagged]
public enum Book2 {
	[Data("Computer Systems: A Programmer's Perspective")]
	CSAPP,
	[Data] // Data is "APUE"
	APUE,
	[Data("Structure and Interpretation of Computer Programs")]
	SICP
}

[Tagged(UseAll = true, Inline = false)]
public enum Book3 {
	[Data("Computer Systems: A Programmer's Perspective")]
	CSAPP,
	APUE, // Data is "APUE"
	SICP // Data is "SICP"
}