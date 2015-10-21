using UnityEngine;
using System.Collections;
using Assets.Code.Board.Block;
using Assets.Code.Controllers;
using Assets.Code.AWGUI;
public class PlayerController : MonoBehaviour {
	private bool isGrounded=false;

	/*
	 * kiedy nie było skoku lub był tylko pierwszy to true, jeśli był drugi to falseś
	 */
	private bool isFirstJump=true;
	public bool isJumpPressed=false;
	private bool isLeftPressed=false;
	private bool isRightPressed=false;

	public LayerMask groundMask;
	public float groundRadius;
	public Transform groundCheck;

	public Transform blockCheck;
	public LayerMask blockMask;
	public float blockRadius;

	private bool isRightDirection=true;

	private float maxSpeed=3.5f;
	private float velocity=8f;

	public bool enableInput=true;

	public bool isEnable;
	public AudioClip hitSound;
	public AudioClip jumpSound;

	public Animator animator;

	public void KillPlayer(){
	//	AudioSource.PlayClipAtPoint(hurtclip, transform.position);
		isEnable=false;
		//	collider2D.enabled=false;
		BoxCollider2D[] boxColliders =GetComponents<BoxCollider2D>();
		CircleCollider2D[] circleColliders =GetComponents<CircleCollider2D>();
		foreach(BoxCollider2D bc in boxColliders) bc.enabled = false;
		foreach(CircleCollider2D bc in circleColliders) bc.enabled = false;
		if (StateControllerManager.stateController.GetData ().isSound == true) {
			AudioSource.PlayClipAtPoint(hitSound, transform.position);
		}
	}
	public void Spawn(){
	//	isEnable=true;
		//	collider2D.enabled=false;
		BoxCollider2D[] boxColliders =GetComponents<BoxCollider2D>();
		CircleCollider2D[] circleColliders =GetComponents<CircleCollider2D>();
		foreach(BoxCollider2D bc in boxColliders) bc.enabled = true;
		foreach(CircleCollider2D bc in circleColliders) bc.enabled = true;
		this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3 (0,0,0);
	}
	void Start () {
		isEnable=true;
	//	enableInput = false;
	}
	void FixedUpdate(){
		if (isEnable==false) {
			return;
		}
		if(groundCheck!=null&&groundMask!=null){
			BoxCollider2D blockCollider = (BoxCollider2D)Physics2D.OverlapCircle (groundCheck.position, groundRadius, groundMask);
			if(blockCollider!=null){
				isGrounded=true;
				isFirstJump=true;
			//	Debug.Log("Grounded");
			}else{
				isGrounded=false;
			}

		}
		if(blockCheck!=null&&blockMask!=null){
			if(isGrounded==true){
				bool resp = Physics2D.OverlapCircle (blockCheck.position,blockRadius, blockMask);
				if(resp){
					KillPlayer();
				}
			}
		}
	}

	void Update () {
		if(enableInput==true&&isEnable==true){
			if(Input.GetMouseButtonDown(0)){
				this.SetMove(Input.mousePosition);
			}else if (Input.GetMouseButtonUp(0)){
				isJumpPressed=false;
				isLeftPressed=false;
				isRightPressed=false;
			}
			if (Input.touchCount > 0) {
				isLeftPressed=false;
				isRightPressed=false;

				for(int i=0;i<Input.touchCount;i++){
					Touch touch = Input.GetTouch(i);
					if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled){
						this.SetMove(touch.position);
					}
				}
				if(Input.touchCount==2&&isLeftPressed&&isRightPressed){//mamy wduszone 2 przyciski ale żaden nie jest skokiem więc resetujemy skok
					isJumpPressed=false;
				}
				if(Input.touchCount==1&&(isLeftPressed||isRightPressed)){//mamy wduszony 1 przycisk ale żaden nie jest skokiem więc resetujemy skok
					isJumpPressed=false;
				}
				Debug.Log ("touchCount "+Input.touchCount+" isLeftPressed "+isLeftPressed+" isRightPressed "+isRightPressed+" isJumpPressed "+isJumpPressed);
			}

			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				isLeftPressed=true;
			}
			if(Input.GetKeyUp(KeyCode.LeftArrow)){
				isLeftPressed=false;
			}
			if(isLeftPressed){
				this.MoveLeft();
			}
			if(Input.GetKeyDown(KeyCode.RightArrow)){
				isRightPressed=true;
			}
			if(Input.GetKeyUp(KeyCode.RightArrow)){
				isRightPressed=false;
			}
			if(isRightPressed){
				this.MoveRight();
			}

