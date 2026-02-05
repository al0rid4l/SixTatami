namespace SixTatami.Extensions;

public static class IEnumerableExtensions {
	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T> cb) where T: class {
		foreach (var item in self) {
			cb(item);
		}
		return self;
	}

	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> self, Action<T, int> cb) where T: class {
		var i = 0;
		foreach (var item in self) {
			cb(item, i++);
		}
		return self;
	}
}