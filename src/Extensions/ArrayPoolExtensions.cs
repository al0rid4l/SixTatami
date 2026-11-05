using System.Text;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Buffers.Text;

namespace SixTatami.Extensions;

public static class ArrayPoolExtensions {
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static SharedObject<T> AutoRent<T>(this ArrayPool<T> self, int minimumLength) => new(self, minimumLength);

	public static SharedObject<byte> FromBase64String(this ArrayPool<byte> self, string value, out int bytesWritten) {
		using var utf8Buffer = AutoRent(self, Encoding.UTF8.GetMaxByteCount(value.Length));
		int utf8BufferSize = Encoding.UTF8.GetBytes(value, utf8Buffer.Value);
		var decodedBuffer = AutoRent(self, Base64.GetMaxDecodedFromUtf8Length(value.Length));

		try {
			Base64.DecodeFromUtf8(utf8Buffer.Value.AsSpan(0, utf8BufferSize), decodedBuffer.Value, out var _, out bytesWritten);
			if (bytesWritten == 0) {
				throw new InvalidOperationException("Error writing to buffer.");
			}
		} catch {
			decodedBuffer.Dispose();
			throw;
		}
		return decodedBuffer;
	}

	public static string ToBase64String(this ArrayPool<byte> self, ReadOnlySpan<byte> value) {
		using var encodeBuffer = AutoRent(self, Base64.GetMaxEncodedToUtf8Length(value.Length));
		Base64.EncodeToUtf8(value, encodeBuffer.Value, out var _, out int bytesWritten);
		return bytesWritten == 0
			? throw new InvalidOperationException("Error writing to buffer.")
			: Encoding.UTF8.GetString(encodeBuffer.Value.AsSpan(0, bytesWritten));
	}
}

public sealed class SharedObject<T>(ArrayPool<T> shared, int minimumLength): IDisposable {
	private int _disposed;
	public readonly T[] Value = shared.Rent(minimumLength);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Dispose() {
		if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0) {
			shared.Return(Value);
		}
	}
}
