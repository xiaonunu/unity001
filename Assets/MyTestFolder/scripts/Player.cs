using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
	private int money;
	private TankeMode[] tankeArray=new TankeMode[5];
	void Awake(){
		TankeMode mode=new TankeMode(0,1);
		tankeArray[0]=mode;
		TankeMode mode1=new TankeMode(1,1);
		tankeArray[1]=mode1;
		DontDestroyOnLoad (gameObject);
	}
	// Use this for initialization
	void Start ()
	{
		
	}
	
	public TankeMode[] getPlayerTanke(){
			return tankeArray;
	}
	public int getPlayerMoney(){
		return money;	
	}
}
