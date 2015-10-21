using UnityEngine;
using System.Collections;
using Assets.Code.Controllers.States;
namespace Assets.Code.Controllers{
	public abstract class AbstractStateController{
		private StateControllerManager manager;
		public void Start (StateControllerManager manager){
			this.manager = manager;
			this.StartController ();
		}
		public StateControllerManager GetManager(){
			return this.manager;
		}
		public abstract void StartController();

		public void ChangeState(IState newState){
			manager.ChangeState(newState);
		}
		public abstract GameData GetData();
		public abstract AdMob GetAd();
		public abstract GuiStatesAssets GetGuiAssets();
	}
}
