using UnityEngine;
using System.Collections;
using Assets.Code.Board.Block;
using System.Collections.Generic;
namespace Assets.Code.Board{
	/**
	 * odpowiada za kordynację wszystkich klas związanych z blokami
	 */ 
	public class BlockController : MonoBehaviour {
		public Transform blockSpawn;
		public Transform tresureSpawn;
		public Transform spikesSpawn;

		public Transform crabSpawn;
		/**
		 * co ile sekund bloki będą się pojawiać
		 */ 
		public float interval = 0.1f; //for 5 second

		public float offsetX;
		public float offsetY;

		private float timer=0;

		public bool isTurnOn=false;

		public bool isInit;
		private int columns;
		private float blockWidth;
		private float blockHeight;
		private bool isEvenColumn;
		[HideInInspector]
		public BlockGroup blockGroup;

		private BoardBuilder boardBuilder;

		/**
		 * w określonym czasie dodajemy do planszy nowe ściany i tło
		 * tu zapisujemy ostatnią dodaną wartość by kolejne dodanie bylo spowodowane zmianą a nie stara wartością
		 */ 
		public int newBoardRows;
		/**
		 * przechowujemy ostatnią wartość zanotowaną. po spadku bloku porównujemy to z zmianą, możemy wtedy wykonać określone akcje
		 */ 
		public int newLinesRows;

		/**
		 * im więcej wierszy tym wieksza wartość (co 200 wzrasta o 1)
		 * i o tyle szybciej spadają bloki
		 */ 
		public float difficulty;
		/**
		 * po ilu pełnych liniach usuwamy linie poniżej
		 * czyli jak wejdziemy na 4 pełną linię usuwamy pierwszy blok
		 * tak samo wywołujemy dla podłogi
		 */ 
		public int fogOfWar=1;

		public List<Transform> enemyList;

		void Start () {
			blockGroup = new BlockGroup ();
			boardBuilder=GetComponent<BoardBuilder>();
			if(boardBuilder==null){
				throw new MissingReferenceException("GameObject "+gameObject.name+" nie ma komponentu BoardBuilder");
			}
			newBoardRows = 0;
			isInit = false;
			enemyList = new List<Transform> ();
		}
		public void ClearEnemy(){
			for(int i=0;i<enemyList.Count;i++){
				Transform enemy=enemyList[i];
				if(enemy!=null){
					Destroy(enemy.gameObject);
					enemyList[i]=null;
				}
			}
			enemyList = new List<Transform> ();
		}

