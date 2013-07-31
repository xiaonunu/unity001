using UnityEngine;
using System.Collections;

public class TankeLogic : MonoBehaviour {
	public GameObject bullet_prefab;//notice  type need macth bullet
	private TankeMode tank;
	void Start () {
		
	}
	public void setTankeMode(){
		
	}
	public void shooting(Vector3 shoot_vector,Vector3 force){
		GameObject bullet=	(GameObject)(Instantiate(bullet_prefab,shoot_vector,Quaternion.identity));
		bullet.GetComponent<BulletLogic>().setValue(tank.id,tank.lv);
		bullet.rigidbody.AddForce(force.x,force.y,0);
	}
}
