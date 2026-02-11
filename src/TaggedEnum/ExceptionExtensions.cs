using System.Diagnostics.CodeAnalysis;

namespace TaggedEnum;

internal static class ExceptionExtensions {
	extension(ArgumentException) {
		[DoesNotReturn]
		public static T ThrowWithMessage<T>(string msg) => throw new ArgumentException(msg);
	}
}