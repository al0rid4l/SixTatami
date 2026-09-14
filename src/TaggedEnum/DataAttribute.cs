#pragma warning disable CA1050 // Declare types in namespaces
[global::Microsoft.CodeAnalysis.EmbeddedAttribute]
[AttributeUsage(
	AttributeTargets.Field,
	Inherited = false,
	AllowMultiple = false
	)]
internal sealed class DataAttribute<TValue>(TValue v): Attribute {
	public TValue V { get; private set; } = v;
}

[global::Microsoft.CodeAnalysis.EmbeddedAttribute]
[AttributeUsage(
	AttributeTargets.Field,
	Inherited = false,
	AllowMultiple = false
	)]
// TValue default is string
internal sealed class DataAttribute: Attribute {
	public object? V { get; }

	public DataAttribute(object str) {
		V = str;
	}

	public DataAttribute() {}
}
#pragma warning restore CA1050 // Declare types in namespaces