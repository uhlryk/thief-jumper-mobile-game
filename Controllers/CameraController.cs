using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public float offsetY;
	public bool isEnable;
	void Start () {
		isEnable = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null&&isEnable) {
			Vector2 position=target.position;
			transform.position=new Vector3(position.x,position.y+offsetY,transform.position.z);
		}
	}
}
