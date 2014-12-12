using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UI;
[CustomEditor (typeof (ButtonScript))]
public class ButtonScriptEditor : Editor {

	public override void OnInspectorGUI () {
		ButtonScript button = (ButtonScript)target;
		button.activeButtonSprite = (Sprite) EditorGUILayout.ObjectField ("Active Button Sprite", button.activeButtonSprite, typeof (Sprite), false);
		button.clickedButtonSprite = (Sprite) EditorGUILayout.ObjectField ("Clicked Button Sprite", button.clickedButtonSprite, typeof (Sprite), false);
		button.text = (string) EditorGUILayout.TextField ("Button Text", button.text);
		button.currentState = (ButtonScript.States) EditorGUILayout.EnumPopup (button.currentState);
		button.size = (Vector2) EditorGUILayout.Vector2Field ("Size", button.size);
		if(GUI.changed)
		{
			EditorUtility.SetDirty( target );
		}
		
	}
}
