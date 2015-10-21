using UnityEngine;
using System.Collections;
using Assets.Code.Board;
using Assets.Code.Board.Block;
using Assets.Code.AWGUI;
namespace Assets.Code.Controllers.States{
	public class HitState : AbstractState {

		private GameObject boardObject;
		private BlockController blockController;

		public int finalCountdown=6;
		private float finishTime;
		private int actualCountdown;

		public GameObject mainCamera;
		public CameraController cameraController;

		private BitmapText bitmapText;
		private BitmapText bitmapScore;
		public GameObject playerObject;
		private PlayerController playerController;

		public override void StartState(){

			this.GetController().GetData().actualLifePoints--;
			mainCamera = GameObject.Find ("Main Camera");
			if (mainCamera == null) {
				throw new MissingReferenceException("Na sscenie brak GameObjecr Main Camera");
			}
			cameraController=mainCamera.GetComponent<CameraController>();
			if (cameraController == null) {
				throw new MissingReferenceException("Main Camera GameObject nie posiada komponentu CameraController");
			}
			cameraController.isEnable = false;

			finishTime = Time.realtimeSinceStartup + finalCountdown;
			actualCountdown = (int)finishTime;

			bitmapText = new BitmapText ();
			bitmapText.setTextures (this.GetController().GetGuiAssets().digitsB);
			bitmapScore = new BitmapText ();
			bitmapScore.setTextures (this.GetController().GetGuiAssets().digitsB);

			boardObject = GameObject.Find ("Board");
			if (boardObject == null) {
				throw new MissingReferenceException("Scena nie posiada Board GameObject");
			}
			blockController=boardObject.GetComponent<BlockController>();
			if (blockController == null) {
				throw new MissingReferenceException("Board GameObject nie posiada komponentu BlockController");
			}

			playerObject = GameObject.Find ("Player");
			if (playerObject == null) {
				throw new MissingReferenceException("Scena nie posiada PLayer GameObject");
			}
			playerController=playerObject.GetComponent<PlayerController>();
			if (playerController == null) {
				throw new MissingReferenceException("Player GameObject nie posiada komponentu PlayerController");
			}
			blockController.TurnOff ();
			actualCountdown = (int)finishTime;



	//		playerObject.transform.position=new Vector3(0,4,0);
		}
		public override void UpdateState () {

			actualCountdown = (int)finishTime - (int)Time.realtimeSinceStartup;
			if (actualCountdown == finalCountdown - 3&&this.GetController().GetData().countAllPlays%10==9) {
				this.GetController ().GetAd ().ShowBanner ();
			}
			if(actualCountdown==finalCountdown-3){
				Time.timeScale = 0;
				blockController.blockGroup.RemoveAllActive ();
				blockController.ClearEnemy();
				int posX=blockController.blockGroup.GetRestMaxPos();
				int posY=blockController.blockGroup.GetRestMaxVal()-1;

				playerController.Spawn();
				cameraController.isEnable = true;
				if(posY>=0){
					BlockObject blockObject=blockController.blockGroup.GetRestBlock(posX,posY);
					blockObject.DestroyAddiction();
					Vector3 blockPosition=blockObject.block.position;
					playerObject.transform.position=new Vector3(blockPosition.x,blockPosition.y+0.92f,0);
				}else{
					playerObject.transform.position=new Vector3(0,0.49f,0);
				}			
			}
			if(actualCountdown==0){
				Time.timeScale = 1;
			}
			if (actualCountdown < 0) {
				blockController.TurnOn();
				playerController.isEnable=true;
				this.GetController ().GetAd ().HideBanner ();
				this.GetManager().ChangeState(new PlayGameState());
			}
		}
		public override void OnGUIState(){
			if (actualCountdown > 0&&actualCountdown<=finalCountdown-3) {
				float readyLabelWidth=Screen.height/3;
				float readyLabelHeight=Screen.height/6;
				if (actualCountdown > 0) {
					GUI.DrawTexture(new Rect((Screen.width-readyLabelWidth)/2,Screen.height*0.12f,readyLabelWidth,readyLabelHeight),this.GetController().GetGuiAssets().getReadyLabel,ScaleMode.ScaleToFit);
					bitmapText.Draw (new Rect((Screen.width-Screen.height*0.25f)/2,Screen.height*0.24f,Screen.height*0.25f,Screen.height*0.1f),actualCountdown.ToString());
				}
			}
			if (actualCountdown == 0) {
				float startLabelWidth=Screen.height/2;
				float startLabelHeight=Screen.height/5;
				GUI.DrawTexture(new Rect((Screen.width-startLabelWidth)/2,Screen.height*0.15f,startLabelWidth,startLabelHeight),this.GetController().GetGuiAssets().startLabel,ScaleMode.ScaleToFit);
			}
			bitmapScore.Draw (new Rect(Screen.width*0.3f,15,Screen.width*0.4f,Screen.height*0.07f),this.GetController().GetData().actualScore.ToString());
			AWGUI.AWGUI.DrawLifes (new Rect(10,15,Screen.width*0.3f,40),this.GetController().GetData().actualLifePoints,this.GetController().GetGuiAssets().life);
		/*	if(GUI.Button(new Rect(Screen.width-75,5,70,70),this.GetController().GetGuiAssets().pauseButton)){
				this.GetManager().ChangeState(new PauseGameState());
			}*/
		}
	}
}
