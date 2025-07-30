### Crossme 答案图签

此仓库收录CrossMe游戏中所有的答案图片，包括黑白版和彩色版，答案会尽量与Apple AppStore更新的迷题同步

- App Store [下载链接](https://apps.apple.com/us/app/nonograms-crossme/id574857255)
- [个人博客答案页面](https://arthur-delacroix.github.io/tags/Nonogram/)
  - 普通版和彩色版均不会收录第一章的所有迷题，因为太简单了
  - 答案编号与APP内迷题编号对应，方便查询
- B站视频链接
  - 每个视频包含100个答案，每个答案时长5秒
- 欢迎有时间的小伙伴贡献答案

# 其他
- 当前答案图片使用iPhone 11 Pro截图，原始分辨率为1125x2346
- 为了使图片更快加载，每个图片会进行裁剪
- 图片宽度裁剪为911
- 高度会使用图片识别的方式，分别识别上下特征图的位置，然后截取中间部分
- 图片格式为jpg
- 上传到仓库中的图片还会使用工具进行二次压缩
- 图片尺寸会压缩为原来的50%，体积压缩为原来的20%，这样在保证视觉效果不影响的前提下最大减小了图片的体积，格式转换、压缩工具也在仓库中，具体查看Tools.zip压缩包
- 由于本人unity C#比较熟练，所以使用特征图上下裁剪在Unity中进行了实现，具体代码实现可以查看仓库中的CropImage.cs脚本，所用到的特征图也在其中
- unity项目中使用了第三方的[OpenCV for Unity](https://assetstore.unity.com/packages/tools/integration/opencv-for-unity-21088)插件