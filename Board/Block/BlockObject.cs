using UnityEngine;
using System.Collections;
using Assets.Code.Board;
namespace Assets.Code.Board.Block{
	/**
	 * jest to wrapper na gameobject, odpowiada za informacje w nim
	 */ 
	public class BlockObject{
		public Transform block;

		public int posX;
		public int posY;
		public float width;
		public float height;

		public enum Status{
			Active,Rest
		}
		public Status status;
		public Transform tresure;
		public Transform spikes;
		public Transform enemy;
		public BlockObject(Transform block,float width,float height){
			this.block =block;
			this.status = Status.Active;
			this.width = width;
			this.height = height;



		}
		public void SetPosition(int posX,int posY){
			this.posX = posX;
			this.posY=posY;
		/*	SpriteRenderer spriteRenderer = (SpriteRenderer) block.renderer;
			int mod = posY/40;
			switch(mod%7){
			case 0:
				spriteRenderer.color = new Color(1,1,1);
				break;
			case 1:
				spriteRenderer.color = new Color(0.8f,0.6f,0.8f);
				break;		
			case 2:
				spriteRenderer.color = new Color(0.8f,0.8f,1);
				break;	
			case 3:
				spriteRenderer.color = new Color(0.7f,1,0.6f);
				break;
			case 4:
				spriteRenderer.color = new Color(1,1,0.6f);
				break;
			case 5:
				spriteRenderer.color = new Color(1,0.6f,1);
				break;
			case 6:
				spriteRenderer.color = new Color(1,0.5f,0.5f);
				break;
			}*/

		}
		public void SetOrder(int order){
			this.block.GetComponent<Renderer>().sortingOrder = order;
			if (tresure != null) {
				this.tresure.GetComponent<Renderer>().sortingOrder = order;
			}
			if(spikes!=null){
				this.spikes.GetComponent<Renderer>().sortingOrder = order+1;
			}
			if(enemy!=null){
				this.enemy.GetComponent<Renderer>().sortingOrder = order+1;
			}
		}
		public void Clear(){
			if(spikes!=null){
				GameObject.Destroy(spikes.gameObject);
			}
			if (block != null) {
				GameObject.Destroy(block.gameObject);
			}
		}
		/**
		 * gdy chcemy gracza teleportowac na to najpierw usuwamy z niego skarb. W przeciwnym razie po respawnie gracz nie wie dlaczego nagle ma więcej punktów (respawn na skarb odrazu dodawał punkty)
		 */ 
		public void DestroyAddiction(){
			if (tresure != null) {
				GameObject.Destroy(tresure.gameObject);
			}
			if (spikes != null) {
				GameObject.Destroy(spikes.gameObject);
			}
		}
		/**
		 * grupa zarządzająca blokami powinna odpalić tą metodę w celu poinformowania bloku że spoczął. Wtedy mogą być wywołane dodatkowe akcje z poziomu bloku
		 */ 
		public void Rest(){
			if(enemy){
				LandEnemyController controller=enemy.GetComponent<LandEnemyController>();
				if(controller!=null){
					controller.enabled=true;
				}
			}
		}
	}
}