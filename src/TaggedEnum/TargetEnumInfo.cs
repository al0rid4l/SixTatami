using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace TaggedEnum;

internal struct TargetEnumInfo {
	public const string DefaultType = "string";

	public TargetEnumInfo() {}

	public bool UseAll = false;

	public bool UseSwitch = true;

	public bool AllowDuplicate = false;

	public bool Inline = true;

	public required SemanticModel SemanticModel { get; init; }

	public required EnumDeclarationSyntax SyntaxNode { get; init; }

	public required INamedTypeSymbol Symbol { get; init; }

	public required string TypeName { get; init; }

	public required string NamespaceName { get; init; }

	// public ITypeSymbol ValueType { get; init; };

	public string DataTypeName = DefaultType;

	public required string Modifiers { get; init; }
	
	public IEnumerable<MemberInfo>? Members;

	// public DiagnosticsInfo? DiagnosticsInfo;
	
	public IEnumerable<Diagnostic>? Diagnostics;
}