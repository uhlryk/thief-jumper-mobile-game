using UnityEngine;
using System.Collections;

namespace Assets.Code{
	public class Spinner : MonoBehaviour {
		void Start () {
			GameObject.DontDestroyOnLoad(this.gameObject);
			StartCoroutine(StartActivityIndicator());
		}
		IEnumerator StartActivityIndicator () {
			#if UNITY_IPHONE
			Handheld.SetActivityIndicatorStyle(UnityEngine.iOS.ActivityIndicatorStyle.WhiteLarge);
			#endif
			#if UNITY_ANDROID
			Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.InversedLarge);
			#endif
			#if UNITY_IPHONE || UNITY_ANDROID
			Handheld.StartActivityIndicator();
			#endif
			yield return null;
		}
		
		void OnLevelWasLoaded() {
			#if UNITY_IPHONE || UNITY_ANDROID
			Handheld.StopActivityIndicator();
			#endif
		//	Debug.Log ("Usunięcie splash screena");
			GameObject.Destroy(gameObject);
		}
	}
}
