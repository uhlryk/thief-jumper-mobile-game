using UnityEngine;
using System.Collections;

public class SpikesController : MonoBehaviour {
	public LayerMask blockMask;
	public LayerMask enemyMask;
	public LayerMask playerMask;
	private GameData gameData;
	private PlayerController playerController;
	void Start () {
		GameObject gameManager = GameObject.Find ("GameManager");
		if(gameManager==null){
			throw new MissingReferenceException("GameObject GameManager nie istnieje na scenie");
		}
		gameData=gameManager.GetComponent<GameData>();
		if(gameData==null){
			throw new MissingReferenceException("GameObject "+gameManager.name+" nie ma komponentu GameData");
		}
		GameObject player = GameObject.Find ("Player");
		if(player==null){
			throw new MissingReferenceException("GameObject Player nie istnieje na scenie");
		}
		playerController=player.GetComponent<PlayerController>();
		if(playerController==null){
			throw new MissingReferenceException("GameObject "+player.name+" nie ma komponentu playerController");
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y+0.15f),0.01f,blockMask)){
			Destroy(gameObject);
		}
		if(Physics2D.OverlapCircle(transform.position,0.55f,enemyMask)){
			Destroy(gameObject);
		}
		//		if (playerController.isEnable) {
		if (Physics2D.OverlapCircle (transform.position, 0.30f, playerMask)) {
			playerController.KillPlayer();
			//		Debug.Log("Actual "+gameData.actualScore+" Get Score "+points);
		//	gameData.actualScore += points;
		//	AudioSource.PlayClipAtPoint(collectSound, transform.position);
		//	Destroy (gameObject);
		}
	}
}
