using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Assets.Code.Board{
	public class SpriteText:MonoBehaviour {
		public GameObject prefarb;

		public Sprite[] bitmapList;
		public string keys;
		public Rect position;
		public string text;
		public Orientation orientation=Orientation.horizontal;


		public enum Orientation{
			horizontal,vertical
		}

		private Dictionary<char,Sprite> bitmapMap;
		private List<GameObject> spriteGameObjectList;

	//	private float oldWidth;
	//	private float oldHeight;
//		private Size lastSize;


		void Start(){
		//	Debug.Log("SpriteText.Start ");
			this.PrepareTexture ();
		//	Debug.Log("SpriteText.Start "+bitmapMap.Count);
			if (text.Length>0) {
				this.Generate(text);
			}
		}

		/**
		 * metoda przygotowuje Dictionary<char,Texture> bitmapMap na podstawie listy bitmap i stringa zawierającego klucze
		 * jeśli zmieniamy dynamicznie Sprite to musimy to najpierw wywołać
		 */ 
		public void PrepareTexture(){
			if(bitmapList!=null&&bitmapList.Length>0&&keys.Length>0){
				this.bitmapMap=new Dictionary<char,Sprite>();
				for(int i=0;i<bitmapList.Length;i++){
					Sprite bitmap=bitmapList[i];
					char c=keys[i];
					if(bitmap!=null&&c!=null){
						this.bitmapMap.Add(c,bitmap);
					}
				}
			}
		}
		private float alpha;
		public void SetAlpha(float alhpa){
			this.alpha = alpha;
		}
		/**
		 *  rysowanie tekstu, gdzie zmieniamy pozycję i rozmiar
		 * sama pozycja jest cachowana, rozmiar nie.
		 * Rozmiar określa prostokąt w któ®ym ma się zmieścić cały tekst, tekst będzie się skalował tak by się zmieścić maksymalnie w danym prostokącie
		 */ 
		public void Generate(string text){
			this.text = text;
			this.RemoveSpriteGameObject ();
			spriteGameObjectList=new List<GameObject>();
			if(bitmapMap!=null){
				float posX=this.transform.position.x;
				for(int i=0;i<text.Length;i++){
					char key=text[i];
				//	Debug.Log("SpriteText.Generate "+key);
					if(key==null)continue;
					if(bitmapMap.ContainsKey(key)==false)continue;
					Sprite sprite=bitmapMap[key];
					if(sprite==null)continue;
					GameObject charBitmapGameObject=(GameObject)GameObject.Instantiate(prefarb);
					SpriteRenderer spriteRenderer =(SpriteRenderer)charBitmapGameObject.GetComponent<Renderer>();
					spriteRenderer.sprite=sprite;
					posX+=spriteRenderer.bounds.size.x/2;
					charBitmapGameObject.transform.position=new Vector3(posX,this.transform.position.y,this.transform.position.z);
					posX+=spriteRenderer.bounds.size.x/2;
					charBitmapGameObject.transform.parent=this.gameObject.transform;
					spriteGameObjectList.Add(charBitmapGameObject);
				}
				//Debug.Log ("SpriteText.Generate "+text);
			}
		}
		/**
		 * przed zmianą tekstu usuwa poprzednie sprity z tekstem
		 * 
		 */ 
		private void RemoveSpriteGameObject(){
			if(spriteGameObjectList!=null){
				for(int i=0;i<spriteGameObjectList.Count;i++){
					Destroy(spriteGameObjectList[i]);
				}
			}
		}
	/*	public class Size{
			public float width;
			public float height;
			public Size(float w,float h){
				width=w;
				height=h;
			}
		}*/

	}
}
