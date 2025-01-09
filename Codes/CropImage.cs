using UnityEngine;
using System.IO;
using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgcodecsModule;
using OpenCVForUnity.ImgprocModule;

public class CropImage : MonoBehaviour
{
    private string featureImg_Up = "Up.jpg";//上半部分 特征图 的路径
    private string featureImg_Down = "Down.jpg";//下半部分 特征图 的路径
    private string outputImg_Path;//输出图片的路径
    private float threshold = 0.95f;//匹配阈值

    public string imageAPath = "a.jpg";//用于被裁剪的图片

    public string outputPath = "z.jpg";


    void Start()
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
        /*
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
                    //OpenCVForUnity.CoreModule.Rect cropArea = new OpenCVForUnity.CoreModule.Rect(0, y, imgA.cols(), cropHeight);
                    croppedMat = new Mat(imgA, cropArea);
                    //Mat croppedMat = new Mat()

                    // 保存结果  
                    //string outputFullPath = System.IO.Path.Combine(Application.streamingAssetsPath, outputPath);
                    //Imgcodecs.imwrite(outputFullPath, croppedMat);
                    //Debug.Log("裁剪完成，保存路径: " + outputFullPath);

                    //croppedMat.Dispose();



                }
                else
                {
                    Debug.Log("未找到匹配");
                }
        */

        //上半部分匹配
        Mat resultUp = new Mat();
        Imgproc.matchTemplate(grayA, grayUpImg, resultUp, Imgproc.TM_CCOEFF_NORMED);

        Core.MinMaxLocResult mmr_u = Core.minMaxLoc(resultUp);
        Point matchLoc_u = mmr_u.maxLoc;

        if (mmr_u.maxVal >= threshold)
        {
            // 计算裁剪区域
            int y = (int)matchLoc_u.y;
            int cropHeight = imgA.rows() - y - 55;

            //OpenCVForUnity.CoreModule.Rect cropArea = new OpenCVForUnity.CoreModule.Rect(0, y - 55, croppedMat.cols(), cropHeight);

            //Mat croppedMat_up = new Mat(croppedMat, cropArea);

            //// 保存结果  
            //string outputFullPath = System.IO.Path.Combine(Application.streamingAssetsPath, outputPath);
            //Imgcodecs.imwrite(outputFullPath, croppedMat_up);
            //Debug.Log("裁剪完成，保存路径: " + outputFullPath);


            // 定义裁剪区域 (x, y, width, height)  
            OpenCVForUnity.CoreModule.Rect cropArea = new OpenCVForUnity.CoreModule.Rect(0, 0, imgA.cols(), y);
            //OpenCVForUnity.CoreModule.Rect cropArea = new OpenCVForUnity.CoreModule.Rect(0, y, imgA.cols(), cropHeight);
            croppedMat = new Mat(imgA, cropArea);
            //Mat croppedMat = new Mat()

            // 保存结果  
            string outputFullPath = System.IO.Path.Combine(Application.streamingAssetsPath, outputPath);
            Imgcodecs.imwrite(outputFullPath, croppedMat);
            Debug.Log("裁剪完成，保存路径: " + outputFullPath);
        }








        // 释放资源  
        imgA.Dispose();
        downImg.Dispose();
        grayA.Dispose();
        grayDownImg.Dispose();
        resultDown.Dispose();
    }
}