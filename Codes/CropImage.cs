using UnityEngine;
using System.IO;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.ImgprocModule;
using System.Collections.Generic;

public class CropImage : MonoBehaviour
{
    private string featureImg_Up = "Up.jpg";//上半部分 特征图 的路径
    private string featureImg_Down = "Down.jpg";//下半部分 特征图 的路径
    private string outputImg_Path;//输出图片的路径
    private float threshold = 0.95f;//匹配阈值

    public string imageAPath = "a.jpg";//用于被裁剪的图片

    public string outputPath = "z.jpg";

    private string[] imgNames;//用于存储所有要转换的图片 的完整路径
    private string folderPath = "Pics";//图片文件夹的路径
    private void Start()
    {
        string fullPath = Path.Combine(Application.streamingAssetsPath, folderPath);

        imgNames = Directory.GetFiles(fullPath, "*.JPG");

        foreach (string imgName in imgNames)
        {
            //Debug.Log("开始处理图片: " + Path.GetFileName(imgName));
        }
    }

    private void aaa()
    {
        Mat croppedMat = new Mat();

        // 加载图片  
        Mat imgA = Imgcodecs.imread(Application.streamingAssetsPath + "/" + imageAPath);
        Mat upImg = Imgcodecs.imread(Application.streamingAssetsPath + "/" + featureImg_Up);
        Mat downImg = Imgcodecs.imread(Application.streamingAssetsPath + "/" + featureImg_Down);

        // 转换为灰度图  
        Mat grayA = new Mat();
        Mat grayUpImg = new Mat();
        Mat grayDownImg = new Mat();
        Imgproc.cvtColor(imgA, grayA, Imgproc.COLOR_BGR2GRAY);
        Imgproc.cvtColor(upImg, grayUpImg, Imgproc.COLOR_BGR2GRAY);
        Imgproc.cvtColor(downImg, grayDownImg, Imgproc.COLOR_BGR2GRAY);

        //下半部分匹配
        // 模板匹配  
        Mat resultDown = new Mat();
        Imgproc.matchTemplate(grayA, grayDownImg, resultDown, Imgproc.TM_CCOEFF_NORMED);

        // 找到最佳匹配位置  
        Core.MinMaxLocResult mmr_d = Core.minMaxLoc(resultDown);
        Point matchLoc_d = mmr_d.maxLoc;

        if (mmr_d.maxVal >= threshold)
        {
            Debug.Log("找到匹配，开始裁剪");

            // 计算裁剪区域
            int y = (int)matchLoc_d.y;
            //Debug.Log("匹配位置: " + y);
            //int cropHeight = imgA.rows() - y;
            //int cropHeight =  y;

            // 定义裁剪区域 (x, y, width, height)  
            OpenCVForUnity.CoreModule.Rect cropArea = new OpenCVForUnity.CoreModule.Rect(0, 0, imgA.cols(), y);
            croppedMat = new Mat(imgA, cropArea);
        }
        else
        {
            Debug.Log("未找到匹配");
        }


        //上半部分匹配
        Mat grayB = new Mat();
        Imgproc.cvtColor(croppedMat, grayB, Imgproc.COLOR_BGR2GRAY);

        Mat resultUp = new Mat();
        Imgproc.matchTemplate(grayB, grayUpImg, resultUp, Imgproc.TM_CCOEFF_NORMED);

        Core.MinMaxLocResult mmr_u = Core.minMaxLoc(resultUp);
        Point matchLoc_u = mmr_u.maxLoc;

        if (mmr_u.maxVal >= threshold)
        {
            // 计算裁剪区域
            int y = (int)matchLoc_u.y;
            //这里计算纵向截取区域，也就是纵向总高 - 匹配位置(匹配位置为左上角) - 匹配的图片高度
            int cropHeight = croppedMat.rows() - y - 55;

            // 定义裁剪区域 (x, y, width, height)  
            //Rect的含义 x为横向从哪开始，y为纵向从哪开始，width为横向截取的宽度，height为纵向截取的高度
            //这里的结果就是，从左边x=0开始，高从识别到的区域最上面-识别图高度开始，截取出识别图宽度，和剩余高度区域
            OpenCVForUnity.CoreModule.Rect cropArea = new OpenCVForUnity.CoreModule.Rect(0, y + 55, croppedMat.cols(), cropHeight);
            croppedMat = new Mat(croppedMat, cropArea);

            //横向截取固定宽度
            int cropWidth = 110;
            //剩余宽度
            int remainWidth = 905;
            OpenCVForUnity.CoreModule.Rect cropArea2 = new OpenCVForUnity.CoreModule.Rect(cropWidth, 0, remainWidth, croppedMat.rows());
            croppedMat = new Mat(croppedMat, cropArea2);



            // 保存结果  
            string outputFullPath = System.IO.Path.Combine(Application.streamingAssetsPath, outputPath);
            Imgcodecs.imwrite(outputFullPath, croppedMat);
            Debug.Log("裁剪完成，保存路径: " + outputFullPath);
        }
    }
}