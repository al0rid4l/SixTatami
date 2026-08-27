#nullable enable
using System.Runtime.CompilerServices;

namespace SixTatami.Extensions;

#pragma warning disable CA1708 // Identifiers should differ by more than case
public static class TaskTupleExtensions {
#pragma warning restore CA1708 // Identifiers should differ by more than case
	extension((ValueTask t1, ValueTask t2) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			await Task.WhenAll(t1, t2).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			await Task.WhenAll(t1, t2, t3).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6, ValueTask t7) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6, ValueTask t7, ValueTask t8) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6, ValueTask t7, ValueTask t8, ValueTask t9) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6, ValueTask t7, ValueTask t8, ValueTask t9, ValueTask t10) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6, ValueTask t7, ValueTask t8, ValueTask t9, ValueTask t10, ValueTask t11) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6, ValueTask t7, ValueTask t8, ValueTask t9, ValueTask t10, ValueTask t11, ValueTask t12) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			var t12 = tuple.t12.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6, ValueTask t7, ValueTask t8, ValueTask t9, ValueTask t10, ValueTask t11, ValueTask t12, ValueTask t13) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			var t12 = tuple.t12.AsTask();
			var t13 = tuple.t13.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6, ValueTask t7, ValueTask t8, ValueTask t9, ValueTask t10, ValueTask t11, ValueTask t12, ValueTask t13, ValueTask t14) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			var t12 = tuple.t12.AsTask();
			var t13 = tuple.t13.AsTask();
			var t14 = tuple.t14.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6, ValueTask t7, ValueTask t8, ValueTask t9, ValueTask t10, ValueTask t11, ValueTask t12, ValueTask t13, ValueTask t14, ValueTask t15) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			var t12 = tuple.t12.AsTask();
			var t13 = tuple.t13.AsTask();
			var t14 = tuple.t14.AsTask();
			var t15 = tuple.t15.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension((ValueTask t1, ValueTask t2, ValueTask t3, ValueTask t4, ValueTask t5, ValueTask t6, ValueTask t7, ValueTask t8, ValueTask t9, ValueTask t10, ValueTask t11, ValueTask t12, ValueTask t13, ValueTask t14, ValueTask t15, ValueTask t16) tuple) {
		public async Task WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			var t12 = tuple.t12.AsTask();
			var t13 = tuple.t13.AsTask();
			var t14 = tuple.t14.AsTask();
			var t15 = tuple.t15.AsTask();
			var t16 = tuple.t16.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16).ConfigureAwait(false);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2>((ValueTask<T1> t1, ValueTask<T2> t2) tuple) {
		public async Task<(T1, T2)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			await Task.WhenAll(t1, t2).ConfigureAwait(false);
			return (t1.Result, t2.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3) tuple) {
		public async Task<(T1, T2, T3)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			await Task.WhenAll(t1, t2, t3).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4) tuple) {
		public async Task<(T1, T2, T3, T4)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			await Task.WhenAll(t1, t2, t3, t4).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5) tuple) {
		public async Task<(T1, T2, T3, T4, T5)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6, T7>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6, ValueTask<T7> t7) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6, T7)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6, T7, T8>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6, ValueTask<T7> t7, ValueTask<T8> t8) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6, T7, T8)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6, T7, T8, T9>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6, ValueTask<T7> t7, ValueTask<T8> t8, ValueTask<T9> t9) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6, ValueTask<T7> t7, ValueTask<T8> t8, ValueTask<T9> t9, ValueTask<T10> t10) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6, ValueTask<T7> t7, ValueTask<T8> t8, ValueTask<T9> t9, ValueTask<T10> t10, ValueTask<T11> t11) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result, t11.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6, ValueTask<T7> t7, ValueTask<T8> t8, ValueTask<T9> t9, ValueTask<T10> t10, ValueTask<T11> t11, ValueTask<T12> t12) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			var t12 = tuple.t12.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result, t11.Result, t12.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6, ValueTask<T7> t7, ValueTask<T8> t8, ValueTask<T9> t9, ValueTask<T10> t10, ValueTask<T11> t11, ValueTask<T12> t12, ValueTask<T13> t13) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			var t12 = tuple.t12.AsTask();
			var t13 = tuple.t13.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result, t11.Result, t12.Result, t13.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6, ValueTask<T7> t7, ValueTask<T8> t8, ValueTask<T9> t9, ValueTask<T10> t10, ValueTask<T11> t11, ValueTask<T12> t12, ValueTask<T13> t13, ValueTask<T14> t14) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			var t12 = tuple.t12.AsTask();
			var t13 = tuple.t13.AsTask();
			var t14 = tuple.t14.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result, t11.Result, t12.Result, t13.Result, t14.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6, ValueTask<T7> t7, ValueTask<T8> t8, ValueTask<T9> t9, ValueTask<T10> t10, ValueTask<T11> t11, ValueTask<T12> t12, ValueTask<T13> t13, ValueTask<T14> t14, ValueTask<T15> t15) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			var t12 = tuple.t12.AsTask();
			var t13 = tuple.t13.AsTask();
			var t14 = tuple.t14.AsTask();
			var t15 = tuple.t15.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result, t11.Result, t12.Result, t13.Result, t14.Result, t15.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

	extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>((ValueTask<T1> t1, ValueTask<T2> t2, ValueTask<T3> t3, ValueTask<T4> t4, ValueTask<T5> t5, ValueTask<T6> t6, ValueTask<T7> t7, ValueTask<T8> t8, ValueTask<T9> t9, ValueTask<T10> t10, ValueTask<T11> t11, ValueTask<T12> t12, ValueTask<T13> t13, ValueTask<T14> t14, ValueTask<T15> t15, ValueTask<T16> t16) tuple) {
		public async Task<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> WhenAll() {
			var t1 = tuple.t1.AsTask();
			var t2 = tuple.t2.AsTask();
			var t3 = tuple.t3.AsTask();
			var t4 = tuple.t4.AsTask();
			var t5 = tuple.t5.AsTask();
			var t6 = tuple.t6.AsTask();
			var t7 = tuple.t7.AsTask();
			var t8 = tuple.t8.AsTask();
			var t9 = tuple.t9.AsTask();
			var t10 = tuple.t10.AsTask();
			var t11 = tuple.t11.AsTask();
			var t12 = tuple.t12.AsTask();
			var t13 = tuple.t13.AsTask();
			var t14 = tuple.t14.AsTask();
			var t15 = tuple.t15.AsTask();
			var t16 = tuple.t16.AsTask();
			await Task.WhenAll(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16).ConfigureAwait(false);
			return (t1.Result, t2.Result, t3.Result, t4.Result, t5.Result, t6.Result, t7.Result, t8.Result, t9.Result, t10.Result, t11.Result, t12.Result, t13.Result, t14.Result, t15.Result, t16.Result);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TaskAwaiter<(T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16)> GetAwaiter() => tuple.WhenAll().GetAwaiter();
	}

}