		/**
	 * wylicza jaki blok ma spaść następny
	 * sprawdza najpierw czy występuje za duża różnica między zapełnionymi blokami, jeśli tak to próbuje minimalizować róźnicę,
	 * w przeciwnym razie losowo oblicza
	 */ 
		private int RandBlockPosition(){
			int minVal = blockGroup.GetSumMinVal ();
			int maxVal = blockGroup.GetSumMaxVal ();

			if(maxVal-minVal>2){//duża różnica między wartością maksymalną i minimalną, zamiast losować wypełniamy ubytki
				return blockGroup.GetSumMinPos();
			}else{
				int halfColumn = (int)Mathf.Floor (columns/2);
				int res=(int)Mathf.Round(Random.Range(-halfColumn,halfColumn+1));
				if (isEvenColumn==true&&res == 0)res=RandBlockPosition();
				return res;
			}
		}
		private void CreateBlock(int level){
			if(blockSpawn!=null){
				int posNum=RandBlockPosition();
				Transform block;
				if(this.isEvenColumn==true){//parzyste
					if(posNum<0){
						block=(Transform)Instantiate(blockSpawn.transform,new Vector3(blockWidth*(posNum+0.5f),(blockHeight*blockGroup.GetRestMaxVal()+level)*blockHeight+offsetY,0),blockSpawn.transform.rotation);
					}else{
						block=(Transform)Instantiate(blockSpawn.transform,new Vector3(blockWidth*(posNum-0.5f),(blockHeight*blockGroup.GetRestMaxVal()+level)*blockHeight+offsetY,0),blockSpawn.transform.rotation);
					}
				}else{//pazyste
					block=(Transform)Instantiate(blockSpawn.transform,new Vector3(blockWidth*posNum,(blockHeight*blockGroup.GetRestMaxVal()+level)*blockHeight+offsetY,0),blockSpawn.transform.rotation);
				}
				BlockObject blockObject=new BlockObject(block,blockWidth,blockHeight);
				int posRow=blockGroup.GetRestColumnCount(posNum)+blockGroup.GetActiveColumnCount(posNum);
				blockObject.SetPosition(posNum,posRow);

				blockGroup.Add(blockObject);

				int randEvent=(int)Random.Range(0,100);
			//	Debug.Log("randEvent "+randEvent+"  newLinesRows "+newLinesRows);
				if(randEvent>70&&newLinesRows>3){
					if(randEvent>95&&newLinesRows>30){
						this.CreateSpikes(blockObject);
					}else if(randEvent>70){//kolce mogą pojawić się tylko jak mamy więcej niż 10 wierszy i wylosujemy 80-90
						CreateCrab(blockObject);
					}
				}else if(randEvent>=0){
					this.CreateTresure(blockObject);
				}
				blockObject.SetOrder(posRow*2+(posNum+10)*10);
			}
		}
		private void CreateTresure(BlockObject blockObject){
			//losujemy czy dany blok ma skarb
			Transform tresure=(Transform)GameObject.Instantiate(tresureSpawn.transform,new Vector3(blockObject.block.transform.position.x+0.1f,blockObject.block.transform.position.y+1.3f,blockObject.block.transform.position.z),tresureSpawn.transform.rotation);
			tresure.GetComponent<TresureController> ().SetType ((int)Random.Range(0,6));
			tresure.parent = blockObject.block;
			blockObject.tresure = tresure;
		
		}
		private void CreateCrab(BlockObject blockObject){
			//losujemy czy dany blok ma skarb
			Transform crab=(Transform)GameObject.Instantiate(crabSpawn.transform,new Vector3(blockObject.block.transform.position.x,blockObject.block.transform.position.y+0.9f,blockObject.block.transform.position.z),crabSpawn.transform.rotation);
			//crab.GetComponent<LandEnemyController> ().SetType ((int)Random.Range(0,6));
			crab.parent = blockObject.block;
			blockObject.enemy = crab;
			enemyList.Add (crab);
			
		}
		private void CreateSpikes(BlockObject blockObject){
			Transform spikes=(Transform)GameObject.Instantiate(spikesSpawn.transform,new Vector3(blockObject.block.transform.position.x,blockObject.block.transform.position.y+0.57f,blockObject.block.transform.position.z),spikesSpawn.transform.rotation);
			spikes.parent = blockObject.block;
			blockObject.spikes = spikes;
		}
		private void UpdateCreateBlock(){
			if (Time.time >= timer) {
				timer = Time.time + interval;
				CreateBlock(13);
				
			}
		}
		public void Start(int columns,float blockWidth,float blockHeight){
			if(isInit==false){
				isTurnOn = true;
				if(blockSpawn==null){
					throw new MissingReferenceException("BlockController potrzebuje wzorca blockSpawn");
				}
				this.columns = columns;
				this.blockWidth=blockWidth;
				this.blockHeight = blockHeight;
				if (this.columns % 2==0) {
					this.isEvenColumn =true;
				}else{
					this.isEvenColumn =false;
				}
			//	Debug.Log ("czy pazyste  "+this.isEvenColumn );
				this.blockGroup.Init (this.columns);
				isInit=true;
				CreateBlock(10);
			}
		}
		public void TurnOn(){
			isTurnOn = true;
		}
		public void TurnOff(){
			isTurnOn = false;
		}
		private void MoveBlocks(){
			foreach (KeyValuePair<int,List<BlockObject>> pair in blockGroup.GetActiveGroup()) {
				List<BlockObject> list=pair.Value;
				int posX=pair.Key;
				float translateY=-(1.8f+difficulty)*Time.deltaTime;
				for(int i=0;i<list.Count;i++){
					BlockObject blockObject=list[i];
					if(blockObject!=null){
						float checkYPos=blockGroup.GetHeightColumn(posX)+blockObject.height+offsetY;
						blockObject.block.transform.Translate(0,translateY,0);
						if(blockObject.block.position.y<checkYPos){//znaczy że dany blok jest w pozycji spoczynku
							blockGroup.SetRest(posX,i,offsetY);
							int maxRestVal=blockGroup.GetRestMaxVal();
							if(maxRestVal>newBoardRows){//mamy nową wysokość należy wykonać akcje z tym związane
								this.boardBuilder.AddRows(maxRestVal-newBoardRows);
								newBoardRows=maxRestVal;
							}
							int actLines=blockGroup.GetFullRows();
							if(actLines>newLinesRows){//mamy nową linię należy wykonać akcje z tym związane
								if(fogOfWar==actLines){
									this.boardBuilder.RemoveFloor();
								}
								int diff=actLines-newLinesRows;
								if(diff>fogOfWar){
									this.boardBuilder.RemoveRows(diff-fogOfWar);
									this.blockGroup.RemoveRowsRest(actLines-fogOfWar);
									newLinesRows+=diff-fogOfWar;	
									difficulty=newLinesRows/200;
								}

							}
						}
					}
					
				}
			}
		}
		void Update () {
			if (isTurnOn == true) {
				this.UpdateCreateBlock ();
				this.MoveBlocks ();
			}
		}
	}
}
