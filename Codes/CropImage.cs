using UnityEngine;
using System.IO;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.ImgprocModule;

public class CropImage : MonoBehaviour
{
    private string featureImg_Up = "Up.jpg";//上半部分 特征图 的路径
    private string featureImg_Down = "Down.jpg";//下半部分 特征图 的路径
    private string outputImg_Path = "OutPath";//输出图片的路径
    private float threshold = 0.95f;//匹配阈值

    private string[] imgNames;//用于存储所有要转换的图片 的完整路径
    private string folderPath = "Pics";//图片文件夹的路径

    Mat upImg;
    Mat downImg;
    private void Start()
    {
        //读取上下两个用于识别的特征图
        featureImg_Up = Path.Combine(Application.streamingAssetsPath, featureImg_Up);
        featureImg_Down = Path.Combine(Application.streamingAssetsPath, featureImg_Down);
        upImg = Imgcodecs.imread(featureImg_Up);
        downImg = Imgcodecs.imread(featureImg_Down);

        string fullPath = Path.Combine(Application.streamingAssetsPath, folderPath);

        imgNames = Directory.GetFiles(fullPath, "*.JPG");

        foreach (string imgName in imgNames)
        {
            Mat sourceImg = Imgcodecs.imread(imgName);
            CutPicture(sourceImg, Path.GetFileName(imgName));
        }
    }

    private void CutPicture(Mat imgA, string imgName)
    {
        Mat croppedMat = new Mat();

        // 转换为灰度图  
        Mat grayA = new Mat();
        Mat grayUpImg = new Mat();
        Mat grayDownImg = new Mat();
        Imgproc.cvtColor(imgA, grayA, Imgproc.COLOR_BGR2GRAY);
        Imgproc.cvtColor(upImg, grayUpImg, Imgproc.COLOR_BGR2GRAY);
        Imgproc.cvtColor(downImg, grayDownImg, Imgproc.COLOR_BGR2GRAY);

        //下半部分匹配
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
            string outputFullPath = Path.Combine(Application.streamingAssetsPath, outputImg_Path, imgName);
            Imgcodecs.imwrite(outputFullPath, croppedMat);
            Debug.Log("裁剪完成，保存路径: " + outputFullPath);
        }
    }
}