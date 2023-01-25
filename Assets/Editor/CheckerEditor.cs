using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Checker))]
public class CheckerEditor : Editor  {

	
	public override void OnInspectorGUI()
    {
		DrawDefaultInspector();

        Checker myTarget = (Checker)target;

        if(GUILayout.Button("Highlight ON"))
        {
            myTarget.turnOnGreenHighlight();
        }

		if(GUILayout.Button("Highlight Off"))
        {
            myTarget.turnOnGreenHighlight();
        }
    }
}
