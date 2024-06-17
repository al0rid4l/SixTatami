using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

using Cysharp.Text;

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

	private static readonly DiagnosticDescriptor Rule5 = new("TAGGED0006",
		"Duplicate Data",
		"[Data] or [Data<T>] is duplicate",
		"SourceGenerator", DiagnosticSeverity.Error, true,
		"[Data] or [Data<T>] is duplicate.");

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
			AllowDuplicate = attrData.NamedArguments.Any(static v => v is {
				Key: "AllowDuplicate",
				Value.Value: true
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
				&& genericArg.SpecialType != SpecialType.System_Decimal
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
		var dataset = new Dictionary<string, bool>();
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
						memberInfo.Data = ZString.Concat('"', m.Name, '"');
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
							memberInfo.Data = data.Value is string str ? ZString.Concat('"', str, '"') : data.Value is char c ? ZString.Concat('\'', c, '\'') : data.Value?.ToString().ToLower() ?? string.Empty;
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
						memberInfo.Data = ZString.Concat('"', m.Name, '"');
						memberInfo.TypeName = "string";
						memberInfo.HasData = true;
					}
				}

				if (!enumTypeInfo.AllowDuplicate) {
					if (dataset.TryGetValue(memberInfo.Data, out var _)) {
						var locations = memberDataAttrData.Select(attrData =>
							attrData.ApplicationSyntaxReference?.GetSyntax(cancellationToken)
								.ChildNodes()
								.OfType<AttributeArgumentListSyntax>()
								.FirstOrDefault()
								.ChildNodes()
								.OfType<AttributeArgumentSyntax>()
								.FirstOrDefault()
								.GetLocation()
							);
						memberInfo.Diagnostics = locations.Select(static location => Diagnostic.Create(Rule5, location));
					} else {
						dataset.Add(memberInfo.Data, true);
					}
				}
				return memberInfo;
			}).ToArray();

		enumTypeInfo.Members = memberDataMap;
		return enumTypeInfo;
	}

	private void GenerateSource(SourceProductionContext ctx, TargetEnumInfo data) {
		// if (!Debugger.IsAttached) {
		// 	Debugger.Launch();
		// }
		using var sb = ZString.CreateUtf8StringBuilder();
		var globalLength = "global::".Length;
		var namespaceName = data.NamespaceName[globalLength..];
		var @namespace = data.NamespaceName == "<global namespace>" ? "" : ZString.Concat("namespace ", namespaceName, ";");
		var fullNamespace = data.NamespaceName == "<global namespace>" ? "" : namespaceName;
		var fullTypeName = data.TypeName[globalLength..];
		var formattedNamespace = ZString.Concat(fullNamespace.Split('.'));
		var formattedTypeName = ZString.Concat(fullTypeName.Split('.'));
		var formattedDataTypeName = ZString.Concat(formattedTypeName, data.DataTypeName);
		var valueNameMap = data.Members
			.Select(m => ZString.Concat('{', data.TypeName, '.', m.MemberName, ", \"", m.MemberName, "\"},"))
			.Aggregate(sb, static (sb, next) => {
				sb.Append("\n\t\t");
				sb.Append(next);
				return sb;
			}).ToString();
		var nameValueMap = data.Members
			.Select(m => ZString.Concat("{\"", m.MemberName, "\", ", data.TypeName, '.', m.MemberName, "},"))
			.Aggregate(sb, static (sb, next) => {
				sb.Append("\n\t\t");
				sb.Append(next);
				return sb;
			}).ToString();
		var valueDataMap = data.Members
			.Select(m => ZString.Concat("{", data.TypeName, '.', m.MemberName, ", (", m.TypeName, ')', m.Data, "},"))
			.Aggregate(sb, static (sb, next) => {
				sb.Append("\n\t\t");
				sb.Append(next);
				return sb;
			}).ToString();
		var dataValueMap = data.Members
			.Select(m => ZString.Concat("{(", m.TypeName, ')', m.Data, ", ", data.TypeName, '.', m.MemberName, "},"))
			.Aggregate(sb, static (sb, next) => {
				sb.Append("\n\t\t");
				sb.Append(next);
				return sb;
			}).ToString();
		var nameDataMap = data.Members
			.Select(m => ZString.Concat("{\"", m.MemberName, "\", (", m.TypeName, ')', m.Data, "},"))
			.Aggregate(sb, static (sb, next) => {
				sb.Append("\n\t\t");
				sb.Append(next);
				return sb;
			}).ToString();
		var valueDataConditionalBranches = data.Members
			.Where(static m => m.HasData)
			.Select(m => ZString.Concat(data.TypeName, '.', m.MemberName, " => (", m.TypeName, ')', m.Data, ','))
			.Aggregate(sb, static (sb, next) => {
				sb.Append("\n\t\t\t");
				sb.Append(next);
				return sb;
			}).ToString();
		var dataValueConditionalBranches = data.Members
			.Where(static m => m.HasData)
			.Select(m => ZString.Concat('(', m.TypeName, ')', m.Data, " => ", data.TypeName, '.', m.MemberName, ','))
			.Aggregate(sb, static (sb, next) => {
				sb.Append("\n\t\t\t");
				sb.Append(next);
				return sb;
			}).ToString();
		var nameDataConditionalBranches = data.Members
			.Where(static m => m.HasData)
			.Select(m => ZString.Concat('"', m.MemberName, "\" => (", m.TypeName, ')', m.Data, ','))
			.Aggregate(sb, static (sb, next) => {
				sb.Append("\n\t\t\t");
				sb.Append(next);
				return sb;
			}).ToString();
		var valueNameConditionalBranches = data.Members
			.Select(m => ZString.Concat(data.TypeName, '.', m.MemberName, " => \"", m.MemberName, "\","))
			.Aggregate(sb, static (sb, next) => {
				sb.Append("\n\t\t\t");
				sb.Append(next);
				return sb;
			}).ToString();
		var nameValueConditionalBranches = data.Members
			.Select(m => ZString.Concat('"', m.MemberName, "\" => ", data.TypeName, '.', m.MemberName, ','))
			.Aggregate(sb, static (sb, next) => {
				sb.Append("\n\t\t\t");
				sb.Append(next);
				return sb;
			}).ToString();
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

		var dataValueMapDict = data.UseSwitch || data.AllowDuplicate ? "" : $$"""
			private static readonly Dictionary<{{data.DataTypeName}}, {{data.TypeName}}> DataValueMap = new(new {{formattedDataTypeName}}Comparer()) {
				{{dataValueMap}}
			};
		""";

		var generatedCodeAttr = """
		[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("TaggedEnum", "1.0")]
		""";

		var dataMethod = data.UseSwitch ? $$"""
		{{inlineAttr}}
			{{generatedCodeAttr}}
			public static {{data.DataTypeName}} Data(this {{data.TypeName}} self)
			=> self switch {
			{{valueDataConditionalBranches}}
				_ => throw new DataNotFoundException($"Data of {ValueNameMap[self]} not found.")
			};
		""" : $$"""
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			{{generatedCodeAttr}}
			public static {{data.DataTypeName}} Data(this {{data.TypeName}} self) {
				if (ValueDataMap.TryGetValue(self, out var data)) {
					return data;
				} else {
					throw new DataNotFoundException($"Data of {ValueNameMap[self]} not found.");
				}
			}
		""";

		var toStringFastMethod = data.UseSwitch ? $$"""
		{{inlineAttr}}
			{{generatedCodeAttr}}
			public static string ToStringFast(this {{data.TypeName}} self)
			=> self switch {
			{{valueNameConditionalBranches}}
				_ => throw new UnreachableException("Never reach here.")
			};
		""" : $$"""
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			{{generatedCodeAttr}}
			public static string ToStringFast(this {{data.TypeName}} self) {
				if (ValueNameMap.TryGetValue(self, out var name)) {
					return name;
				} else {
					throw new UnreachableException("Never reach here.");
				}
			}
		""";

		var tryGetDataByNameMethod = data.UseSwitch ? $$"""
		{{inlineAttr}}
		{{generatedCodeAttr}}
			public static bool TryGetDataByName(string name, [NotNullWhen(true)]out {{data.DataTypeName}}? v) {
				v = name switch {
				{{nameDataConditionalBranches}}
					_ => null
				};
				return v is not null;
			}
		""" : $$"""
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			{{generatedCodeAttr}}
			public static bool TryGetDataByName(string name, [NotNullWhen(true)]out {{data.DataTypeName}}? v) {
				var result = NameDataMap.TryGetValue(name, out var vv);
				v = vv;
				return result;
			}
		""";

		var tryGetValueByNameMethod = data.UseSwitch ? $$"""
		{{inlineAttr}}
		{{generatedCodeAttr}}
			public static bool TryGetValueByName(string name, [NotNullWhen(true)]out {{data.TypeName}}? v) {
				v = name switch {
				{{nameValueConditionalBranches}}
					_ => null
				};
				return v is not null;
			}
		""" : $$"""
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			{{generatedCodeAttr}}
			public static bool TryGetValueByName(string name, [NotNullWhen(true)]out {{data.TypeName}}? v) {
				var result = NameValueMap.TryGetValue(name, out var vv);
				v = vv;
				return result;
			}
		""";

		var tryGetValueByDataMethod = data.AllowDuplicate ? "" : data.UseSwitch ? $$"""
		{{inlineAttr}}
		{{generatedCodeAttr}}
			public static bool TryGetValueByData({{data.DataTypeName}} data, [NotNullWhen(true)]out {{data.TypeName}}? v) {
				v = data switch {
				{{dataValueConditionalBranches}}
					{{(data.DataTypeName == "bool" ? "" : "_ => null")}}
				};
				return v is not null;
			}
		""" : $$"""
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			{{generatedCodeAttr}}
			public static bool TryGetValueByData({{data.DataTypeName}} data, [NotNullWhen(true)]out {{data.TypeName}}? v) {
				var result = DataValueMap.TryGetValue(data, out var vv);
				v = vv;
				return result;
			}
		""";

		var dataKeyEqualMethod = data.DataTypeName == "string"
			? $"public bool Equals({data.DataTypeName}? x, {data.DataTypeName}? y) => x == y;"
			: $"public bool Equals({data.DataTypeName} x, {data.DataTypeName} y) => x == y;";

		var valueToDataConverter = data.AllowDuplicate ? "" : $$"""
		{{generatedCodeAttr}}
		public sealed class {{formattedTypeName}}ToDataConverter: JsonConverter<{{data.TypeName}}> {
			public override {{data.TypeName}} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				{{PrimitiveTypeToDeclareType(data.DataTypeName)}} token = reader.{{PrimitiveTypeToReadJsonType(data.DataTypeName)}};
				return {{(data.DataTypeName == "string" ? $$"""
					string.IsNullOrEmpty(token)
					? throw new JsonException($"Couldn't convert data \"{token}\" to {{data.TypeName}}.")
					: {{formattedTypeName}}Extension.TryGetValueByData(token, out var result)
						? result.Value : throw new JsonException($"Couldn't find \"{token}\" in {{data.TypeName}} data.")
				""" : $$"""
					{{formattedTypeName}}Extension.TryGetValueByData(token, out var result)
						? result.Value : throw new JsonException($"Couldn't find \"{token}\" in {{data.TypeName}} data.")
				""")}};
			}

			public override void Write(Utf8JsonWriter writer, {{data.TypeName}} value, JsonSerializerOptions options) {
				writer.{{PrimitiveTypeToWriteJsonType(data.DataTypeName, "value.Data()")}};
			}
		}
		""";

		var valueToNameConverter = $$"""
		{{generatedCodeAttr}}
		public sealed class {{formattedTypeName}}ToNameConverter: JsonConverter<{{data.TypeName}}> {
			public override {{data.TypeName}} Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				string? token = reader.GetString();
				return string.IsNullOrEmpty(token)
					? throw new JsonException($"Couldn't convert name \"{token}\" to {{data.TypeName}}.")
					: {{formattedTypeName}}Extension.TryGetValueByName(token, out var result)
						? result.Value : throw new JsonException($"Couldn't find \"{token}\" in {{data.TypeName}} name.");
			}

			public override void Write(Utf8JsonWriter writer, {{data.TypeName}} value, JsonSerializerOptions options) {
				writer.WriteStringValue(value.ToStringFast());
			}
		}
		""";

		var valueToDataArrayConverter = data.AllowDuplicate ? "" : $$"""
		{{generatedCodeAttr}}
		public sealed class {{formattedTypeName}}ArrayToDataArrayConverter: JsonConverter<{{data.TypeName}}[]> {
			public override {{data.TypeName}}[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				var list = new List<{{data.TypeName}}>();
				while (reader.Read()) {
					switch (reader.TokenType) {
						case JsonTokenType.StartArray:
							continue;
						case JsonTokenType.EndArray:
							goto label;
						case JsonTokenType.String:
							var v = {{ReaderGetToken(data.DataTypeName)}};
							list.Add({{formattedTypeName}}Extension.TryGetValueByData(
								v,
								out var item)
								? item!.Value : throw new JsonException($"Couldn't find \"{v}\" in {{data.TypeName}} data."));
							continue;
						default:
							continue;
					}
				}
				label:
				return list.ToArray();
			}

			public override void Write(Utf8JsonWriter writer, {{data.TypeName}}[] arr, JsonSerializerOptions options) {
				writer.WriteStartArray();
				for (int i = 0, len = arr.Length; i < len; ++i) {
					writer.{{PrimitiveTypeToWriteJsonType(data.DataTypeName, "arr[i].Data()")}};
				}
				writer.WriteEndArray();
			}
		}
		""";

		var nullableValueToDataArrayConverter = data.AllowDuplicate ? "" : $$"""
		{{generatedCodeAttr}}
		public sealed class Nullable{{formattedTypeName}}ArrayToDataArrayConverter: JsonConverter<{{data.TypeName}}?[]> {
			public override {{data.TypeName}}?[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				var list = new List<{{data.TypeName}}?>();
				while (reader.Read()) {
					switch (reader.TokenType) {
						case JsonTokenType.StartArray:
							continue;
						case JsonTokenType.EndArray:
							goto label;
						case JsonTokenType.Null:
							list.Add(null);
							continue;
						case JsonTokenType.String:
							var v = {{ReaderGetToken(data.DataTypeName)}};
							list.Add({{formattedTypeName}}Extension.TryGetValueByData(
								v,
								out var item)
								? item!.Value : throw new JsonException($"Couldn't find \"{v}\" in {{data.TypeName}} data."));
							continue;
						default:
							continue;
					}
				}
				label:
				return list.ToArray();
			}

			public override void Write(Utf8JsonWriter writer, {{data.TypeName}}?[] arr, JsonSerializerOptions options) {
				writer.WriteStartArray();
				for (int i = 0, len = arr.Length; i < len; ++i) {
					var v = arr[i];
					if (v is null) {
						writer.WriteNullValue();
					} else {
						writer.{{PrimitiveTypeToWriteJsonType(data.DataTypeName, "v.Value.Data()")}};
					}

				}
				writer.WriteEndArray();
			}
		}
		""";

		var valueToDataEnumerableConverter = data.AllowDuplicate ? "" : $$"""
		{{generatedCodeAttr}}
		public sealed class {{formattedTypeName}}EnumerableToDataEnumerableConverter: JsonConverter<IEnumerable<{{data.TypeName}}>> {
			public override IEnumerable<{{data.TypeName}}> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				var list = new List<{{data.TypeName}}>();
				while (reader.Read()) {
					switch (reader.TokenType) {
						case JsonTokenType.StartArray:
							continue;
						case JsonTokenType.EndArray:
							goto label;
						case JsonTokenType.String:
							var v = {{ReaderGetToken(data.DataTypeName)}};
							list.Add({{formattedTypeName}}Extension.TryGetValueByData(
								v,
								out var item)
								? item!.Value : throw new JsonException($"Couldn't find \"{v}\" in {{data.TypeName}} data."));
							continue;
						default:
							continue;
					}
				}
				label:
				return list.ToArray();
			}

			public override void Write(Utf8JsonWriter writer, IEnumerable<{{data.TypeName}}> values, JsonSerializerOptions options) {
				writer.WriteStartArray();
				foreach (var v in values) {
					writer.{{PrimitiveTypeToWriteJsonType(data.DataTypeName, "v.Data()")}};
				}
				writer.WriteEndArray();
			}
		}
		""";

		var nullableValueToDataEnumerableConverter = data.AllowDuplicate ? "" : $$"""
		{{generatedCodeAttr}}
		public sealed class Nullable{{formattedTypeName}}EnumerableToDataEnumerableConverter: JsonConverter<IEnumerable<{{data.TypeName}}?>> {
			public override IEnumerable<{{data.TypeName}}?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				var list = new List<{{data.TypeName}}?>();
				while (reader.Read()) {
					switch (reader.TokenType) {
						case JsonTokenType.StartArray:
							continue;
						case JsonTokenType.EndArray:
							goto label;
						case JsonTokenType.Null:
							list.Add(null);
							continue;
						case JsonTokenType.String:
							var v = {{ReaderGetToken(data.DataTypeName)}};
							list.Add({{formattedTypeName}}Extension.TryGetValueByData(
								v,
								out var item)
								? item!.Value : throw new JsonException($"Couldn't find \"{v}\" in {{data.TypeName}} data."));
							continue;
						default:
							continue;
					}
				}
				label:
				return list.ToArray();
			}

			public override void Write(Utf8JsonWriter writer, IEnumerable<{{data.TypeName}}?> values, JsonSerializerOptions options) {
				writer.WriteStartArray();
				foreach (var v in values) {
					if (v is null) {
						writer.WriteNullValue();
					} else {
						writer.{{PrimitiveTypeToWriteJsonType(data.DataTypeName, "v.Value.Data()")}};
					}
				}
				writer.WriteEndArray();
			}
		}
		""";

		var valueToNameArrayConverter = $$"""
		{{generatedCodeAttr}}
		public sealed class {{formattedTypeName}}ArrayToNameArrayConverter: JsonConverter<{{data.TypeName}}[]> {
			public override {{data.TypeName}}[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				var list = new List<{{data.TypeName}}>();
				while (reader.Read()) {
					switch (reader.TokenType) {
						case JsonTokenType.StartArray:
							continue;
						case JsonTokenType.EndArray:
							goto label;
						case JsonTokenType.String:
							list.Add({{formattedTypeName}}Extension.TryGetValueByName(
								{{ReaderGetToken("string")}},
								out var item)
								? item!.Value : throw new JsonException($"Couldn't find \"{token}\" in {{data.TypeName}} name."));
							continue;
						default:
							continue;
					}
				}
				label:
				return list.ToArray();
			}

			public override void Write(Utf8JsonWriter writer, {{data.TypeName}}[] arr, JsonSerializerOptions options) {
				writer.WriteStartArray();
				for (int i = 0, len = arr.Length; i < len; ++i) {
					writer.{{PrimitiveTypeToWriteJsonType("string", "arr[i].ToStringFast()")}};
				}
				writer.WriteEndArray();
			}
		}
		""";

		var nullableValueToNameArrayConverter = $$"""
		{{generatedCodeAttr}}
		public sealed class Nullable{{formattedTypeName}}ArrayToNameArrayConverter: JsonConverter<{{data.TypeName}}?[]> {
			public override {{data.TypeName}}?[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				var list = new List<{{data.TypeName}}?>();
				while (reader.Read()) {
					switch (reader.TokenType) {
						case JsonTokenType.StartArray:
							continue;
						case JsonTokenType.EndArray:
							goto label;
						case JsonTokenType.Null:
							list.Add(null);
							continue;
						case JsonTokenType.String:
							list.Add({{formattedTypeName}}Extension.TryGetValueByName(
								{{ReaderGetToken("string")}},
								out var item)
								? item!.Value : throw new JsonException($"Couldn't find \"{token}\" in {{data.TypeName}} name."));
							continue;
						default:
							continue;
					}
				}
				label:
				return list.ToArray();
			}

			public override void Write(Utf8JsonWriter writer, {{data.TypeName}}?[] arr, JsonSerializerOptions options) {
				writer.WriteStartArray();
				for (int i = 0, len = arr.Length; i < len; ++i) {
					var v = arr[i];
					if (v is null) {
						writer.WriteNullValue();
					} else {
						writer.{{PrimitiveTypeToWriteJsonType("string", "v.Value.ToStringFast()")}};
					}
				}
				writer.WriteEndArray();
			}
		}
		""";

		var valueToNameEnumerableConverter = $$"""
		{{generatedCodeAttr}}
		public sealed class {{formattedTypeName}}EnumerableToNameEnumerableConverter: JsonConverter<IEnumerable<{{data.TypeName}}>> {
			public override IEnumerable<{{data.TypeName}}> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				var list = new List<{{data.TypeName}}>();
				while (reader.Read()) {
					switch (reader.TokenType) {
						case JsonTokenType.StartArray:
							continue;
						case JsonTokenType.EndArray:
							goto label;
						case JsonTokenType.String:
							list.Add({{formattedTypeName}}Extension.TryGetValueByName(
								{{ReaderGetToken("string")}},
								out var item)
								? item!.Value : throw new JsonException($"Couldn't find \"{token}\" in {{data.TypeName}} name."));
							continue;
						default:
							continue;
					}
				}
				label:
				return list.ToArray();
			}

			public override void Write(Utf8JsonWriter writer, IEnumerable<{{data.TypeName}}> values, JsonSerializerOptions options) {
				writer.WriteStartArray();
				foreach (var v in values) {
					writer.{{PrimitiveTypeToWriteJsonType("string", "v.ToStringFast()")}};
				}
				writer.WriteEndArray();
			}
		}
		""";

		var nullableValueToNameEnumerableConverter = $$"""
		{{generatedCodeAttr}}
		public sealed class Nullable{{formattedTypeName}}EnumerableToNameEnumerableConverter: JsonConverter<IEnumerable<{{data.TypeName}}?>> {
			public override IEnumerable<{{data.TypeName}}?> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
				var list = new List<{{data.TypeName}}?>();
				while (reader.Read()) {
					switch (reader.TokenType) {
						case JsonTokenType.StartArray:
							continue;
						case JsonTokenType.EndArray:
							goto label;
						case JsonTokenType.Null:
							list.Add(null);
							continue;
						case JsonTokenType.String:
							list.Add({{formattedTypeName}}Extension.TryGetValueByName(
								{{ReaderGetToken("string")}},
								out var item)
								? item!.Value : throw new JsonException($"Couldn't find \"{token}\" in {{data.TypeName}} name."));
							continue;
						default:
							continue;
					}
				}
				label:
				return list.ToArray();
			}

			public override void Write(Utf8JsonWriter writer, IEnumerable<{{data.TypeName}}?> values, JsonSerializerOptions options) {
				writer.WriteStartArray();
				foreach (var v in values) {
					if (v is null) {
						writer.WriteNullValue();
					} else {
						writer.{{PrimitiveTypeToWriteJsonType("string", "v.Value.ToStringFast()")}};
					}
				}
				writer.WriteEndArray();
			}
		}
		""";

		var source = $$"""
		// <auto-generated />
		#nullable enable
		using System.Runtime.CompilerServices;
		using System.Diagnostics.CodeAnalysis;
		using System.Diagnostics;
		using System.Text.Json;
		using System.Text.Json.Serialization;
		using TaggedEnum;

		{{@namespace}}

		{{generatedCodeAttr}}
		{{data.Modifiers}} static class {{formattedTypeName}}Extension {
			private static readonly Dictionary<{{data.TypeName}}, string> ValueNameMap = new(new {{formattedTypeName}}Comparer()) {
				{{valueNameMap}}
			};

			{{nameValueMapDict}}

			{{nameDataMapDict}}

			{{dataValueMapDict}}

			{{valueDataMapDict}}

			{{dataMethod}}

			{{toStringFastMethod}}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			{{generatedCodeAttr}}
			public static bool HasName(this {{data.TypeName}} self, string name)
				=> self.ToStringFast() == name;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			{{generatedCodeAttr}}
			public static bool HasData(this {{data.TypeName}} self, {{data.DataTypeName}} data)
				=> self.Data() == data;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			{{generatedCodeAttr}}
			public static bool Equals(this {{data.TypeName}} self, {{data.TypeName}} v)
				=> self == v;

			{{tryGetDataByNameMethod}}

			{{tryGetValueByNameMethod}}

			{{tryGetValueByDataMethod}}
		}

		{{generatedCodeAttr}}
		internal sealed class {{formattedTypeName}}Comparer: IEqualityComparer<{{data.TypeName}}> {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public bool Equals({{data.TypeName}} x, {{data.TypeName}} y) => x == y;

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int GetHashCode([DisallowNull] {{data.TypeName}} obj) => (int)obj;
		}

		{{generatedCodeAttr}}
		internal sealed class {{formattedDataTypeName}}Comparer: IEqualityComparer<{{data.DataTypeName}}> {
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			{{dataKeyEqualMethod}}

			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public int GetHashCode([DisallowNull] {{data.DataTypeName}} obj) => {{getHashCodeImpl}};
		}

		{{valueToDataConverter}}

		{{valueToDataArrayConverter}}

		{{nullableValueToDataArrayConverter}}

		{{valueToDataEnumerableConverter}}

		{{nullableValueToDataEnumerableConverter}}

		{{valueToNameConverter}}

		{{valueToNameArrayConverter}}

		{{nullableValueToNameArrayConverter}}

		{{valueToNameEnumerableConverter}}

		{{nullableValueToNameEnumerableConverter}}

		""";
		ctx.AddSource(ZString.Concat(data.NamespaceName == "<global namespace>" ? "" : namespaceName, fullTypeName, "TaggedEnum.g.cs"), SourceText.From(source, Encoding.UTF8));
	}

	private static string PrimitiveTypeToReadJsonType(string type)
		=> type switch {
			"byte" => "GetByte()",
			"sbyte" => "GetSByte()",
			"short" => "GetInt16()",
			"int" => "GetInt32()",
			"long" => "GetInt64()",
			"ushort" => "GetUInt16()",
			"uint" => "GetUInt32()",
			"ulong" => "GetUInt64()",
			"float" => "GetSingle()",
			"double" => "GetDouble()",
			"decimal" => "GetDecimal()",
			"char" => "GetString()?[0] ?? throw new JsonException(\"Couldn't convert empty string to char\")",
			"string" => "GetString()",
			"bool" => "GetBoolean()",
			_ => throw new ArgumentException("Invalid type name.")
		};

	private static string PrimitiveTypeToWriteJsonType(string type, string arg)
		=> type switch {
			"byte" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"sbyte" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"short" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"int" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"long" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"ushort" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"uint" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"ulong" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"float" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"double" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"decimal" => ZString.Concat("WriteNumberValue(", arg, ')'),
			"char" => ZString.Concat("WriteStringValue(new string(", arg, ", 1))"),
			"string" => ZString.Concat("WriteStringValue(", arg, ')'),
			"bool" => ZString.Concat("WriteBooleanValue(", arg, ')'),
			_ => throw new ArgumentException("Invalid type name.")
		};

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static string PrimitiveTypeToDeclareType(string type)
		=> type == "string" ? "string?" : type;

	public static string ReaderGetToken(string type)
		=> type == "string"
				? $@"reader.GetString() is not {{}} token ? throw new UnreachableException(""JsonTokenType is string but reader.GetString() is null."") : token"
				: ZString.Concat("reader.", PrimitiveTypeToReadJsonType(type));
}