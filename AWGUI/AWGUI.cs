using UnityEngine;
using System.Collections;
using Assets.Code.Controllers;
namespace Assets.Code.AWGUI{
public class AWGUI{
	public static void DrawLifes(Rect position,int lifes,Texture lifeIcon) {
		float width = position.width;
			float singleWidth = width/5;
			float singleHeight = width/5;
		int newLine = 5;
		for(int i=0;i<lifes;i++){
			int line=(int)i/newLine;
				GUI.DrawTexture(new Rect(position.x+singleWidth*(i-line*newLine),position.y+line*singleHeight,singleWidth,singleHeight),lifeIcon,ScaleMode.ScaleToFit);
		}
	}
	public static bool ButtonTexture( Rect r, Texture t,ScaleMode s){
			GUI.DrawTexture( r, t,s);
			return GUI.Button( r, "", "");
	}
	public static float navAnimation;
		private static bool isFadeId=false;
	public static void DrawNavigation(float alpha,bool isAnimation){
			float centerX=Screen.width/2;
			float centerY=Screen.height/4*3*0.99f;
			float marginX = Screen.width/3.5f;
			float marginY = Screen.height/4;

			if(isAnimation==true){
				if(isFadeId){
					navAnimation+=0.2f*Time.deltaTime;
				}else{
					navAnimation-=0.2f*Time.deltaTime;
				}
				if (navAnimation <= 0) {
					navAnimation = 0;
					isFadeId=true;
				}else if(navAnimation>alpha){
					navAnimation = alpha;
					isFadeId=false;
				}
			}else{
				navAnimation = alpha;
			}

			Color oldColor = GUI.color;
			GUI.color = new Color (oldColor.r,oldColor.g,oldColor.b,navAnimation);
			float size = Screen.height * 0.11f;
			//if (Application.platform == RuntimePlatform.Android||Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Texture navLeft=StateControllerManager.stateController.GetGuiAssets().navLeftImg;
				GUI.DrawTexture(new Rect(centerX-size/2-marginX,centerY-size/2,size,size),navLeft,ScaleMode.ScaleToFit);

				Texture navRight=StateControllerManager.stateController.GetGuiAssets().navRightImg;
				GUI.DrawTexture(new Rect(centerX-size/2+marginX,centerY-size/2,size,size),navRight,ScaleMode.ScaleToFit);


				Texture navUp=StateControllerManager.stateController.GetGuiAssets().navUpImg;
				GUI.DrawTexture(new Rect(centerX-size/2,centerY-size/2-marginY*1.3f,size,size),navUp,ScaleMode.ScaleToFit);
			}
			GUI.color = oldColor;
	}
}
}
