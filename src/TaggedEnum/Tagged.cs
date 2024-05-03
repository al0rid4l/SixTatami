namespace TaggedEnum;

[AttributeUsage(
	AttributeTargets.Enum,
	Inherited = false,
	AllowMultiple = false
	)]
public sealed class Tagged<TValue>: Attribute;

[AttributeUsage(
	AttributeTargets.Enum,
	Inherited = false,
	AllowMultiple = false
	)]
// TValue default is string
public sealed class Tagged: Attribute {
	public bool UseAll = false;
}