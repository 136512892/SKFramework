# Audio 音频

## 背景音乐

* 播放

```csharp
void Play(AudioClip bgm)
```

> **参数：**
>
> bgm：背景音乐AudioClip资产

* 停止

```csharp
void Stop()
```

* 是否循环

```csharp
bool IsLoop { get; set; }
```

* 音量

```csharp
float Volume { get; set; }
```

* 是否暂停

```csharp
bool IsPaused { get; set; }
```

* 是否静音

```csharp
bool IsMuted { get; set; }
```

* 是否正在播放

```csharp
bool IsPlaying { get; }
```

* 播放进度

```csharp
float Progress { get; }
```

* 暂停

```csharp
void Pause()
```

* 取消暂停

```csharp
void Unpause()
```

> 背景音乐有两种暂停方式：
>
> 1. 将IsPaused属性设为false；
> 2. 调用Pause方法。
>
> 区别：IsPaused设为false时立即暂停背景音乐播放，调用Pause方法是音量通过插值方式逐渐降为0，最终暂停背景音乐播放，有一个渐进的过程。

**调用示例：**

```csharp
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        Main.Audio.BGM.Play(clip);
        Main.Audio.BGM.Stop();
        Main.Audio.BGM.IsLoop = true;
        Main.Audio.BGM.Volume = 0.5f;
        Main.Audio.BGM.IsPaused = false;
        Main.Audio.BGM.IsMuted = false;
        bool isPlaying = Main.Audio.BGM.IsPlaying;
        float progress = Main.Audio.BGM.Progress;
        Main.Audio.BGM.Pause();
        Main.Audio.BGM.Unpause();
    }
}
```

## 音效

* 播放

```csharp
AudioHandler Play(AudioClip clip, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, AudioMixerGroup output, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, Vector3 position, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, Vector3 position, AudioMixerGroup output, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, Transform followTarget, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, Transform followTarget, AudioMixerGroup output, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, float volume, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, float volume, Vector3 position, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, float volume, Transform followTarget, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, float volume, float pitch, Vector3 position, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, float volume, float pitch, Transform followTarget, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, float volume, Vector3 position, float minDistance, float maxDistance, bool autoRecycle = true)
AudioHandler Play(AudioClip clip, float volume, Transform followTarget, float minDistance, float maxDistance, bool autoRecycle = true)
```

> **参数：**
>
> clip：音效AudioClip资产
>
> output：混音组
>
> position：音效播放的位置（3D音效）
>
> followTarget：音效播放跟随的目标（3D音效）
>
> volume：音量大小
>
> minDistance：最小距离（3D音效设置）
>
> maxDistance：最大距离（3D音效设置）
>
> autoRecycle：表示是否自动回收AudioHandler音频处理器（*默认由对象池自动回收音频处理器*）
>
> **返回值：**
>
> AudioHandler音频处理器

* 停止

```csharp
void Stop()
```

* 是否暂停

```csharp
bool IsPaused { get; set; }
```

* 是否静音

```csharp
bool IsMuted { get; set; }
```

**调用示例：**

```csharp
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void Start()
    {
        Main.Audio.SFX.Play(clip, 0.7f, transform.position);
        Main.Audio.SFX.Stop();
        Main.Audio.SFX.IsPaused = false;
        Main.Audio.SFX.IsMuted = false;
    }
}
```

## 音频库

* 编辑音频库

![image-20230131173241705](C:\Users\admin\AppData\Roaming\Typora\typora-user-images\image-20230131173241705.png "编辑音频库")

* 在音频库中获取音频

```csharp
AudioClip FromDatabase(string databaseName, string clipName)
```

> **参数：**
>
> databaseName：音频库名称
>
> clipName：音频名称
>
> **返回值：**
>
> AudioClip音频

**调用示例：**

```csharp
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    private void Start()
    {
        AudioClip clip = Main.Audio.FromDatabase("Click", "click01");
    }
}
```

## Audio Listener

* 设置Listener

```csharp
void SetListener(Transform listenerTrans)
```

> **参数：**
>
> listenerTrans：Audio Listener跟随的物体，仅影响3D音效。
>
> *注：Audio Listener组件设置在框架Prefab中，注意移除Main Camera中的Audio Listener。*

**调用示例：**

```csharp
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    private void Start()
    {
        Main.Audio.SetListener(transform);
    }
}
```

# Actions 事件

## Action 事件类型

* Simple Action：普通事件，可以理解为一个简单的Action回调函数。

* Delay Action：延迟事件，指定一个时长，在经过该时长后执行指定的回调函数。
* Timer Action：定时事件，可以理解为定时器，分为正计时和倒计时，通过参数isReverse指定，事件为float参数类型事件，通过已经计时的时长（正计时）或剩余的时长（倒计时）调用执行。
* Until Action：条件事件，直到条件成立，执行指定的回调函数。
* While Action：条件事件，与Until条件事件不同的是，While条件事件中设置的回调函数在条件成立时一直被调用，当条件不再成立时，事件结束。
* Tween Action：动画事件，依赖DoTween插件，Tween事件表示播放一个DoTween动画，动画播放完后，事件结束。
* Animate Action：动画事件，通过Animator播放动画，需要指定Animator组件和Animator Controller中动画状态State的名称，动画播放完后，事件结束。
* Timeline Action：时间轴事件，指定事件开始的时间节点，和事件执行的时长，需要配合Timeline ActionChain时间轴事件链使用。

