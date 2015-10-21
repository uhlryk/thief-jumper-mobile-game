using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	/**
	 * plansza zbudowana jest na wielokrotności wysokości i szerokości bloku, a dokładniej jego przekroju w sytuacji gdy blok jest pseudo 3D
	 */ 
	public float blockWidth;
	/**
	 * plansza zbudowana jest na wielokrotności wysokości i szerokości bloku, a dokładniej jego przekroju w sytuacji gdy blok jest pseudo 3D
	 */ 
	public float blockHeight;

	/**
	 * szerokość planszy wyrażona w wielkokrotności szerokości bloku
	 */ 
	public int boardSize;

	/**
	 * startowa liczba punktów żyć gracza. jedno skuszenia zabiera jedno życie
	 */ 
	public int startLifePoints;

	/**
	 * aktualna liczba punktów żyć
	 */ 
	public int actualLifePoints;

	/**
	 * zapisuje najwyższą ilość żyć w danej grze jaką gracz posiadał
	 */ 
	public int actMaxLifePoints;

	/**
	 * rekordowy wynik gracza
	 */ 
	public int bestScore;
	/**
	 * ilość zabitych wrogów w danej grze. potrzebne do achievementów
	 */ 
	public int actEnemyKilled;

	/**
	 * w danej grze maksymalna wysokość gracza
	 */ 
	public int actMaxHeight;

	/**
	 * aktualny wynik gracza w danej grze
	 */ 
	public int actualScore;

	/**
	 * mamy włączoną muzykę
	 */ 
	public bool isMusic;
	/**
	 * czy mamy włączone dźwięki
	 */ 
	public bool isSound;

	/**
	 * jaki był ostatni poziom otrzymania życia za punkty
	 * zaczynamy np od 100, jak gracz osiągnie ten poziom to wtedy musi mieć np 200
	 */
	public int lastPointsLife=0;
	/**
	 * ile punktów jest wymienianych na życie
	 */ 
	public int exchangePoinsts=200;

	/**
	 * łączna liczba włączeń gry
	 */ 
	public int countGames;
	/**
	 * łączna liczba rozgrywek (restart game jest liczone jako kolejna rozgrywka)
	 */ 
	public int countAllPlays;
	/**
	 * liczba rozgrywek w ramach tego włączenia
	 */ 
	public int countActPlays;

	/**
	 * może być wywołane przez dowolny stan w scenie planszy, sprawdzany jest w menu, jeśli true to wyświetla banner
	 */ 
	public bool showBanner;
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
