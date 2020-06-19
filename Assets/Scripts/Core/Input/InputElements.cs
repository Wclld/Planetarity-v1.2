using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
internal sealed class InputElements
{
	[SerializeField] Transform _rootObject = default;
	[SerializeField] List<GameObject> _inputPrefabs = default;

	internal IInput GetNewInput ( InputType type )
	{
		var index = (int)type;

		var newObject = GameObject.Instantiate( _inputPrefabs[index], _rootObject );
		var result = newObject.GetComponent<IInput>( );

		return result;
	}
}
