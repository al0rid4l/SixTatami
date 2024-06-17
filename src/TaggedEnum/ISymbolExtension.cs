using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis;
using static TaggedEnum.Util;

namespace TaggedEnum;

public static class ISymbolExtension {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<AttributeData> GetAttributesDataByAttribute(this ISymbol self, ISymbol attrClassSymbol)
		=> self.GetAttributes()
				.Where(attrData =>
					NotNullRefType(attrData)
					&& NotNullRefType(attrData.AttributeClass)
					&& SymbolEqualityComparer.Default.Equals(attrData.AttributeClass, attrClassSymbol));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IEnumerable<AttributeData> GetAttributesDataByAttribute(this ISymbol self, IEnumerable<ISymbol> attrClassSymbols)
		=> self.GetAttributes()
				.Where(attrData =>
					NotNullRefType(attrData)
					&& NotNullRefType(attrData.AttributeClass)
					&& (attrClassSymbols.Any(v => SymbolEqualityComparer.Default.Equals(attrData.AttributeClass, v))
						|| (!attrData.AttributeClass.IsDefinition
							&& attrClassSymbols.Any(v => SymbolEqualityComparer.Default.Equals(attrData.AttributeClass.OriginalDefinition, v)))));

}