> *除上述事件类型外，可以通过继承AbstractAction抽象事件类，重写OnInvoke和OnReset函数来自定义事件。*

## Action Chain 事件链

* 添加事件

```csharp
IActionChain Append(IAction action)
```

> **参数：**
>
> action：要添加的事件
>
> **返回值：**
>
> IActionChain事件链

* 开启

```csharp
IActionChain Begin()
```

* 终止

```csharp
void Stop()
```

* 暂停

```csharp
void Pause()
```

* 恢复

```csharp
void Resume()
```

* 是否暂停

```csharp
bool IsPaused { get; }
```

* 设置终止条件

```csharp
IActionChain StopWhen(Func<bool> predicate)
```

> **参数：**
>
> predicate：终止事件链的条件，该条件成立时事件链会终止。
>
> **返回值：**
>
> IActionChain事件链

* 设置回调函数

```csharp
IActionChain OnStop(UnityAction action)
```

> **参数：**
>
> action：事件链终止时执行的回调函数
>
> **返回值：**
>
> IActionChain事件链

* 设置事件链循环次数

```csharp
IActionChain SetLoops(int loops)
```

> **参数：**
>
> loops：事件链循环执行的次数，默认为1，若要一直执行可以将其设为-1，
>
> **返回值：**
>
> IActionChain事件链



事件链的执行依赖于携程，通过this获取事件链表示以当前的MonoBehaviour开启携程，也可以通过模块管理器开启协程，如下所示：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.Actions;

public class Example : MonoBehaviour
{
    private void Start()
    {
        //通过当前MonoBehaviour执行事件链
        IActionChain chain1 = Main.Actions.Sequence(this);
        //通过模块管理器执行事件链
        IActionChain chain2 = Main.Actions.Sequence();
    }
}
```

### Sequence 序列事件链

序列事件链中的事件是依次执行的，即只有上一个事件执行结束后，才会开始执行下一个事件。示例如下，事件链中包含三个事件，Event指的是Simple普通事件，该事件链首先打印日志Begin，第一个事件结束，第二个事件开始执行，为延迟事件，在经历3秒的延迟后，第二个事件结束，第三个事件开始执行，打印日志"3f"。

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.Actions;

public class Example : MonoBehaviour
{
    private void Start()
    {
        Main.Actions.Sequence()
            .Event(() => Debug.Log("Begin"))
            .Delay(3f)
            .Event(() => Debug.Log("3f"))
            .Begin();
    }
}
```

