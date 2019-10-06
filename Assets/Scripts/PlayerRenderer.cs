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

	public void PowerLevel(int level)
	{
		switch (level)
		{
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
			case 5:
				powerRenderer.sprite = pLevel5;
				break;
		}

	}

	private enum PlayerState
	{
		Left,
		Right,
		Straight
	}
}
