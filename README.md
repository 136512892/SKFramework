<B>SKFramework</B>
* 本框架开发所用环境：Unity 2020.3.16
* 请将SKFramework文件夹放在Assets根目录下使用
---

# Audio
## 🎈一、背景音乐
### 🔸将一个AudioClip资产作为背景音乐进行播放
```c#
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    [SerializeField] private AudioClip combat;

    private void Start()
    {
        Audio.BGM.Play(combat);        
    }
}   
```
### 🔸设置背景音乐是否循环
```c#
Audio.BGM.IsLoop = true;
```
### 🔸设置背景音乐音量
```c#
Audio.BGM.Volume = .3f;
```
### 🔸设置背景音乐是否暂停
```c#
Audio.BGM.IsPaused = true;
```
### 🔸设置背景音乐是否静音
```c#
Audio.BGM.IsMuted = true;
```
## 🎈二、音效
### 🔸将一个AudioClip资产作为音效进行播放
```c#
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        Audio.SFX.Play(clip);
    }
}   
```
### 🔸在三维空间中的指定坐标位置播放音效
```c#
Audio.SFX.Play(clip, transform.position);
```
### 🔸音效跟随物体进行播放
```c#
Audio.SFX.Play(clip, transform);
```
### 🔸音效跟随物体进行播放
```c#
Audio.SFX.Play(clip, transform);
```
### 🔸所有播放音效的重载函数
![](https://bbs-img.huaweicloud.com/blogs/img/20220721/1658388767634964796.png "音效播放重载")
### 🔸设置音效是否静音
```c#
Audio.SFX.IsMuted = true;
```
### 🔸设置音效是否暂停
```c#
Audio.SFX.IsPaused = true;
```
### 🔸停止所有音效播放
```c#
Audio.SFX.Stop();
```
## 🎈三、音频库
### 🔸创建音频库
![](https://bbs-img.huaweicloud.com/blogs/img/20220721/1658389142636343275.png)
### 🔸添加音频数据
将`AudioClip`资产拖拽到`Drop AudioClips Here`区域以添加音频数据
![](https://bbs-img.huaweicloud.com/blogs/img/20220721/1658389265815186081.gif)
为音频数据命名：
![](https://bbs-img.huaweicloud.com/blogs/img/20220721/1658389312733133472.png)
### 🔸加载音频库
```c#
Audio.Database.Load("ClickAudioDatabase", out AudioDatabase clickAudioDatabase);
```
第一个参数传入音频库资产的`Resources`路径
### 🔸卸载音频库
```c#
Audio.Database.Unload("Click");
```
参数传入音频库的`名称`
![](https://bbs-img.huaweicloud.com/blogs/img/20220721/1658389462288508459.png)
### 🔸获取音频库
同样的，参数传入音频库的`名称`
```c#
AudioDatabase database = Audio.Database.Get("Click");
```
### 🔸播放音频库中的音频
音频作为`音效`进行播放
```c#
Audio.Database.Load("ClickAudioDatabase", out AudioDatabase clickAudioDatabase);
clickAudioDatabase.PlayAsSFX("点击音效01");
```
音频作为`背景音乐`进行播放
```c#
Audio.Database.Load("ClickAudioDatabase", out AudioDatabase clickAudioDatabase);
clickAudioDatabase.PlayAsBGM("点击音效01");
```

//TODO:
