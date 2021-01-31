using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCon : MonoBehaviour
{

    public int txtNumber = 0; //文本数量 除第一Sibling子对象是背景

    public bool flashIn = true;//是否直接显示第一张

    public float time = 0.8f;//切换时间

    bool onChange = true;//正在切换？

    List<List<Text>> textList;
    List<List<Image>> imgList;
    int nowTxtId = 1; //当前正在播放第几张
    float playTime = 0; //当前播放进度



    public void playUpdate() //每帧调用，实现播放
    {
        if (nowTxtId > txtNumber) //完成播放
        {
            UIPlayerMainCon.getInstance().playOver();
            return;
        }

        if (!onChange) //接收开启切换
        {
            if (Input.GetMouseButtonDown(0))
            {
                onChange = true;
                playTime = 0;
                ++nowTxtId;
            }

        }
        else // 切换中
        {
            playTime += Time.deltaTime;
            if(playTime > time) //完成切换
            {
                if (nowTxtId - 2 >= 0) //上一张
                {
                    foreach (Text txt in textList[nowTxtId - 2])
                    {
                        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0f);
                    }

                    foreach (Image img in imgList[nowTxtId - 2])
                    {
                        img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
                    }
                }

                //下一张
                foreach (Text txt in textList[nowTxtId-1 ])
                {
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
                }

                foreach (Image img in imgList[nowTxtId-1])
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 1);
                }


                onChange = false;
                return;
            }

            if (nowTxtId - 2 >= 0) //淡出上一张
            {
                foreach (Text txt in textList[nowTxtId - 2])
                {
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, (time-playTime)/time);
                }

                foreach (Image img in imgList[nowTxtId - 2])
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, (time - playTime) / time);
                }
            }

            //淡入下一张
            foreach (Text txt in textList[nowTxtId -1])
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, playTime / time);
            }

            foreach (Image img in imgList[nowTxtId -1])
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, playTime / time);
            }
        }
    }

    public void playInit() //初始化播放
    {
        textList = new List<List<Text>>();
        imgList = new List<List<Image>>();
        //加载

        int i=1;

        for( ; i <= txtNumber; ++i)
        {
            var panel = transform.GetChild(i).GetComponent<Transform>();
            if (!panel) return;

            List<Text> tlist = new List<Text>();
            List<Image> ilist = new List<Image>();

            for (int j = 0; j < panel.childCount; ++j)
            {
                var item = panel.GetChild(j);

                var txt = item.GetComponent<Text>();
                var img = item.GetComponent<Image>();
                if (txt) tlist.Add(txt);
                if (img) ilist.Add(img);
            }

            textList.Add(tlist);
            imgList.Add(ilist);

            
        }

        txtNumber = i - 1;

        if (txtNumber == 0) return;

        //切出第一张

        if (flashIn) //第一张立即出现
        {
            onChange = false;

            foreach(Text txt in textList[0])
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1f);
            }

            foreach(Image img in imgList[0])
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
            }

            for(int j = 1; j < textList.Count; ++j)
            {
                foreach (Text txt in textList[j])
                {
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0f);
                }

                foreach (Image img in imgList[j])
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
                }
            }

        }
        else //从第一张开始切入
        {
            foreach(List<Text> tlist in textList)
            {
                foreach(Text txt in tlist)
                {
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0f);
                }
            }

            foreach (List<Image> ilist in imgList)
            {
                foreach (Image img in ilist)
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
                }
            }
        }



    }



}