![img](https://img-blog.csdnimg.cn/1a055fdbeba344eb8c416d7f8207bebd.gif)

其他一些事件的调用示例如下：

```csharp
using UnityEngine;
using DG.Tweening;
using SK.Framework;
using SK.Framework.Actions;

public class Example : MonoBehaviour
{
    private void Start()
    {
        Main.Actions.Sequence()
            //普通事件
            .Event(() => Debug.Log("Begin"))
            //延迟2秒
            .Delay(2f)
            //普通事件
            .Event(() => Debug.Log("2f"))
            //直到按下键盘A键
            .Until(() => Input.GetKeyDown(KeyCode.A))
            //普通事件
            .Event(() => Debug.Log("A Pressed."))
            //DoTween动画事件
            .Tween(() => transform.DOMove(new Vector3(0f, 0f, 1f), 2f))
            //定时事件
            .Timer(3f, false, s => Debug.Log(s))
            //开始执行事件链
            .Begin()
            //设置回调函数
            .OnStop(() => Debug.Log("Completed"));
    }
}
```

### Concurrent 并发事件链

并发事件链中的事件是并发执行的，在事件链启动时同时开启执行，在所有的事件都执行完成后，事件链终止。示例如下：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.Actions;

public class Example : MonoBehaviour
{
    private void Start()
    {
        Main.Actions.Concurrent()
            .Event(() => Debug.Log("Begin"))
            .Delay(1f, () => Debug.Log("1f"))
            .Delay(2f, () => Debug.Log("2f"))
            .Delay(3f, () => Debug.Log("2f"))
            .Until(() => Input.GetKeyDown(KeyCode.A))
            .Begin()
            .OnStop(() => Debug.Log("Completed"));
    }
}
```

![img](https://img-blog.csdnimg.cn/893cd9dc65ce422292a7f4dc497de13d.gif)

### Timeline 时间轴事件链

* 当前执行的时间节点

```csharp
float CurrentTime { get; set; }
```

* 执行的速度

```csharp
float Speed { get; set; }
```

调用示例：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.Actions;

public class Example : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject sphere;

    private TimelineActionChain timeline;

    private void Start()
    {
        timeline = Main.Actions.Timeline()
            //通过Append添加时间轴事件
            //第一个参数表示该事件开始的时间节点
            //第二个参数表示该事件的时长
            .Append(0f, 5f, s => cube.transform.position = Vector3.Lerp(Vector3.zero, new Vector3(0, 0, 5f), s))
            .Append(2f, 4f, s => sphere.transform.position = Vector3.Lerp(Vector3.zero, Vector3.up * 2f, s))
            .Begin() as TimelineActionChain;

        //2倍速
        timeline.Speed = 2f;
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("时间轴");
        //通过Slider调整CurrentTime 实现从指定的时间节点执行
        timeline.CurrentTime = GUILayout.HorizontalSlider(timeline.CurrentTime, 0f, 6f, GUILayout.Width(300f), GUILayout.Height(50f));
        GUILayout.EndHorizontal();
    }
}
```

![img](https://img-blog.csdnimg.cn/2e45c2c109d44eda94e454e803384986.gif)

### 事件链嵌套

事件链支持相互嵌套，示例如下：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.Actions;

public class Example : MonoBehaviour
{
    private void Start()
    {
        Main.Actions.Sequence()
            .Event(() => Debug.Log("Begin"))
            //嵌套一个并发事件链
            .Append(new ConcurrentActionChain()
                .Delay(1f, () => Debug.Log("1f"))
                .Delay(2f, () => Debug.Log("2f"))
                .Delay(3f, () => Debug.Log("3f"))
                as IAction)
            //并发事件链执行完成后 继续执行序列事件链
            .Until(() => Input.GetKeyDown(KeyCode.A))
            .Event(() => Debug.Log("A Pressed."))
            .Timer(3f, false, s => Debug.Log(s))
            .Begin()
            .OnStop(() => Debug.Log("Completed."));
    }
}
```

![img](https://img-blog.csdnimg.cn/04d26236876a41d4b2e5413cccd3919d.gif)



# FSM 有限状态机

* 创建状态机

```csharp
T Create<T>(string stateMachineName) where T : StateMachine, new()
```

> **参数：**
>
> stateMachineName：状态机命名
>
> **返回值：**
>
> StateMachine状态机

* 销毁状态机

```csharp
bool Destroy(string stateMachineName)
```

> **参数：**
>
> stateMachineName：要销毁的目标状态机的名称
>
> **返回值：**
>
> true：成功销毁
>
> false：目标状态机不存在，销毁失败

* 获取状态机

```csharp
T GetMachine<T>(string stateMachineName) where T : StateMachine
```

> **参数：**
>
> stateMachineName：获取的目标状态机的名称
>
> **返回值：**
>
> StateMachine状态机

## State 状态

* 名称

```csharp
string Name { get; set; }
```

* 是否可切换至自身（默认为false）

```csharp
bool CanSwitch2Self { get; set; }
```

* 所属状态机

```csharp
StateMachine Machine { get; }
```

* 设置切换条件

```csharp
void SwitchWhen(Func<bool> predicate, string targetStateName)
```

> **参数：**
>
> predicate：切换条件，条件成立时切换至目标状态
>
> targetStateName：目标状态名称

* 初始化事件

```csharp
virtual void OnInitialization()
```

* 进入事件

```csharp
virtual void OnEnter()
```

* 停留事件

```csharp
virtual void OnStay()
```

* 退出事件

```csharp
virtual void OnExit()
```

* 终止事件

```csharp
virtual void OnTermination()
```



## State Machine 状态机

* 名称

```csharp
string Name { get; }
```

* 当前状态

```csharp
State CurrentState { get; }
```

* 添加状态

```csharp
int Add(State state)
```

> **参数：**
>
> state：添加的状态
>
> **返回值：**
>
> 0：添加成功
>
> -1：状态已存在，无需重复添加
>
> -2：存在同名状态，添加失败

* 移除状态

```csharp
bool Remove(string stateName)
```

> **参数：**
>
> stateName：要移除的状态的名称
>
> **返回值：**
>
> true：移除成功
>
> false：状态不存在，移除失败

* 切换状态

```csharp
int Switch(string stateName)
```

> **参数：**
>
> stateName：要切换的目标状态的名称
>
> **返回值：**
>
> 0：切换成功
>
> -1：状态不存在，切换失败
>
> -2：当前状态已经是目标状态，并且改状态不可切换至自身，切换失败

* 切换至下一状态

```csharp
bool Switch2Next()
```

> **返回值：**
>
> true：切换成功
>
> false：状态机中不存在任何状态，切换失败

* 切换至上一状态

```csharp
bool Switch2Last()
```

> **返回值：**
>
> true：切换成功
>
> false：状态机中不存在任何状态，切换失败

* 切换至空状态（退出当前状态）

```csharp
void Switch2Null()
```

* 获取状态

```csharp
T GetState<T>(string stateName) where T : State
```

> **参数：**
>
> stateName：状态名称
>
> **返回值：**
>
> State状态

* 设置状态切换条件

```csharp
StateMachine SwitchWhen(Func<bool> predicate, string targetStateName)
```

> **参数：**
>
> predicate：切换条件，当条件成立时从当前状态切换至目标状态
>
> targetStateName：目标状态名称
>
> **返回值：**
>
> StateMachine状态机

* 设置状态切换条件

```csharp
StateMachine SwitchWhen(Func<bool> predicate, string sourceStateName, string targetStateName)
```

> **参数：**
>
> predicate：切换条件，当条件成立且当前状态是源状态时切换至目标状态
>
> sourceStateName：源状态名称
>
> targetStateName：目标状态名称
>
> **返回值：**
>
> StateMachine状态机

* 构建状态

```csharp
StateBuilder<T> Build<T>(string stateName = null) where T : State, new()
```

> **参数：**
>
> stateName：状态命名
>
> **返回值：**
>
> StateBuilder状态构建器

## State Builder 状态构建器

* 设置状态初始化事件

```csharp
StateBuilder<T> OnInitialization(Action<T> onInitialization)
```

* 设置状态进入事件

```csharp
StateBuilder<T> OnEnter(Action<T> onEnter)
```

* 设置状态停留事件

```csharp
StateBuilder<T> OnStay(Action<T> onStay)
```

* 设置状态退出事件

```csharp
StateBuilder<T> OnExit(Action<T> onExit)
```

* 设置状态终止事件

```csharp
StateBuilder<T> OnTermination(Action<T> onTermination)
```

* 设置状态切换条件

```csharp
StateBuilder<T> SwitchWhen(Func<bool> predicate, string targetStateName)
```

* 构建完成

```csharp
StateMachine Complete()
```



调用示例一：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.FSM;

public class Example : MonoBehaviour
{
    public class ExampleState : State
    {
        public override void OnInitialization()
        {
            Debug.Log(string.Format("{0}状态初始化", Name));
        }
        public override void OnEnter()
        {
            Debug.Log(string.Format("{0}状态进入", Name));
        }
        public override void OnStay()
        {
            Debug.Log(string.Format("{0}状态停留", Name));
        }
        public override void OnExit()
        {
            Debug.Log(string.Format("{0}状态退出", Name));
        }
        public override void OnTermination()
        {
            Debug.Log(string.Format("{0}状态终止", Name));
        }
    }

    private void Start()
    {
        var machine = Main.FSM.Create<StateMachine>("示例状态机");
        machine.Add(new ExampleState() { Name = "State1" });
        machine.Add<State>("State2");
        machine.Add<State>("State3");
        machine.Switch("State2");
        machine.Switch2Next();
        machine.Switch2Last();
        machine.Switch2Null();
        machine.SwitchWhen(() => Input.GetKeyDown(KeyCode.Alpha1), "State1");
        machine.SwitchWhen(() => Input.GetKeyDown(KeyCode.Alpha2), "State2");
        machine.SwitchWhen(() => Input.GetKeyDown(KeyCode.Alpha3), "State3");
    }
}
```

调用示例二：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.FSM;

public class Example : MonoBehaviour
{
    public class TestState : State
    {
        public string stringValue;
    }

    private void Start()
    {
        //创建状态机
        var machine = Main.FSM.Create<StateMachine>("示例状态机")
            //构建状态一
            .Build<TestState>("状态一")
                //设置状态一初始化事件
                .OnInitialization(state => state.stringValue = "A")
                //设置状态一进入事件
                .OnEnter(state => Debug.Log("进入状态一"))
                //设置状态一停留事件
                .OnStay(state => Debug.Log("状态一"))
                //设置状态一推出事件
                .OnExit(state => Debug.Log("退出状态一"))
                //设置状态一销毁事件
                .OnTermination(state => state.stringValue = null)
            //状态一构建完成
            .Complete()
            //构建状态二
            .Build<State>("状态二")
                //设置状态二进入事件
                .OnEnter(state => Debug.Log("进入状态二"))
                //设置状态二停留事件
                .OnStay(state => Debug.Log("状态二"))
                //设置状态二退出事件
                .OnExit((state => Debug.Log("退出状态二")))
            //状态二构建完成
            .Complete()
            //构建状态三
            .Build<State>("状态三")
                //设置状态三进入事件
                .OnEnter(state => Debug.Log("进入状态三"))
                //设置状态三停留事件
                .OnStay(state => Debug.Log("状态三"))
                //设置状态三退出事件
                .OnExit((state => Debug.Log("退出状态三")))
            //状态三构建完成
            .Complete()
            //添加状态切换条件 当按下快捷键1时 切换至状态一
            .SwitchWhen(() => Input.GetKeyDown(KeyCode.Alpha1), "状态一")
            //添加状态切换条件 当按下快捷键2时 切换至状态二
            .SwitchWhen(() => Input.GetKeyDown(KeyCode.Alpha2), "状态二")
            //添加状态切换条件 当按下快捷键3时 切换至状态三
            .SwitchWhen(() => Input.GetKeyDown(KeyCode.Alpha3), "状态三")
            //为状态一至状态二添加切换条件：若当前状态为状态一时 按下快捷键4 切换至状态二
            .SwitchWhen(() => Input.GetKeyDown(KeyCode.Alpha4), "状态一", "状态二");

        //切换到指定状态
        machine.Switch("状态一");
        //切换到下一状态
        machine.Switch2Next();
        //切换到上一状态
        machine.Switch2Last();
    }
}
```



# ObjectPool 对象池

* 当前缓存的数量

```csharp
int CurrentCacheCount { get; }
```

* 最大可缓存的数量

```csharp
int MaxCacheCount { get; set; }
```

* 分配对象

```csharp
T Allocate()
```

* 回收对象

```csharp
bool Recycle(T t)
```

* 释放对象池

```csharp
void Release()
```

* 设置创建方法（Mono类型对象池）

```csharp
void CreateBy(Func<T> createMethod)
```

## IPoolable 接口

> 为需要实现对象池管理的对象类继承IPoolable接口

* 是否已经回收

```csharp
bool IsRecycled { get; set; }
```

* 回收事件

```csharp
void OnRecycled()
```



调用示例一：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.ObjectPool;

public class Example : MonoBehaviour
{
    public class Person : IPoolable
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public float Weight { get; set; }

        public bool IsRecycled { get; set; }
        public void OnRecycled()
        {
            Name = null;
            Age = 0;
            Weight = 0f;
        }
    }

    private void Start()
    {
        //分配对象
        Person person = Main.ObjectPool.Allocate<Person>();
        person.Name = "CoderZ";
        person.Age = 30;
        person.Weight = 66f;
        //设置对象池的最大缓存数量
        Main.ObjectPool.SetMaxCacheCount<Person>(100);
        //对象池中当前缓存的数量
        int count = Main.ObjectPool.GetCurrentCacheCount<Person>();
        //回收对象
        Main.ObjectPool.Recycle(person);
        //释放对象池
        Main.ObjectPool.Release<Person>();
    }
}
```

调用示例二：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.ObjectPool;

public class Bullet : MonoBehaviour, IPoolable
{
    public bool IsRecycled { get; set; }

    public void OnRecycled()
    {
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}

public class Example : MonoBehaviour
{
    //子弹预制体
    [SerializeField] private GameObject bulletPrefab;

    private void Start()
    {
        //设置创建方法
        Main.ObjectPool.Mono.CreateBy(() =>
        {
            var instance = Instantiate(bulletPrefab);
            instance.transform.SetParent(transform);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.SetActive(true);
            Bullet bullet = instance.GetComponent<Bullet>();
            return bullet;
        });
        //分配对象
        Bullet bullet = Main.ObjectPool.Mono.Allocate<Bullet>();
        //设置对象池的最大缓存数量
        Main.ObjectPool.Mono.SetMaxCacheCount<Bullet>(100);
        //对象池中当前缓存的数量
        int count = Main.ObjectPool.Mono.GetCurrentCacheCount<Bullet>();
        //回收对象
        Main.ObjectPool.Mono.Recycle(bullet);
        //释放对象池
        Main.ObjectPool.Mono.Release<Bullet>();
    }
}
```



# Event 事件

## 发布

```csharp
void Publish(int eventId)
void Publish<T>(int eventId, T arg)
void Publish<T1, T2>(int eventId, T1 arg1, T2 arg2)
void Publish<T1, T2, T3>(int eventId, T1 arg1, T2 arg2, T3 arg3)
void Publish<T1, T2, T3, T4>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
void Publish<T1, T2, T3, T4, T5>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
void Publish<T1, T2, T3, T4, T5, T6>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
void Publish<T1, T2, T3, T4, T5, T6, T7>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
void Publish<T1, T2, T3, T4, T5, T6, T7, T8>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
void Publish<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
void Publish<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int eventId, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
```

> **参数：**
>
> eventId：事件ID
>
> arg1 ... arg10：事件参数

## 订阅

```csharp
void Subscribe(int eventId, Action callback)
void Subscribe<T>(int eventId, Action<T> callback)
void Subscribe<T1, T2>(int eventId, Action<T1, T2> callback)
void Subscribe<T1, T2, T3>(int eventId, Action<T1, T2, T3> callback)
void Subscribe<T1, T2, T3, T4>(int eventId, Action<T1, T2, T3, T4> callback)
void Subscribe<T1, T2, T3, T4, T5>(int eventId, Action<T1, T2, T3, T4, T5> callback)
void Subscribe<T1, T2, T3, T4, T5, T6>(int eventId, Action<T1, T2, T3, T4, T5, T6> callback)
void Subscribe<T1, T2, T3, T4, T5, T6, T7>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7> callback)
void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
void Subscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
```

> **参数：**
>
> eventId：事件ID
>
> callback：回调函数

## 取消订阅

```csharp
bool Unsubscribe(int eventId, Action callback)
bool Unsubscribe<T>(int eventId, Action<T> callback)
bool Unsubscribe<T1, T2>(int eventId, Action<T1, T2> callback)
bool Unsubscribe<T1, T2, T3>(int eventId, Action<T1, T2, T3> callback)
bool Unsubscribe<T1, T2, T3, T4>(int eventId, Action<T1, T2, T3, T4> callback)
bool Unsubscribe<T1, T2, T3, T4, T5>(int eventId, Action<T1, T2, T3, T4, T5> callback)
bool Unsubscribe<T1, T2, T3, T4, T5, T6>(int eventId, Action<T1, T2, T3, T4, T5, T6> callback)
bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7> callback)
bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8> callback)
bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> callback)
bool Unsubscribe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(int eventId, Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> callback)
```

> **参数：**
>
> eventId：事件ID
>
> callback：回调函数

调用示例：

```csharp
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    public class AttackEvent
    {
        public static readonly int EventID = typeof(AttackEvent).GetHashCode();

        public int power;

        public AttackEvent(int power)
        {
            this.power = power;
        }
    }

    private void Start()
    {
        //订阅
        Main.Events.Subscribe<GameObject, AttackEvent>(AttackEvent.EventID, OnAttackEvent);
        //发布
        Main.Events.Publish(AttackEvent.EventID, gameObject, new AttackEvent(50));
        //取消订阅
        Main.Events.Unsubscribe<GameObject, AttackEvent>(AttackEvent.EventID, OnAttackEvent);
    }

    private void OnAttackEvent(GameObject attacker, AttackEvent ae)
    {
        Debug.Log(string.Format("Attacker:{0}, Attack Power:{1}", attacker.name, ae.power));
    }
}
```



# Timer 时间类工具

## ITimer 接口

> Timer模块实现了一系列计时工具，包括定时器（倒计时）、计时器、秒表、闹钟等，它们均继承自ITimer接口

* 是否已经完成

```csharp
bool IsCompleted { get; }
```

* 是否暂停

```csharp
bool IsPaused { get; }
```

* 启动

```csharp
void Launch()
```

* 暂停

```csharp
void Pause()
```

* 恢复

```csharp
void Resume()
```

* 终止

```csharp
void Stop()
```



计时类工具的运行依赖于协程，通过this获取定时器表示使用当前的MonoBehaviour开启协程，也可以通过模块管理器开启协程。示例如下：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.Timer;

public class Example : MonoBehaviour
{
    private void Start()
    {
        //通过当前Monobehaviour开启协程运行计时器
        Countdown countdown1 = Main.Timer.Countdown(this, 5f);
        //通过模块管理器开启协程运行计时器
        Countdown countdown2 = Main.Timer.Countdown(5f);
    }
}
```

