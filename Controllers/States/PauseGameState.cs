using UnityEngine;
using Assets.Code.Board;
using Assets.Code.Board.Block;
namespace Assets.Code.Controllers.States{
	public class PauseGameState: AbstractState {
		private PlayerController playerController;
		private GameObject boardObject;
		private BlockController blockController;

		public override void StartState(){
			Time.timeScale = 0;
			GameObject playerObject = GameObject.Find ("Player");
			if(playerObject==null){
				throw new MissingReferenceException("Brakuje GameObject Player");
			}
			playerController=playerObject.GetComponent<PlayerController>();
			playerController.enableInput = false;
			playerController.isEnable=false;
			AudioSource music = GameObject.Find ("Main Camera").GetComponent<AudioSource>();
			music.volume = 0.1f;
			boardObject = GameObject.Find ("Board");
			if (boardObject == null) {
				throw new MissingReferenceException("Scena nie posiada Board GameObject");
			}
			blockController=boardObject.GetComponent<BlockController>();
			if (blockController == null) {
				throw new MissingReferenceException("Board GameObject nie posiada komponentu BlockController");
			}
			this.GetController ().GetAd ().ShowBanner ();
		}
		public override void UpdateState () {
#if UNITY_ANDROID
				if (Input.GetKeyUp(KeyCode.Escape)){
					this.GetManager().ChangeScene("menu");
					this.GetManager().ChangeState(new MainMenuState());
				}
#endif
		}
		public override void OnGUIState(){
			float relativeSize = Screen.height;
			if(relativeSize>1300)relativeSize=1300;
			float windowWidth = relativeSize * 0.55f;
			float windowHeight = relativeSize * 0.55f / 2.2f;
			float marginWidth = relativeSize * 0.04f;
			float marginHeight = relativeSize * 0.07f;
			float buttonSize = relativeSize * 0.13f;
			if(buttonSize<50)buttonSize=50;
			GUI.BeginGroup (new Rect((Screen.width-windowWidth)/2,(Screen.height-windowHeight)/2,windowWidth,windowHeight));

			GUI.DrawTexture(new Rect(0,0,windowWidth,windowHeight),this.GetController().GetGuiAssets().pauseWindow,ScaleMode.ScaleToFit);

			if(AWGUI.AWGUI.ButtonTexture(new Rect(marginWidth,(windowHeight-marginHeight)/2,buttonSize,buttonSize),this.GetController().GetGuiAssets().restartButton,ScaleMode.ScaleToFit)){
				this.GetController ().GetAd ().HideBanner ();

				/**
				 * do wywołania popupów
				 * co określoną liczbę gier przy przeładowaniu będzie przenosił do menu i wyświetlał popup
				 */ 
			/*	if(this.GetController().GetData().countActPlays%2==0){
					this.GetController().GetData().showBanner=true;
					this.GetController().GetAd().StartInterstitial();
					this.GetManager().ChangeScene("menu");
					this.GetManager().ChangeState(new MainMenuState());
				}else*/
				{
					playerController.isEnable=true;
					this.GetManager().ChangeScene("board");
					this.GetManager().ChangeState(new BeginGameState());
				}
			}

			if(AWGUI.AWGUI.ButtonTexture(new Rect((windowWidth-buttonSize)/2,(windowHeight-marginHeight)/2,buttonSize,buttonSize),this.GetController().GetGuiAssets().menuButton,ScaleMode.ScaleToFit)){
				this.GetController ().GetAd ().HideBanner ();
				this.GetManager().ChangeScene("menu");
				this.GetManager().ChangeState(new MainMenuState());
			}

			if(AWGUI.AWGUI.ButtonTexture(new Rect(windowWidth-marginWidth-buttonSize,(windowHeight-marginHeight)/2,buttonSize,buttonSize),this.GetController().GetGuiAssets().playOrangeButton,ScaleMode.ScaleToFit)){
				this.GetController ().GetAd ().HideBanner ();
				playerController.isEnable=true;
				this.GetManager().ChangeState(new PlayGameState());
				playerController.enableInput = true;
			//	blockController.TurnOn();

			}
			GUI.EndGroup ();
		}
	}
}

