using System.Diagnostics.CodeAnalysis;
namespace TaggedEnum;

public sealed class DataNotFoundException(string msg): Exception(msg) {
	[DoesNotReturn]
	public static T ThrowWithMessage<T>(string message) => throw new DataNotFoundException(message);
}