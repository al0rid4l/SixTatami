using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
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
		public Enumerator GetEnumerator() => new(range);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Enumerator AsEnumerable() => new(range);

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

	public struct Enumerator: IEquatable<Enumerator>, IEnumerable<int>, IEnumerator<int> {
		public int Current { get; private set; }

		readonly object IEnumerator.Current => Current;

		private readonly int _End;

		public Enumerator(Range rg) {
			ValidateRange(rg);
			checked {
				Current = rg.Start.Value - 1;
			}
			_End = rg.End.Value;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool MoveNext() => ++Current < _End;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly Enumerator GetEnumerator() => this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		readonly IEnumerator<int> IEnumerable<int>.GetEnumerator() => this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		readonly IEnumerator IEnumerable.GetEnumerator() => this;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly void Reset() => throw new NotImplementedException();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly void Dispose() {}

		public readonly override string ToString() => Current.ToString(CultureInfo.InvariantCulture);

		public readonly override int GetHashCode() => Current;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool Equals(Enumerator obj) => Current == obj.Current;

		public readonly override bool Equals(object? obj) => obj is Enumerator o && o.Current == Current;


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(Enumerator left, Enumerator right) => left.Equals(right);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(Enumerator left, Enumerator right) => !left.Equals(right);
	}
}
