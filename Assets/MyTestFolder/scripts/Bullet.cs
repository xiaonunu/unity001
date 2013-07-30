using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	
	private Transform myTransform;
	public GameObject sparkObject;
	private int radius;
	private int attackValue;
	private int maxHp;
	private int currentHp;
	private float radiusUp;
	private float attackUp;
	private float maxHpUp;
	private float bulletNumsUp;
	// Use this for initialization
	void Start ()
	{
		myTransform = this.transform;
		Invoke ("DestroyNow", 10);
	}
	
	public void setValue(Tanke.TankeType type,int index){
		radius=TankeData.getRadius(type);
		attackValue=TankeData.getAttack(type);
		maxHp=TankeData.getMaxHp(type);
		radiusUp=TankeData.getRadiusUp(type);
		attackUp=TankeData.getAttackUp(type);
		maxHpUp=TankeData.getMaxHpUp(type);
		bulletNumsUp=TankeData.getBulletNumsUp(type);
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
