using UnityEngine;
using System.Collections;

static public class MyAnchorCameraTool {
	static public T FindInParents<T> (GameObject go) where T : Component
	{
		if (go == null) return null;
		object comp = go.GetComponent<T>();

		if (comp == null)
		{
			Transform t = go.transform.parent;

			while (t != null && comp == null)
			{
				comp = t.gameObject.GetComponent<T>();
				t = t.parent;
			}
		}
		return (T)comp;
	}
}
