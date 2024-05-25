namespace TaggedEnum;

[AttributeUsage(
	AttributeTargets.Field,
	Inherited = false,
	AllowMultiple = false
	)]
public sealed class Data<TValue>(TValue v): Attribute {
	public TValue V { get; private set; } = v;
}

[AttributeUsage(
	AttributeTargets.Field,
	Inherited = false,
	AllowMultiple = false
	)]
// TValue default is string
public sealed class Data: Attribute {
	public object? V { get; private set; }

	public Data(object str) {
		V = str;
	}
	
	public Data() {}
}