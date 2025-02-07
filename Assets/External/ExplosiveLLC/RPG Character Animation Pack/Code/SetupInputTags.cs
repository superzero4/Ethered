using UnityEditor;
using UnityEngine;

namespace RPGCharacterController
{

	public class SetupMessageWindow:EditorWindow
	{
		void OnGUI()
		{
			EditorGUILayout.LabelField("Before attempting to use the RPG Character Mecanim Animation Pack, you must first ensure that the tags and inputs are correctly defined.  There is an included InputManager.preset and TagManager.preset which contains all the settings that you can load in.", EditorStyles.wordWrappedLabel);
		}
	}

	class SetupInputTags:AssetPostprocessor
	{
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			SetupMessageWindow window  = (SetupMessageWindow)EditorWindow.GetWindow(typeof(SetupMessageWindow), true, "Load Input and Tag Presets");
			window.maxSize = new Vector2(330f, 80f);
			window.minSize = window.maxSize;
		}
	}
}