using UnityEngine;
using System.Collections;

public class ShootLogic : MonoBehaviour
{
	private Ray ray ;
	private RaycastHit hit;
	private bool press_flag;
	public GameObject power_object;
	public GameObject shoot_object;
	public GameObject bullet_prefab;
	private Vector3 press_vector;
	private int press_time;
	private float power_value=0;
	public int power_max=200;
	// Use this for initialization
	void Start ()
	{
		power_object.transform.localScale=new Vector3(0,2,2);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetMouseButton (0)) {
			if (!press_flag) {
				press_flag=true;
				press_time=0;
				power_value=0;
				createRay ();	
			}else{
				press_time++;
				power_value=(float)(press_time*100*0.0001);
				power_value=power_value>1?1:power_value;
				power_object.transform.localScale=new Vector3(power_value,2,2);
			}
		}
		if (Input.GetMouseButtonUp(0)) {
			press_flag=false;	
			press_time=0;
			power_object.transform.localScale=new Vector3(0,2,2);
			shoot_start();
		}
	}

	void createRay ()
	{
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray, out hit, 500)) {
			press_vector= hit.point;
			print(shoot_object.transform.position+"~~~~~~~~~~"+hit.point);
		}
	}
	
	void shoot_start(){
		Vector3 shoot_vector=shoot_object.transform.position;
		GameObject bullet=	(GameObject)(Instantiate(bullet_prefab,shoot_vector,Quaternion.identity));
		float power_x,power_y;
		Vector3 temp_vector=press_vector-shoot_vector;
		float long_value=Mathf.Sqrt(Mathf.Pow(temp_vector.x,2)+Mathf.Pow(temp_vector.y,2));
		power_x=power_value*temp_vector.x*power_max/long_value;
		power_y=power_value*temp_vector.y*power_max/long_value;
		print(power_x+"###"+power_y+"##"+temp_vector);
	//	float angle=Vector3.Angle(temp_vector,Vector3.right);
	//	power_x=Mathf.Cos(angle)*power_value*power_max;
	//	power_y=Mathf.Sin(angle)*power_value*power_max;
		bullet.rigidbody.AddForce(power_x,power_y,0);
	}
}