## Countdown 倒计时

* 设置启动事件

```csharp
Countdown OnLaunch(UnityAction onLaunch)
```

* 设置运行事件

```csharp
Countdown OnExecute(UnityAction<float> onExecute)
```

* 设置暂停事件

```csharp
Countdown OnPause(UnityAction onPause)
```

* 设置恢复事件

```csharp
Countdown OnResume(UnityAction onResume)
```

* 设置终止事件

```csharp
Countdown OnStop(UnityAction onStop)
```

* 设置终止条件

```csharp
Countdown StopWhen(Func<bool> predicate)
```

调用示例：

```csharp
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    private void Start()
    {
        Main.Timer.Countdown(this, 5f)
            .OnLaunch(() => Debug.Log("定时器启动"))
            .OnExecute(s => Debug.Log(string.Format("剩余时间{0}", s)))
            .OnPause(() => Debug.Log("定时器暂停"))
            .OnResume(() => Debug.Log("定时器恢复"))
            .OnStop(() => Debug.Log("定时器终止"))
            .StopWhen(() => Input.GetKeyDown(KeyCode.A))
            .Launch();
    }
}
```

## Clock 计时器

* 设置启动事件

```csharp
Clock OnLaunch(UnityAction onLaunch)
```

* 设置运行事件

```csharp
Clock OnExecute(UnityAction<float> onExecute)
```

