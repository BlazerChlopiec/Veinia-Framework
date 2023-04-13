using Myra.Graphics2D.UI;
using System.Collections.Generic;

namespace VeiniaFramework.Editor
{
	public class EditorLabelManager : Component
	{
		private static List<EditorLabel> editorLabels = new List<EditorLabel>();


		public override void Update()
		{
			base.Update();


			foreach (var editorLabel in editorLabels)
			{
				if (!level.Myra.Widgets.Contains(editorLabel.Label))
					level.Myra.Widgets.Add(editorLabel.Label);
			}
		}

		public static void Add(string identifier, Label label)
		{
			var alreadyExists = editorLabels.Find(x => x.Identifier == identifier);

			if (alreadyExists != null)
			{
				alreadyExists.Label.Text = label.Text;
				return;
			}

			var editorLabel = new EditorLabel
			{
				Identifier = identifier,
				Label = label
			};

			editorLabels.Add(editorLabel);
		}
	}
}
