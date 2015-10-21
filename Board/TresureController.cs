using UnityEngine;
using System.Collections;

using Assets.Code.Controllers;
namespace Assets.Code.Board{
	public class TresureController : MonoBehaviour {

		public Sprite[] spriteList;
		public int[] pointList;
		public LayerMask blockMask;
		public LayerMask enemyMask;
		public LayerMask playerMask;
		

		private int type;
		private int points;

		private GameData gameData;

		public AudioClip collectSound;

		//private PlayerController playerController;
		void Start () {
			GameObject gameManager = GameObject.Find ("GameManager");
			if(gameManager==null){
				throw new MissingReferenceException("GameObject GameManager nie istnieje na scenie");
			}
			gameData=gameManager.GetComponent<GameData>();
			if(gameData==null){
				throw new MissingReferenceException("GameObject "+gameManager.name+" nie ma komponentu GameData");
			}
	/*		GameObject player = GameObject.Find ("Player");
			if(player==null){
				throw new MissingReferenceException("GameObject Player nie istnieje na scenie");
			}*/
		//	playerController=player.GetComponent<PlayerController>();
		//	if(playerController==null){
		//		throw new MissingReferenceException("GameObject "+player.name+" nie ma komponentu PlayerController");
		//	}

		}
		/**
		 * określa jaki typ skarbu ma być wyświetlany
		 */ 
		public void SetType(int type){
			this.type = type;
			points=pointList[type];
			GetComponent<SpriteRenderer> ().sprite = spriteList [type];
		}
		void FixedUpdate(){
			if(Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y+0.3f),0.15f,blockMask)){
				Destroy(gameObject);
			}
			if(Physics2D.OverlapCircle(transform.position,0.55f,enemyMask)){
				Destroy(gameObject);
			}
			if (Physics2D.OverlapCircle (transform.position, 0.55f, playerMask)) {
				gameData.actualScore += points;
				if (StateControllerManager.stateController.GetData ().isSound == true) {
					AudioSource.PlayClipAtPoint(collectSound, transform.position);
				}
				Destroy (gameObject);
			}
		}
	}
}
