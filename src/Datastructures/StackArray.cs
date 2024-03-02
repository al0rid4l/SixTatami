using System.Runtime.CompilerServices;

namespace SixTatami.DataStructures;

#pragma warning disable IDE0051, IDE0044
[InlineArray(8)]
public struct StackArray8<T> {
	private T _element;
}

[InlineArray(16)]
public struct StackArray16<T> {
	private T _element;
}

[InlineArray(32)]
public struct StackArray32<T> {
	private T _element;
}

[InlineArray(64)]
public struct StackArray64<T> {
	private T _element;
}

[InlineArray(128)]
public struct StackArray128<T> {
	private T _element;
}

[InlineArray(256)]
public struct StackArray256<T> {
	private T _element;
}
#pragma warning restore IDE0044, IDE0051