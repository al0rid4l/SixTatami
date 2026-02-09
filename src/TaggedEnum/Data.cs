#pragma warning disable CA1050 // Declare types in namespaces
[AttributeUsage(
	AttributeTargets.Field,
	Inherited = false,
	AllowMultiple = false
	)]
internal sealed class Data<TValue>(TValue v): Attribute {
	public TValue V { get; private set; } = v;
}

[AttributeUsage(
	AttributeTargets.Field,
	Inherited = false,
	AllowMultiple = false
	)]
// TValue default is string
internal sealed class Data: Attribute {
	public object? V { get; }

	public Data(object str) {
		V = str;
	}

	public Data() {}
}
#pragma warning restore CA1050 // Declare types in namespaces