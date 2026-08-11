using System.Runtime.CompilerServices;

namespace SixTatami.Extensions;

public static class StringExtensions {
	extension(string str) {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<char> TrimAsSpan() => str.AsSpan().Trim();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<char> TrimAsSpan(char c) => str.AsSpan().Trim(c);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<char> TrimStartAsSpan() => str.AsSpan().TrimStart();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<char> TrimStartAsSpan(char c) => str.AsSpan().TrimStart(c);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<char> TrimEndAsSpan() => str.AsSpan().TrimEnd();


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<char> TrimEndAsSpan(char c) => str.AsSpan().TrimEnd(c);
	}
}


