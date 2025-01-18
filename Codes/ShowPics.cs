using UnityEngine;
using DG.Tweening;
using System;
using System.IO;
using Mono.Cecil;
using UnityEngine.UI;

public class ShowPics : MonoBehaviour
{
    private string folderPath = "Pics";//ͼƬ�ļ��е�·��
    private string[] imgNames;//���ڴ洢����Ҫת����ͼƬ ������·��

    public int i = 1;//��ʼͼƬ���
    public int maxlength = 10;//���ͼƬ���
    public RawImage rawImage;//��ʾͼƬ��RawImage���
    public float interval = 5;//ͼƬ�л����

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
                Debug.Log("���ص�" + i + "��ͼƬ");

                Texture2D img = Resources.Load<Texture2D>("2." + i);

                rawImage.rectTransform.sizeDelta = new Vector2(img.width, img.height);
                rawImage.texture = img;

                i++;

                LoadPic();
            }
        });
    }
}