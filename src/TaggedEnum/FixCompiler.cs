// from https://stackoverflow.com/questions/64749385/predefined-type-system-runtime-compilerservices-isexternalinit-is-not-defined
namespace System.Runtime.CompilerServices {
	internal static class IsExternalInit;

	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	public class RequiredMemberAttribute: Attribute;

	[AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
	// from https://github.com/dotnet/core/issues/8016
	#pragma warning disable CS9113
	public class CompilerFeatureRequiredAttribute(string name) : Attribute {}
	#pragma warning restore CS9113
}
