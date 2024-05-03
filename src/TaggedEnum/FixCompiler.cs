// from https://stackoverflow.com/questions/64749385/predefined-type-system-runtime-compilerservices-isexternalinit-is-not-defined
namespace System.Runtime.CompilerServices {
	internal static class IsExternalInit {}
	public class RequiredMemberAttribute: Attribute {}
	// from https://github.com/dotnet/core/issues/8016
	public class CompilerFeatureRequiredAttribute: Attribute {
		public CompilerFeatureRequiredAttribute(string name) { }
	}
}
