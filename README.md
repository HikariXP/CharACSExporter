# CharACSExporter

[Chinese](README.md)
[English](README-EN.md)

Unity的Addressable本身是好用的，对于单机游戏而言。  
当你需要用到它来处理一些线上运营游戏的热更的时候，就会发现很多逻辑在文档上写得不明不白，甚至有些功能也设计得及其抽象。  
比如部分版本升级会导致AddressableSetting出错，UI面板不适配、使用AddressablesContentState.bin检查更新资源后，有更多依赖的资源被纳入"需更新"但是打到了本地（清单中也是本地）......

# 目的
将Addressables用来比较资源变动情况的清单addressablesContentState导出为json格式，用于做进一步的比较操作，比如用WinMerge和上一次的AddressablesContentState文件进行比较。

# 功能
点击Untiy上面的Tools/CharSui/Export AddressableContentState to JSON，依次选择需要反编译的内容、导出的路径

