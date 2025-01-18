using UnityEngine;
using DG.Tweening;
using System;
using System.IO;
using Mono.Cecil;
using UnityEngine.UI;

public class ShowPics : MonoBehaviour
{
    private string folderPath = "Pics";//图片文件夹的路径
    private string[] imgNames;//用于存储所有要转换的图片 的完整路径

    public int i = 1;//起始图片序号
    public int maxlength = 10;//最大图片序号
    public RawImage rawImage;//显示图片的RawImage组件
    public float interval = 5;//图片切换间隔

    private void Start()
    {
        Texture2D img = Resources.Load<Texture2D>("2." + i);

        rawImage.rectTransform.sizeDelta = new Vector2(img.width, img.height);
        rawImage.texture = img;

        i++;

        LoadPic();
    }

    private void LoadPic()
    {
        DOVirtual.DelayedCall(2, () =>
        {
            if (i <= maxlength)
            {
                Debug.Log("加载第" + i + "张图片");

                Texture2D img = Resources.Load<Texture2D>("2." + i);

                rawImage.rectTransform.sizeDelta = new Vector2(img.width, img.height);
                rawImage.texture = img;

                i++;

                LoadPic();
            }
        });
    }
}