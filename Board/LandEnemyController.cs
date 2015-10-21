using UnityEngine;
using System.Collections;
using Assets.Code.Controllers;
/**
 * sterujemy przeciwnikami którzy się poruszają po blokach
 */ 
public class LandEnemyController : MonoBehaviour {
	public LayerMask pathMask;
	public LayerMask wallMask;
	public LayerMask playerMask;
	public bool enabled=false;
	private PlayerController playerController;
	public AudioClip hitSound;

	private GameObject gameManager;
	private GameData gameData;

	void Start () {
		if(Random.value<0.5f){
			isRightDirection=true;
		}else{
			isRightDirection=false;
		}
		GameObject player = GameObject.Find ("Player");
		if(player==null){
			throw new MissingReferenceException("GameObject Player nie istnieje na scenie");
		}
		playerController=player.GetComponent<PlayerController>();
		if(playerController==null){
			throw new MissingReferenceException("GameObject "+player.name+" nie ma komponentu PlayerController");
		}
		isEnemyKilled = false;

		gameManager = GameObject.Find ("GameManager");
		gameData=gameManager.GetComponent<GameData>();
	}
	private bool isRightDirection;
	/*
		 * jeśli jakiś warunek zmienił kierunek a potem następny potrzebował zmiany kierunku to znaczy że oba kierunki są zablokowane i postać ma się nie ruszać;
		 */ 
	private bool currChangeDirection = false;
	/**
	 *  domyślnie false
	 * po trafieniu jest true
	 * ma to na celu by zanim obiekt zniknie nie zabił gracza;
	 */ 
	private bool isEnemyKilled;
	void FixedUpdate () {
		if(isEnemyKilled){
			return;
		}
		/**
		 * gracz zabił wroga
		 */ 
		if(Physics2D.OverlapCircle (new Vector2 (transform.position.x, transform.position.y+0.2f),0.3f,playerMask)){
			if (StateControllerManager.stateController.GetData ().isSound == true) {
				AudioSource.PlayClipAtPoint(hitSound, transform.position);
			}
			gameData.actualScore+=10;
			gameData.actEnemyKilled++;
			playerController.Jump();
			Destroy(gameObject);
			isEnemyKilled=true;
			return;

		}
		if(Physics2D.OverlapPoint (new Vector2 (transform.position.x, transform.position.y+0.2f),pathMask)){
			Destroy(gameObject);
			isEnemyKilled=true;
			return;
		}
		int direction;
		if(isRightDirection){
			direction=1;;
		}else{
			direction=-1;;
		}
		/*
		 * sprawdzamy kontakt z ścianami
		 */ 
		if(Physics2D.OverlapPoint (new Vector2 (transform.position.x+0.35f*direction, transform.position.y-0.15f),playerMask)){
			playerController.KillPlayer();
		}
		if(enabled==true){
			currChangeDirection=false;
			this.Move();
		}
	}
	/**
	 * obiekt porusza się w wyznaczonym kierunku
	 * nie mamy rigidbody! więc musimy kontrolować by nie spadali
	 */ 
	private void Move(){

		int direction;
		if(isRightDirection){
			direction=1;;
		}else{
			direction=-1;;
		}
		/*
		 * sprawdzamy kontakt z ścianami
		 */ 
		if(Physics2D.OverlapPoint (new Vector2 (transform.position.x+0.3f*direction, transform.position.y),wallMask)){
			if(this.PrepareFlipDirection()==false)return;
			this.Move();
			return;
		}
		/**
		 * sprawdzamy czy przed obiektem jest przepaść
		 */ 
		if(Physics2D.OverlapPoint (new Vector2 (transform.position.x+0.6f*direction, transform.position.y-0.7f),pathMask)==false){
			if(this.PrepareFlipDirection()==false)return;
			this.Move();
			return;
		}
		/**
		 * sprawdzamy czy przed obiektem jest przeszkoda
		 */ 
		if(Physics2D.OverlapPoint (new Vector2 (transform.position.x+0.6f*direction, transform.position.y),pathMask)){
			if(this.PrepareFlipDirection()==false)return;
			this.Move();
			return;
		}



		if(currChangeDirection==true){
			FlipDirection();
		}
		float translateX=(1.0f)*direction*Time.deltaTime;;
		gameObject.transform.Translate(translateX,0,0);
	}
	/*
	 * metoda zmienia kierunek. Jeśli jednak w danej turze zmieniła już kierunek to ponowna zmiana jest bez sensu, więc nie zmienia i zwraca false
	 */ 
	private bool PrepareFlipDirection(){
		if(currChangeDirection==true){
			return false;
		}
		currChangeDirection=true;
		isRightDirection = !isRightDirection;
		return true;
	}
	private void FlipDirection(){
		transform.localScale = new Vector3 (-transform.localScale.x,transform.localScale.y,transform.localScale.z);
	}

}