* 设置暂停事件

```csharp
Clock OnPause(UnityAction onPause)
```

* 设置恢复事件

```csharp
Clock OnResume(UnityAction onResume)
```

* 设置终止事件

```csharp
Clock OnStop(UnityAction onStop)
```

* 设置终止条件

```csharp
Clock StopWhen(Func<bool> predicate)
```

调用示例：

```csharp
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    private void Start()
    {
        Main.Timer.Clock(this)
            .OnLaunch(() => Debug.Log("计时启动"))
            .OnExecute(s => Debug.Log(string.Format("已计时长{0}", s)))
            .OnPause(() => Debug.Log("计时暂停"))
            .OnResume(() => Debug.Log("计时恢复"))
            .OnStop(() => Debug.Log("计时终止"))
            .StopWhen(() => Input.GetKeyDown(KeyCode.A))
            .Launch();
    }
}
```

## Chronometer 秒表

* 设置启动事件

```csharp
Chronometer OnLaunch(UnityAction onLaunch)
```

* 设置运行事件

```csharp
Chronometer OnExecute(UnityAction<float> onExecute)
```

* 设置暂停事件

```csharp
Chronometer OnPause(UnityAction onPause)
```

* 设置恢复事件

```csharp
Chronometer OnResume(UnityAction onResume)
```

