### Crossme 答案图签

此仓库收录CrossMe游戏中所有的答案图片，包括黑白版和彩色版，答案会尽量与Apple AppStore更新的迷题同步

- [App Store 下载链接](https://apps.apple.com/us/app/nonograms-crossme/id574857255)
- [个人博客答案页面](https://arthur-delacroix.github.io/tags/Nonogram/)
- [B站视频链接](https://www.bilibili.com/video/BV1ww4dzCECg/?spm_id_from=333.1387.homepage.video_card.click&vd_source=5146fa881c829ea55b6954babc75e637)

- 普通版和彩色版均不会收录第一章的所有迷题，因为太简单了
- 答案编号与APP内迷题编号对应，方便查询
- 每个视频包含50个答案，每个答案时长5秒
- 欢迎小伙伴贡献答案

# 其他
- 当前迷题答案图片使用iPhone 11 Pro截图，原始分辨率为1125x2436
- 为了使图片更快加载，每个图片会进行裁剪
- 图片宽度裁剪为 911，高度因为每个迷题答案不同会有所不同
- 高度会使用图片识别的方式，分别识别上下特征图的位置，然后截取中间部分
- 图片格式为jpg
- 上传到仓库中的图片还会使用[图压](https://soft.3dmgame.com/down/294557.html)进行二次压缩
- 压缩时选择 压缩强度，其值调整为7.这样在保证视觉效果不影响的前提下最大减小了图片的体积。
- 由于本人unity C#比较熟练，所以使用特征图上下裁剪在Unity中进行了实现，具体代码实现可以查看仓库中的CropImage.cs脚本，所用到的特征图也在其中
- unity项目中使用了第三方的[OpenCV for Unity](https://assetstore.unity.com/packages/tools/integration/opencv-for-unity-21088)插件
- 制作视频使用unity recorder插件进行录屏制作
  - 图片为每5秒切换一次，切换图片的计时器偷懒使用了dotweenn
  - 在timeline中创建recorder track进行录制 输出参数为 FHD-1080p，16:9(1.7778)，H.264 MP4，High
  - 因为切换图片代码使用了`DOVirtual.DelayedCall`方法，所以在钱5秒回空着，这里就偷懒将recorder track从第4秒开始录制了
  - 因为recorder不能和dotween的计时器完全同步，dotween回慢1秒，所以从第4秒开始录制
  - 在屏幕左上角同时会显示当前迷题的序号