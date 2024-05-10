using System.Diagnostics;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using static TaggedEnum.Util;

namespace TaggedEnum;

[Generator(LanguageNames.CSharp)]
public sealed class TaggedEnumSourceGenerator: IIncrementalGenerator {
	private const string TaggedAttrName = "TaggedEnum.Tagged";
	private const string TaggedGenericAttrName = "TaggedEnum.Tagged`1";
	private const string DataAttrName = "TaggedEnum.Data";
	private const string DataGenericAttrName = "TaggedEnum.Data`1";

	public void Initialize(IncrementalGeneratorInitializationContext context) {
		static bool IsEnum(SyntaxNode node, CancellationToken _) => node is EnumDeclarationSyntax;

		// if (!Debugger.IsAttached){
		// 	Debugger.Launch();
		// }
		
		Span<string> taggedAttrs = [TaggedAttrName, TaggedGenericAttrName];
		foreach (var taggedAttrName in taggedAttrs) {
			var provider = context.SyntaxProvider.ForAttributeWithMetadataName(taggedAttrName, 
				IsEnum,
				TransformEnumPayload
			)
			.Where(NotNullValueType)
			.Select(NullableToValueType)
			.Select(ProcessMembers);

			context.RegisterSourceOutput(provider, GenerateSource);
		}
	}

	private static TypeInfo? TransformEnumPayload(GeneratorAttributeSyntaxContext ctx, CancellationToken _) {
		// if (!Debugger.IsAttached){
		// 	Debugger.Launch();
		// }

		if (ctx is not {
			TargetNode: EnumDeclarationSyntax {
				Modifiers: var modifiers
			} targetNode,
			TargetSymbol: INamedTypeSymbol targetSymbol,
			SemanticModel.Compilation: var compilation,
			Attributes: [{
				AttributeClass: {} attr
			} attrConstructor, ..]
		}) {
			return null;
		}
		
		var taggedType = compilation.GetTypeByMetadataName(TaggedAttrName);
		var taggedGenericType = compilation.GetTypeByMetadataName(TaggedGenericAttrName);
		var taggedAttrList = targetSymbol.GetAttributesDataByAttribute([taggedType!, taggedGenericType!]);
		
		if (taggedAttrList.Count() > 1) {
			// TODO　发出错误警告
			return null;
		}
		

		var data = new TypeInfo() {
				Node = targetNode,
				SemanticModel = ctx.SemanticModel,
				Symbol = targetSymbol,
				TypeName = targetSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
				NamespaceName = targetSymbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
				Modifiers = modifiers.ToString()
		};
		if (attr.IsGenericType) {
			var genericArg = attr.TypeArguments[0];
			if (genericArg.SpecialType != SpecialType.System_String
				&& genericArg.SpecialType != SpecialType.System_Char
				&& genericArg.SpecialType != SpecialType.System_Int16
				&& genericArg.SpecialType != SpecialType.System_Int32
				&& genericArg.SpecialType != SpecialType.System_Int64
				&& genericArg.SpecialType != SpecialType.System_UInt16
				&& genericArg.SpecialType != SpecialType.System_UInt32
				&& genericArg.SpecialType != SpecialType.System_UInt64
				&& genericArg.SpecialType != SpecialType.System_Boolean
				&& genericArg.SpecialType != SpecialType.System_Byte
				&& genericArg.SpecialType != SpecialType.System_SByte
				&& genericArg.SpecialType != SpecialType.System_Single
				&& genericArg.SpecialType != SpecialType.System_Double
				) {
				// TODO　发出错误警告
				return null;
			}
			data.DataTypeName = genericArg.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
		} else if (attrConstructor is {
			NamedArguments: [{
				Key: "UseAll",
				Value.Value: true
			}]
		}) {
			data.UseAll = true;
		}
		return data;
	}
	
