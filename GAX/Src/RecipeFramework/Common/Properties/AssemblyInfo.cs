﻿#region Using directives

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#endregion

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Microsoft.Practices.RecipeFramework.Common")]
[assembly: AssemblyDescription("Microsoft.Practices.RecipeFramework.Common")]
[assembly: InternalsVisibleTo("Microsoft.Practices.RecipeFramework")]
[assembly: InternalsVisibleTo("Microsoft.Practices.RecipeFramework.Configuration.Test")]
[assembly: InternalsVisibleTo("Microsoft.Practices.RecipeFramework.Test")]
[assembly: InternalsVisibleTo("Microsoft.Practices.WizardFramework.Test")]
[assembly: CLSCompliant(true)]

/// <summary>
/// Provides easy access to assembly-level attributes.
/// </summary>
internal sealed class ThisAssembly
{
	public const string Title = "Recipe Framework Common";
}