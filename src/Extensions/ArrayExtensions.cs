using System.Diagnostics.CodeAnalysis;

namespace SixTatami.Extensions;

public static class ArrayExtensions {
	[DoesNotReturn]
	private static void ThrowInvalidRange(Range range)
		=> throw new ArgumentOutOfRangeException($"Range: {range.Start.Value} to {range.End.Value} out of range.");

	public static T[] Fill<T>(this T[] arr, Func<T> cb) {
		for (int i = 0, length = arr.Length; i < length; ++i) {
			arr[i] = cb();
		}
		return arr;
	}

	public static T[] Fill<T>(this T[] arr, Func<int, T> cb) {
		for (int i = 0, length = arr.Length; i < length; ++i) {
			arr[i] = cb(i);
		}
		return arr;
	}

	extension(int[] arr) {
		public int[] Fill(Range range) {
			var start = range.Start.IsFromEnd ? arr.Length - range.Start.Value : range.Start.Value;
			var end = range.End.IsFromEnd ? arr.Length - range.End.Value : range.End.Value;
			var length = end - start;

			if (length < 0 || length > arr.Length) {
				ThrowInvalidRange(range);
			}

			for (int i = 0; i < length; ++i) {
				arr[i] = start + i;
			}
			return arr;
		}
	}
}
