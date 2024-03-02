namespace SixTatami.Extensions;

public static class IEnumerableExtensions {
	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> ien, Action<T> cb) {
		foreach (var item in ien) {
			cb(item);
		}
		return ien;
	}

	public static IEnumerable<T> ForEach<T>(this IEnumerable<T> ien, Action<T, int> cb) {
		var i = 0;
		foreach (var item in ien) {
			cb(item, i++);
		}
		return ien;
	}
}