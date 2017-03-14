using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    public abstract class TextAnimation
    {
        /// <summary>
        /// 动画是否开始
        /// </summary>
        protected bool isStart = false;

        /// <summary>
        /// 动画是否结束
        /// </summary>
        protected bool isEnd = false;

        /// <summary>
        /// 变换矩阵。动画实现的本质
        /// </summary>
        protected Matrix transform = new Matrix();

        /// <summary>
        /// 是否是循环动画。有些动画，没有明确的停止条件。true：表示该动画没有停止条件，需要程序设定停止；false：到达停止条件后动画自动结束
        /// </summary>
        //protected bool isCycleAnimation = false;

        /// <summary>
        /// 获取动画的变化矩阵
        /// </summary>
        /// <param name="isStillRun"></param>
        /// <returns></returns>
        public virtual Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart || isEnd)//动画未开始
                return transform;
            ApplyTransformation(transform);
            isStillRun = !IsAnimationEnd();
            return transform;
        }

        //public TextAnimation()
        //{
        //    isCycleAnimation = false;
        //}

        /// <summary>
        /// 获取动画的变化矩阵
        /// </summary>
        /// <param name="transform"></param>
        public abstract void ApplyTransformation(Matrix transform);

        /// <summary>
        /// 动画是否结束
        /// </summary>
        /// <returns></returns>
        public abstract bool IsAnimationEnd();

        /// <summary>
        /// 开始动画
        /// </summary>
        public virtual void Start()
        {
            if (isStart)
                return;
            isStart = true;
            isEnd = false;
        }

        /// <summary>
        /// 重置动画
        /// </summary>
        public virtual void Reset()
        {
            isStart = false;
            isEnd = false;
            transform.Reset();
        }

        /// <summary>
        /// 取消动画
        /// </summary>
        public virtual void Cancel()
        {
            isStart = false;
            isEnd = true;
        }

        /// <summary>
        /// 停止动画
        /// </summary>
        public virtual void Stop()
        {
            isEnd = true;
        }

        //public virtual bool IsCycleAnimation()
        //{
        //    return isCycleAnimation;
        //}

        public virtual bool IsStart()
        {
            return isStart;
        }
    }
}