	private void GenerateSource(SourceProductionContext ctx, FinalResult data) {
		// if (!Debugger.IsAttached) {
		// 	Debugger.Launch();
		// }
		var globalLength = "global::".Length;
		var @namespace = data.Type.NamespaceName == "<global namespace>" ? "" : $"namespace {data.Type.NamespaceName[globalLength..]};";
		var fullNamespace = data.Type.NamespaceName == "<global namespace>" ? "" : data.Type.NamespaceName[globalLength..];
		var fullTypeName = data.Type.TypeName[globalLength..];
		var formattedNamespace = string.Join("", fullNamespace.Split('.'));
		var formattedTypeName = string.Join("", fullTypeName.Split('.'));
		var nameValueMap = data.Members.Select(m => $@"{{{data.Type.TypeName}.{m.MemberName}, ""{data.Type.TypeName}.{m.MemberName}""}},").Aggregate("", static (current, next) => current + "\n\t\t" + next);
		var valueNameMap = data.Members.Select(m => $@"{{""{m.MemberName}"", {data.Type.TypeName}.{m.MemberName}}},").Aggregate("", static (current, next) => current + "\n\t\t" + next);
		var valueDataConditionalBranches = data.Members.Where(static m => m.HasData).Select(m => $"{data.Type.TypeName}.{m.MemberName} => ({m.TypeName}){m.Data},").Aggregate("", static (current, next) => current + "\n\t\t\t" + next);
		var nameDataConditionalBranches = data.Members.Where(static m => m.HasData).Select(m => $@"""{m.MemberName}"" => ({m.TypeName}){m.Data},").Aggregate("", static (current, next) => current + "\n\t\t\t" + next);
		var valueNameConditionalBranches = data.Members.Select(m => $@"{data.Type.TypeName}.{m.MemberName} => ""{m.MemberName}"",").Aggregate("", static (current, next) => current + "\n\t\t\t" + next);
		var source = $$"""
		// <auto-generated />
		#nullable enable
		using System.Runtime.CompilerServices;
		using System.Diagnostics.CodeAnalysis;
		using System.Diagnostics;
		using TaggedEnum;

		{{@namespace}}

		[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
		{{data.Type.Modifiers}} static class {{formattedTypeName}}Extension {
			private static readonly Dictionary<{{data.Type.TypeName}}, string> NameValueMap = new(new {{formattedTypeName}}Comparer()) {
				{{nameValueMap}}
			};

			private static readonly Dictionary<string, {{data.Type.TypeName}}> ValueNameMap = new() {
				{{valueNameMap}}
			};

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static {{data.Type.DataTypeName}} Data(this {{data.Type.TypeName}} self) 
				=> self switch {
					{{valueDataConditionalBranches}}
					_ => throw new DataNotFoundException($"Data of {NameValueMap[self]} not found.")
				};

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static string ToStringFast(this {{data.Type.TypeName}} self) 
				=> self switch {
					{{valueNameConditionalBranches}}
					_ => throw new UnreachableException("Should never reach here.")
				};

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static bool HasName(this {{data.Type.TypeName}} self, string name) 
				=> self.ToStringFast() == name;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static bool HasData(this {{data.Type.TypeName}} self, {{data.Type.DataTypeName}} data) 
				=> self.Data() == data;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static bool Equals(this {{data.Type.TypeName}} self, {{data.Type.TypeName}} v) 
				=> self == v;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static {{data.Type.DataTypeName}} GetDataByName(string name) 
				=> name switch {
					{{nameDataConditionalBranches}}
					_ => throw new DataNotFoundException($"Data of {name} not found.")
				};

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static {{data.Type.TypeName}} GetValueByName(string name) 
				=> ValueNameMap[name];
		}
		
		internal sealed class {{formattedTypeName}}Comparer: IEqualityComparer<{{data.Type.TypeName}}> {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool Equals({{data.Type.TypeName}} x, {{data.Type.TypeName}} y) => x == y;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int GetHashCode([DisallowNull] {{data.Type.TypeName}} obj) => (int)obj;
		}
		""";
		ctx.AddSource($"{(data.Type.NamespaceName == "<global namespace>" ? "" : data.Type.NamespaceName["global::".Length..])}{data.Type.TypeName["global::".Length..]}TaggedEnum.g.cs", SourceText.From(source, Encoding.UTF8));
	}

	private static FinalResult ProcessMembers(TypeInfo enumTypeInfo, CancellationToken cancellationToken) {
		// if (!Debugger.IsAttached) {
		// 	Debugger.Launch();
		// }
		var dataType = enumTypeInfo.SemanticModel.Compilation.GetTypeByMetadataName(DataAttrName);
		var dataGenericType = enumTypeInfo.SemanticModel.Compilation.GetTypeByMetadataName(DataGenericAttrName);
		var memberDataMap = enumTypeInfo.Symbol.GetMembers()
			.OfType<IFieldSymbol>()
			.Select((m, _) => {
				var memberDataAttrData = m.GetAttributesDataByAttribute([dataType!, dataGenericType!]);

				var dataAttrCount = memberDataAttrData.Count();
				var memberInfo = new MemberInfo() {
					MemberName = m.Name,
					FieldValue = m.ConstantValue!.ToString()
				};

				if (dataAttrCount > 1) {
					// TODO error
				} else if (dataAttrCount == 0) {
					if (enumTypeInfo.UseAll) {
						memberInfo.Data = $@"""{m.Name}""";
						memberInfo.TypeName = "string";
						memberInfo.HasData = true;
					} else {
						// TODO error
					}
				} else {
					var dataAttrData = memberDataAttrData.FirstOrDefault();

					if (dataAttrData.ConstructorArguments.Length > 0) {
						var data = dataAttrData.ConstructorArguments.FirstOrDefault();
						var dataTypeName = data.Type?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
						if (dataTypeName != enumTypeInfo.DataTypeName) {
							// TODO error
						} else {
							memberInfo.Data = data.Value is string str ? $@"""{str}""" : data.Value?.ToString() ?? string.Empty;
							memberInfo.TypeName = dataTypeName; 
							memberInfo.HasData = true;
						}
					} else if (enumTypeInfo.DataTypeName != TypeInfo.DefaultType) {
						// TODO error
					} else {
						memberInfo.Data = $@"""{m.Name}""";
						memberInfo.TypeName = "string";
						memberInfo.HasData = true;
					}
				}
				return memberInfo;
			});
		
		return new FinalResult() {
			Type = enumTypeInfo,
			Members = memberDataMap
		};
	}
	
}

internal struct TypeInfo {
	public const string DefaultType = "string";
	public TypeInfo() {}
	public bool UseAll = false;
	public required SemanticModel SemanticModel { get; init; }
	public required EnumDeclarationSyntax Node { get; init; }
	public required INamedTypeSymbol Symbol { get; init; }
	public required string TypeName { get; init; }
	public required string NamespaceName { get; init; }
	// public ITypeSymbol ValueType { get; init; };
	public string DataTypeName = DefaultType;
	public required string Modifiers { get; init; }
}

internal struct MemberInfo {
	public MemberInfo() {}
	
	public bool HasData = false;
	
	public required string MemberName { get; init; }
	
	public string Data = string.Empty;

	public string TypeName = string.Empty;

	public required string FieldValue;
}

internal readonly struct FinalResult {
	public FinalResult() {}
	
	public required TypeInfo Type { get; init; }
	public required IEnumerable<MemberInfo> Members { get; init; }
}