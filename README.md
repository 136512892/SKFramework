# Unity SKFramework

SKFramework是基于Unity的一个小型开发框架，模块间相对独立，致力于提高各类项目的开发效率，提供持续维护、升级，若使用过程中有任何疑问或发现任何bug、缺陷，可以联系作者指出。

## 环境

- Unity版本：2020.3.16
- .Net API版本：4.x

## 模块简介

- [Actions]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 事件链模块，包含顺序事件链、并行事件链、时间轴事件链，事件包含普通事件、延时事件、定时事件、条件事件、动画事件等类型。
- [Audio]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 音频管理模块，分为背景音乐管理器、音效管理器、音频库管理器三部分，提供音频的播放、暂停、恢复、停止等接口。
- [Debugger]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 调试器，支持打包后运行程序时日志的查看、Hierarchy层级的查看、部分组件的调试。
- [Event]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 事件模块，包含事件的发布、订阅系统和消息的打包、拆包系统两部分，为代码的解耦提供支持。
- [Extension]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 函数拓展模块，使用this关键字封装了部分类的拓展函数，提供链式编程支持。
- [FSM]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 有限状态机模块，提供了状态基类、状态机基类的封装，为步骤、流程类型内容、角色动画状态、角色AI等内容的处理提供了支持。
- [Log]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 日志模块，支持打印日志的开关控制。
- [ObjectPool]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 对象池模块，分为普通类型对象池和Mono类型对象池两部分，为对象的复用提供支持。
- [Resource]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 资源模块，支持异步加载AssetBundle资源。
- [Timer]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 时间工具模块，提供了多种时间类工具，包括倒计时/定时器、时钟/计时器、闹钟、秒表等等。
- [UI]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - UI模块，集中管理UI视图，提供了视图的加载、显示、隐藏、卸载等接口，并集成了DoTween类型动画的编辑功能。
- [WebRequest]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 网络请求模块，集中管理网络接口，为发起网络请求提供支持。
- [Package Manager]([Unity SKFramework Documentation_CoderZ1010的博客-CSDN博客](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501)) - 开发工具包管理器，类似于官方的Package Manager，提供了作者开发工作中封装的各种小工具的介绍、下载、升级、移除等功能。

## 注意事项

- 1.请将SKFramework放在Assets根目录下使用；

- 2.在框架的Package Manager中下载的工具包不要轻易移动其目录位置，若发生移动，不能再通过Package Manager移除；
