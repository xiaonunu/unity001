using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MyAnchorCamera))]
public class MyAnchorCameraEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

//		MyAnchorCamera _target = (MyAnchorCamera)target;
		if (GUI.changed)
		{
			EditorUtility.SetDirty(target);
			MyUIAnchor[] allAlignmentObjects = GameObject.FindObjectsOfType(typeof(MyUIAnchor)) as MyUIAnchor[];
			foreach (var v in allAlignmentObjects)
			{
				EditorUtility.SetDirty(v);
			}
		}
		
		if (GUILayout.Button("Create Anchor"))
		{
			MyAnchorCamera cam = (MyAnchorCamera)target;
			
			GameObject go = new GameObject("Anchor");
			go.transform.parent = cam.transform;
			MyUIAnchor cameraAnchor = go.AddComponent<MyUIAnchor>();
			cameraAnchor.MyAnchorCamera = cam;
		}
	}
}
