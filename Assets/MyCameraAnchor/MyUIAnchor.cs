using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class MyUIAnchor : MonoBehaviour 
{
	public enum Anchor
	{
//		UpperLeft,
//		UpperRight,
//		LowerLeft,		
//		LowerRight,
		Upper,
		Lower,

		MiddleLeft,
		MiddleCenter,
		MiddleRight,

		TopLeftCorner,
		TopRightCorner,
		LowerLeftCorner,
		LowerRightCorner,
	}
	public Anchor anchor;
	public Vector2 offset = Vector2.zero;
	
	public MyAnchorCamera MyAnchorCamera;
	Transform _transform;
	
	void Awake()
	{
		_transform = transform;	
	}
	
	void Start()
	{
		UpdateTransform();
		MyAnchorCamera = MyAnchorCameraTool.FindInParents<MyAnchorCamera>(gameObject);
	}
	
	void UpdateTransform()
	{
		if (MyAnchorCamera != null && MyAnchorCamera.scale != 0)
		{
			Vector3 position = _transform.localPosition;	
			Vector3 anchoredPosition = Vector3.zero;

			switch (anchor)
			{
			case Anchor.Upper: 	anchoredPosition = new Vector3(0, (Screen.height / 2.0F) / MyAnchorCamera.scale , position.z); break;				
			case Anchor.MiddleLeft: 	anchoredPosition = new Vector3((-Screen.width / 2.0F) / MyAnchorCamera.scale, 0, position.z); break;				
			case Anchor.MiddleCenter: 	anchoredPosition = new Vector3(0, 0, position.z); break;
			case Anchor.MiddleRight: 	anchoredPosition = new Vector3((Screen.width / 2.0F) / MyAnchorCamera.scale, 0, position.z); break;			
			case Anchor.Lower: 	anchoredPosition = new Vector3(0,(-Screen.height / 2.0F) / MyAnchorCamera.scale, position.z); break;
			case Anchor.TopLeftCorner: 		anchoredPosition = new Vector3((-Screen.width / 2.0f) / MyAnchorCamera.scale, 
					(Screen.height / 2.0F) / MyAnchorCamera.scale, position.z); break;
			case Anchor.TopRightCorner: 	anchoredPosition = new Vector3((Screen.width / 2.0F) / MyAnchorCamera.scale, 
					(Screen.height / 2.0F) / MyAnchorCamera.scale, position.z); break;
			case Anchor.LowerLeftCorner: 		anchoredPosition = new Vector3((-Screen.width / 2.0F) / MyAnchorCamera.scale, 
					(-Screen.height / 2.0F) / MyAnchorCamera.scale, position.z); break;				
			case Anchor.LowerRightCorner: 	anchoredPosition = new Vector3((Screen.width / 2.0F) / MyAnchorCamera.scale,
					(-Screen.height / 2.0F) / MyAnchorCamera.scale, position.z); break;
				
//			case Anchor.TopLeftCorner:  
//										{
//											anchoredPosition = new Vector3((-Screen.width / 2.0F) / MyAnchorCamera.scale, 0, position.z);
//											anchoredPosition = new Vector3((-Screen.width / 2.0f) / MyAnchorCamera.scale, 
//					(Screen.height / 2.0F) / MyAnchorCamera.scale, position.z); 
//										}break;
//			case Anchor.TopRightCorner:  
//										{
//											anchoredPosition = new Vector3((Screen.width / 2.0F) / MyAnchorCamera.scale, 0, position.z);
//											anchoredPosition = new Vector3((Screen.width / 2.0F) / MyAnchorCamera.scale, 
//					(Screen.height / 2.0F) / MyAnchorCamera.scale, position.z); 
//										}break;	
//			case Anchor.LowerLeftCorner:
//										{
//											anchoredPosition = new Vector3((-Screen.width / 2.0F) / MyAnchorCamera.scale, 0, position.z);
//											anchoredPosition = new Vector3((-Screen.width / 2.0F) / MyAnchorCamera.scale, 
//					(-Screen.height / 2.0F) / MyAnchorCamera.scale, position.z);
//										}break;	
//			case Anchor.LowerRightCorner:
//										{
//											anchoredPosition = new Vector3((Screen.width / 2.0F) / MyAnchorCamera.scale, 0, position.z);
//											anchoredPosition = new Vector3((Screen.width / 2.0F) / MyAnchorCamera.scale,
//					(-Screen.height / 2.0F) / MyAnchorCamera.scale, position.z);
//										}break;	
			}
			
			_transform.localPosition = anchoredPosition + new Vector3(offset.x, offset.y, 0);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateTransform();
	}
}
