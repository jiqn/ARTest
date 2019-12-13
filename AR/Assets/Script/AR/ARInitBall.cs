using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARInitBall : MonoBehaviour
{
    public GameObject[] ballPrefab;
    public Transform BallPosi;

    private void Awake()
    {
        ballPrefab = Resources.LoadAll<GameObject>("Balls");
    }
    // Start is called before the first frame update
    void Start()
    {
        InitBall_AR();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitBall_AR()
    {
        GameObject _ball = Instantiate(ballPrefab[0], BallPosi.transform.position, BallPosi.transform.rotation);
        _ball.transform.SetParent(BallPosi);

        _ball.gameObject.AddComponent<SphereCollider>();//给小球添加一个球形碰撞器
    }
}
