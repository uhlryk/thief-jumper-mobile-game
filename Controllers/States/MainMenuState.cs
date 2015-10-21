using UnityEngine;
using System.Collections;
using Assets.Code.AWGUI;


namespace Assets.Code.Controllers.States{
	public class MainMenuState : AbstractState {
		private int bestScore;
		private BitmapText bitmapText;
		private AudioSource music;

		public override void StartState(){

			bestScore = PlayerPrefs.GetInt ("BestScore");
			bitmapText = new BitmapText ();
			bitmapText.SetText(bestScore.ToString());
			bitmapText.setTextures (this.GetController().GetGuiAssets().digitsB);

			if (PlayerPrefs.GetInt ("MusicStatus") == 0) {//turn on 0
				this.GetController().GetData().isMusic=true;
			}else{
				this.GetController().GetData().isMusic=false;
			}
			if (PlayerPrefs.GetInt ("SoundStatus") == 0) {//turn on 0
				this.GetController().GetData().isSound=true;
			}else{
				this.GetController().GetData().isSound=false;
			}



			music = GameObject.Find ("Main Camera").GetComponent<AudioSource>();
			if (this.GetController ().GetData ().isMusic == false) {
				music.enabled=false;
			}else{
				music.enabled=true;
				music.volume = 0.25f;
			}
			if(this.GetController().GetData().showBanner==true){
				Debug.Log("Próba wyświetlenia popapu !!!!");
				this.GetController().GetData().showBanner=false;
				this.GetController().GetAd().ShowInterstitial();
			}else{
				this.GetController ().GetAd ().ShowBanner ();
			}
		}

		public override void UpdateState () {
#if UNITY_ANDROID
				if (Input.GetKeyUp(KeyCode.Escape)){
					Application.Quit();
				}
#endif
			if (Input.GetKeyDown (KeyCode.Space)) {
				GameObject.Instantiate(this.GetController ().GetGuiAssets ().splashScreen);
				this.GetManager().ChangeScene("board");
				this.GetManager().ChangeState(new BeginGameState());			
			}
			if(music==null){
				music = GameObject.Find ("Main Camera").GetComponent<AudioSource>();
			}
			if (this.GetController ().GetData ().isMusic == false) {
				music.enabled=false;
			}else{
				music.enabled=true;
				music.volume = 0.25f;
			}
		}
		public override void OnGUIState(){
			float bgW = this.GetController ().GetGuiAssets ().background.width*1.5f;
			float bgH = this.GetController ().GetGuiAssets ().background.height*1.5f;
			int numX = (int)Mathf.Ceil (Screen.width/bgW);
			int numY = (int)Mathf.Ceil (Screen.height/bgH);
			for(int x=0;x<numX;x++){
				for(int y=0;y<numY;y++){
					GUI.DrawTexture (new Rect(x*bgW,y*bgH,bgW,bgH),this.GetController().GetGuiAssets().background,ScaleMode.ScaleToFit);
				}
			}
			GUI.DrawTexture (new Rect(0,Screen.height*0.24f,Screen.width*0.3f,Screen.height*0.5f),this.GetController().GetGuiAssets().mainMenuPlayer,ScaleMode.ScaleToFit);
			GUI.DrawTexture (new Rect(Screen.width*0.1f,Screen.height*0.32f,Screen.width*0.3f,Screen.height*0.5f),this.GetController().GetGuiAssets().mainMenuCrystal,ScaleMode.ScaleToFit);
			GUI.DrawTexture (new Rect(Screen.width*0.7f,Screen.height*0.22f,Screen.width*0.3f,Screen.height*0.5f),this.GetController().GetGuiAssets().mainMenuEnemy,ScaleMode.ScaleToFit);


			GUI.DrawTexture (new Rect(0,-Screen.height*0.05f,Screen.width,Screen.height*0.5f),this.GetController().GetGuiAssets().title,ScaleMode.ScaleToFit);

			GUI.DrawTexture (new Rect(0,Screen.height*0.64f,Screen.width,Screen.height*0.07f),this.GetController().GetGuiAssets().bestScoreLabel,ScaleMode.ScaleToFit);

			bitmapText.Draw (new Rect(Screen.width*0.25f,Screen.height*0.72f,Screen.width*0.5f,Screen.height*0.07f));

			float playButtonSize = Screen.height * 0.18f;
			if(playButtonSize>200)playButtonSize=200;
			if(playButtonSize<70)playButtonSize=70;
			if(AWGUI.AWGUI.ButtonTexture(new Rect((Screen.width-playButtonSize)/2,(Screen.height-playButtonSize)/2-Screen.height*0.01f,playButtonSize,playButtonSize),this.GetController().GetGuiAssets().playButton,ScaleMode.ScaleToFit)){
				GameObject.Instantiate(this.GetController ().GetGuiAssets ().splashScreen);
				this.GetController ().GetAd ().HideBanner ();
				this.GetManager().ChangeScene("board");
				this.GetManager().ChangeState(new BeginGameState());

			}
			float smallButtonSize = Screen.height * 0.15f;;
			if(smallButtonSize>120)smallButtonSize=120;
			if(smallButtonSize<45)smallButtonSize=45;
			float smallButtonMarginWidth = 10;
			float smallButtonMarginHeight = Screen.height / 7;;
			if(this.GetController().GetData().isMusic){
				if(AWGUI.AWGUI.ButtonTexture(new Rect(smallButtonMarginWidth,Screen.height-smallButtonMarginHeight-smallButtonSize,smallButtonSize,smallButtonSize),this.GetController().GetGuiAssets().musicOnButton,ScaleMode.ScaleToFit)){
					this.GetController().GetData().isMusic=false;
					PlayerPrefs.SetInt ("MusicStatus",1);//turn off 1
					PlayerPrefs.Save();
				}
			}else{
				if(AWGUI.AWGUI.ButtonTexture(new Rect(smallButtonMarginWidth,Screen.height-smallButtonMarginHeight-smallButtonSize,smallButtonSize,smallButtonSize),this.GetController().GetGuiAssets().musicOffButton,ScaleMode.ScaleToFit)){
					this.GetController().GetData().isMusic=true;
					PlayerPrefs.SetInt ("MusicStatus",0);//turn on 0
					PlayerPrefs.Save();
				}
			}
			if(this.GetController().GetData().isSound){
				if(AWGUI.AWGUI.ButtonTexture(new Rect(Screen.width-smallButtonMarginWidth-smallButtonSize,Screen.height-smallButtonMarginHeight-smallButtonSize,smallButtonSize,smallButtonSize),this.GetController().GetGuiAssets().soundOnButton,ScaleMode.ScaleToFit)){
					this.GetController().GetData().isSound=false;
					PlayerPrefs.SetInt ("SoundStatus",1);//turn off 1
					PlayerPrefs.Save();
				}
			}else{
				if(AWGUI.AWGUI.ButtonTexture(new Rect(Screen.width-smallButtonMarginWidth-smallButtonSize,Screen.height-smallButtonMarginHeight-smallButtonSize,smallButtonSize,smallButtonSize),this.GetController().GetGuiAssets().soundOffButton,ScaleMode.ScaleToFit)){
					this.GetController().GetData().isSound=true;
					PlayerPrefs.SetInt ("SoundStatus",0);//turn on 0
					PlayerPrefs.Save();
				}
			}
		}
	}
}
