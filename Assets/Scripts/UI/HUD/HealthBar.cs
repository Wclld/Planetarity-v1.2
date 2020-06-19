﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent( typeof( Slider ) )]
public class HealthBar : MonoBehaviour
{
	[SerializeField] Text _healthText = default;
	[SerializeField] Slider _scrollbar = default;
	private int _maxHealth = default;

	private void Awake ( )
	{
		if ( _scrollbar == null )
		{
			_scrollbar = GetComponent<Slider>( );
		}
	}

	public void Init ( int maxHealth )
	{
		_maxHealth = maxHealth;
		ChangeHealth( maxHealth );
	}

	public void ChangeHealth ( int health )
	{
		float healthPercent = (float)health / _maxHealth;
		_scrollbar.value = healthPercent;

		_healthText.text = $"{health}/{_maxHealth}";
	}
}
