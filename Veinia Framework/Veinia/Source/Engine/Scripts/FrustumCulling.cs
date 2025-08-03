using Microsoft.Xna.Framework;

namespace VeiniaFramework
{
	public class FrustumCulling
	{
		public void Update()
		{
			var frustum = Globals.camera.GetBoundingFrustum(scaleFactor: 1.5f);
			foreach (var gameObject in Globals.loader.current.scene)
			{
				var cull = frustum.Contains(gameObject.transform.screenPos.ToVector3()) == ContainmentType.Disjoint;
				gameObject.frustumCulled = cull;
			}
		}
	}
}