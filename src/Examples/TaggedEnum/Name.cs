using TaggedEnum;

namespace Examples;

[Tagged<int>]
public enum Name {
	[Data<int>(22)]
	a,
	[Data<int>(33)]
	b,
	[Data<int>(44)]
	c
}