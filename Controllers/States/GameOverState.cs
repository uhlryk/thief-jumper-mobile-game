using UnityEngine;
using System.Collections;
using Assets.Code.Board;
using Assets.Code.AWGUI;
namespace Assets.Code.Controllers.States{
	public class GameOverState : AbstractState {
		
		private GameObject boardObject;
		private BlockController blockController;
		
		public int finalCountdown=2;
		private float finishTime;
		private int actualCountdown;
		
		public GameObject mainCamera;
		public CameraController cameraController;
		
		private BitmapText bitmapBestScore;
		private BitmapText bitmapScore;

		public GameObject playerObject;
		private PlayerController playerController;

		private int bestScore;

		public override void StartState(){
			mainCamera = GameObject.Find ("Main Camera");
			if (mainCamera == null) {
				throw new MissingReferenceException("Na scenie brak GameObjecr Main Camera");
			}
			cameraController=mainCamera.GetComponent<CameraController>();
			if (cameraController == null) {
				throw new MissingReferenceException("Main Camera GameObject nie posiada komponentu CameraController");
			}
			cameraController.isEnable = false;
			
			finishTime = Time.realtimeSinceStartup + finalCountdown;
			actualCountdown = (int)finishTime;

			bitmapBestScore = new BitmapText ();
			bitmapBestScore.setTextures (this.GetController().GetGuiAssets().digitsB);

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

			bestScore=PlayerPrefs.GetInt ("BestScore");
			if (this.GetController ().GetData ().actualScore > bestScore) {
				PlayerPrefs.SetInt("BestScore",this.GetController ().GetData ().actualScore);
				PlayerPrefs.Save();
			}
			AudioSource music = GameObject.Find ("Main Camera").GetComponent<AudioSource>();
			music.volume = 0.1f;
			this.GetController ().GetAd ().ShowBanner ();

			GameCenter.AddLeaderboard (GameCenter.Leaderboard.SCORE,this.GetController ().GetData ().actualScore);
			GameCenter.AddLeaderboard (GameCenter.Leaderboard.HEIGHT,this.GetController ().GetData ().actMaxHeight);

			GameCenter.AddAchievement (GameCenter.Achievement.HEIGHT_70,this.GetController ().GetData ().actMaxHeight/70*100);
			GameCenter.AddAchievement (GameCenter.Achievement.HEIGHT_150,this.GetController ().GetData ().actMaxHeight/150*100);

			GameCenter.AddAchievement (GameCenter.Achievement.SCORE_800,this.GetController ().GetData ().actualScore/800*100);
			GameCenter.AddAchievement (GameCenter.Achievement.SCORE_3000,this.GetController ().GetData ().actualScore/3000*100);

			GameCenter.AddAchievement (GameCenter.Achievement.KILL_30,this.GetController ().GetData ().actEnemyKilled/30*100);
			GameCenter.AddAchievement (GameCenter.Achievement.KILL_100,this.GetController ().GetData ().actEnemyKilled/100*100);

			GameCenter.AddAchievement (GameCenter.Achievement.LIFE_5,this.GetController ().GetData ().actMaxLifePoints/5*100);
			GameCenter.AddAchievement (GameCenter.Achievement.LIFE_13,this.GetController ().GetData ().actMaxLifePoints/13*100);
			//blockController.blockGroup.DestroyAllActive ();
			//		playerObject.transform.position=new Vector3(0,4,0);
		}
		public override void UpdateState () {
			actualCountdown = (int)finishTime - (int)Time.realtimeSinceStartup;
#if UNITY_ANDROID
				if (Input.GetKeyUp(KeyCode.Escape)){
					GameObject.Instantiate(this.GetController ().GetGuiAssets ().splashScreen);
					this.GetManager().ChangeScene("menu");
					this.GetManager().ChangeState(new MainMenuState());
				}
#endif
		}
		public override void OnGUIState(){
			if (actualCountdown < 1) {//po kilku sekundach wyświetlamy popup końca gry
				float relativeSize = Screen.height;
				if(relativeSize>1500)relativeSize=1500;
			//	if(relativeSize>430)relativeSize=430;
				float windowWidth = relativeSize * 0.55f;
				float windowHeight = relativeSize * 0.55f *1.146f;

			//	float marginWidth = relativeSize * 0.07f;
				float marginHeight = relativeSize * 0.16f;
				float buttonSize = relativeSize * 0.12f;
				if(buttonSize<60)buttonSize=60;


				GUI.BeginGroup (new Rect((Screen.width-windowWidth)/2,(Screen.height-windowHeight)/2-marginHeight*0.2f,windowWidth,windowHeight));
				
				GUI.DrawTexture(new Rect(0,0,windowWidth,windowHeight),this.GetController().GetGuiAssets().gameOverWindow,ScaleMode.ScaleToFit);

			/*	if(GUI.Button(new Rect(windowWidth*0.2f,marginHeight*2.2f,windowWidth*0.6f,marginHeight*0.45f),this.GetController().GetGuiAssets().facebookButton)){
					this.Share();
				}*/

				if(AWGUI.AWGUI.ButtonTexture(new Rect((windowWidth-buttonSize)/2-buttonSize,(windowHeight-marginHeight*1.1f),buttonSize,buttonSize),this.GetController().GetGuiAssets().restartButton,ScaleMode.ScaleToFit)){
					this.GetController ().GetAd ().HideBanner ();
					this.GetManager().ChangeScene("board");
					this.GetManager().ChangeState(new BeginGameState());
				}
				
				if(AWGUI.AWGUI.ButtonTexture(new Rect((windowWidth-buttonSize)/2+buttonSize,(windowHeight-marginHeight*1.1f),buttonSize,buttonSize),this.GetController().GetGuiAssets().menuButton,ScaleMode.ScaleToFit)){
					GameObject.Instantiate(this.GetController ().GetGuiAssets ().splashScreen);
					this.GetController ().GetAd ().HideBanner ();
					this.GetManager().ChangeScene("menu");
					this.GetManager().ChangeState(new MainMenuState());
				}

				Texture scoreLabel=this.GetController().GetGuiAssets().scoreLabel;
				GUI.DrawTexture(new Rect(0,marginHeight*0.9f,windowWidth,marginHeight*0.35f),scoreLabel,ScaleMode.ScaleToFit);
				bitmapScore.Draw (new Rect(0,marginHeight*1.4f,windowWidth,marginHeight*0.35f),this.GetController().GetData().actualScore.ToString());

				Texture bestScoreLabel=this.GetController().GetGuiAssets().bestScoreLabel;
				GUI.DrawTexture(new Rect(0,1.9f*marginHeight,windowWidth,marginHeight*0.35f),bestScoreLabel,ScaleMode.ScaleToFit);
				bitmapBestScore.Draw (new Rect(0,marginHeight*2.4f,windowWidth,marginHeight*0.35f),bestScore.ToString());

				GUI.EndGroup ();
			}

		}
		private void Share(){

		}
	}
}
