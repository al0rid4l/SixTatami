using System.Diagnostics.CodeAnalysis;

namespace SixTatami.Extensions;

public static class SpanExtensions {
	[DoesNotReturn]
	private static void ThrowInvalidRange(Range range)
		=> throw new ArgumentOutOfRangeException($"Range: {range.Start.Value} to {range.End.Value} out of range.");

	extension<T>(Span<T> arr) {
		public Span<T> Fill(Func<T> cb) {
			for (int i = 0, length = arr.Length; i < length; ++i) {
				arr[i] = cb();
			}
			return arr;
		}

		public Span<T> Fill(Func<int, T> cb) {
			for (int i = 0, length = arr.Length; i < length; ++i) {
				arr[i] = cb(i);
			}
			return arr;
		}
	}

	extension(Span<int> arr) {
		public Span<int> Fill(Range range) {
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
