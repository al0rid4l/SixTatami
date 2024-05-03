namespace TaggedEnum;

[AttributeUsage(
	AttributeTargets.Field,
	Inherited = false,
	AllowMultiple = false
	)]
public sealed class Value<TValue>(TValue v): Attribute {
	public TValue V { get; private set; } = v;
}

[AttributeUsage(
	AttributeTargets.Field,
	Inherited = false,
	AllowMultiple = false
	)]
// TValue default is string
public sealed class Value: Attribute {
	public string? V { get; private set; }

	public Value(string str) {
		V = str;
	}
	
	public Value() {}
}