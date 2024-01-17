using System.Runtime.CompilerServices;
using Microsoft.Extensions.Hosting;

namespace SixTatami.Extensions;

public static class IHostApplicationLifetimeExtensions {
	// 这个B东西不给设置退出代码...
	// https://github.com/dotnet/runtime/issues/67437
	// https://github.com/dotnet/runtime/issues/67146
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void StopApplication(this IHostApplicationLifetime self, int code) {
		Environment.ExitCode = code;
		self.StopApplication();
	}
}