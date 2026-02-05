namespace Simulator.DigitalCircuits;

public static class ArrayExtensions {
	public static T[] Fill<T>(this T[] arr, Func<T> cb) {
		for (int i = 0, length = arr.Length; i < length; ++i) {
			arr[i] = cb();
		}
		return arr;
	}

	public static T[] Fill<T>(this T[] arr, Func<int, T> cb) {
		for (int i = 0, length = arr.Length; i < length; ++i) {
			arr[i] = cb(i);
		}
		return arr;
	}
}