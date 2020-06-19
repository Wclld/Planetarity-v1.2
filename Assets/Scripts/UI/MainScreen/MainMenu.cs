﻿using System;
using UnityEngine;

internal sealed class MainMenu : MonoBehaviour
{
	private GameManager _gameManager;
	private CanvasGroup _canvasGroup;


	private void Awake ( )
	{
		_canvasGroup = GetComponent<CanvasGroup>( );
		_gameManager = FindObjectOfType<GameManager>( );

		if ( _gameManager == null )
		{
			Destroy( gameObject );
		}

		_gameManager.OnWin += ShowMenuCanvas;
		_gameManager.OnLoose += ShowMenuCanvas;
	}

	public void StartGame ( )
	{
		HideMenuCanvas( );
		_gameManager.InitGame( );
	}

	public void LoadSavedGame ( )
	{
		HideMenuCanvas( );
	}


	private void HideMenuCanvas ( )
	{
		_canvasGroup.alpha = 0;
		_canvasGroup.blocksRaycasts = false;
		_canvasGroup.interactable = false;
	}

	private void ShowMenuCanvas ( )
	{
		_canvasGroup.alpha = 1;
		_canvasGroup.blocksRaycasts = true;
		_canvasGroup.interactable = true;
	}
}
