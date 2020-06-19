using UnityEngine;

internal sealed class Pause : MonoBehaviour
{
	private bool _isPaused = false;

	private void Update ( )
	{
		if ( Input.GetKeyDown( KeyCode.P ) )
		{
			SwitchPause( );
		}
	}

	private void SwitchPause ( )
	{
		_isPaused = !_isPaused;
		Time.timeScale = _isPaused ? 0 : 1;
	}
}
