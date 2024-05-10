using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SixTatami.Utilities;

public static class Util {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotNullRefType<T>([NotNullWhen(true)]T? v) where T: class => v is not null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotNullValueType<T>([NotNullWhen(true)]T? v) where T: struct => v is not null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T NullableToValueType<T>(T? v) where T: struct => v.GetValueOrDefault();
}