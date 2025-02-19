using System.Buffers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

using SixTatami.DataStructures;
using SixTatami.Extensions;

namespace SixTatami.Utilities;

public static class CharsetTypeExtensions {
	private const string Numbers = "0123456789";

	private const string AlphabetLower = "abcdefghijklmnopqrstuvwxyz";

	private const string AlphabetUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

	private const string HexLower = "abcdef";

	private const string HexUpper = "ABCDEF";

	private const string Binary = "01";

	private const string Octal = "01234567";

	private const string Symbols = @"!@#$%^&*()-=_+[]{}\|;':"",./<>?`~";

	// NOTE 所有预定义字符串总长度,新增则需要修改此数
	private const int TotalCharsetLength = 116;

	private static readonly string[] _CharsetTypeMap = [Numbers, AlphabetLower, AlphabetUpper, HexLower, HexUpper, Octal, Binary, Symbols];

	private static int ResolvingPerformanceDegradation(ref StackArray8<string> charsets, CharsetType types) {
		var charsetCount = 0;

		for (int i = 0; i < _CharsetTypeMap.Length; ++i) {
			var flag = (byte)types & (1 << i);
			if ((CharsetType)flag != CharsetType.None) {
				charsets[charsetCount++] = _CharsetTypeMap[(int)Math.Log2(flag)];
			}
		}

		return charsetCount;
	}

	// TODO 扩展方法不支持getter
	public static string Value(this CharsetType self) {
		Span<(CharsetType remove, CharsetType duplicate)> patterns = [
			(~CharsetType.Binary, CharsetType.Numbers | CharsetType.Binary),
			(~CharsetType.Octal, CharsetType.Numbers | CharsetType.Octal),
			(~CharsetType.Binary, CharsetType.Octal | CharsetType.Binary),
			((~CharsetType.Binary) & (~CharsetType.Octal), CharsetType.Octal | CharsetType.Binary | CharsetType.Numbers),
			(~CharsetType.HexLower, CharsetType.AlphabetLower | CharsetType.HexLower),
			(~CharsetType.HexUpper, CharsetType.AlphabetUpper | CharsetType.HexUpper),
		];

		Span<char> result = stackalloc char[128];
		var charsets = new StackArray8<string>();
		var offset = 0;

		for (var i = 0; i < patterns.Length; ++i) {
			var (remove, duplicate) = patterns[i];
			if ((self & duplicate) == duplicate) {
				self &= remove;
			}
		}

		// 虽然可以TotalCharsetLength,但128内存对齐肯定没问题,免得出些奇怪问题
		// Span<char> result = stackalloc char[TotalCharsetLength];
		var charsetCount = ResolvingPerformanceDegradation(ref charsets, self);

		for (int i = 0; i < charsetCount; ++i) {
			var charset = charsets[i];
			var len = charset.Length;
			var span = result.Slice(offset, len);
			charset.CopyTo(span);
			offset += len;
		}

		return result[..offset].ToString();
	}

	// 天知道为什么这段代码会和上面相差慢十倍
	// public static string Value(this CharsetType self) {
	// 	// Span<char> result = stackalloc char[TotalCharsetLength];
	// 	// 虽然可以TotalCharsetLength,但128内存对齐肯定没问题,免得出些奇怪问题

	// 	Span<(CharsetType remove, CharsetType duplicate)> patterns = [
	// 		(~CharsetType.Binary, CharsetType.Numbers | CharsetType.Binary),
	// 		(~CharsetType.Octal, CharsetType.Numbers | CharsetType.Octal),
	// 		(~CharsetType.Binary, CharsetType.Octal | CharsetType.Binary),
	// 		((~CharsetType.Binary) & (~CharsetType.Octal), CharsetType.Octal | CharsetType.Binary | CharsetType.Numbers),
	// 		(~CharsetType.HexLower, CharsetType.AlphabetLower | CharsetType.HexLower),
	// 		(~CharsetType.HexUpper, CharsetType.AlphabetUpper | CharsetType.HexUpper),
	// 	];

	// 	for (var i = 0; i < patterns.Length; ++i) {
	// 		var (remove, duplicate) = patterns[i];
	// 		if ((self & duplicate) == duplicate) {
	// 			self &= remove;
	// 		}
	// 	}

