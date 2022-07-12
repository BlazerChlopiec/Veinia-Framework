namespace Veinia.Editor
{
	public class EditorObject
	{
		public EditorObject(string prefabName, Transform transform)
		{
			this.prefabName = prefabName;
			this.transform = transform;
		}

		public string prefabName;
		public Transform transform;
	}
}
