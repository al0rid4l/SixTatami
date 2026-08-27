using System.Runtime.CompilerServices;

namespace SixTatami.DataStructures;

public readonly struct CowString(string owned, int offset, int length): IEquatable<CowString> {
	private readonly string _borrowedOrOwned = owned;
	private readonly int _offset = offset;
	private readonly int _length = length;

#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable CA1707 // Identifiers should not contain underscores
	public CowString(string _owned): this(_owned, 0, _owned.Length) => _borrowedOrOwned = _owned;
#pragma warning restore CA1707 // Identifiers should not contain underscores
#pragma warning restore IDE1006 // Naming Styles

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly ReadOnlySpan<char> AsSpan() => _borrowedOrOwned.AsSpan(_offset, _length);

	public readonly override string ToString() => _offset == 0 && _length == _borrowedOrOwned.Length ? _borrowedOrOwned : _borrowedOrOwned.Substring(_offset, _length);

	public readonly override bool Equals(object? obj) => Equals(obj);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public readonly bool Equals(CowString obj) => obj._length == _length && obj._offset == _offset && obj._borrowedOrOwned == _borrowedOrOwned;
	public readonly override int GetHashCode() => HashCode.Combine(_offset, _length, _borrowedOrOwned);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator ==(CowString left, CowString right) => left.Equals(right);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool operator !=(CowString left, CowString right) => !(left == right);
}