using UnityEngine;
using System.Collections;

namespace Assets.Code.Controllers.States{
	public interface IState{
		void Start(StateControllerManager manager);
		void Update();
		void OnGUI();
	}
}
