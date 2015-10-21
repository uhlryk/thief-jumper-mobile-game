using UnityEngine;
using System.Collections;
using Assets.Code.Controllers.States;
namespace Assets.Code.Controllers{
	public class StateControllerManager : MonoBehaviour {
		private IState activeState;
		private IState saveState;
		private static StateControllerManager instance;

		/**
		 *  obsługa customowa stanów
		 */ 
		public static AbstractStateController stateController;

		void Awake(){
			if(instance==null){
				instance=this;
				DontDestroyOnLoad(this.gameObject);
			}else{
				DestroyImmediate(this.gameObject);
			}
		}

		void Start () {
			stateController = new StateController();
			stateController.Start (this);
		}
		/**
		 * odwołanie do konkretnego kontrolera. Kontroler zarządza grą, a manager jest raczej uniwersalną biblioteką obsługującą uniwersalne zachowania. kontroler obsługuje specyficzne dla gry
		 * Tak by nie mieszac flow gry z mechanizmami do obsługi
		 * controller będzie posiadał referencje do danyc zapisanych w grze.
		 */ 
		public AbstractStateController GetController(){
			return stateController;
		}
		void Update () {
			if(activeState!=null){
				activeState.Update();
			}
		}
		void OnGUI(){
			if(activeState!=null){
				activeState.OnGUI();
			}
		}
		/**
		 *  Zmienie stan gry na podany w argumencie
		 */ 
		public void ChangeState(IState newState){
			activeState = newState;
			activeState.Start(this);
		}
		/**
		 * służy do zmiany sceny. Lepiej w ten sposób wywolywać ponieważ możliwe że kontroler będzie chciał wykonać jakąś akcję dla zmiany sceny również
		 */ 
		public void ChangeScene(string scene){
			Application.LoadLevel (scene);

		//	Application.LoadLevelAsync (scene);
		}
	}
}
