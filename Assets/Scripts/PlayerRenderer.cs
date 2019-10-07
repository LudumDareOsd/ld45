using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{
	public Sprite middle;
	public Sprite left;
	public Sprite right;

	public Sprite pLevel1;
	public Sprite pLevel2;
	public Sprite pLevel3;
	public Sprite pLevel4;
	public Sprite pLevel5;

	public Sprite pLevel1Orange;
	public Sprite pLevel2Orange;
	public Sprite pLevel3Orange;
	public Sprite pLevel4Orange;
	public Sprite pLevel5Orange;

	public GameObject powerlevel;

	private PlayerState state = PlayerState.Straight;
	public SpriteRenderer shipRenderer;
	private SpriteRenderer powerRenderer;

	public void Awake()
	{
		shipRenderer = GetComponent<SpriteRenderer>();
		powerRenderer = powerlevel.GetComponent<SpriteRenderer>();
	}

	public void TurnLeft()
	{
		if (state != PlayerState.Left)
		{
			shipRenderer.sprite = left;
			state = PlayerState.Left;
		}
	}

	public void TurnRight()
	{
		if (state != PlayerState.Right)
		{
			shipRenderer.sprite = right;
			state = PlayerState.Right;
		}
	}

	public void Straight()
	{
		if (state != PlayerState.Straight)
		{
			shipRenderer.sprite = middle;
			state = PlayerState.Straight;
		}
	}

	public void Forward()
	{

	}

	public void PowerLevel(int level, WeaponType weaponType)
	{

		if (weaponType.Equals(WeaponType.Plasma))
		{
			switch (level)
			{
				case 0:
					powerRenderer.sprite = null;
					break;

				case 1:
					powerRenderer.sprite = pLevel1;
					break;
				case 2:
					powerRenderer.sprite = pLevel2;
					break;
				case 3:
					powerRenderer.sprite = pLevel3;
					break;
				case 4:
					powerRenderer.sprite = pLevel4;
					break;
				default:
					powerRenderer.sprite = pLevel5;
					break;
			}
		}
		else {
			switch (level)
			{
				case 0:
					powerRenderer.sprite = null;
					break;

				case 1:
					powerRenderer.sprite = pLevel1Orange;
					break;
				case 2:
					powerRenderer.sprite = pLevel2Orange;
					break;
				case 3:
					powerRenderer.sprite = pLevel3Orange;
					break;
				case 4:
					powerRenderer.sprite = pLevel4Orange;
					break;
				default:
					powerRenderer.sprite = pLevel5Orange;
					break;
			}
		}
		
	}

	private enum PlayerState
	{
		Left,
		Right,
		Straight
	}
}
