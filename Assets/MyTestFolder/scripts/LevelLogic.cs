using UnityEngine;
using System.Collections;

public class LevelLogic : MonoBehaviour
{
	private AsyncOperation async;
	private Player player;
	public GameObject sprite_p;
	private Vector3 sprite_v=new Vector3(0.4f,0,0);
	public GameObject infoListObject;
	public GameObject name_object;
	public GameObject lv_object;
	public GameObject hp_object;
	public GameObject info_object;
	private tk2dTextMesh name_text;
	private tk2dTextMesh lv_text;
	private tk2dTextMesh hp_text;
	private tk2dTextMesh info_text;
	// Use this for initialization
	void Start ()
	{
		infoListObject.SetActiveRecursively(false);
		name_text=name_object.GetComponent<tk2dTextMesh>();
		lv_text=lv_object.GetComponent<tk2dTextMesh>();
		hp_text=hp_object.GetComponent<tk2dTextMesh>();
		info_text=info_object.GetComponent<tk2dTextMesh>();
		player=GameObject.FindGameObjectWithTag("player_data").GetComponent<Player>();
		show_player_tanke();
	}
	
	
	private void show_player_tanke(){
		TankeMode[] tankes=player.getPlayerTanke();
		int length=tankes.Length;
		for(int i=0;i<length;i++){
			if(tankes[i]!=null){
				Instantiate(tankes[i].sprite,sprite_p.transform.position+sprite_v*i,Quaternion.identity);
			}else{
				break;	
			}
		}
		show_tanke_info(tankes[0]);
	}
	
	private void show_tanke_info(TankeMode tanke){
		infoListObject.SetActiveRecursively(true);
		name_text.text=tanke.name;
		lv_text.text=tanke.lv+"";
		hp_text.text=tanke.hp+"";
		info_text.text=tanke.info;
		name_text.Commit();
		lv_text.Commit();
		hp_text.Commit();
		info_text.Commit();
	}
	public void start_game ()
	{
		StartCoroutine (loadGame ());
	}

	IEnumerator loadGame ()
	{
		async = Application.LoadLevelAsync (2);
		yield return async;
	}
}
