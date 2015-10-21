using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Code.Board.Block{
	/**
	 * kontroluje spadające bloki. odpowiada za ich spadanie i ma informacje o ich pozycjach, można np wyzerować spadające bloki w danej kolumnie
	 */ 
	public class BlockGroup{

		/**
		 *  <Column,List<Block>>
		 * Zawiera aktywne bloki które spadają
		 */ 
		private Dictionary<int,List<BlockObject>> activeGroup;
		/**
		 * 
		 */ 
		private Dictionary<int,List<BlockObject>> restGroup;


		private Values activeValues;
		private Values restValues;
		private Values allValues;
		/**
		 * przechowuje ilość bloków w każdej kolumnie dla bloków aktywnych
		 */ 
		private Dictionary<int,int> activeColumnCount;
		/**
		 * przechowuje ilość bloków w każdej kolumnie dla bloków rest
		 */
		private Dictionary<int,int> restColumnCount;

		/**
		 * wysokości aktualnych kolumn
		 */ 
		private Dictionary<int,float> hightColumn;

		/**
		 * liczba kolumnu jakie są w użyciu
		 */ 
		private int columns;

		/**
		 *  wyświetla liczbę zapełnionych wierszy
		 * ułatwi to usuwanie pełnych linii np 3 poniżej aktualnego poziomu
		 */ 
		private int fullRows;


	//	public bool isInit=false;

		public void Init(int columns){
			this.columns = columns;
			fullRows = 0;
			activeGroup = new Dictionary<int,List<BlockObject>> ();
			restGroup=new Dictionary<int,List<BlockObject>>();

			activeColumnCount = new Dictionary<int,int> ();
			restColumnCount = new Dictionary<int,int> ();
			hightColumn = new Dictionary<int,float> ();
			activeValues = new Values ();
			restValues = new Values ();
			allValues = new Values ();
			int halfColumn = (int)Mathf.Floor (columns/2);
			for (int i=-halfColumn; i<=halfColumn; i++) {
				List<BlockObject> activeList=new List<BlockObject>();
				activeGroup.Add(i,activeList);
				List<BlockObject> restList=new List<BlockObject>();
				restGroup.Add(i,restList);
				activeColumnCount.Add(i,0);
				restColumnCount.Add(i,0);
				hightColumn.Add(i,0);
			}
		}
		public void Add(BlockObject blockObject){
			int posX = blockObject.posX;
			List<BlockObject> activeList = activeGroup [posX];
			activeList.Add(blockObject);
			activeColumnCount [posX]++;
			this.CalculateMinMax (activeValues,activeColumnCount);
			this.CalculateMinMax (allValues,activeColumnCount,restColumnCount);
			blockObject.block.GetComponent<BlockData>().blockObject=blockObject;
		}
		public Dictionary<int,List<BlockObject>> GetActiveGroup(){
			return activeGroup;
		}
		public Dictionary<int,List<BlockObject>> GetRestGroup(){
			return restGroup;
		}
		/**
		 * 
		 * zwraca ilość bloków w danej kolumnie active
		 */
		public int GetActiveColumnCount(int posX){
			return activeColumnCount [posX];
		}
		/**
		 * 
		 * zwraca ilość bloków w danej kolumnie rest
		 */
		public int GetRestColumnCount(int posX){
			return restColumnCount [posX];
		}
		public float GetHeightColumn(int posX){
			return hightColumn [posX];
		}
		/**
		 *  pierwszy element aktywny w danej kolumnie ustawia w spoczynek
		 */ 
		public void SetRest(int posX,int posY,float offsetY){
			List<BlockObject> activeList = activeGroup [posX];
			BlockObject blockObject = activeList [posY];
			activeColumnCount [posX]--;
			restColumnCount [posX]++;
			hightColumn [posX]+=blockObject.height;
			//blockObject.SetOrder ();
			activeList [posY] = null;
			this.SetColumnOther (posX);
			List<BlockObject> restList = restGroup [posX];
			restList.Add (blockObject);

			blockObject.block.position = new Vector3 (blockObject.block.position.x,hightColumn [posX]+offsetY,blockObject.block.position.z);
			blockObject.Rest();
			this.CalculateMinMax (activeValues,activeColumnCount);
			this.CalculateMinMax (restValues,restColumnCount);
			//tu mogą być operacje dodatkowe na listach, np sprawdzenie czy można usunąć dolne bloki w spoczynku, czy można wyłączyć kolizje dla bloku itp
		}
		/**
		 * dla danej kolumny ustawia bloki które są poniżej nowego bloku, np usuwa z nich floor, czy inne dekoracje jeśli je mają
		 */ 
		private void SetColumnOther(int posX){
			List<BlockObject> restList = restGroup [posX];
			for (int i=0; i<restList.Count; i++) {
				BlockObject blockObject=restList[i];
				if(blockObject!=null){

				}
			}
		}
		/**
		 * metoda niszczy wszystkie spadające bloki
		 * zastosowanie np gdy gracz zostanie ranny
		 */ 
		public void RemoveAllActive(){

			activeColumnCount = new Dictionary<int,int> ();
			activeValues = new Values ();	
			allValues = new Values ();
			int halfColumn = (int)Mathf.Floor (columns/2);
			for (int i=-halfColumn; i<=halfColumn; i++) {
				List<BlockObject> activeList=activeGroup[i];
				for(int j=0;j<activeList.Count;j++){
					BlockObject block=activeList[j];
					if(block!=null){
						block.Clear();
						activeList[j]=null;
					}
				}
				activeGroup.Remove(i);
				activeList=new List<BlockObject>();
				activeGroup.Add(i,activeList);
			//	activeGroup.Add(i,activeList);

				activeColumnCount.Add(i,0);

			}
			//activeGroup = new Dictionary<int,List<BlockObject>> ();
			this.CalculateMinMax (activeValues,activeColumnCount);
			this.CalculateMinMax (allValues,activeColumnCount,restColumnCount);
		}
		/**
		 * usuwa linie do linii row
		 */ 
		public void RemoveRowsRest(int row){
			int halfColumn = (int)Mathf.Floor (columns/2);
			for (int i=-halfColumn; i<=halfColumn; i++) {
				int posX=i;
				List<BlockObject> restList = restGroup [posX];
				for(int j=0;j<row;j++){
					BlockObject block=restList[j];
					if(block!=null){
						block.Clear();
						restList[j]=null;
					}
				}
			}
		}
		/**
		 * zwraca aktywny blok na podstawie jego kolumny i wiersza
		 */ 
		public BlockObject GetActiveBlock(int posX,int posY){
		//	Debug.Log ("BlockGroup.GetActiveBlock("+posX+","+posY+")");
			List<BlockObject> activeList = activeGroup [posX];
			BlockObject blockObject = activeList [posY];
			return blockObject;
		}
		/**
		 * zwraca aktywny blok na podstawie jego kolumny i wiersza
		 */ 
		public BlockObject GetRestBlock(int posX,int posY){
		//	Debug.Log ("BlockGroup.GetRestBlock("+posX+","+posY+")");
			List<BlockObject> restList = restGroup [posX];
			BlockObject blockObject = restList [posY];
			return blockObject;
		}
		/**
		 * zwraca pozycję kolumny która ma najwięcej spadających bloków 
		 */
		public int GetActiveMaxPos(){
			return activeValues.maxPos;
		}
		/**
		 * metoda zwraca ilość bloków w nailiczniejszej spadającej kolumnie
		 */ 
		public int GetActiveMaxVal(){
			return activeValues.maxVal;
		}
		/**
		 * zwraca pozycję najmniej licznej kolumny w blokach spadających
		 */ 
		public int GetActiveMinPos(){
			return activeValues.minPos;
		}
		/**
		 * zwraca ilość blokó∑ w najmniej licznej spadającej kolumnie
		 */ 
		public int GetActiveMinVal(){
			return activeValues.minVal;
		}
		/**
		 * zwraca pozycję kolumny która ma najwięcej rest bloków 
		 */ 
		public int GetRestMaxPos(){
			return restValues.maxPos;
		}
		/**
		 * metoda zwraca ilość bloków w nailiczniejszej rest kolumnie
		 */
		public int GetRestMaxVal(){
			return restValues.maxVal;
		}
		/**
		 * zwraca pozycję najmniej licznej kolumny w blokach rest
		 */ 
		public int GetRestMinPos(){
			return restValues.minPos;
		}
		/**
		 * zwraca ilość blokó∑ w najmniej licznej rest kolumnie
		 */ 
		public int GetRestMinVal(){
			return restValues.minVal;
		}
		/**
		 * zwraca pozycję kolumny która ma najwięcej bloków 
		 */ 
		public int GetSumtMaxPos(){
			return allValues.maxPos;
		}
		/**
		 * metoda zwraca ilość bloków w nailiczniejszej kolumnie
		 */
		public int GetSumMaxVal(){
			return allValues.maxVal;
		}
		/**
		 * zwraca pozycję najmniej licznej kolumny w blokach
		 */ 
		public int GetSumMinPos(){
			return allValues.minPos;
		}
		/**
		 * zwraca ilość blokó∑ w najmniej licznej kolumnie
		 */ 
		public int GetSumMinVal(){
			return allValues.minVal;
		}
		/**
		 *  obliczamy wartość minimalną i maksymalna dla kolumn bloków rest lub active
		 */ 
		private void CalculateMinMax(Values values,Dictionary<int,int> columnCount){
			this.CalculateMinMax (values,columnCount,null);
		}
		private void CalculateMinMax(Values values,Dictionary<int,int> columnCount,Dictionary<int,int> columnSecondCount){
			values.Reset ();
			foreach (KeyValuePair<int,int> pair in columnCount) {
				int pos=pair.Key;
				int count=0;
				if(columnSecondCount==null){
					count=pair.Value;
				}else{
					count=pair.Value+columnSecondCount[pos];
				}
				if(count<values.minVal||values.minVal==-1){
					values.minVal=count;
					values.minPos=pos;
				}
				if(count>values.maxVal){
					values.maxVal=count;
					values.maxPos=pos;
				}
			}
		}
		/**
		 * wyświetla zapełnione linie - wystarczy pobrać minimalną wartość dla kolumny rest
		 */ 
		public int GetFullRows(){
			return GetRestMinVal ();
		}
		private class Values{
			public void Reset(){
				minPos = 0;
				maxPos = 0;
				minVal = -1;
				maxVal = -1;
			}
			public int minPos;
			public int maxPos;
			public int minVal;
			public int maxVal;
		}
	}
}
