using Microsoft.Xna.Framework.Content;
using System;
using System.IO;
using System.Reflection;

namespace VeiniaFramework
{
	public sealed class VeiniaContent : ContentManager
	{
		private readonly Assembly assembly;

		public VeiniaContent(IServiceProvider services)
			: this(services, typeof(VeiniaContent).Assembly)
		{
		}

		public VeiniaContent(IServiceProvider services, Assembly assembly) : base(services) => this.assembly = assembly;

		protected override Stream OpenStream(string assetName)
		{
			string normalizedAssetName = assetName.Replace('/', '\\');

			string resourceName = normalizedAssetName + ".xnb";

			Stream stream = assembly.GetManifestResourceStream(resourceName);

			if (stream != null)
				return stream;

			throw new ContentLoadException(
				$"Veinia content '{assetName}' was not found in " +
				$"{assembly.GetName().Name}. Expected embedded resource '{resourceName}'.");
		}
	}
}