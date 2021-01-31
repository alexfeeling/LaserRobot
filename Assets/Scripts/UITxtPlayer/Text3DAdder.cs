using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Text3DAdder : MonoBehaviour
{
    //单例----------------------------------------------------------------------------
    static Text3DAdder _Instance;

    public static Text3DAdder getInstance()
    {
        if (_Instance) return _Instance;
        else
        {
            Debug.Log("错误的获取时机!!!");
            return null;
        }
    }

    //---------------------------------------------------

    class TextInform
    {
        public Transform trans;
        public TextMesh textMesh;
        public float playTime; //上移时间
        public float nowTime=0f; //当前时间
        public float baseHeigth;//基础高度
        public float moveHeigth; //位移高度（距离）

        public TextInform( Transform trs,
TextMesh tMesh,
 float pTime,//上移时间
 float bHeigth,//基础高度
 float mHeigth) //位移高度（距离）)
        {
            trans = trs;
            textMesh = tMesh;
            playTime = pTime;
            baseHeigth = bHeigth;
            moveHeigth = mHeigth;

        }

    }

    List<TextInform> texts;

    public void addText(string txt,Color color , float pTime=2.0f,float bHeigth=0.5f,float mHeigth=2.0f)
    {
        var text3D = Instantiate(Resources.Load<Transform>("Prefab/3DText"));

        var txtMesh = text3D.GetComponent<TextMesh>();

        txtMesh.text = txt;
        txtMesh.color = color;

        texts.Add(new TextInform ( text3D, txtMesh, pTime, bHeigth, mHeigth ));

    }





    //Mono---------------------------------------------------

    private void Awake()
    {
        if (!_Instance) _Instance = this;
        else
        {
            if (_Instance != this) Destroy(this); //OR destory gameObject
        }

        texts = new List<TextInform>();
    }

    private void Update()
    {
        //测试

        //if (Input.GetKeyDown(KeyCode.Space))
        //{

        //    Text3DAdder.getInstance().addText("获得蓝色激光", Color.blue);

        //}

        for(int i=0;i<texts.Count;++i)
        {
            var textInform = texts[i];
            textInform.nowTime += Time.deltaTime;
            if(textInform.nowTime>textInform.playTime)
            {
                Destroy(textInform.trans.gameObject);
                texts.Remove(textInform);
                --i;
                continue;
            }
            textInform.trans.position = new Vector3(gameObject.transform.position.x,
                gameObject.transform.position.y+textInform.baseHeigth+textInform.moveHeigth*textInform.nowTime/textInform.playTime,
                gameObject.transform.position.z);
            textInform.textMesh.color = new Color(textInform.textMesh.color.r, textInform.textMesh.color.g, textInform.textMesh.color.b, (textInform.playTime - textInform.nowTime) / textInform.playTime);

        }
    }

}
