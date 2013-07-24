using UnityEngine;
using System.Collections;
//[ExecuteInEditMode]
public class BKAnchor : MonoBehaviour {
	public enum Anchor
	{
		Anchor_x,
		Anchor_y,
		Anchor_x_y,
	}
	private MyAnchorCamera MyAnchorCamera;
	private Transform MyTransform;
	private float MyWidth;
	private float MyHight;
	private float scale_x = 0;
	private float scale_y = 0;
	public Anchor anchor;
	// Use this for initialization

	void Start () {
		MyAnchorCamera = GameObject.FindWithTag("MainCamera").GetComponent<MyAnchorCamera>();
		MyTransform = transform;
		MyWidth = MyTransform.renderer.material.mainTexture.width * MyTransform.localScale.x;
		MyHight = MyTransform.renderer.material.mainTexture.height * MyTransform.localScale.y;	
		
		scale_x = ((Screen.width - MyWidth)) / MyWidth;
		scale_y = ((Screen.height - MyHight)) / MyHight;
//		Debug.Log(MyWidth + " "+ MyHight);
		BKAdapt();
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	public void BKAdapt()
	{
		switch(anchor)
		{
		case Anchor.Anchor_x:		MyTransform.localScale = new Vector3((MyTransform.localScale.x + scale_x) / MyAnchorCamera.scale,
				MyTransform.localScale.y,MyTransform.localScale.z);break;
		case Anchor.Anchor_y:		MyTransform.localScale = new Vector3(MyTransform.localScale.x,
				(MyTransform.localScale.y + scale_y) /  MyAnchorCamera.scale,MyTransform.localScale.z);break;			
		case Anchor.Anchor_x_y:		MyTransform.localScale = new Vector3((MyTransform.localScale.x + scale_x) / MyAnchorCamera.scale,
				(MyTransform.localScale.y + scale_y) / MyAnchorCamera.scale,MyTransform.localScale.z);break;
		}
		
	}
}
