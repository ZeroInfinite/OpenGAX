﻿//===================================================================================
// Microsoft patterns & practices
// Guidance Automation Toolkit
//===================================================================================
// Copyright (c) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===================================================================================
// License: MS-LPL
//===================================================================================

using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.ComponentModel;
using EnvDTE;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Microsoft.Practices.RecipeFramework.MetaGuidancePackage.Actions
{
	/// <summary>
	/// Converts a non-vsix guidance package project file into vsix by adding
	/// some properties and imports.
	/// </summary>
	[ServiceDependency(typeof(DTE))]
	public class UpdateProjectFileToVsixAction : Action
	{
		/// <summary>
		/// The guidance package project.
		/// </summary>
		[Input(Required = true)]
		public Project PackageProject { get; set; }

		/// <summary>
		/// Does nothing, as un-registration must be done explicitly.
		/// </summary>
		public override void Undo()
		{
			// Must un-register to undo.
		}

		/// <summary>
		/// Converts a non-vsix guidance package project file into vsix by adding
		/// some properties and imports.
		/// </summary>
		public override void Execute()
		{
			Trace.TraceInformation("Updating guidance package project file to VSIX...");

			this.PackageProject.Save();

			var tempPath = Path.GetTempFileName();
			File.Copy(this.PackageProject.FullName, tempPath, true);

			var project = new Build.Evaluation.Project(tempPath);

			Trace.TraceInformation("\tSetting VSIX properties...");
			// Add properties
			AddProperty(project, "GeneratePkgDefFile", "false");
			AddProperty(project, "IncludeAssemblyInVSIXContainer", "true");
			AddProperty(project, "IncludeDebugSymbolsInVSIXContainer", "false");
			AddProperty(project, "IncludeDebugSymbolsInLocalVSIXDeployment", "true");
			AddProperty(project, "CopyBuildOutputToOutputDirectory", "true");
			AddProperty(project, "CopyOutputSymbolsToOutputDirectory", "true");
			AddProperty(project, "ProjectTypeGuids", "{82b43b9b-a64c-4715-b499-d71e9ca2bd60};{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");
			AddProperty(project, "TargetFrameworkVersion", "v4.0");		

			// Add imports
			Trace.TraceInformation("\tAdding VS SDK targets...");
			AddImport(project, @"$(MSBuildExtensionsPath)\Microsoft\VisualStudio\v10.0\VSSDK\Microsoft.VsSDK.targets");
			Trace.TraceInformation("\tAdding GAX targets...");
			AddImport(project, @"$(RecipeFrameworkPath)\Microsoft.Practices.RecipeFramework.Build.targets");
			
			project.Save();

			File.WriteAllText(this.PackageProject.FullName, File.ReadAllText(tempPath));
		}

		private void AddImport(Microsoft.Build.Evaluation.Project project, string importProjectFile)
		{
			if (project.Xml.Imports.FirstOrDefault(import => string.Equals(import.Project, importProjectFile, StringComparison.InvariantCultureIgnoreCase)) == null)
				project.Xml.AddImport(importProjectFile);
		}

		private void AddProperty(Microsoft.Build.Evaluation.Project project, string propertyName, string propertyValue)
		{
			project.SetProperty(propertyName, propertyValue);
		}
	}
}
