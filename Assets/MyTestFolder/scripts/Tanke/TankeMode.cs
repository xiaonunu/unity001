using UnityEngine;
using System.Collections;

public class TankeMode {

	public int id;
	public int hp;
	public int lv;
	public string name;
	public string info;
	public GameObject sprite;
	
	public TankeMode(int id,int lv){
		this.id=id;
		this.lv=lv;
		hp=TankeData.getMaxHp(id);
		name=TankeData.getName(id);
		info=TankeData.getInfo(id);
		sprite=GameObject.FindGameObjectWithTag("icon_data").GetComponent<Icon>().getSprite(id);
	}
}
