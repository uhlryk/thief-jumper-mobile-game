using UnityEngine;
using System.Collections;


namespace Assets.Code.Controllers.States{
	public abstract class AbstractState : IState {
		private StateControllerManager manager;
		/**
		 * zwraca stateControllerManager który obsługuje stany, wywołanie służy przedewszystkim do wywołania
		 * GetManager().ChangeState(<konkretny stan>)
		 * lub
		 * GetManager().ChangeScene(<scena>)
		 */ 
		public StateControllerManager GetManager(){
			return this.manager;
		}
		/**
		 * służy do wywołania kontrolera obsługującego konkretne akcje. Tak by nie mieszac flow gry z mechanizmami do obsługi flo
		 * controller będzie posiadał referencje do danyc zapisanych w grze.
		 */ 
		public AbstractStateController GetController(){
			return this.manager.GetController ();
		}
		public void Start(StateControllerManager manager){
			this.manager = manager;
			this.StartState();
		}
		public void Update () {
			this.UpdateState ();
		}
		public void OnGUI(){
			GUI.skin = this.GetController ().GetGuiAssets ().skin;
			this.OnGUIState ();
		}
		public abstract void StartState();
		public abstract void UpdateState ();
		public abstract void OnGUIState ();


	}
}