* 设置终止事件

```csharp
Chronometer OnStop(UnityAction onStop)
```

* 设置终止条件

```csharp
Chronometer StopWhen(Func<bool> predicate)
```

* 记录

```csharp
void Shot(object context)
```

* 设置记录条件

```csharp
Chronometer ShotWhen(Func<bool> predicate)
```

* 记录集合

```csharp
ReadOnlyCollection<Record> Records { get; }
```

调用示例：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.Timer;

public class Example : MonoBehaviour
{
    private Chronometer chronometer;

    private void Start()
    {
        chronometer = Main.Timer.Chronometer(this, true);
        chronometer.Launch();
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Shot", GUILayout.Width(200f), GUILayout.Height(50f)))
        {
            chronometer.Shot();
        }
        if (GUILayout.Button("Log", GUILayout.Width(200f), GUILayout.Height(50f)))
        {
            var records = chronometer.Records;
            for (int i = 0; i < records.Count; i++)
            {
                Debug.Log(string.Format("No.{0}: {1}", i + 1, records[i].time));
            }
        }
    }
}
```

## Alarm 闹钟

* 设置终止事件

```csharp
Alarm OnStop(UnityAction onStop)
```

调用示例：

```csharp
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    private void Start()
    {
        //10点30分唤醒闹钟
        Main.Timer.Alarm(this, 10, 30, 0, () => Debug.Log("唤醒闹钟")).Launch();    
    }
}
```

## EverySeconds

* 设置启动事件

```csharp
EverySeconds OnLaunch(UnityAction onLaunch)
```

* 设置运行事件

```csharp
EverySeconds OnExecute(UnityAction<float> onExecute)
```

* 设置暂停事件

```csharp
EverySeconds OnPause(UnityAction onPause)
```

* 设置恢复事件

```csharp
EverySeconds OnResume(UnityAction onResume)
```

* 设置终止事件

```csharp
EverySeconds OnStop(UnityAction onStop)
```

* 设置终止条件

```csharp
EverySeconds StopWhen(Func<bool> predicate)
```

调用示例：

```csharp
using System;
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    private void Start()
    {
        //每秒执行一次事件
        Main.Timer.EverySecond(this, () => Debug.Log(DateTime.Now)).Launch();
        //每隔2.5秒执行一次事件
        Main.Timer.EverySeconds(this, 2.5f, () => Debug.Log(DateTime.Now)).Launch();    
    }
}
```

## EveryFrames

* 设置启动事件

```csharp
EverySeconds OnLaunch(UnityAction onLaunch)
```

* 设置运行事件

```csharp
EverySeconds OnExecute(UnityAction onExecute)
```

* 设置暂停事件

```csharp
EverySeconds OnPause(UnityAction onPause)
```

* 设置恢复事件

```csharp
EverySeconds OnResume(UnityAction onResume)
```

* 设置终止事件

```csharp
EverySeconds OnStop(UnityAction onStop)
```

* 设置终止条件

```csharp
EverySeconds StopWhen(Func<bool> predicate)
```

调用示例：

```csharp
using UnityEngine;
using SK.Framework;

