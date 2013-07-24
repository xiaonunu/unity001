using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MyAnchorCamera : MonoBehaviour 
{
	public enum AnchorModel
	{
		Auto,
		Tall,
		Width,
	}	
	public AnchorModel Model;
	public float scale = 1.0F;
	public Camera cam;	
	public float suitableUI_width = 0.0f;
	public float suitableUI_height = 0.0f;
	public bool ShowResolution = false;

	// Use this for initialization
	void Awake () 
	{
		cam = GetComponent<Camera>();
		if (cam != null)
		{
			UpdateCameraMatrix();
		}
	}
	
	void Update() 
	{
		UpdateCameraMatrix();
	}

	void SelectMode()
	{
		switch(Model)
		{
		case AnchorModel.Auto:
			if(suitableUI_height != 0 && suitableUI_width != 0)
			{
				if(Mathf.Abs(Screen.width - suitableUI_width) > Mathf.Abs(Screen.height - suitableUI_height))
					scale = Screen.width / suitableUI_width;
				else
					scale = Screen.height / suitableUI_height;
			}
			else
				scale = 1.0f;
			break;
		case AnchorModel.Tall:
			if(suitableUI_height != 0)
				scale = Screen.height / suitableUI_height;
			else
				scale = 1.0f;
			break;
		case AnchorModel.Width:
			if(suitableUI_width != 0)
				scale = Screen.width / suitableUI_width;
			else
				scale = 1.0f;
			break;
		}		
	}

	public void UpdateCameraMatrix( )
	{
		SelectMode();
		float left = 0.0f;
		float top = 0.0f;
		float right = cam.pixelWidth, bottom = cam.pixelHeight;		
		float far = cam.farClipPlane;
		float near = cam.near;		
		float x =  (2.0f)  / (right - left) * scale;
		float y = (2.0f)  / (bottom - top) * scale;
		float z = -2.0f / (far - near);
		float a = 0;
		float b = 0;
		float c = -(2.0f * far * near) / (far - near);

		Matrix4x4 m = new Matrix4x4();
		m[0,0] = x;  m[0,1] = 0;  m[0,2] = 0;  m[0,3] = a;
		m[1,0] = 0;  m[1,1] = y;  m[1,2] = 0;  m[1,3] = b;
		m[2,0] = 0;  m[2,1] = 0;  m[2,2] = z;  m[2,3] = c;
		m[3,0] = 0;  m[3,1] = 0;  m[3,2] = 0;  m[3,3] = 1;
		
		cam.projectionMatrix = m;	
	}
	
	void OnGUI()
	{
		if(ShowResolution)
		{
			GUI.Label(new Rect(50,100,100,100),"Width = " + Screen.width.ToString());
			GUI.Label(new Rect(50,130,100,100),"Height = " + Screen.height.ToString());
		}
	}
	
}
