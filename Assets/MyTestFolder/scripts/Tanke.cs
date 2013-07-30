using UnityEngine;
using System.Collections;

public class Tanke : MonoBehaviour {
	public enum TankeType { ball, bow } 
	public TankeType type;
	public GameObject bullet_prefab;//notice  type need macth bullet
	private int level;

	// Use this for initialization,get basic value for lv1
	void Start () {
		
	}
	
	public void setValue(int lv){//set value on this lv
		level=lv;
	}
	
	public void shooting(Vector3 shoot_vector,Vector3 force){
		GameObject bullet=	(GameObject)(Instantiate(bullet_prefab,shoot_vector,Quaternion.identity));
		bullet.GetComponent<Bullet>().setValue(type,level);
		bullet.rigidbody.AddForce(force.x,force.y,0);
	}
}
