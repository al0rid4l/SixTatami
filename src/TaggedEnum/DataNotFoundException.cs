#pragma warning disable CA1050 // Declare types in namespaces
public sealed class DataNotFoundException(string msg): Exception(msg);
#pragma warning restore CA1050 // Declare types in namespaces