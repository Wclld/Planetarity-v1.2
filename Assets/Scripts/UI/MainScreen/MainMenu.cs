using System;
using UnityEngine;
using UnityEngine.UI;

internal sealed class MainMenu : MonoBehaviour
{
	[SerializeField] Button _loadButton = default;
	private GameManager _gameManager;
	private CanvasGroup _canvasGroup;
	private Saver _saver;

	private void Awake ( )
	{
		_canvasGroup = GetComponent<CanvasGroup>( );
		_gameManager = FindObjectOfType<GameManager>( );
		_saver = FindObjectOfType<Saver>( );

		if ( _gameManager == null )
		{
			Destroy( gameObject );
		}

		_gameManager.OnWin += ShowMenuCanvas;
		_gameManager.OnLoose += ShowMenuCanvas;

		ShowMenuCanvas( );
	}

	public void StartGame ( )
	{
		HideMenuCanvas( );
		_gameManager.InitGame( );
	}

	public void LoadSavedGame ( )
	{
		HideMenuCanvas( );
		_gameManager.LoadFromSave( );
	}


	private void HideMenuCanvas ( )
	{
		_canvasGroup.alpha = 0;
		_canvasGroup.blocksRaycasts = false;
		_canvasGroup.interactable = false;
	}

	private void ShowMenuCanvas ( )
	{
		_loadButton.interactable = _saver.HaveSave;

		_canvasGroup.alpha = 1;
		_canvasGroup.blocksRaycasts = true;
		_canvasGroup.interactable = true;
	}
}