public class Example : MonoBehaviour
{
    private void Start()
    {
        //每帧执行一次事件
        Main.Timer.EveryFrame(this, () => Debug.Log(Time.frameCount)).Launch();
        //每隔10帧执行一次事件
        Main.Timer.EveryFrames(this, 10, () => Debug.Log(Time.frameCount)).Launch();    
        //下一帧执行事件
        Main.Timer.NextFrame(this, () => Debug.Log(Time.frameCount)).Launch();
    }
}
```





# Resource 资源

![image-20230202131130583](C:\Users\admin\AppData\Roaming\Typora\typora-user-images\image-20230202131130583.png)

`IsEditorMode`：是否为编辑器模式

`AssetBundleUrl`；AssetBundle资源包所在路径

`AssetBundleManifestName`：manifest文件名称



## 加载资产

```csharp
void LoadAssetAsync<T>(string assetPath, Action<float> onLoading, Action<bool, T> onCompleted)
void LoadAssetAsync<T>(MonoBehaviour executer, string assetPath, Action<float> onLoading, Action<bool, T> onCompleted)
```

> **参数：**
>
> executer：开启加载协程的Monobehaviour
>
> assetPath：资产路径
>
> onLoading：加载中回调
>
> onCompleted：加载完成回调
>
> **返回值：**
>
> Coroutine加载协程

## 卸载资产

```csharp
void UnloadAsset(AssetInfo assetInfo, bool unloadAllLoadedObjects)
```

> **参数：**
>
> assetInfo：资产信息
>
> unloadAllLoadedObjects：是否卸载其所有实例，默认为false

卸载所有资产

```csharp
UnloadAllAsset(bool unloadAllLoadedObjects)
```

## 加载场景

```csharp
void LoadSceneAsync(string sceneAssetPath, Action<float> onLoading, Action onCompleted)
void LoadSceneAsync(MonoBehaviour executer, string sceneAssetPath, Action<float> onLoading, Action onCompleted)
```

> **参数：**
>
> executer：开启加载协程的Monobehaviour
>
> sceneAssetPath：场景资源路径
>
> onLoading：加载中回调
>
> onCompleted：加载完成回调
>
> **返回值：**
>
> Coroutine加载协程

## 卸载场景

```csharp
bool UnloadScene(SceneInfo sceneInfo)
```

> **参数：**
>
> sceneInfo：场景信息
>
> **返回值：**
>
> true：卸载成功
>
> false：未加载该场景，卸载失败

调用示例：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.Resource;

public class Example : MonoBehaviour
{
    private void Start()
    {
        //异步加载资产
        Main.Resource.LoadAssetAsync<AudioClip>("Assets/click01.mp3", onCompleted: (success, clip) =>
        {
            if (success)
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.clip = clip;
                source.Play();
            }
        });
        //异步加载场景
        Main.Resource.LoadSceneAsync("Assets/Scenes/Example.unity");
    }
}
```

# UI 模块

## 加载视图

```csharp
int LoadView(string viewName, string viewResourcePath, ViewLevel level, out IUIView view, IViewData data, bool instant)
```

> **参数：**
>
> viewName：视图命名
>
> viewResourcePath：视图Prefab在Resources文件夹中的路径
>
> level：视图层级
>
> view：加载的视图
>
> data：视图数据
>
> instant：是否立刻显示（不播放视图动画过程），默认为false
>
> **返回值：**
>
> 0：加载成功
>
> -1：视图已存在，无需重复加载
>
> -2：加载失败，请检查资源路径
>
> *注：该接口通过Resources.Load方式加载视图资源*

```csharp
T LoadView<T>(string viewName, string viewResourcePath, ViewLevel level, IViewData data, bool instant) where T : UIView
```

> **参数：**
>
> viewName：视图命名
>
> viewResourcePath：视图Prefab在Resources文件夹中的路径
>
> level：视图层级
>
> data：视图数据
>
> instant：是否立刻显示（不播放视图动画过程），默认为false
>
> **返回值：**
>
> UIView视图
>
> *注：该接口通过Resources.Load方式加载视图资源*