			if(Input.GetKeyDown(KeyCode.RightArrow)){
				this.MoveRight();
			}
			if(Input.GetKeyDown(KeyCode.Space)){
				this.MoveJump();
				isJumpPressed=true;
			}else if(Input.GetKeyUp(KeyCode.Space)){
				isJumpPressed=false;
			}
			this.ControlSpeed();
		}else{
			isJumpPressed=false;
			isLeftPressed=false;
			isRightPressed=false;
		}
		if (animator != null) {
			animator.SetFloat ("velocity", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
		}
	}
	private float marginNavX = Screen.height*0.02f;
	private float marginNavY = Screen.height*0.02f;
	private float sizeNavButton = Screen.height * 0.15f;


	private void MoveLeft(){
		Vector2 actVelocity=GetComponent<Rigidbody2D>().velocity;
		GetComponent<Rigidbody2D>().velocity=new Vector2(actVelocity.x-velocity*Time.deltaTime,actVelocity.y);
		if(isRightDirection){
			this.FlipDirection();
		}
	}
	private void MoveRight(){
		Vector2 actVelocity=GetComponent<Rigidbody2D>().velocity;
		GetComponent<Rigidbody2D>().velocity=new Vector2(actVelocity.x+velocity*Time.deltaTime,actVelocity.y);
		if(isRightDirection==false){
			this.FlipDirection();
		}
	}
	private void MoveJump(){
		if((isGrounded||isFirstJump)&&isJumpPressed==false){
			this.Jump();
			if(isGrounded==false){//znaczy że byliśmy w powietrzu a to był już drugi skok
				isFirstJump=false;
			}
			isGrounded=false;
		}
	}
	private void ControlSpeed(){
		Vector2 velocity=GetComponent<Rigidbody2D>().velocity;
		float maxTimeSpeed = maxSpeed;
		if(velocity.x>maxTimeSpeed){
			velocity.x=maxTimeSpeed;
		}
		if(velocity.x<-maxTimeSpeed){
			velocity.x=-maxTimeSpeed;
		}
		GetComponent<Rigidbody2D>().velocity = velocity;
	}
	private void SetMove(Vector3 point){
	//	Debug.Log ("SetMove "+point.x+" "+point.y);
		if(point.y<Screen.height*0.8f){
			if(point.x>0&&point.x<sizeNavButton+marginNavX+marginNavX/2){
				Debug.Log("PRESS LEFT");
				isLeftPressed=true;
			}else if(point.x>=sizeNavButton+marginNavX+marginNavX/2&&point.x<2*sizeNavButton+2*marginNavX+marginNavX/2){
				Debug.Log("PRESS RIGHT");
				isRightPressed=true;
			}else{
				Debug.Log("PRESS JUMP");
				this.MoveJump();

				isJumpPressed=true;		
			}
		}
		this.ControlSpeed();
	}
	private void StopMove(Vector3 point){
		if(point.y>transform.position.y+4f)return;
		if(point.y>transform.position.y+1f){
			if(point.x>0&&point.x<sizeNavButton+marginNavX+marginNavX/2){
				isLeftPressed=false;
			}else if(point.x>=sizeNavButton+marginNavX+marginNavX/2&&point.x<2*sizeNavButton+2*marginNavX+marginNavX/2){
				isRightPressed=false;
			}else{
				isJumpPressed=false;		
			}
		}
	//	this.ControlSpeed();
	}
	void OnGUI(){
		if(isEnable==true){
			if(sizeNavButton<70)sizeNavButton=70;
			Texture navLeft=StateControllerManager.stateController.GetGuiAssets().navLeftImg;
			GUI.DrawTexture (new Rect (marginNavX, Screen.height - marginNavY - sizeNavButton, sizeNavButton, sizeNavButton), navLeft, ScaleMode.ScaleToFit);

			Texture navRight=StateControllerManager.stateController.GetGuiAssets().navRightImg;
			GUI.DrawTexture (new Rect(2*marginNavX+sizeNavButton,Screen.height-marginNavY-sizeNavButton,sizeNavButton,sizeNavButton),navRight,ScaleMode.ScaleToFit);
				
			Texture navUp=StateControllerManager.stateController.GetGuiAssets().navUpImg;
			GUI.DrawTexture (new Rect(Screen.width-marginNavX-sizeNavButton,Screen.height-marginNavY-sizeNavButton,sizeNavButton,sizeNavButton),navUp,ScaleMode.ScaleToFit);
		}
	}
	public void Jump(){
		GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x,5.7f);
		if (StateControllerManager.stateController.GetData ().isSound == true) {
			AudioSource.PlayClipAtPoint(jumpSound, transform.position);
		}	
	}
	private void FlipDirection(){
		isRightDirection = !isRightDirection;
		//	float xScale = -transform.localScale.x;
		transform.localScale = new Vector3 (-transform.localScale.x,transform.localScale.y,transform.localScale.z);
	}
}
