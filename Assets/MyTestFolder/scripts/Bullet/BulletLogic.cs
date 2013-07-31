using UnityEngine;
using System.Collections;

public class BulletLogic : MonoBehaviour
{
	
	private Transform myTransform;
	public GameObject sparkObject;
	private int radius;
	private int attackValue;
	private float radiusUp;
	private float attackUp;
	// Use this for initialization
	void Start ()
	{
		myTransform = this.transform;
		Invoke ("DestroyNow", 10);
	}
	
	public void setValue(int id,int tank_lv){
		radius=BulletData.getRadius(id);
		attackValue=BulletData.getAttack(id);
		radiusUp=BulletData.getRadiusUp(id);
		attackUp=BulletData.getAttackUp(id);
	}
	//enter a collisider
	void OnCollisionEnter (Collision collision)
	{
		Destroy (gameObject);
		Instantiate (sparkObject, myTransform.position, Quaternion.identity);
	}
	//enter tigger
	void OnTriggerEnter (Collider other)
	{
		myTransform.localScale = new Vector3 (1, 1, 1);
	}

	void DestroyNow ()
	{
		Destroy (gameObject);
	}
}
