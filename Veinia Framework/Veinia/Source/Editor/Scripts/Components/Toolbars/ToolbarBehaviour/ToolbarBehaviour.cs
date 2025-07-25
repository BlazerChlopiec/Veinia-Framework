﻿using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework.Editor
{
	public class ToolbarBehaviour
	{
		public GameObject gameObject;

		public virtual void OnInitialize() { }
		public virtual void OnEnterTab() { }
		public virtual void OnExitTab(Toolbar newToolbar) { }
		public virtual void OnUpdate() { }
		public virtual void OnDraw(SpriteBatch sb) { }
	}
}
