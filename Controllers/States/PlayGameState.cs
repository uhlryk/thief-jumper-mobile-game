using UnityEngine;
using System.Collections;
using Assets.Code.Board;
using Assets.Code.AWGUI;
namespace Assets.Code.Controllers.States{
	public class PlayGameState : AbstractState {
		private GameObject boardObject;
		private BlockController blockController;
		private BitmapText bitmapScore;
		private PlayerController playerController;

		public override void StartState(){
			boardObject = GameObject.Find ("Board");
			if (boardObject == null) {
				throw new MissingReferenceException("Scena nie posiada Board GameObject");
			}
			blockController=boardObject.GetComponent<BlockController>();
			if (blockController == null) {
				throw new MissingReferenceException("Board GameObject nie posiada komponentu BlockController");
			}
			blockController.Start(this.GetController().GetData().boardSize,this.GetController().GetData().blockWidth,this.GetController().GetData().blockHeight);

			bitmapScore = new BitmapText ();
			bitmapScore.setTextures (this.GetController().GetGuiAssets().digitsB);

			GameObject playerObject = GameObject.Find ("Player");
			if (playerObject == null) {
				throw new MissingReferenceException("Scena nie posiada PLayer GameObject");
			}
			playerController=playerObject.GetComponent<PlayerController>();
			if (playerController == null) {
				throw new MissingReferenceException("Player GameObject nie posiada komponentu PlayerController");
			}
			AudioSource music = GameObject.Find ("Main Camera").GetComponent<AudioSource>();
			music.volume = 0.25f;
			Time.timeScale = 1;
			playerController.isJumpPressed = true;
		}
		public override void UpdateState () {
#if UNITY_ANDROID
				if (Input.GetKeyUp(KeyCode.Escape)){
					this.GetManager().ChangeState(new PauseGameState());
				}
#endif
			if (playerController.isEnable==false) {
			//	this.GetController().GetData().actualLifePoints--;
				if(this.GetController().GetData().actualLifePoints>1){
					this.GetManager().ChangeState(new HitState());
				}else{
					this.GetManager().ChangeState(new GameOverState());
				}
			}
			if(this.GetController().GetData().actualScore>this.GetController().GetData().lastPointsLife+this.GetController().GetData().exchangePoinsts){
				this.GetController().GetData().lastPointsLife+=this.GetController().GetData().exchangePoinsts;
				this.GetController().GetData().actualLifePoints++;
				if(this.GetController().GetData().actualLifePoints>this.GetController().GetData().actMaxLifePoints){
					this.GetController().GetData().actMaxLifePoints=this.GetController().GetData().actualLifePoints;
				}
			}
		}
		public override void OnGUIState(){
			float buttonSize = Screen.height * 0.15f;
			if(buttonSize>150)buttonSize=150;
			if(buttonSize<60)buttonSize=60;
			bitmapScore.Draw (new Rect(Screen.width*0.3f,15,Screen.width*0.4f,Screen.height*0.07f),this.GetController().GetData().actualScore.ToString());
			AWGUI.AWGUI.DrawLifes (new Rect(10,15,Screen.width*0.3f,40),this.GetController().GetData().actualLifePoints,this.GetController().GetGuiAssets().life);
			if(AWGUI.AWGUI.ButtonTexture(new Rect(Screen.width-buttonSize,5,buttonSize,buttonSize),this.GetController().GetGuiAssets().pauseButton,ScaleMode.ScaleToFit)){
				this.GetManager().ChangeState(new PauseGameState());
			}
		//	AWGUI.AWGUI.DrawNavigation(1f,false);
		}
	}
}