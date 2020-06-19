using UnityEngine;
using UnityEngine.UI;

public class Cooldown : MonoBehaviour
{
	[SerializeField] Image _cooldownImg;
	[SerializeField] Image _rocketImg;

	public void UpdateCooldown ( float timeLeftPercent )
	{
		_cooldownImg.fillAmount = timeLeftPercent;
	}

	public void SetReady ( )
	{
		_cooldownImg.fillAmount = 0;
		ChangeRocketAlpha( 1 );
	}

	public void ResetCooldown ( )
	{
		_cooldownImg.fillAmount = 1;

		ChangeRocketAlpha( .25f );
	}

	private void ChangeRocketAlpha ( float alpha )
	{
		var rocketColor = _rocketImg.color;
		rocketColor.a = alpha;
		_rocketImg.color = rocketColor;
	}
}
