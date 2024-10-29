# Unity SKFramework

SKFramework是基于Unity的一个小型开发框架，致力于提高各类项目的开发效率，提供持续维护、升级，若使用过程中有任何疑问或发现任何bug、缺陷，可以联系作者指出。

## 环境

- Unity版本：2020.3.16
- .Net API版本：4.x

## 模块简介

- [Actions](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501#t6) - 事件链模块，包含顺序事件链、并行事件链、时间轴事件链，事件包含普通事件、延时事件、定时事件、条件事件、动画事件等类型。
- [Audio](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501#t1) - 音频管理模块，分为背景音乐管理器、音效管理器、音频库管理器三部分，提供音频的播放、暂停、恢复、停止等接口。
- [Debugger](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501) - 调试器，支持打包后运行程序时日志的查看、Hierarchy层级的查看、组件的调试。
- [Event](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501#19) - 事件模块，提供事件的发布、订阅，为代码的解耦提供支持。
- [Extension](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501) - 函数拓展模块，使用this关键字封装了部分类的拓展函数，提供链式编程支持。
- [FSM](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501#13) - 有限状态机模块，提供了状态基类、状态机基类的封装，为步骤、流程类型内容、角色动画状态、角色AI等内容的处理提供了支持。
- [Log](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501) - 日志模块。
- [ObjectPool](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501#17) - 对象池模块，为对象的复用提供支持。
- [Resource](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501#31) - 资源模块，支持异步加载资源。
- [UI](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501#34) - UI模块，集中管理UI视图，提供了视图的加载、打开、关闭、卸载等接口。
- [WebRequest](https://blog.csdn.net/qq_42139931/article/details/128849528?spm=1001.2014.3001.5501#35) - 网络请求模块，集中管理网络接口，为发起网络请求提供支持。

## 注意事项

- 请将SKFramework.prefab预制件放入初始场景使用。
