#pragma warning disable CA1050 // Declare types in namespaces
[AttributeUsage(
	AttributeTargets.Enum,
	Inherited = false,
	AllowMultiple = false
	)]
internal sealed class Tagged<TValue>: Attribute {
	public bool Inline = true;

	public bool UseSwitch = true;

	public bool AllowDuplicate = false;
}

[AttributeUsage(
	AttributeTargets.Enum,
	Inherited = false,
	AllowMultiple = false
	)]
// TValue default is string
internal sealed class Tagged: Attribute {
		public bool UseAll = false;

	public bool Inline = true;

	public bool UseSwitch = true;

	public bool AllowDuplicate = false;
}
#pragma warning restore CA1050 // Declare types in namespaces