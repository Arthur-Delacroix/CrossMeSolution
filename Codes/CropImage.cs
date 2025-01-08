using UnityEngine;
using System.IO;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.ImgprocModule;

public class CropImage : MonoBehaviour
{
    public string imageAPath = "a.jpg";
    public string imageBPath = "b.jpg";
    public string outputPath = "z.jpg";
    public float threshold = 0.8f;

    void Start()
    {
        // 加载图片  
        Mat imgA = Imgcodecs.imread(Application.streamingAssetsPath + "/" + imageAPath);
        Mat imgB = Imgcodecs.imread(Application.streamingAssetsPath + "/" + imageBPath);

        // 转换为灰度图  
        Mat grayA = new Mat();
        Mat grayB = new Mat();
        Imgproc.cvtColor(imgA, grayA, Imgproc.COLOR_BGR2GRAY);
        Imgproc.cvtColor(imgB, grayB, Imgproc.COLOR_BGR2GRAY);

        // 模板匹配  
        Mat result = new Mat();
        Imgproc.matchTemplate(grayA, grayB, result, Imgproc.TM_CCOEFF_NORMED);

        // 找到最佳匹配位置  
        Core.MinMaxLocResult mmr = Core.minMaxLoc(result);
        Point matchLoc = mmr.maxLoc;

        if (mmr.maxVal >= threshold)
        {
            Debug.Log("找到匹配，开始裁剪");

            // 计算裁剪区域：从匹配位置向下到底部  
            int y = (int)matchLoc.y;
            int cropHeight = imgA.rows() - y;

            // 定义裁剪区域 (x, y, width, height)  
            OpenCVForUnity.CoreModule.Rect cropArea = new OpenCVForUnity.CoreModule.Rect(0, y, imgA.cols(), cropHeight);
            Mat croppedMat = new Mat(imgA, cropArea);
            //Mat croppedMat = new Mat()

            // 保存结果  
            string outputFullPath = System.IO.Path.Combine(Application.streamingAssetsPath, outputPath);
            Imgcodecs.imwrite(outputFullPath, croppedMat);
            Debug.Log("裁剪完成，保存路径: " + outputFullPath);

            croppedMat.Dispose();
        }
        else
        {
            Debug.Log("未找到匹配");
        }

        // 释放资源  
        imgA.Dispose();
        imgB.Dispose();
        grayA.Dispose();
        grayB.Dispose();
        result.Dispose();
    }
}