namespace SixTatami.Extensions;

public static class IEnumerableExtensions {
	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> cb) {
		foreach (var item in self) {
			cb(item);
		}
		return self;
	}

	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T, int> cb) {
		var i = 0;
		foreach (var item in self) {
			cb(item, i++);
		}
		return self;
	}
}