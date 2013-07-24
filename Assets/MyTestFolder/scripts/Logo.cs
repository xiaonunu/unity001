using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour {
	
	private AsyncOperation async;
	public GameObject load_text;
	private tk2dTextMesh text_mesh;
	// Use this for initialization
	void Start () {
		StartCoroutine(loadGame());
		text_mesh=load_text.GetComponent<tk2dTextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		text_mesh.text=async.progress*100+"%";
		text_mesh.Commit();
	}
	IEnumerator loadGame(){
		async=Application.LoadLevelAsync(1);
		yield return async;
	}
}
