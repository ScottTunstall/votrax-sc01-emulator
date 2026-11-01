#if NETFRAMEWORK
#pragma warning disable IDE0130 // The compiler looks this type up by its framework namespace.
namespace System.Runtime.CompilerServices;
#pragma warning restore IDE0130

/// <summary>
/// Lets the compiler emit <c>init</c> accessors (used by records) when targeting .NET Framework,
/// which predates this marker type.
/// </summary>
internal static class IsExternalInit;
#endif
