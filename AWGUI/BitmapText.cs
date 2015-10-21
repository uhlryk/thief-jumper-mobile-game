using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace Assets.Code.AWGUI{
	public class BitmapText {
		private Dictionary<char,Texture> bitmapMap;
		private Texture[] bitmapList;
		private string text;
		private bool isPrepare=false;
		private List<DrawData> drawDataList;
		private Orientation orientation=Orientation.horizontal;
		private float oldWidth;
		private float oldHeight;
		private Size lastSize;
		/**
		 * ustawiamy tekstury przez mapę która zawiera char jako klucz i teksturę jako wartość
		 */ 
		public void setTextures(Dictionary<char,Texture> bitmapMap){
			this.bitmapMap = bitmapMap;
			isPrepare=false;
		}
		/**
		 *  nadaje się tylko do cyfr
		 * pobiera listę tekstur które mają indeksy od 0 do 9 co odpowiada teksturom, indeksy są kluczami
		 */ 
		public void setTextures(Texture[] bitmapList){
			this.bitmapList = bitmapList;
			isPrepare=false;
		}

		/**
		 * metoda przygotowuje Dictionary<char,Texture> bitmapMap na podstawie listy bitmap i stringa zawierającego klucze
		 */ 
		public void setTexture(Texture[] bitmapList,string keys){
			this.bitmapMap=new Dictionary<char,Texture>();
			for(int i=0;i<bitmapList.Length;i++){
				Texture bitmap=bitmapList[i];
				char c=keys[i];
				if(bitmap!=null&&c!=null){
					this.bitmapMap.Add(c,bitmap);
				}
			}
		}
		/**
		 * ustawiamy orientację, tekst może być standardowo horyzontalny ale również wertykalny
		 */ 
		public void SetOrientation(Orientation orientation){
			this.orientation = orientation;
			isPrepare=false;
		}
		/**
		 * ustalamy tekst który będzie wyświetlany
		 */ 
		public void SetText(string text){
			if (this.text == text) {//nie uległ zmianie tekst więc pozostaje bez zmian wszystko
						//nic tu nie robimy
			} else { //należy wczytać na nowo dane DrawData
				this.text = text;
				isPrepare = false;
			}
		}
		/**
		 * metoda przetwarza tekst i tekstury, ustawiając ich pozycję i skalę. Otrzymany wynik jest cachowany. Do momentu zmiany rozmiaru, tekstur lub tekstu
		 * zmiana pozycji całego tekstu nie wpływa na ponowne przykogowanie. 
		 */ 
		private Size Prepare(float width,float height){
			if(oldWidth!=width||oldHeight!=height){
				oldWidth=width;
				oldHeight=height;
				isPrepare=false;
			}
			if(isPrepare==false){


				Size size=PrepareBitmap(1);
				float widthScale=width/size.width;
				float heightScale=height/size.height;
				if(widthScale<heightScale){
					size=PrepareBitmap(widthScale);
				}else{
					size=PrepareBitmap(heightScale);
				}
				isPrepare = true;
				lastSize=size;
				return size;
			}else{
				return lastSize;
			}
		}
		/**
		 *  oblicza tekstury, po otrzymaniu wyniku rozmiar jest porównywany z rozmiarem w którym ma się zmieścić wynik, jeśli się nie nieści następuje
		 * przeskalowanie i ponowne obliczanie
		 */ 
		private Size PrepareBitmap(float scale){
			float width=0;
			float height = 0;
			drawDataList=new List<DrawData>();
			for(int i=0;i<this.text.Length;i++){
				char c=this.text[i];
				Texture bitmap=null;
				if(bitmapMap!=null){
					Texture tempBitmap=bitmapMap[c];
					if(tempBitmap!=null){
						bitmap=tempBitmap;
					}
				}
				if(bitmapList.Length>0){
					Texture tempBitmap=bitmapList[int.Parse(c.ToString())];
					if(tempBitmap!=null){
						bitmap=tempBitmap;
					}
				}
				if(bitmap!=null){
					DrawData data=new DrawData();
					data.bitmap=bitmap;
					if(orientation==Orientation.horizontal){
						data.position=new Rect(width,0,bitmap.width*scale,bitmap.height*scale);
						width+=bitmap.width*scale;
						if(height<bitmap.height*scale)height=bitmap.height*scale;
					}else{
						data.position=new Rect(0,height,bitmap.width,bitmap.height*scale);
						height+=bitmap.height*scale;
						if(width<bitmap.width*scale)width=bitmap.width*scale;
					}
					drawDataList.Add(data);
				}
			}
			return new Size (width,height);
		}
		/**
		 * rysowanie tekstu z jego zmianą
		 */ 
		public Size Draw(Rect position,string text){
			this.SetText (text);
			return this.Draw (position);
		}
		/**
		 *  rysowanie tekstu, gdzie zmieniamy pozycję i rozmiar
		 * sama pozycja jest cachowana, rozmiar nie.
		 * Rozmiar określa prostokąt w któ®ym ma się zmieścić cały tekst, tekst będzie się skalował tak by się zmieścić maksymalnie w danym prostokącie
		 */ 
		public Size Draw(Rect position){
			Size size=this.Prepare (position.width,position.height);
			for (int i =0; i < drawDataList.Count; i++) {
				GUI.DrawTexture (
					new Rect(
					position.x+drawDataList[i].position.x+(position.width-size.width)/2,
					position.y+drawDataList[i].position.y+(position.height-size.height)/2,
						drawDataList[i].position.width,
						drawDataList[i].position.height
					),drawDataList[i].bitmap);
			}
			return size;
		}
		/**
		 * dane dla jednego znaku 
		 * czyli dla każdego znaku przechowujemy jego pozycję i rozmiar i teksturę, 
		 */ 
		private class DrawData{
			public Texture bitmap;
			public Rect position;
		}
		public class Size{
			public float width;
			public float height;
			public Size(float w,float h){
				width=w;
				height=h;
			}
		}
		public enum Orientation{
			horizontal,vertical
		}
	}
}
