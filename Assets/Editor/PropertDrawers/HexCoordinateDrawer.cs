using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(HexCoordinates))]
public class HexCoordinateDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		HexCoordinates coordinates = new HexCoordinates(
			   property.FindPropertyRelative("_x").intValue,
			   property.FindPropertyRelative("_z").intValue
		   );

		position = EditorGUI.PrefixLabel(position, 1, label);
		GUI.Label(position, coordinates.ToString()); 
	}
}
