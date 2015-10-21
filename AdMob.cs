using System;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdMob {
	
	private BannerView bannerView;
	private InterstitialAd interstitial;

	#if UNITY_EDITOR
	private string adUnitId = "unused";
	#elif UNITY_ANDROID
	private string adUnitId = "ca-app-pub-7054698623904367/9252197836";
	#elif UNITY_IPHONE
	private string adUnitId = "ca-app-pub-7054698623904367/3205664236";
	#else
	private string adUnitId = "unexpected_platform";
	#endif


	public void StartBanner(){
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		StartBanner (AdPosition.Bottom);
		#endif
	}
	/**
	 * wyświetlenie banera
	 */ 
	public void StartBanner(AdPosition position){
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		bannerView = new BannerView(adUnitId, AdSize.SmartBanner,position);
		bannerView.AdLoaded += HandleAdLoaded;
		bannerView.AdFailedToLoad += HandleAdFailedToLoad;
		bannerView.AdOpened += HandleAdOpened;
		bannerView.AdClosing += HandleAdClosing;
		bannerView.AdClosed += HandleAdClosed;
		bannerView.AdLeftApplication += HandleAdLeftApplication;

		bannerView.LoadAd(createAdRequest());
		#endif
	}
	public void StartInterstitial(){
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		interstitial = new InterstitialAd(adUnitId);
		interstitial.AdLoaded += HandleInterstitialLoaded;
		interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
		interstitial.AdOpened += HandleInterstitialOpened;
		interstitial.AdClosing += HandleInterstitialClosing;
		interstitial.AdClosed += HandleInterstitialClosed;
		interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
		interstitial.LoadAd(createAdRequest());
		#endif
	}

	private AdRequest createAdRequest(){
		AdRequest request = new AdRequest.Builder()
			.AddTestDevice(AdRequest.TestDeviceSimulator)
			.AddKeyword("game")
			.SetGender(Gender.Male)
			.SetBirthday(new DateTime(1985, 1, 1))
			.TagForChildDirectedTreatment(false)
			.Build();
		return request;
	}
	/**
	 * próba wyświetlenia banera
	 */ 
	public void ShowBanner() {
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		bannerView.Show();
		#endif
	}
	/**
	 * ukrycie banera
	 */ 
	public void HideBanner() {
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		bannerView.Hide();
		#endif
	}
	/**
	 * próba wyświetlenia popupa
	 */ 
	public void ShowInterstitial(){
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		if (interstitial.IsLoaded()){
			Debug.Log("Interstitial jest wy≥świetlany !!!.");
			interstitial.Show();
		}
		else{
			Debug.Log("Interstitial is not ready yet.");
		}
		#endif
	}
	/**
	 * zniszczenie bannera
	 */ 
	public void DestroyBanner(){
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		bannerView.Destroy();
		#endif
	}
	public void DestroyInterstitial(){
		#if SIMULATOR
		return;
		#endif
		#if UNITY_ANDROID || UNITY_IPHONE
		interstitial.Destroy();
		#endif
	}
	#region Banner callback handlers
	public void HandleAdLoaded(object sender, EventArgs args){
		Debug.Log("HandleAdLoaded event received.");
	}
	public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args){
		Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
	}
	public void HandleAdOpened(object sender, EventArgs args){
		Debug.Log("HandleAdOpened event received");
	}
	void HandleAdClosing(object sender, EventArgs args){
		Debug.Log("HandleAdClosing event received");
	}
	public void HandleAdClosed(object sender, EventArgs args){
		Debug.Log("HandleAdClosed event received");
	}
	public void HandleAdLeftApplication(object sender, EventArgs args){
		Debug.Log("HandleAdLeftApplication event received");
	}
	#endregion

	#region Interstitial callback handlers
	public void HandleInterstitialLoaded(object sender, EventArgs args){
		Debug.Log("HandleInterstitialLoaded event received.");
	}
	public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args){
		Debug.Log("HandleInterstitialFailedToLoad event received with message: " + args.Message);
	}
	public void HandleInterstitialOpened(object sender, EventArgs args){
		Debug.Log("HandleInterstitialOpened event received");
	}
	void HandleInterstitialClosing(object sender, EventArgs args){
		Debug.Log("HandleInterstitialClosing event received");
	}
	public void HandleInterstitialClosed(object sender, EventArgs args){
		Debug.Log("HandleInterstitialClosed event received");
	}
	public void HandleInterstitialLeftApplication(object sender, EventArgs args){
		Debug.Log("HandleInterstitialLeftApplication event received");
	}
	#endregion
}