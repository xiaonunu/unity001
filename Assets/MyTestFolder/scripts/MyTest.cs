using UnityEngine;
using System.Collections;

public class MyTest : MonoBehaviour {
	
	public GameObject bullet_prefab;
	public GameObject temp_object;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//shoot a bullet
	public void shoot_bullet(){
		GameObject bullet=	(GameObject)(Instantiate(bullet_prefab,temp_object.transform.position,Quaternion.identity));
		bullet.rigidbody.AddForce(200,50,0);
	}
}
