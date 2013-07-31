using UnityEngine;
using System.Collections;

public class Icon : MonoBehaviour {
	
	public GameObject sprite_ball;
	public GameObject sprite_bow;
	public GameObject iconListObject;
	
	void Start(){
		iconListObject.SetActiveRecursively(false);
		DontDestroyOnLoad(gameObject);	
	}
	public GameObject getSprite(int id){
		if(id==0){
			return  sprite_ball;
		}
		if(id==1){
			return sprite_bow;
		}
		return sprite_ball;
	}
}
