using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARShootBall : MonoBehaviour
{
    public float ForwardForce = 200f;//设置向前的力的大小
    public Vector3 StanTra = new Vector3(0, 1f, 0);//设置一个夹角的参照数值

    private bool blTouch = false;//是否触摸
    private bool blShooted = false;//是否发射
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 offset;//滑动偏移向量
    private float howLong;//滑动距离
    private int timeFlick;//滑动时间，帧数来记录
    private float speedFlick;//滑动速度

    private Camera camera;//记录主摄像机，用于坐标换算


    // Start is called before the first frame update
    void Start()
    {
        //先给主摄像机赋值，因为只能在start或awake里面
        camera = Camera.main;//赋值为主摄像机
    }

    // Update is called once per frame
    void Update()
    {
        if (blTouch)
        {
            slip();
        }
    }

    private void resetVar()//重置参数，把起始位置和终止位置设为手指按下的位置
    {
        startPosition = Input.mousePosition;
        endPosition = Input.mousePosition;
    }

    private void OnMouseDown()//允许手指滑动，通过精灵球是否发射来判断
    {
        if (blShooted == false)//如果精灵球未发射，运行手指滑动
        {
            blTouch = true;
        }
    }

    private void slip()
    {
        timeFlick += 25;//时间每帧增加25
        if (Input.GetMouseButtonDown(0))//当手指按下的时候，参数清零
        {
            resetVar();
        }

        if (Input.GetMouseButton(0))//当手指一直在屏幕上的时候
        {
            endPosition = Input.mousePosition;//始终接收最新的定位
            offset = camera.transform.rotation * (endPosition - startPosition);//获取手指在世界坐标上的偏移变量
            howLong = Vector3.Distance(startPosition, endPosition);//计算手指滑动距离
        }

        if (Input.GetMouseButtonUp(0))
        {
            speedFlick = howLong / timeFlick;
            blTouch = false;
            timeFlick = 0;
            if (howLong > 20 && endPosition.y - startPosition.y > 0)
            {
                shootBall();
            }
        }
    }

    private void shootBall()
    {
        transform.gameObject.AddComponent<Rigidbody>();
        //给精灵球添加一个刚体组件
        Rigidbody _rigBall = transform.GetComponent<Rigidbody>();
        //局部变量获取刚体组件
        //Rigidbody _rigBall = transform.gameObject.AddComponent<Rigidbody>();
        _rigBall.velocity = offset * 0.003f * speedFlick;
        //给精灵球一个初始速度，这个速度等于方向（单位向量）乘以速度
        _rigBall.AddForce(camera.transform.forward * ForwardForce);
        //给精力球一个向前方的力
        _rigBall.AddTorque(transform.right);
        //让精灵球旋转起来
        _rigBall.drag = 0.5f;
        //设置精灵球的阻力
        blShooted = true;
        //这个精灵球的已经发射出去了
        transform.parent = null;
        //扔出去之后把父级关联取消

        StartCoroutine(LateInsBall());
        //调用延时函数
    }

    IEnumerator LateInsBall()
    {
        yield return new WaitForSeconds(1.2f);
        //延时0.5s向下执行
        ARInitBall.Instance.InitBall_AR();
    }

}
