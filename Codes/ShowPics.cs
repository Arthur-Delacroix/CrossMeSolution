using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

//这里要注意，保存在Resources文件夹中的图片，格式为Sprite(2D and UI)
//不然的话，图片会被强制变成2的幂次，图片就会变形

public class ShowPics : MonoBehaviour
{
    public string pathPrefix;//"Pics/2."
    public string filePrefix;//"2."
    public int startIndex = 1;//起始图片序号
    public int maxIndex = 10;//最大图片序号
    public RawImage rawImage;//显示图片的RawImage组件
    public float interval = 5;//图片切换间隔

    private float screenWidth = 1820.0f;//图片最大宽度
    private float screenHeight = 980.0f;//图片最大高度

    [SerializeField] private float intervalTime = 5.0f;//图片切换间隔时间

    [SerializeField] private Text titleText;//当前图片的标题

    private void Start()
    {
        LoadPic();
    }

    private void LoadPic()
    {
        DOVirtual.DelayedCall(intervalTime, () =>
        {
            if (startIndex <= maxIndex)
            {
                Debug.Log("加载第" + startIndex + "张图片");

                Texture2D img = Resources.Load<Texture2D>(pathPrefix + startIndex);

                titleText.text = filePrefix + startIndex;

                if (img.height > screenHeight && img.width < screenWidth)//只有图片高度大于屏幕最大高度
                {
                    float _ratio = screenHeight / img.height; //计算缩放比例
                    float tmp_width = img.width * _ratio; //计算缩放后的宽度

                    rawImage.rectTransform.sizeDelta = new Vector2(tmp_width, screenHeight);
                    rawImage.texture = img;
                }
                else if (img.width > screenWidth && img.height < screenHeight)
                {
                    float _ratio = screenWidth / img.width;
                    float tmp_height = img.height * _ratio;

                    rawImage.rectTransform.sizeDelta = new Vector2(screenWidth, tmp_height);
                    rawImage.texture = img;
                }
                else if (img.width > screenWidth && img.height > screenHeight)//图片宽度高度都大于屏幕最大宽度高度
                {
                    float _ratio_W = screenWidth / img.width;
                    float _ratio_H = screenHeight / img.height;

                    //哪个越小，说明哪个边越长，要使用最长边的系数

                    if (_ratio_W > _ratio_H)
                    {
                        float tmp_height = img.height * _ratio_H;
                        float tmp_width = img.width * _ratio_H;

                        rawImage.rectTransform.sizeDelta = new Vector2(tmp_width, tmp_height);
                        rawImage.texture = img;
                    }
                    else
                    {
                        float tmp_height = img.height * _ratio_W;
                        float tmp_width = img.width * _ratio_W;

                        rawImage.rectTransform.sizeDelta = new Vector2(tmp_width, tmp_height);
                        rawImage.texture = img;
                    }
                }
                else
                {
                    rawImage.rectTransform.sizeDelta = new Vector2(img.width, img.height);
                    rawImage.texture = img;
                }

                startIndex++;

                LoadPic();
            }
        });
    }
}