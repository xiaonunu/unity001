using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	
	private Transform myTransform;
	public GameObject sparkObject;
	// Use this for initialization
	void Start () {
		myTransform=this.transform;
		Invoke ("DestroyNow", 10);
	}
	void Update(){
	}
	//enter a collisider
	void OnCollisionEnter(Collision collision){
		print(collision.gameObject.name+"is collider for bullet,destroy bullet");
		Destroy(gameObject);
		Instantiate(sparkObject,myTransform.position,Quaternion.identity);
	}
	//enter tigger
	void OnTriggerEnter(Collider other){
		print(other.name+"is  trigger for bullet,some change to bullet --change bigger...");
		myTransform.localScale=new Vector3(1,1,1);
	}
	void DestroyNow(){
		Destroy(gameObject);
	}
}
