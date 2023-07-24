using UnityEngine;

public class ChaosMotion : MonoBehaviour
{
    [SerializeField] float _minPause;
    [SerializeField] float _maxPause;

    [SerializeField] bool _limitedLifetime;
	[SerializeField] float _timeLeft;
    bool _move = true;
    Vector3 _finishPos;
    float _timeSpent;
    float _randomPauseTime;
	float _startTimeOut;

    SpriteRenderer _sr;

	Color _color;

    public bool Move
    {
        get { return _move; }
    }

	private void Start()
	{
		_sr = GetComponent<SpriteRenderer>();
		_startTimeOut = _timeLeft;
        SetNewStartPos();
	}

	private void Update()
	{
		if (GameManager.Single.GameActive)
        {
            if (_move)
            {
				if (Vector2.Distance(transform.position, _finishPos) > 0.05)
				{
					transform.position = Vector2.MoveTowards(transform.position, _finishPos, Time.deltaTime * GameManager.Single.Speed);
				}
				else
				{
					_move = false;
				}
			}
			else
			{
				_timeSpent += Time.deltaTime;
				if (_timeSpent > _randomPauseTime)
				{
					_timeSpent = 0;
					_move = true;

					SetNewFinishPos();
				}
			}

			if (_limitedLifetime)
			{
				_timeLeft -= Time.deltaTime;

				_color = new Color(1, 1, 1, _timeLeft / _startTimeOut);
				_sr.color = _color;

				if (_timeLeft <= 0)
				{
					_limitedLifetime = false;
					_move = true;
					_timeSpent = 0;
					_randomPauseTime = 10;
					_finishPos = new Vector3(transform.position.x > 0 ? GameManager.Single.RightUpperCorner.x + 2 : -GameManager.Single.RightUpperCorner.x - 2, transform.position.y, 0);
					Destroy(gameObject, 2);
				}
			}
		}
	}

	private void SetNewStartPos()
	{
        transform.position = new(Random.value > 0.5f ? -GameManager.Single.RightUpperCorner.x - 1 : GameManager.Single.RightUpperCorner.x + 1, 
            Random.Range(-GameManager.Single.RightUpperCorner.y, GameManager.Single.RightUpperCorner.y), 0);

        SetNewFinishPos();
	}

	private void SetNewFinishPos()
	{
        _finishPos = new(Random.Range(-GameManager.Single.RightUpperCorner.x + 0.5f, GameManager.Single.RightUpperCorner.x - 0.5f),
            Random.Range(-GameManager.Single.RightUpperCorner.y + 0.5f, GameManager.Single.RightUpperCorner.y), 0);
		_randomPauseTime = Random.Range(_minPause, _maxPause);

		float rot = Mathf.Atan2(_finishPos.y - transform.position.y, _finishPos.x - transform.position.x) * Mathf.Rad2Deg - 90;
		transform.rotation = Quaternion.Euler(0, 0, rot);
	}

	public void Clicked()
	{
		SetNewStartPos();
		_timeSpent = 0;
		_move = true;
		_timeLeft = _startTimeOut;
	}
}
