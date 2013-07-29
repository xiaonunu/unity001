using UnityEngine;
using System.Collections;

public class Tanke : MonoBehaviour {
	public enum TankeType { ball, bow } 
	public TankeType type;
	private Bullet bullet;//notice  type need macth bullet
	private int level;
	private int radius;
	private int attackValue;
	private int maxHp;
	private int currentHp;
	private float radiusUp;
	private float attackUp;
	private float maxHpUp;
	private float bulletNumsUp;
	// Use this for initialization,get basic value for lv1
	void Start () {
		radius=Data.getRadius(type);
		attackValue=Data.getAttack(type);
		maxHp=Data.getMaxHp(type);
	}
	
	public void setValue(int lv){//set value on this lv
		level=lv;
	}
	
	public void shooting(){
		
	}
	// Update is called once per frame
	void Update () {
	
	}
}
