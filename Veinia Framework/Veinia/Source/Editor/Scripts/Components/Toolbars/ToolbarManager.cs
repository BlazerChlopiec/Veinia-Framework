﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Myra.Graphics2D.UI;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class ToolbarManager : Component
	{
		List<Toolbar> toolbars = new List<Toolbar>();

		Toolbar currentToolbar;
		Toolbar previousToolbar;

		TabControl tabControl;


		public ToolbarManager(List<Toolbar> toolbars) => this.toolbars = toolbars;

		public override void Initialize()
		{
			tabControl = new TabControl();
			tabControl.TabSelectorPosition = TabSelectorPosition.Left;

			foreach (var toolbar in toolbars)
			{
				tabControl.Items.Add(new TabItem { Text = toolbar.toolbarName, Tag = toolbar, Content = toolbar.content });

				//can't be more than 10 toolbars because this wouldn't make sense lmao
				toolbar.shortcut = Keys.D1 + toolbars.IndexOf(toolbar);

				toolbar.OnInitialize(gameObject);
			}

			RefreshToolbar();

			// this is important as the first tab doens't show the contents if you don't flip it which is weird
			tabControl.SelectedIndex = 1;
			tabControl.SelectedIndex = 0;

			tabControl.SelectedIndexChanged += (o, e) => { RefreshToolbar(); };

			var window = new Window
			{
				Title = "Toolbar",
				Content = tabControl,
			};
			window.CloseButton.RemoveFromParent();

			window.Show(Globals.myraDesktop, Point.Zero);
		}

		public override void Update()
		{
			foreach (var toolbar in toolbars)
			{
				if (Globals.input.GetKeyDown(toolbar.shortcut))
				{
					tabControl.SelectedIndex = toolbars.IndexOf(toolbar);
				}
			}
		}

		private void RefreshToolbar()
		{
			previousToolbar = currentToolbar;
			currentToolbar = (Toolbar)tabControl.SelectedItem.Tag;

			if (currentToolbar != null)
			{
				currentToolbar?.OnFocus(gameObject);
				tabControl.SelectedItem.Content = currentToolbar.content;
			}
			previousToolbar?.OnLostFocus(gameObject);
		}
	}
}