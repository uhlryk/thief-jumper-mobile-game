using UnityEngine;
using System.Collections;

namespace Assets.Code.Controllers{
	public class StateController:AbstractStateController {
		private GameData gameData;
		private GuiStatesAssets guiAssets;
		private AdMob adMob;
		public override void StartController(){
			Application.targetFrameRate = 30;
			this.gameData=this.GetManager ().GetComponent<GameData>();
			if (this.gameData == null) {
				throw new UnassignedReferenceException("Obiekt "+this.GetManager().gameObject.name+" nie ma komponentu GameData");
			}
			this.guiAssets = this.GetManager ().GetComponent<GuiStatesAssets> ();
			if (this.guiAssets == null) {
				throw new UnassignedReferenceException("Obiekt "+this.GetManager().gameObject.name+" nie ma komponentu GuiStatesAssets");
			}
			/**
			 * zliczamy włączenia gry
			 */ 
			this.GetData ().countGames = PlayerPrefs.GetInt ("CountGames"); 
			this.GetData ().countGames++;
			PlayerPrefs.SetInt ("CountGames",this.GetData ().countGames);
			PlayerPrefs.Save();

			adMob = new AdMob ();
			adMob.StartBanner ();

			this.ChangeState(new States.MainMenuState());
			GameCenter.Authenticate();
		}
		public override AdMob GetAd(){
			return adMob;
		}
		public override GameData GetData(){
			return gameData;
		}
		public override GuiStatesAssets GetGuiAssets (){
			return guiAssets;
		}
	}
}
