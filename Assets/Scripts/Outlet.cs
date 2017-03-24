using UnityEngine;

public class Outlet : MonoBehaviour
{
    public Transform shoot1;
    public Transform shoot2;
    public GameObject electricBall;
    public int direction = 1;

    [Range(1.0f, 10.0f)]
    public float waitingTime = 5.0f;
    float _shootTime;

    [Range(1.0f, 10.0f)]
    public float preparingTime = 3.0f;
    float _prepareTime;
    bool _isPrepering = false;

    [Range(1.0f, 10.0f)]
    public float loadingTime = 2.0f;
    float _loadTime;
    bool _isLoading = false;

    Animator _animator;

    void Awake()
    {
        _shootTime = 5;
        _prepareTime = 0;
        _isPrepering = false;
        _isLoading = false;
        _loadTime = 5;

        _animator = GetComponent<Animator>();
        if (_animator == null) // if Animator is missing
            Debug.LogError("Animator component missing from this gameobject");

    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPrepering)
        {
            if (_isLoading)
            {
                if (Time.time >= _loadTime)
                {
                    _shootTime = Time.time + waitingTime;
                    _isLoading = false;
                    _animator.SetTrigger("ToIdle");
                }
            }
            else if (Time.time >= _shootTime)
            {
                _prepareTime = Time.time + preparingTime;
                _isPrepering = true;
                _animator.SetBool("isShooting", true);
            }
        }
        else
        {
            if (Time.time >= _prepareTime)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        _loadTime = Time.time + loadingTime;
        _isLoading = true;
        _isPrepering = false;
        direction = Mathf.FloorToInt(Random.Range(1, 4.99f));
        GameObject eb1 = Instantiate(electricBall, shoot1.position, Quaternion.identity);
        GameObject eb2 = Instantiate(electricBall, shoot2.position, Quaternion.identity);

        //1 right
        //2 left
        int eb1Direction = 1;
        int eb2Direction = 1;

        switch (direction)
        {
            case 1:
                eb1Direction = 1;
                eb2Direction = 2;
                break;
            case 2:
                eb1Direction = 2;
                eb2Direction = 1;
                break;
            case 3:
                eb1Direction = 1;
                eb2Direction = 1;
                break;
            case 4:
                eb1Direction = 2;
                eb2Direction = 2;
                break;
            default:
                break;
        }

        eb1.GetComponent<ElectricBall>().BallDirection(eb1Direction);
        eb2.GetComponent<ElectricBall>().BallDirection(eb2Direction);

        _animator.SetBool("isShooting", false);

    }
}
