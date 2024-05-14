using Microsoft.CodeAnalysis;

namespace TaggedEnum;

internal struct MemberInfo {
	public MemberInfo() {}
	
	public bool HasData = false;
	
	public required string MemberName { get; init; }
	
	public string Data = string.Empty;

	public string TypeName = string.Empty;

	public required string FieldValue;

	public IEnumerable<Diagnostic>? Diagnostics;
}