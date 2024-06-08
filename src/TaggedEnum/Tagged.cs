namespace TaggedEnum;

[AttributeUsage(
	AttributeTargets.Enum,
	Inherited = false,
	AllowMultiple = false
	)]
public sealed class Tagged<TValue>: Attribute {
	public bool Inline = true;

	public bool UseSwitch = true;
}

[AttributeUsage(
	AttributeTargets.Enum,
	Inherited = false,
	AllowMultiple = false
	)]
// TValue default is string
public sealed class Tagged: Attribute {
	public bool UseAll = false;

	public bool Inline = true;

	public bool UseSwitch = true;
}