```csharp
T LoadView<T>(ViewLevel level, IViewData data, bool instant) where T : UIView
```

> **参数：**
>
> level：视图层级
>
> data：视图数据
>
> instant：是否立刻显示（不播放视图动画过程），默认为false
>
> **返回值：**
>
> UIView视图
>
> *注：该接口省略了viewName和viewResourcePath参数，表示该视图Prefab以typeof(T).Name命名且直接放在Resources文件夹下*

异步加载视图：

```csharp
void LoadViewAsync<T>(string assetPath, ViewLevel level, IViewData data, bool instant, Action<float> onLoading, Action<bool, T> onCompleted) where T : UIView
void LoadViewAsync<T>(string viewName, string assetPath, ViewLevel level, IViewData data, bool instant, Action<float> onLoading, Action<bool, T> onCompleted) where T : UIView
```

> **参数：**
>
> viewName：视图命名
>
> assetPath：视图资产路径
>
> level：视图层级
>
> data：视图数据
>
> instant：是否立刻显示（不播放视图动画过程），默认为false
>
> onLoading：加载中回调
>
> onCompleted：加载完成回调

## 显示视图

```csharp
IUIView ShowView(string viewName, IViewData data, bool instant)
```

> **参数：**
>
> viewName：视图名称
>
> data：视图数据
>
> instant：是否立刻显示（不播放视图动画过程），默认为false
>
> **返回值：**
>
> UIView视图

```csharp
T ShowView<T>(IViewData data, bool instant) where T : UIView
```

> **参数：**
>
> data：视图数据
>
> instant：是否立刻显示（不播放视图动画过程），默认为false
>
> **返回值：**
>
> UIView视图
>
> *注：该接口省略了viewName参数，表示显示名称为typeof(T).Name的视图*

## 隐藏视图

```csharp
IUIView HideView(string viewName, bool instant)
```

> **参数：**
>
> viewName：视图名称
>
> instant：是否立刻隐藏（不播放视图动画过程），默认为false
>
> **返回值：**
>
> UIView视图

```csharp
T HideView<T>(bool instant) where T : UIView
```

> **参数：**
>
> instant：是否立刻隐藏（不播放视图动画过程），默认为false
>
> **返回值：**
>
> UIView视图
>
> *注：该接口省略了viewName参数，表示隐藏名称为typeof(T).Name的视图*

## 卸载视图

```csharp
bool UnloadView(string viewName, bool instant)
```

> **参数：**
>
> viewName：视图名称
>
> instant：是否立刻卸载（不播放视图动画过程），默认为false
>
> **返回值：**
>
> true：卸载成功
>
> false：不存在该视图，卸载失败

```csharp
bool UnloadView<T>(bool instant) where T : UIView
```

> **参数：**
>
> instant：是否立刻卸载（不播放视图动画过程），默认为false
>
> **返回值：**
>
> true：卸载成功
>
> false：不存在该视图，卸载失败
>
> *注：该接口省略了viewName参数，表示卸载名称为typeof(T).Name的视图*

卸载所有视图：

```csharp
void UnloadAll()
```

## 获取视图

```csharp
IUIView GetView(string viewName)
```

> **参数：**
>
> viewName：视图名称
>
> **返回值：**
>
> UIView视图

```csharp
T GetView<T>() where T : UIView
```

>*注：该接口省略了viewName参数，表示获取名称为typeof(T).Name的视图*



调用示例：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.UI;

public class Example : MonoBehaviour
{
    private void Start()
    {
        Main.UI.LoadView<ExampleView>(ViewLevel.COMMON);

        Main.UI.HideView<ExampleView>();

        Main.UI.ShowView<ExampleView>();

        Main.UI.UnloadView<ExampleView>();
    }
}
```



# WebRequest 网络请求

* 发起网络请求

```csharp
void Send(string url, WebRequestData data, Action<DownloadHandler> onSuccess, Action<string> onFailure = null, MonoBehaviour sender = null)
```

> **参数：**
>
> url：网络请求地址
>
> data：网络请求数据
>
> onSuccess：请求成功回调函数
>
> onFailure：请求失败回调函数
>
> sender：网络请求的发起依赖于协程，sender表示开启协程的Monobehaviour，传null表示通过模块管理器开启协程

## WebRequest Data

* 请求方式（GET/POST）

```csharp
WebRequestType RequestType { get; }
```

* 表单

```csharp
WWWForm WWWForm { get; }
```

* 请求头

```csharp
Dictionary<string, string> Headers { get; }
```

* 请求数据

```csharp
byte[] PostData { get; }
```



调用示例：

```csharp
using UnityEngine;
using SK.Framework;
using SK.Framework.Networking;

public class Example : MonoBehaviour
{
    public class WebInterface
    {
        public static string BaiDu
        {
            get
            {
                return "https://www.baidu.com/";
            }
        }
    }
    private void Start()
    {
        Main.WebRequest.Send(WebInterface.BaiDu, WebRequestData.Allocate(WebRequestType.GET),
            onSuccess: handler => Debug.Log(handler.text),
            onFailure: error => Debug.Log(string.Format("发起网络请求失败：{0}", error)));
    }
}
```

