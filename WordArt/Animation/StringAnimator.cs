using WordArt.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace WordArt.Animation
{
    public delegate void TransformEvent(object sender, TransformEventArgs args);

    /// <summary>
    /// 动画执行器
    /// </summary>
    public class StringAnimator
    {
        //private static int ANIMATON_END_TIMESPAN = 5000;//如果没有动画没有确定的停止条件，则在运行2秒后结束运行。

        public static int TIME_INTERVAL = 100;

        //决定动画的执行速度
        private int speedFactor = 1;

        //重复的次数
        private int repeatCount = 1;

        //停留时间
        private int residenceTimeSpan = 100;
        private int residenceTimeCount = 0;

        //定时器
        private System.Timers.Timer timer;

        //定时事件的处理进行加锁
        protected int inTimerHandler = 0;

        /// <summary>
        /// 是否并行动画
        /// </summary>
        private bool isParallelAnimation = false;

        /// <summary>
        /// 结束时间间隔计数
        /// </summary>
        //private int endTimeCount = 0;

        public StringAnimator(AnimationConfig config, bool isParallelAnimation = false)
        {
            UpdateAnimationConfig(config);
            this.isParallelAnimation = isParallelAnimation;
        }

        private List<LinkedList<AnimationChar>> screens;

        public List<LinkedList<AnimationChar>> Screens
        {
            set { screens = value; }
        }

        private int screenIndex = 0;
        private bool beginResidence = false;// 是否处于停留时间
        private int state = TransformEventArgs.IDLE;

        private LinkedListNode<AnimationChar> currentChar = null;

        /// <summary>
        /// 执行动画事件
        /// </summary>
        public event TransformEvent Transformed;

        public void UpdateAnimationConfig(AnimationConfig config)
        {
            residenceTimeSpan = AnimationConfig.ResidenceTime;
            residenceTimeCount = 0;
            TIME_INTERVAL = (int)(100 / AnimationConfig.AnimationSpeedFactor);
            if (TIME_INTERVAL > 500) TIME_INTERVAL = 500;
            if (TIME_INTERVAL < 20) TIME_INTERVAL = 20;
            //endTimeCount = 0;
        }

        /// <summary>
        /// 开启动画执行器
        /// </summary>
        public void Start()
        {
            timer = new System.Timers.Timer();
            timer.Interval = TIME_INTERVAL * speedFactor;
            timer.AutoReset = true;
            timer.Elapsed += TimerHandler;
            timer.Enabled = true;
            //endTimeCount = 0;
        }

        /// <summary>
        /// 停止动画
        /// </summary>
        public void Stop()
        {
            timer.Enabled = false;
            timer.Close();
            timer = null;
            //endTimeCount = 0;
        }

        /// <summary>
        /// 重置动画
        /// </summary>
        public void Reset()
        {
            Stop();
            Start();
            //endTimeCount = 0;
        }

        /// <summary>
        /// 定时触发回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerHandler(object sender, ElapsedEventArgs e)
        {
            if (Interlocked.Exchange(ref inTimerHandler, 1) == 0)
            {
                HandleTimeElapsed();
                Interlocked.Exchange(ref inTimerHandler, 0);
            }
        }

        /// <summary>
        /// 处理业务
        /// </summary>
        protected virtual void HandleTimeElapsed()
        {
            if (beginResidence)//一屏显示结束，停留指定的时间
            {
                state = TransformEventArgs.RUNNING;
                residenceTimeCount++;
                if (residenceTimeCount * TIME_INTERVAL >= residenceTimeSpan)
                {
                    residenceTimeCount = 0;
                    beginResidence = false;

                    screenIndex++;
                    if (screenIndex >= screens.Count)//所有的屏显示完成
                    {
                        if (!AnimationConfig.IsUseGif)
                        {
                            screenIndex = 0;//从头开始
                            //恢复动画
                            foreach (LinkedList<AnimationChar> chars in screens)
                            {
                                LinkedListNode<AnimationChar> animChar = chars.First;
                                while (animChar != null)
                                {
                                    animChar.Value.Animation.Reset();
                                    animChar = animChar.Next;
                                }
                            }
                        }   
                        state = TransformEventArgs.END;
                    }
                }

                if (Transformed != null)
                {
                    TransformEventArgs args = new TransformEventArgs(screenIndex);
                    args.State = state;
                    Transformed(this, args);
                }

                return;
            }

            state = TransformEventArgs.IDLE;
            if (screenIndex < screens.Count)
            {
                state = TransformEventArgs.RUNNING;
                if (currentChar == null && screenIndex == 0)
                {
                    state = TransformEventArgs.BEGIN;
                }

                if (currentChar == null)//首次运行
                {
                    LinkedList<AnimationChar> chars = screens[screenIndex];
                    if (isParallelAnimation)//并行动画，同时开始
                    {
                        LinkedListNode<AnimationChar> node = chars.First;
                        currentChar = node;
                        while (node != null)
                        {
                            node.Value.Animation.Start();
                            node = node.Next;
                        }
                    }
                    else
                    {
                        currentChar = chars.First;
                        currentChar.Value.Animation.Start();
                    }
                    //endTimeCount = 0;
                }

                if (currentChar.Value.Animation.IsAnimationEnd())//动画结束
                //if (IsEnd(currentChar.Value.Animation))
                {
                    //currentChar = currentChar.Next;//进入一个动画
                    if (isParallelAnimation)//并行动画，同时取消
                    {
                        LinkedListNode<AnimationChar> node = currentChar;
                        while (node != null)
                        {
                            node.Value.Animation.Cancel();
                            node = node.Next;
                        }
                        currentChar = null;
                    }
                    else
                    {
                        currentChar = currentChar.Next;//进入一个动画
                    }
                    if (currentChar == null)//无下个，表示该屏字符显示完成
                    {
                        //screenIndex++;
                        //if (screenIndex >= screens.Count)//所有的屏显示完成
                        //{
                        //    //screenIndex = 0;//从头开始
                        //    state = TransformEventArgs.END;
                        //}
                        beginResidence = true;
                        residenceTimeCount = 0;
                    }
                    else
                    {
                        currentChar.Value.Animation.Start();
                        //endTimeCount = 0;
                    }
                }
            }

            if (Transformed != null)
            {
                TransformEventArgs args = new TransformEventArgs(screenIndex);
                args.State = state;
                Transformed(this, args);
            }
        }

        //private bool IsEnd(TextAnimation animation)
        //{
        //    if (!animation.IsCycleAnimation())//非循环动画，达到条件自动停止
        //        return animation.IsAnimationEnd();

        //    if (animation is ContinuouseMovementAnimation && !AnimationConfig.IsUseGif)//连续移动
        //        return true;

        //    if (endTimeCount * TIME_INTERVAL < ANIMATON_END_TIMESPAN)
        //    {
        //        endTimeCount++;
        //        return false;
        //    }
        //    return true;
        //}
    }
}
