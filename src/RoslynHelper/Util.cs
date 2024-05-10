using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SixTatami.RoslynHelper;

public static class Util {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotNullRefType<T>([NotNullWhen(true)]T? v) where T: class => v is not null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotNullValueType<T>([NotNullWhen(true)]T? v) where T: struct => v is not null;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T NullableToValueType<T>(T? v) where T: struct => v.GetValueOrDefault();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static T NullableToValueType<T>(T? v, CancellationToken _) where T: struct => v.GetValueOrDefault();
}