using UnityEngine;

[RequireComponent( typeof ( CanvasGroup ) )]
internal sealed class PlayerHUD : MonoBehaviour
{
	[SerializeField] Cooldown _cooldown;
	[SerializeField] HealthBar _healthBar;
	private CanvasGroup _canvas;


	private void Awake ( )
	{
		_canvas = GetComponent<CanvasGroup>( );
	}


	public void ShowHUD ( )
	{
		_canvas.alpha = 1;
	}

	public void HideHUD ( )
	{
		_canvas.alpha = 0;
	}

	internal void SubscribePlayer ( Planet planet )
	{
		_healthBar.Init( planet.MaxHealth );

		planet.OnHealthChange += _healthBar.ChangeHealth;
		planet.OnUpdateCooldown += _cooldown.UpdateCooldown;

		planet.OnDeath += x => HideHUD( );
		planet.OnDeath += Unsubscribe;

		ShowHUD( );
	}

	private void Unsubscribe ( Planet planet )
	{
		planet.OnHealthChange -= _healthBar.ChangeHealth;
		planet.OnUpdateCooldown -= _cooldown.UpdateCooldown;
		planet.OnDeath -= Unsubscribe;
	}
}
