using UnityEngine;

public class Saver : MonoBehaviour
{
	private const string SAVE_KEY = "SAVE_KEY";

	public bool HaveSave => PlayerPrefs.HasKey( SAVE_KEY );
	private void Update ( )
	{
		if ( Input.GetKeyDown( KeyCode.Space ) )
		{
			Save( );
		}
	}

	public void Save ( )
	{
		var newSer = new SaveInfo( );
		newSer.AddPlanetesInfo( );
		newSer.AddRocketsInfo( );
		var saveString = newSer.Serialize( );
		PlayerPrefs.SetString( SAVE_KEY, saveString );
		PlayerPrefs.Save( );
	}

	public SaveInfo Load ( )
	{
		var saveInfo = new SaveInfo( );
		if ( PlayerPrefs.HasKey( SAVE_KEY ) )
		{
			var saveString = PlayerPrefs.GetString( SAVE_KEY );
			saveInfo = JsonUtility.FromJson<SaveInfo>( saveString );
		}
		else
		{
			Debug.Log( "[Saver.Load]: KeyNotFound!" );
		}
		return saveInfo;
	}

}
