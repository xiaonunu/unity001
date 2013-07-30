using UnityEngine;
using System.Collections;

public class PrefabList : MonoBehaviour
{

	public GameObject tanke_ball;
	public GameObject tanke_bow;
	public GameObject bullet_ball;
	public GameObject bullet_bow;
	
	public GameObject getTankeObject (Tanke.TankeType type)
	{
		if (type.Equals (Tanke.TankeType.ball)) {
			return tanke_ball;
		}
		if (type.Equals (Tanke.TankeType.bow)) {
			return tanke_bow;
		}	
		return tanke_ball;
	}

	public GameObject getBulletObject (Tanke.TankeType type)
	{
		if (type.Equals (Tanke.TankeType.ball)) {
			return bullet_ball;
		}
		if (type.Equals (Tanke.TankeType.bow)) {
			return bullet_bow;
		}	
		return bullet_ball;
	}
}
