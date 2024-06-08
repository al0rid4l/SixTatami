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

	private static readonly DiagnosticDescriptor Rule0 = new("TAGGED0001",
		"Multiple attributes are not allowed",
		"Multiple [Tagged] attributes are not allowed",
		"SourceGenerator", DiagnosticSeverity.Error, true,
		"Simultaneous use of [Tagged] and [Tagged<T>] attributes is not allowed.");

	private static readonly DiagnosticDescriptor Rule1 = new("TAGGED0002",
		"Invalid type argument",
		"The type arguments of [Tagged<T>] can only be primitive types",
		"SourceGenerator", DiagnosticSeverity.Error, true,
		"The type arguments of [Tagged<T>] can only be string, char, byte, sbyte, short, ushort, int, uint, long, ulong, float, double, bool.");

	private static readonly DiagnosticDescriptor Rule2 = new("TAGGED0003",
		"Multiple attributes are not allowed",
		"Multiple [Data] attributes are not allowed",
		"SourceGenerator", DiagnosticSeverity.Error, true,
		"Simultaneous use of [Data] and [Data<T>] attributes is not allowed.");

	private static readonly DiagnosticDescriptor Rule3 = new("TAGGED0004",
		"Invalid type argument",
		"The type arguments of [Data] and [Data<T>] must be consistent with [Tagged<T>]",
		"SourceGenerator", DiagnosticSeverity.Error, true,
		"The type arguments of [Data] and [Data<T>] must be consistent with [Tagged<T>], default is string.");

	private static readonly DiagnosticDescriptor Rule4 = new("TAGGED0005",
		"Attribute is missing",
		"[Data] or [Data<T>] is missing",
		"SourceGenerator", DiagnosticSeverity.Error, true,
		"[Data] or [Data<T>] is missing.");

	public void Initialize(IncrementalGeneratorInitializationContext context) {
		static bool IsEnum(SyntaxNode node, CancellationToken _) => node is EnumDeclarationSyntax;

		// if (!Debugger.IsAttached){
		// 	Debugger.Launch();
		// }
		
		Span<string> taggedAttrs = [TaggedAttrName, TaggedGenericAttrName];
		foreach (var taggedAttrName in taggedAttrs) {
			var taggedProvider = context.SyntaxProvider.ForAttributeWithMetadataName(taggedAttrName, 
				IsEnum,
				TransformEnumPayload
			)
			.Where(NotNullValueType)
			.Select(NullableToValueType);
			var taggedDiagnosticProvider = taggedProvider.Where(static v => v.Diagnostics is not null).Select(static (v, _) => v.Diagnostics!).Collect();
			var dataProvider = taggedProvider.Where(static v => v.Diagnostics is null).Select(ProcessMembers).Where(static v => v.Members is not null);
			var dataDiagnosticProvider = dataProvider.Where(static v => v.Members.Any(static m => m.Diagnostics is not null))
				.SelectMany(static (v, _) => v.Members!).Select(static (v, _) => v.Diagnostics).Collect();
			var provider = dataProvider.Where(static v => v.Members.All(static m => m.Diagnostics is null));
			var diagnosticProvider = dataDiagnosticProvider.Combine(taggedDiagnosticProvider)
				.Select(static (v, _)
				=> (v.Left.Aggregate(Enumerable.Empty<Diagnostic>(), static (cur, next) => cur.Concat(next ?? [])) ?? [])
					.Concat(v.Right.Aggregate(Enumerable.Empty<Diagnostic>(), static (cur, next) => cur.Concat(next ?? []))));

			context.RegisterSourceOutput(provider, GenerateSource);
			context.RegisterSourceOutput(diagnosticProvider, static (ctx, data) => {
				// if (!Debugger.IsAttached){
				// 	Debugger.Launch();
				// }
				foreach (var diagnostic in data) {
					ctx.ReportDiagnostic(diagnostic);
				}
			});
		}
	}

	private static TargetEnumInfo? TransformEnumPayload(GeneratorAttributeSyntaxContext ctx, CancellationToken cancellationToken) {
		// if (!Debugger.IsAttached){
		// 	Debugger.Launch();
		// }

		if (ctx is not {
			TargetNode: EnumDeclarationSyntax {
				Modifiers: var modifiers
			} targetNode,
			TargetSymbol: INamedTypeSymbol targetSymbol,
			SemanticModel: {
				Compilation: var compilation
			} semanticModel,
			Attributes: [{
				AttributeClass: {} attr
			} attrData, ..]
		}) {
			return null;
		}
		
		var taggedType = compilation.GetTypeByMetadataName(TaggedAttrName);
		var taggedGenericType = compilation.GetTypeByMetadataName(TaggedGenericAttrName);
		var taggedAttrList = targetSymbol.GetAttributesDataByAttribute([taggedType!, taggedGenericType!]);

		var data = new TargetEnumInfo() {
			Inline = !attrData.NamedArguments.Any(static v => v is {
				Key: "Inline",
				Value.Value: false
			}),
			UseSwitch = !attrData.NamedArguments.Any(static v => v is {
				Key: "UseSwitch",
				Value.Value: false
			}),
			SyntaxNode = targetNode,
			SemanticModel = semanticModel,
			Symbol = targetSymbol,
			TypeName = targetSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
			NamespaceName = targetSymbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat),
			Modifiers = modifiers.ToString()
		};
		
		if (taggedAttrList.Count() > 1) {
			var location = attrData.ApplicationSyntaxReference?.GetSyntax(cancellationToken).GetLocation();
			data.Diagnostics = [Diagnostic.Create(Rule0, location)];
			return data;
		}
		

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
				var location = attrData.ApplicationSyntaxReference?.GetSyntax(cancellationToken)
					.ChildNodes()
					.OfType<GenericNameSyntax>()
					.FirstOrDefault()
					.ChildNodes()
					.OfType<TypeArgumentListSyntax>()
					.FirstOrDefault()
					.GetLocation();
				data.Diagnostics = [Diagnostic.Create(Rule1, location)];
				return data;
			}
			data.DataTypeName = genericArg.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
		} else if (attrData.NamedArguments.Any(static v => v is {
			Key: "UseAll",
			Value.Value: true
		})) {
			data.UseAll = true;
		}
		
		return data;
	}

	private static TargetEnumInfo ProcessMembers(TargetEnumInfo enumTypeInfo, CancellationToken cancellationToken) {
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
					var locations = memberDataAttrData.Select(attrData => attrData.ApplicationSyntaxReference?.GetSyntax(cancellationToken).GetLocation());
					memberInfo.Diagnostics = locations.Select(static location => Diagnostic.Create(Rule2, location));
				} else if (dataAttrCount == 0) {
					if (enumTypeInfo.UseAll) {
						memberInfo.Data = $@"""{m.Name}""";
						memberInfo.TypeName = "string";
						memberInfo.HasData = true;
					} else {
						var location = m.DeclaringSyntaxReferences[0].GetSyntax(cancellationToken).GetLocation();
						memberInfo.Diagnostics = [Diagnostic.Create(Rule4, location)];
					}
				} else {
					var dataAttrData = memberDataAttrData.FirstOrDefault();

					if (dataAttrData.ConstructorArguments.Length > 0) {
						var data = dataAttrData.ConstructorArguments.FirstOrDefault();
						var dataTypeName = data.Type?.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
						if (dataTypeName != enumTypeInfo.DataTypeName) {
							var locations = memberDataAttrData.Select(attrData => attrData.AttributeClass!.IsGenericType ?
								attrData.ApplicationSyntaxReference?.GetSyntax(cancellationToken)
									.ChildNodes()
									.OfType<GenericNameSyntax>()
									.FirstOrDefault()
									.ChildNodes()
									.OfType<TypeArgumentListSyntax>()
									.FirstOrDefault()
									.GetLocation()
								: attrData.ApplicationSyntaxReference?.GetSyntax(cancellationToken).GetLocation());
							memberInfo.Diagnostics = locations.Select(static location => Diagnostic.Create(Rule3, location));
						} else {
							memberInfo.Data = data.Value is string str ? $@"""{str}""" : data.Value?.ToString() ?? string.Empty;
							memberInfo.TypeName = dataTypeName; 
							memberInfo.HasData = true;
						}
					} else if (enumTypeInfo.DataTypeName != TargetEnumInfo.DefaultType) {
						var locations = memberDataAttrData.Select(attrData => attrData.AttributeClass!.IsGenericType ?
							attrData.ApplicationSyntaxReference?.GetSyntax(cancellationToken)
								.ChildNodes()
								.OfType<GenericNameSyntax>()
								.FirstOrDefault()
								.ChildNodes()
								.OfType<TypeArgumentListSyntax>()
								.FirstOrDefault()
								.GetLocation()
							: attrData.ApplicationSyntaxReference?.GetSyntax(cancellationToken).GetLocation());
						memberInfo.Diagnostics = locations.Select(static location => Diagnostic.Create(Rule3, location));
					} else {
						memberInfo.Data = $@"""{m.Name}""";
						memberInfo.TypeName = "string";
						memberInfo.HasData = true;
					}
				}
				return memberInfo;
			});
		
		enumTypeInfo.Members = memberDataMap;
		return enumTypeInfo;
	}
	
	private void GenerateSource(SourceProductionContext ctx, TargetEnumInfo data) {
		// if (!Debugger.IsAttached) {
		// 	Debugger.Launch();
		// }
		var globalLength = "global::".Length;
		var @namespace = data.NamespaceName == "<global namespace>" ? "" : $"namespace {data.NamespaceName[globalLength..]};";
		var fullNamespace = data.NamespaceName == "<global namespace>" ? "" : data.NamespaceName[globalLength..];
		var fullTypeName = data.TypeName[globalLength..];
		var formattedNamespace = string.Concat(fullNamespace.Split('.'));
		var formattedTypeName = string.Concat(fullTypeName.Split('.'));
		var formattedDataTypeName = $"{formattedTypeName}{data.DataTypeName}";
		var valueNameMap = data.Members
			.Select(m => $@"{{{data.TypeName}.{m.MemberName}, ""{m.MemberName}""}},")
			.Aggregate("", static (current, next) => current + "\n\t\t" + next);
		var nameValueMap = data.Members
			.Select(m => $@"{{""{m.MemberName}"", {data.TypeName}.{m.MemberName}}},")
			.Aggregate("", static (current, next) => current + "\n\t\t" + next);
		var valueDataMap = data.Members
			.Select(m => $@"{{{data.TypeName}.{m.MemberName}, ({m.TypeName}){m.Data}}},")
			.Aggregate("", static (current, next) => current + "\n\t\t" + next);
		var dataValueMap = data.Members
			.Select(m => $@"{{({m.TypeName}){m.Data}, {data.TypeName}.{m.MemberName}}},")
			.Aggregate("", static (current, next) => current + "\n\t\t" + next);
		var nameDataMap = data.Members
			.Select(m => $@"{{""{m.MemberName}"", ({m.TypeName}){m.Data}}},")
			.Aggregate("", static (current, next) => current + "\n\t\t" + next);
		var valueDataConditionalBranches = data.Members
			.Where(static m => m.HasData)
			.Select(m => $"{data.TypeName}.{m.MemberName} => ({m.TypeName}){m.Data},")
			.Aggregate("", static (current, next) => current + "\n\t\t\t" + next);
		var dataValueConditionalBranches = data.Members
			.Where(static m => m.HasData)
			.Select(m => $"({m.TypeName}){m.Data} => {data.TypeName}.{m.MemberName},")
			.Aggregate("", static (current, next) => current + "\n\t\t\t" + next);
		var nameDataConditionalBranches = data.Members
			.Where(static m => m.HasData)
			.Select(m => $@"""{m.MemberName}"" => ({m.TypeName}){m.Data},")
			.Aggregate("", static (current, next) => current + "\n\t\t\t" + next);
		var valueNameConditionalBranches = data.Members
			.Select(m => $@"{data.TypeName}.{m.MemberName} => ""{m.MemberName}"",")
			.Aggregate("", static (current, next) => current + "\n\t\t\t" + next);
		var nameValueConditionalBranches = data.Members
			.Select(m => $@"""{m.MemberName}"" => {data.TypeName}.{m.MemberName},")
			.Aggregate("", static (current, next) => current + "\n\t\t\t" + next);
		var inlineAttr = data.Inline ? "[MethodImpl(MethodImplOptions.AggressiveInlining)]" : "";
		
		var getHashCodeImpl = data.DataTypeName == "short" ||
													data.DataTypeName == "int" ||
													data.DataTypeName == "long" ||
													data.DataTypeName == "ushort" ||
													data.DataTypeName == "uint" ||
													data.DataTypeName == "ulong" ||
													data.DataTypeName == "byte" ||
													data.DataTypeName == "sbyte"
		 											? "(int)obj" : "obj.GetHashCode()";
		
		var nameValueMapDict = data.UseSwitch ? "" : $$"""
			private static readonly Dictionary<string, {{data.TypeName}}> NameValueMap = new() {
				{{nameValueMap}}
			};
		""";

		var nameDataMapDict = data.UseSwitch ? "" : $$"""
			private static readonly Dictionary<string, {{data.DataTypeName}}> NameDataMap = new() {
				{{nameDataMap}}
			};
		""";

		var valueDataMapDict = data.UseSwitch ? "" : $$"""
			private static readonly Dictionary<{{data.TypeName}}, {{data.DataTypeName}}> ValueDataMap = new(new {{formattedTypeName}}Comparer()) {
				{{valueDataMap}}
			};
		""";

		var dataValueMapDict = data.UseSwitch ? "" : $$"""
			private static readonly Dictionary<{{data.DataTypeName}}, {{data.TypeName}}> DataValueMap = new(new {{formattedDataTypeName}}Comparer()) {
				{{dataValueMap}}
			};
		""";
		
		var dataMethod = data.UseSwitch ? $$"""
			=> self switch {
					{{valueDataConditionalBranches}}
					_ => throw new DataNotFoundException($"Data of {ValueNameMap[self]} not found.")
				};
		""" : $$"""
		{
				if (ValueDataMap.TryGetValue(self, out var data)) {
					return data;
				} else {
					throw new DataNotFoundException($"Data of {ValueNameMap[self]} not found.");
				}
			}
		""";

		var toStringFastMethod = data.UseSwitch ? $$"""
			=> self switch {
					{{valueNameConditionalBranches}}
					_ => throw new UnreachableException("Never reach here.")
				};
		""" : $$"""
		{
				if (ValueNameMap.TryGetValue(self, out var name)) {
					return name;
				} else {
					throw new DataNotFoundException("Never reach here.");
				}
			}
		""";

		var tryGetDataByNameMethod = data.UseSwitch ? $$"""
			v = name switch {
				{{nameDataConditionalBranches}}
					_ => null
				};
				return v is not null;
		""" : $$"""
			var result = NameDataMap.TryGetValue(name, out var vv);
				v = vv;
				return result;
		""";

		var tryGetValueByNameMethod = data.UseSwitch ? $$"""
			v = name switch {
				{{nameValueConditionalBranches}}
					_ => null
				};
				return v is not null;
		""" : $$"""
			var result = NameValueMap.TryGetValue(name, out var vv);
				v = vv;
				return result;
		""";

		var tryGetValueByDataMethod = data.UseSwitch ? $$"""
			v = data switch {
				{{dataValueConditionalBranches}}
					_ => null
				};
				return v is not null;
		""" : $$"""
			var result = DataValueMap.TryGetValue(data, out var vv);
				v = vv;
				return result;
		""";
		
		var dataKeyEqualMethod = data.DataTypeName == "string"
			? $"public bool Equals({data.DataTypeName}? x, {data.DataTypeName}? y) => x == y;"
			: $"public bool Equals({data.DataTypeName} x, {data.DataTypeName} y) => x == y;";
		
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
		{{data.Modifiers}} static class {{formattedTypeName}}Extension {
			private static readonly Dictionary<{{data.TypeName}}, string> ValueNameMap = new(new {{formattedTypeName}}Comparer()) {
				{{valueNameMap}}
			};

			{{nameValueMapDict}}

			{{nameDataMapDict}}

			{{dataValueMapDict}}

			{{valueDataMapDict}}

			{{inlineAttr}}
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static {{data.DataTypeName}} Data(this {{data.TypeName}} self) 
			{{dataMethod}}

			{{inlineAttr}}
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static string ToStringFast(this {{data.TypeName}} self) 
			{{toStringFastMethod}}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static bool HasName(this {{data.TypeName}} self, string name) 
				=> self.ToStringFast() == name;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static bool HasData(this {{data.TypeName}} self, {{data.DataTypeName}} data) 
				=> self.Data() == data;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static bool Equals(this {{data.TypeName}} self, {{data.TypeName}} v) 
				=> self == v;

			{{inlineAttr}}
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static bool TryGetDataByName(string name, [NotNullWhen(true)]out {{data.DataTypeName}}? v) {
			{{tryGetDataByNameMethod}}
			}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static bool TryGetValueByName(string name, [NotNullWhen(true)]out {{data.TypeName}}? v) {
			{{tryGetValueByNameMethod}}
			}

			{{inlineAttr}}
			[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
			[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
			public static bool TryGetValueByData({{data.DataTypeName}} data, [NotNullWhen(true)]out {{data.TypeName}}? v) {
			{{tryGetValueByDataMethod}}
			}
		}
		
		internal sealed class {{formattedTypeName}}Comparer: IEqualityComparer<{{data.TypeName}}> {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool Equals({{data.TypeName}} x, {{data.TypeName}} y) => x == y;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int GetHashCode([DisallowNull] {{data.TypeName}} obj) => (int)obj;
		}

		internal sealed class {{formattedDataTypeName}}Comparer: IEqualityComparer<{{data.DataTypeName}}> {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			{{dataKeyEqualMethod}}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int GetHashCode([DisallowNull] {{data.DataTypeName}} obj) => {{getHashCodeImpl}};
		}
		""";
		ctx.AddSource($"{(data.NamespaceName == "<global namespace>" ? "" : data.NamespaceName["global::".Length..])}{data.TypeName["global::".Length..]}TaggedEnum.g.cs", SourceText.From(source, Encoding.UTF8));
	}
}