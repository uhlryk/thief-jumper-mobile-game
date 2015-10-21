using UnityEngine;
using System.Collections;

namespace Assets.Code.Board{
	public class HeightLabelController : MonoBehaviour {

		public GameObject wallText1;
		private SpriteText spriteText1;
		public GameObject wallText2;
		private SpriteText spriteText2;
		public GameObject wallText3;
		private SpriteText spriteText3;

		public GameObject wallTextScore;
		private SpriteText spriteTextScore;


		private BlockController blockController;
		private GameObject gameManager;
		private GameData gameData;


		private int nextPosition=1;

		private int nextScorePosition=1;
		/**
		 * co ile bloków następuje wyświetlanie na ścianie wysokości
		 */ 
		private int posStep=4;
		void Start () {
			spriteText1=wallText1.GetComponent<SpriteText>();
			blockController=GetComponent<BlockController>();
			spriteText2=wallText2.GetComponent<SpriteText>();
			spriteText3=wallText3.GetComponent<SpriteText>();

			spriteTextScore=wallTextScore.GetComponent<SpriteText>();

			gameManager = GameObject.Find ("GameManager");
			gameData=gameManager.GetComponent<GameData>();

			this.SetHeightLabel (10,gameData.blockHeight*posStep*2,Type.First);
			this.SetHeightLabel (20,gameData.blockHeight*posStep*4,Type.Second);
			this.SetHeightLabel (30,gameData.blockHeight*posStep*6,Type.Third);
			this.SetLifeScoreExchange (gameData.exchangePoinsts,gameData.blockHeight*posStep);

		}
		public void SetLifeScoreExchange(int points,float height){
			spriteTextScore.Generate (points+"P=1L");
			wallTextScore.transform.position=new Vector3(-3f,height,0);
		}
		public void SetHeightLabel(int meter,float height,Type type){
//			Debug.Log ("height"+height);
			if(type==Type.First){
				spriteText1.Generate (meter+"m");
				wallText1.transform.position=new Vector3(-1f,height,0);
			}else if(type==Type.Second){
				spriteText2.Generate (meter+"m");
				wallText2.transform.position=new Vector3(-1f,height,0);
			}else if(type==Type.Third){
				spriteText3.Generate (meter+"m");
				wallText3.transform.position=new Vector3(-1f,height,0);
			}
		}
		public enum Type{First,Second,Third}

		void Update(){
			if (blockController.isInit ==true) {
				float r=(float)1/posStep/2*10;
			//	Debug.Log("R "+r);
				gameData.actMaxHeight = (int)(blockController.blockGroup.GetRestMaxVal ()*r);
			}else{
				gameData.actMaxHeight=0;
			}
			int actPos=(int)(blockController.newLinesRows /posStep);

			if (nextPosition==(int)(actPos/2)) {

				nextPosition++;
				Type type=Type.First;
				switch(nextPosition%3){
					case 2:
					type=Type.First;
						break;
					case 0:
					type=Type.Second;
						break;
					case 1:
					type=Type.Third;
						break;
				}
				this.SetHeightLabel((nextPosition+2)*10,(nextPosition+2)*posStep*2*gameData.blockHeight,type);
			}
			if (nextScorePosition==(int)(actPos/4)) {
				nextScorePosition++;
				this.SetLifeScoreExchange(gameData.lastPointsLife+gameData.exchangePoinsts,(nextScorePosition*posStep*4+posStep)*gameData.blockHeight);
			}
		}
	}
}