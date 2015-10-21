using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Assets.Code.Board{
	public class BoardBuilder : MonoBehaviour {
		public Transform floorBlock;
		public Transform leftWallBlock;
		public Transform rightWallBlock;
		public Transform bgBlock;

		public Transform door;
		public Transform gate;
		public Transform window;

		public Transform topCollider;

		public float leftWallOffsetX;
		public float leftWallOffsetY;
		public float rightWallOffsetX;
		public float rightWallOffsetY;

		public float floorOffsetX;
		public float floorOffsetY;

		public float bgOffsetX;
		public float bgOffsetY;

		private int columns;
		public float blockWidth;
		public float blockHeight;

		private List<Transform> leftWallList;
		private List<Transform> rightWallList;
		private List<List<Transform>> bgRowList;
		private List<List<Transform>> decorationRowList;
		private List<Transform> floorRow;

		/**
		 * 
		 * określa wysokość rysowaną w górę
		 */ 
		public int wallRows=20;
		/**
		 * dodatkowo dodane wiersze np przez metoę AddRow
		 */ 
		public int addictionalRows = 0;
		/**
		 * sprawdzamy czy kolumny są parzyste. Jeśli nie są to środkowe pole jest na środku układu współrzędnynch a połowa pozostałych po lewej a druga połowa po prawej
		 * jeśli są parzyste to połowa po lewej połowa po prawej
		 */ 
		private bool isEvenColumn;

		public void Build(int columns,float blockWidth,float blockHeight){
			if(floorBlock==null){
				throw new MissingReferenceException("BoardBuilder potrzebuje wzorca floorBlock");
			}
			if(leftWallBlock==null){
				throw new MissingReferenceException("BoardBuilder potrzebuje wzorca leftWallBlock");
			}
			if(rightWallBlock==null){
				throw new MissingReferenceException("BoardBuilder potrzebuje wzorca rightWallBlock");
			}
			if(bgBlock==null){
				throw new MissingReferenceException("BoardBuilder potrzebuje wzorca bgBlock");
			}
			leftWallList = new List<Transform> ();
			rightWallList = new List<Transform> ();
			bgRowList = new List<List<Transform>> ();
			floorRow = new List<Transform> ();

			decorationRowList=new List<List<Transform>> ();

			this.columns = columns;
			this.blockWidth=blockWidth;
			this.blockHeight = blockHeight;
			if (this.columns % 2==0) {
				this.isEvenColumn =true;
			}else{
				this.isEvenColumn =false;
			}
			this.BuildFloor ();
			this.BuildLeftWall ();
			this.BuildRightWall ();
			this.BuildBg ();


			this.BuildTopCollider ();

		}
		/**
		 * odpowiada za rysowanie dekoracji podłogi
		 */ 
		private void BuildDecorationFloor(){
			List<Transform> decorationRow=new List<Transform>();
			int type = (int)Random.Range (0,4);
			switch (type) {
			case 0:
				decorationRow.Add((Transform)Instantiate(door.transform,new Vector3(blockWidth*(1)+bgOffsetX,bgOffsetY-blockHeight/2,0),door.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(-1)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				break;
			case 1:
				decorationRow.Add((Transform)Instantiate(door.transform,new Vector3(blockWidth*(-1)+bgOffsetX,bgOffsetY-blockHeight/2,0),door.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(1)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(2)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				break;
			case 2:
				decorationRow.Add((Transform)Instantiate(door.transform,new Vector3(blockWidth*(2)+bgOffsetX,bgOffsetY-blockHeight/2,0),door.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(-2)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(-1)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(0)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(1)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				break;
			case 3:
				decorationRow.Add((Transform)Instantiate(door.transform,new Vector3(blockWidth*(-2)+bgOffsetX,bgOffsetY-blockHeight/2,0),door.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(2)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(-1)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(0)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(1)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				break;
			}
		//	Debug.Log ("DECORATION ROW LIST COUNT "+decorationRowList.Count);
			decorationRowList.Add (decorationRow);
		//	Debug.Log ("DECORATION ROW LIST COUNT "+decorationRowList.Count);
		}
		/**
		 * odpowiada za rysowanie dekoracji tła inneg niż przy podłodze
		 */ 
		private void BuildDecorationWall(int posY){
			List<Transform> decorationRow=new List<Transform>();
			int type = (int)Random.Range (0,6);
			switch (type) {
			case 0:
				decorationRow.Add((Transform)Instantiate(gate.transform,new Vector3(blockWidth*(1)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),gate.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(-1)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),window.transform.rotation));
				break;
			case 1:
				decorationRow.Add((Transform)Instantiate(gate.transform,new Vector3(blockWidth*(-1)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),gate.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(1)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(2)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),window.transform.rotation));
				break;
			case 2:
				decorationRow.Add((Transform)Instantiate(gate.transform,new Vector3(blockWidth*(2)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),gate.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(-2)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),window.transform.rotation));
		//		decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(-1)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(0)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(1)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),window.transform.rotation));
				break;
			case 3:
				decorationRow.Add((Transform)Instantiate(gate.transform,new Vector3(blockWidth*(-2)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),gate.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(2)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),window.transform.rotation));
			//	decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(-1)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
			//	decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(0)+bgOffsetX,bgOffsetY-blockHeight/4,0),window.transform.rotation));
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(1)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),window.transform.rotation));
				break;
			case 4:
				decorationRow.Add((Transform)Instantiate(window.transform,new Vector3(blockWidth*(0)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),window.transform.rotation));
				break;
			case 5:
				decorationRow.Add((Transform)Instantiate(gate.transform,new Vector3(blockWidth*(-1)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),gate.transform.rotation));
				decorationRow.Add((Transform)Instantiate(gate.transform,new Vector3(blockWidth*(1)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),gate.transform.rotation));
				decorationRow.Add((Transform)Instantiate(gate.transform,new Vector3(blockWidth*(2)+bgOffsetX,bgOffsetY-blockHeight/4+posY*blockHeight,0),gate.transform.rotation));
				break;
			}
			decorationRowList.Add (decorationRow);
		}
		/**
		 * konfiguruje obiekt topCollider by miał szerokość planszy i zawsze był na górze. Blokuje to gracza przed opuszczeniem planszy
		 */ 
		private void BuildTopCollider(){
			BoxCollider2D boxCollider=(BoxCollider2D)topCollider.GetComponent<Collider2D>();
			boxCollider.size = new Vector2(columns * this.blockWidth,1);
			topCollider.transform.position = new Vector3 (0,wallRows*this.blockHeight,0);
		}
		private void BuildFloor(){
		//	Debug.Log ("Build floor "+columns);
			int halfColumn = (int)Mathf.Floor (columns/2);
			for(int i=-halfColumn;i<=halfColumn;i++){
				Transform floor;
				if(this.isEvenColumn==true){
					if(i==0)continue;

					if(i<0){
						floor = (Transform)Instantiate(floorBlock.transform,new Vector3(blockWidth*(i+0.5f)+floorOffsetX,floorOffsetY,0),floorBlock.transform.rotation);
					}else{
						floor = (Transform)Instantiate(floorBlock.transform,new Vector3(blockWidth*(i-0.5f)+floorOffsetX,floorOffsetY,0),floorBlock.transform.rotation);
					}

				}else{
					floor = (Transform)Instantiate(floorBlock.transform,new Vector3(blockWidth*i+floorOffsetX,floorOffsetY,0),floorBlock.transform.rotation);
				}
				floor.name="Floor";
				floorRow.Add(floor);
			}
		}
		/**
		 * usuwa podłogę, gdy jesteśmy odpowiednio wysoko to jej i tak nie widać
		 */ 
		public void RemoveFloor(){
			if(floorRow!=null){
				for(int i=0;i<floorRow.Count;i++){
					Transform floor=floorRow[i];
					GameObject.Destroy(floor.gameObject);
				}
				floorRow=null;
				for(int i=0;i<2;i++){
					this.RemoveLeftWallRow ();
					this.RemoveRightWallRow ();
				}
			}

		}
		/**
		 * usuwamy określoną ilość wierszy - ściany i tło, od dołu, usuwa kolejne wiersze
		 */ 
		public void RemoveRows(int num){
			for(int i=0;i<num;i++){
				this.RemoveRow();
			}		
		}
		/**
		 * usuwa najniższy wiersz. zastosowanie do oszczędzania zasobów
		 */ 
		public void RemoveRow(){
			this.RemoveLeftWallRow ();
			this.RemoveRightWallRow ();
			this.RemoveBgRow ();
		}
		private void RemoveLeftWallRow(){
			for(int i=0;i<leftWallList.Count;i++){
				Transform wall=leftWallList[i];
				if(wall!=null){
					GameObject.Destroy(wall.gameObject);
					leftWallList[i]=null;
					break;
				}
			}
		}
		private void RemoveRightWallRow(){
			for(int i=0;i<rightWallList.Count;i++){
				Transform wall=rightWallList[i];
				if(wall!=null){
					GameObject.Destroy(wall.gameObject);
					rightWallList[i]=null;
					break;
				}
			}
		}
		private void RemoveDecorationRow(){
			for(int i=0;i<decorationRowList.Count;i++){
				List<Transform> decorationRow=decorationRowList[i];
				if(decorationRow!=null){
					Debug.Log("remove index in "+decorationRow.Count);
					for(int j=0;j<decorationRow.Count;j++){
						Transform decoration=decorationRow[j];
						GameObject.Destroy(decoration.gameObject);
					}
					decorationRowList[i]=null;
					break;
				}else{
					Debug.Log("remove index off");
				}
			}
		}
		private void RemoveBgRow(){
			for(int i=0;i<bgRowList.Count;i++){
				List<Transform> bgList=bgRowList[i];
				if(bgList!=null){
				//	Debug.Log("index "+i);
					if((i%3==1&&i>3)||i==0){
					//	Debug.Log("index in");
						this.RemoveDecorationRow();
					}
					for(int j=0;j<bgList.Count;j++){
						Transform bg=bgList[j];
						GameObject.Destroy(bg.gameObject);
					}
					bgRowList[i]=null;
					break;
				}
			}
		}
		/**
		 * dodaje określoną liczbę kolejnych wierszy
		 */ 
		public void AddRows(int num){
			for(int i=0;i<num;i++){
				this.AddRow();
			}
			topCollider.transform.position = new Vector3 (0,(wallRows+addictionalRows)*this.blockHeight,0);
		}
		/**
		 *  na samej górze dodajemy kolejny wiersz
		 */ 
		public void AddRow(){
			int i = wallRows + addictionalRows;
			this.BuildLeftWallRow(i);
			this.BuildRightWallRow(i);
			this.BuildBgRow(i);
			if(i%3==1){
				this.BuildDecorationWall(i);
			}
			addictionalRows++;
		//	wallRows++;

		}
		private void BuildLeftWallRow(int posY){
			int halfColumn = (int)Mathf.Floor (columns/2)+1;
			Transform leftWall;
			if(this.isEvenColumn==true){
				leftWall = (Transform)Instantiate(leftWallBlock.transform,new Vector3(blockWidth*(-halfColumn+0.5f)+leftWallOffsetX,leftWallOffsetY+posY*blockHeight,0),leftWallBlock.transform.rotation);
			}else{
				leftWall = (Transform)Instantiate(leftWallBlock.transform,new Vector3(-blockWidth*halfColumn+leftWallOffsetX,leftWallOffsetY+posY*blockHeight,0),leftWallBlock.transform.rotation);
			}
			leftWall.name="LeftWall";
			leftWall.gameObject.GetComponent<Renderer>().sortingOrder = 10;
			leftWallList.Add (leftWall);

		}
		private void BuildLeftWall(){
			for(int i=-2;i<wallRows;i++){
				this.BuildLeftWallRow(i);
			}
		}
		private void BuildRightWallRow(int posY){
			int halfColumn = (int)Mathf.Floor (columns/2)+1;
			Transform rightWall;
			if(this.isEvenColumn==true){
				rightWall = (Transform)Instantiate(rightWallBlock.transform,new Vector3(blockWidth*(halfColumn-0.5f)+rightWallOffsetX,rightWallOffsetY+posY*blockHeight,0),rightWallBlock.transform.rotation);
			}else{
				rightWall = (Transform)Instantiate(rightWallBlock.transform,new Vector3(blockWidth*halfColumn+rightWallOffsetX,rightWallOffsetY+posY*blockHeight,0),rightWallBlock.transform.rotation);
			}
			rightWall.name="RightWall";
			rightWall.gameObject.GetComponent<Renderer>().sortingOrder = 10;
			rightWallList.Add (rightWall);
		}
		private void BuildRightWall(){
			for(int i=-2;i<wallRows;i++){
				this.BuildRightWallRow(i);
			}
		}
		private void BuildBgRow(int posY){
			int halfColumn = (int)Mathf.Floor (columns/2);
			List<Transform> rowList=new List<Transform>();
			for(int i=-halfColumn;i<=halfColumn;i++){
				Transform bg;
				if(this.isEvenColumn==true){
					if(i==0)continue;
					if(i<0){
						bg = (Transform)Instantiate(bgBlock.transform,new Vector3(blockWidth*(i+0.5f)+bgOffsetX,bgOffsetY+posY*blockHeight,0),bgBlock.transform.rotation);
					}else{
						bg = (Transform)Instantiate(bgBlock.transform,new Vector3(blockWidth*(i-0.5f)+bgOffsetX,bgOffsetY+posY*blockHeight,0),bgBlock.transform.rotation);
					}
					
				}else{
					bg = (Transform)Instantiate(bgBlock.transform,new Vector3(blockWidth*i+bgOffsetX,bgOffsetY+posY*blockHeight,0),bgBlock.transform.rotation);
				}
				bg.name="Bg";
				rowList.Add(bg);
			}
			bgRowList.Add (rowList);
		}
		private void BuildBg(){
			for(int j=0;j<wallRows;j++){
				this.BuildBgRow(j);	
				if(j%3==1&&j>3){
					this.BuildDecorationWall(j);
				}
				if(j==0){
					this.BuildDecorationFloor ();
				}
			}

		}
	}
}
