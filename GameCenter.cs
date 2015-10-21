using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms.GameCenter;
public class GameCenter{
	/**
	 * inicjujemy gameCenter
	 */ 
	public static void Authenticate(){
		#if UNITY_EDITOR
		return;
		#endif
		#if UNITY_IPHONE
		Social.localUser.Authenticate(GameCenter.CallbackCheckAuthIOS);
		#endif
	}
	private static void CallbackCheckAuthIOS(bool success){
		if (success) {
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
			Debug.Log("Autoryzacja Success "+Social.localUser.userName);
		}else{
			Debug.Log("Autoryzacja failed "+Social.localUser.userName);
		}
	}
	/**
	 * rejestrujemy achievement
	 */ 
	public static void AddAchievement(string acheivementId,double progress){
		#if UNITY_EDITOR
		return;
		#endif
		#if UNITY_IPHONE
		if(progress>100)progress=100;
		if(Social.localUser.authenticated){
			Social.ReportProgress(acheivementId,progress,GameCenter.CallbackCheckAchievement);
		}
		#endif
	}
	private static void CallbackCheckAchievement(bool success){
		if (success) {
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
			Debug.Log("Achievement Success "+Social.localUser.userName);
		}else{
			Debug.Log("Achievement failed "+Social.localUser.userName);
		}
	}
	/**
	 * rejestrujemy zdobyte punkty 
	 */
	public static void AddLeaderboard(string leaderboardId,long score){
		#if UNITY_EDITOR
		return;
		#endif
		#if UNITY_IPHONE
		if(Social.localUser.authenticated){
			Social.ReportScore(score,leaderboardId,GameCenter.CallbackCheckScore);
		}
		#endif
	}
	private static void CallbackCheckScore(bool success){
		if (success) {
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
			Debug.Log("Score Success "+Social.localUser.userName);
		}else{
			Debug.Log("Score failed "+Social.localUser.userName);
		}
	}
	public static class Leaderboard{
		public static string SCORE="pl.artwave.jumperthief.lScore";
		public static string HEIGHT="pl.artwave.jumperthief.lHeight";
	}
	public static class Achievement{
		public static string SCORE_800 = "pl.artwave.jumperthief.aScore800";
		public static string SCORE_3000 = "pl.artwave.jumperthief.aScore3000";

		public static string HEIGHT_70 = "pl.artwave.jumperthief.aHeight70";
		public static string HEIGHT_150 = "pl.artwave.jumperthief.aHeight150";

		public static string KILL_30 = "pl.artwave.jumperthief.aKill30";
		public static string KILL_100 = "pl.artwave.jumperthief.aKill100";

		public static string LIFE_5 = "pl.artwave.jumperthief.aLife5";
		public static string LIFE_13 = "pl.artwave.jumperthief.aLife13";
	}
}
