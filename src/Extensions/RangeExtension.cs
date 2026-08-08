using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SixTatami.Extensions;

public static class RangeExtension {
	[DoesNotReturn]
	private static void ThrowInvalidRange()
		=> throw new ArgumentOutOfRangeException("Can't use Range without a collection context (cannot contain ^ relative indices).");

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void ValidateRange(Range range) {
		if (range.Start.IsFromEnd || range.End.IsFromEnd) {
			ThrowInvalidRange();
		}
	}

	// Range比较小, 不用ref
	extension(Range range) {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Enumerator GetEnumerator() {
			ValidateRange(range);
			return new(range);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IEnumerable<int> AsEnumerable() {
			ValidateRange(range);
			return generator(range);

			static IEnumerable<int> generator(Range rg) {
				for (var i = rg.Start.Value; i< rg.End.Value; ++i) {
					yield return i;
				}
			}
		}

		public Range ForEach(Action<int> cb) {
			ValidateRange(range);

			foreach (var item in range) {
				cb(item);
			}
			return range;
		}

		public int[] ToArray() {
			ValidateRange(range);
			var start = range.Start.Value;
			var length = range.End.Value - start;

			if (length <= 0) return [];

			var result = GC.AllocateUninitializedArray<int>(length);
			for (var i = 0; i < length; ++i) {
				result[i] = start + i;
			}
			return result;
		}
	}

	public ref struct Enumerator: IEquatable<Enumerator> {
		public int Current { get; private set; }
		private readonly int _End;

		public Enumerator(Range rg) {
			Current = rg.Start.Value - 1;
			_End = rg.End.Value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool MoveNext() {
			++Current;
			return Current < _End;
		}

		public readonly override string ToString() => "";

		public readonly override int GetHashCode() => Guid.NewGuid().GetHashCode();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool Equals(Enumerator obj) => false;

		public readonly override bool Equals(object? obj) => false;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Enumerator left, Enumerator right) => false;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Enumerator left, Enumerator right) => true;
	}
}
