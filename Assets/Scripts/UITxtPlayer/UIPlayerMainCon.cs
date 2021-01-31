using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerMainCon : MonoBehaviour
{

    //单例----------------------------------------------------------------------------
    static UIPlayerMainCon _Instance;

    public static UIPlayerMainCon getInstance()
    {
        if (_Instance) return _Instance;
        else
        {
            Debug.Log("错误的获取时机!!!"); 
            return null;
        }
    }

    //播放功能-------------------------------------------------------------------------

    bool isOnPlay = false;//是否正在播放

    public bool IsOnPlay { get; }

    TextCon ptext;//正在播放的文本








    public void playText(int id) //启动播放
    {
        if (isOnPlay) return;
        isOnPlay = true;


        var panle = Instantiate(Resources.Load<RectTransform>("UIText/Text_" + id));

        if(!panle)
        {
            Debug.Log("未找到文本!!!");
            return;
        }

        panle.SetParent(gameObject.transform);

        panle.offsetMax = Vector2.zero;
        panle.offsetMin = Vector2.zero;


        ptext = panle.GetComponent<TextCon>();

        ptext.playInit();

        //
        Debug.Log("开始播放文本" + id);

    }


    public void playOver() //终止播放
    {
        if (!isOnPlay) return;

        isOnPlay = false;

        Debug.Log("播放完成！！！");

        Destroy(ptext.gameObject);


    }


    public void showTeam()
    {
        UIPlayerMainCon.getInstance().playText(0);
    }




    //Mono----------------------------------------------------------------------------

    private void Awake()
    {
        if (!_Instance) _Instance = this;
        else
        {
            if (_Instance != this) Destroy(this); //OR destory gameObject
        }
    }

    
    
    // Update is called once per frame
    void Update()
    {

        //测试------------------------------------
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UIPlayerMainCon.getInstance().playText(1);
        }


        //播放
        if (isOnPlay) ptext.playUpdate();
    }
}
