#pragma warning disable CA1050 // Declare types in namespaces
[global::Microsoft.CodeAnalysis.EmbeddedAttribute]
[AttributeUsage(
	AttributeTargets.Enum,
	Inherited = false,
	AllowMultiple = false
	)]
internal sealed class TaggedAttribute<TValue>: Attribute {
	public bool Inline = true;

	public bool UseSwitch = true;

	public bool AllowDuplicate = false;
}

[global::Microsoft.CodeAnalysis.EmbeddedAttribute]
[AttributeUsage(
	AttributeTargets.Enum,
	Inherited = false,
	AllowMultiple = false
	)]
// TValue default is string
internal sealed class TaggedAttribute: Attribute {
		public bool UseAll = false;

	public bool Inline = true;

	public bool UseSwitch = true;

	public bool AllowDuplicate = false;
}
#pragma warning restore CA1050 // Declare types in namespaces