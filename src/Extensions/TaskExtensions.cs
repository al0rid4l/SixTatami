namespace SixTatami.Extensions;

public static class TaskExtensions {
	extension(Task task) {
		public Task IgnoreExceptions() =>
			task.ContinueWith(static t =>
				_ = t.Exception,
				TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.DenyChildAttach);
	}

	extension<T>(Task<T> task) {
		public Task<T> IgnoreExceptions(T @default) =>
			task.ContinueWith(t => {
				_ = t.Exception;
				return @default;
			},
			TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.DenyChildAttach);
	}
}
