using UnityEngine;
using System.Collections;
using Assets.Code.Board;
using Assets.Code.AWGUI;
namespace Assets.Code.Controllers.States{
	public class BeginGameState : AbstractState {

		public int finalCountdown=3;
		private float finishTime;
		private int actualCountdown;
		private GameObject boardObject;
		private BoardBuilder boardBuilder;
		private BitmapText bitmapText;
		private BitmapText bitmapScore;
		/**
		 * zmienna ma stan false, w update sprawdzamy czy scena się wczytała jeśli się wczytała to ustawiami isInit na true, odpalamy jednorazowo metodę init
		 * mechanizm trzeba stosować tam gdzie stan wywołany jest razem ze zmianą sceny
		 * w przyszłości zmiana sceny powinna być bardziej kontrolowana przez manager który zadba o załadowanie sceny
		 */ 
		private bool isInit;
		public override void StartState(){
			isInit = false;
		//	Time.timeScale = 0;
		}
		private void Init(){

			this.GetController().GetData ().countAllPlays = PlayerPrefs.GetInt ("countAllPlays"); 
			this.GetController().GetData ().countAllPlays++;
			PlayerPrefs.SetInt ("countAllPlays",this.GetController().GetData ().countAllPlays);
			PlayerPrefs.Save();

			this.GetController().GetData ().countActPlays++;

			Time.timeScale = 0;
			this.isInit = true;
			boardObject = GameObject.Find ("Board");
			if (boardObject == null) {
				throw new MissingReferenceException("Scena nie posiada Board GameObject");
			}
			boardBuilder=boardObject.GetComponent<BoardBuilder>();
			if (boardBuilder == null) {
				throw new MissingReferenceException("Board GameObject nie posiada komponentu BoardBuilder");
			}
			boardBuilder.Build (this.GetController().GetData().boardSize,this.GetController().GetData().blockWidth,this.GetController().GetData().blockHeight);
			finishTime = Time.realtimeSinceStartup + finalCountdown;

			bitmapText = new BitmapText ();
			bitmapText.setTextures (this.GetController().GetGuiAssets().digitsB);
			bitmapScore = new BitmapText ();
			bitmapScore.setTextures (this.GetController().GetGuiAssets().digitsB);
			actualCountdown = 0;
			this.GetController ().GetData ().actualLifePoints = this.GetController ().GetData ().startLifePoints;
			this.GetController ().GetData ().lastPointsLife = 0;
			this.GetController ().GetData ().actualScore = 0;

			this.GetController ().GetData ().actEnemyKilled = 0;
			this.GetController ().GetData ().actMaxHeight = 0;
			this.GetController ().GetData ().actMaxLifePoints = this.GetController ().GetData ().startLifePoints;
			AudioSource music = GameObject.Find ("Main Camera").GetComponent<AudioSource>();
			if (this.GetController ().GetData ().isMusic == false) {
				music.enabled=false;
			}else{
				music.enabled=true;
				music.volume = 0.1f;
			}
			actualCountdown = (int)finishTime;
			GameObject playerObject = GameObject.Find ("Player");
			if(playerObject==null){
				throw new MissingReferenceException("Brakuje GameObject Player");
			}
			playerObject.transform.position=new Vector3(0,0.49f,0);
		}
		public override void UpdateState () {
			if (Application.isLoadingLevel == false) {//kiedy scena się wczyta to odpali się ten blok
				if(isInit==false){//jednorazowo inicjujemy stan przez wywołanie metody Init
					this.Init();
				}else{//mamy wszystko zainicjowane możemy przeprowadzać akcje powtarzalne
					actualCountdown = (int)finishTime - (int)Time.realtimeSinceStartup;
					if(actualCountdown==0){
						Time.timeScale = 1;
					}
					if (actualCountdown < 0) {
						this.GetManager().ChangeState(new PlayGameState());
					}
				}
			}


		}
		public override void OnGUIState(){
			if (this.isInit==true) {
				float buttonSize = Screen.height * 0.15f;
				if(buttonSize>150)buttonSize=150;
				if(buttonSize<60)buttonSize=60;
				float readyLabelWidth=Screen.height/3;
				float readyLabelHeight=Screen.height/6;

				if (actualCountdown > 0) {
					GUI.DrawTexture(new Rect((Screen.width-readyLabelWidth)/2,Screen.height*0.12f,readyLabelWidth,readyLabelHeight),this.GetController().GetGuiAssets().getReadyLabel,ScaleMode.ScaleToFit);
					bitmapText.Draw (new Rect((Screen.width-Screen.height*0.25f)/2,Screen.height*0.24f,Screen.height*0.25f,Screen.height*0.1f),actualCountdown.ToString());
				}
				if (actualCountdown == 0) {
					float startLabelWidth=Screen.height/2;
					float startLabelHeight=Screen.height/5;
					GUI.DrawTexture(new Rect((Screen.width-startLabelWidth)/2,Screen.height*0.15f,startLabelWidth,startLabelHeight),this.GetController().GetGuiAssets().startLabel,ScaleMode.ScaleToFit);
				}
				bitmapScore.Draw (new Rect(Screen.width*0.3f,15,Screen.width*0.4f,Screen.height*0.07f),this.GetController().GetData().actualScore.ToString());
				AWGUI.AWGUI.DrawLifes (new Rect(10,15,Screen.width*0.3f,40),this.GetController().GetData().actualLifePoints,this.GetController().GetGuiAssets().life);
				if(AWGUI.AWGUI.ButtonTexture(new Rect(Screen.width-buttonSize,5,buttonSize,buttonSize),this.GetController().GetGuiAssets().pauseButton,ScaleMode.ScaleToFit)){
					this.GetManager().ChangeState(new PauseGameState());
				}
			}
		//	GUI.Label(new Rect((Screen.width-100)/2,(Screen.height-100)/2+100,100,100),actualCountdown.ToString());
		}
	}
}