	// 	var charsets = new StackArray8<string>();
	// 	var charsetCount = 0;
	// 	for (int i = 0; i < _CharsetTypeMap.Length; ++i) {
	// 		var flag = (byte)allCharset & (1 << i);
	// 		if ((CharsetType)flag != CharsetType.None) {
	// 			charsets[charsetCount++] = _CharsetTypeMap[(int)Math.Log2(flag)];
	// 		}
	// 	}

	// 	Span<char> result = stackalloc char[128];
	// 	var offset = 0;
	// 	for (int i = 0; i < charsetCount; ++i) {
	// 		var charset = charsets[i];
	// 		var len = charset.Length;
	// 		var span = result.Slice(offset, len);
	// 		charset.CopyTo(span);
	// 		offset += len;
	// 	}

	// 	return result[..offset].ToString();
	// }
}

[Flags]
public enum CharsetType: byte {
	None = 0,
	Numbers = 1,
	AlphabetLower = 2,
	AlphabetUpper = 4,
	HexLower = 8,
	HexUpper = 16,
	Octal = 32,
	Binary = 64,
	Symbols = 128,
	All = Numbers | AlphabetLower | AlphabetUpper | HexLower | HexUpper | Binary | Octal | Symbols,
}

public static class RandomString {
	// safe
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GenerateSafe(string charset, int length = 10) => RandomNumberGenerator.GetString(charset, length);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GenerateSafe(CharsetType charset = CharsetType.AlphabetLower, int length = 10) => GenerateSafe(charset.Value(), length);

	public static string GenerateSafeRandomLength(string charset, int minLength = 5, int maxLength = 10) {
		if (minLength < 0 || maxLength < 0 || minLength > maxLength) {
			throw new ArgumentException($"Invalid length, {nameof(minLength)}: {minLength}, {nameof(maxLength)}: {maxLength}, {nameof(minLength)} and {nameof(maxLength)} must be greater than 0, and {nameof(maxLength)} must be greater than {nameof(minLength)}.");
		}

		var rnd = new Random();
		return GenerateSafe(charset, rnd.Next(minLength, maxLength));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GenerateSafeRandomLength(CharsetType charset = CharsetType.AlphabetLower, int minLength = 5, int maxLength = 10) => GenerateSafeRandomLength(charset.Value(), minLength, maxLength);

	// unsafe
	// [MethodImpl(MethodImplOptions.AggressiveInlining)]
	// private static string FastGenerate(string charset, int length, Random rnd) {
	// 	// 考虑大小太大可能爆栈
	// 	Span<char> sb = stackalloc char[length];

	// 	for (var i = 0; i < length; ++i) {
	// 		sb[i] = charset[rnd.Next(charset.Length)];
	// 	}
	// 	return sb.ToString();
	// }

	// 此函数比上面非AOT略快, AOT略慢
	private static string GenerateFast(string charset, int length, Random rnd) {
		using var sharedObject = ArrayPool<char>.Shared.AutoRent(length);
		var sb = sharedObject.Value;

		for (var i = 0; i < length; ++i) {
			sb[i] = charset[rnd.Next(charset.Length)];
		}
		var result = new string(sb, 0, length);
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GenerateFast(string charset, int length = 10) => GenerateFast(charset, length, new());

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GenerateFast(CharsetType charset = CharsetType.AlphabetLower, int length = 10) => GenerateFast(charset.Value(), length);

	public static string GenerateFastRandomLength(string charset, int minLength = 5, int maxLength = 10) {
		if (minLength > maxLength) {
			throw new ArgumentException($"Invalid length, {nameof(minLength)}: {minLength}, {nameof(maxLength)}: {maxLength}, {nameof(minLength)} and {nameof(maxLength)} must be greater than 0, and {nameof(maxLength)} must be greater than {nameof(minLength)}.");
		}

		var rnd = new Random();
		var seed = rnd.NextSingle();
		int length = (int)Math.Floor((maxLength - minLength) * seed) + minLength;
		return GenerateFast(charset, length, rnd);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GenerateFastRandomLength(CharsetType charset = CharsetType.AlphabetLower, int minLength = 5, int maxLength = 10) => GenerateFastRandomLength(charset.Value(), minLength, maxLength);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GenerateHexStringFast(int length = 8, bool lowerCase = false) => lowerCase ? GenerateFast(CharsetType.HexLower, length) : GenerateFast(CharsetType.HexUpper, length);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GenerateHexStringSafe(int length = 8, bool lowerCase = false) => RandomNumberGenerator.GetHexString(length, lowerCase);
}