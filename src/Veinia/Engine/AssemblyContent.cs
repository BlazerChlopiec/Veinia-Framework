using Microsoft.Xna.Framework.Content;
using System;
using System.IO;
using System.Reflection;

namespace VeiniaFramework
{
	public class AssemblyContent : ContentManager
	// this is meant for loading .xnb assets stored in .dll assemblies
	// veinia uses this to load default prefab textures or default trail shaders 
	{
		Assembly assembly;

		public AssemblyContent(IServiceProvider services, Assembly assembly) : base(services)
			=> this.assembly = assembly ?? typeof(Veinia).Assembly;

		protected override Stream OpenStream(string assetName)
		{
			string normalizedAssetName = assetName.Replace('/', '\\');

			string resourceName = normalizedAssetName + ".xnb";

			Stream stream = assembly.GetManifestResourceStream(resourceName);

			if (stream != null) return stream;

			throw new ContentLoadException($"Assembly content '{assetName}' was not found. Expected embedded resource - '{resourceName}'.");
		}

		public void GetAssemblyFileNames()
		{
			foreach (var name in assembly.GetManifestResourceNames())
			{
				Say.Line(name);
			}
		}
	}
}