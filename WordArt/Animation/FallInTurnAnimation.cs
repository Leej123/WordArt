using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    /// <summary>
    /// 依次落下动画
    /// </summary>
    public class FallInTurnAnimation : TextAnimation
    {
        private const float CHANGE_VALUE = 5;
        /// <summary>
        /// 开始的位置
        /// </summary>
        private float fromY;

        /// <summary>
        /// 回弹的位置
        /// </summary>
        private float bounceY;

        /// <summary>
        /// 结束的位置
        /// </summary>
        private float toY;

        /// <summary>
        /// 旋转角度
        /// </summary>
        private int angle;
        private PointF pivot;
        /// <summary>
        /// 是否回弹
        /// </summary>
        private bool bounceBack;
        private bool beginBounceBack = false;//开始回弹
        private bool back = false;//从回弹位置返回

        /// <summary>
        /// 当前动画是否结束
        /// </summary>
        private bool curAnimationEnd = false;

        private float curY = 0;

        private FallInTurnAnimation nextAnimation;
        public FallInTurnAnimation NextAnimation
        {
            set { nextAnimation = value; }
        }

        public FallInTurnAnimation(float fromY, float toY, bool bounceBack, int rotateAngle, PointF pivot,  float bounceY) : base()
        {
            this.fromY = fromY;
            this.toY = toY;
            this.bounceBack = bounceBack;
            this.angle = rotateAngle;
            this.bounceY = bounceY;
            this.pivot = pivot;

            curY = fromY;
            beginBounceBack = false;
        }

        public override System.Drawing.Drawing2D.Matrix GetTransformation(ref bool isStillRun)
        {
            if (!isStart)
            {
                isStillRun = false;
                transform.Reset();
                return transform;
            }

            if (curAnimationEnd)
            {
                isStillRun = false;
                transform.Reset();
                transform.Translate(0, toY - fromY);
                return transform;
            }

            ApplyTransformation(transform);
            isStillRun = true;
            return transform;
        }

        public override void ApplyTransformation(System.Drawing.Drawing2D.Matrix transform)
        {
            if (bounceBack && beginBounceBack)
            {
                BounceBack();
            }
            else
            {
                Fall();
            }

            if (curY == toY)//到达结束位置
            {
                if (bounceBack)
                {
                    if (beginBounceBack)//已经开始了回弹，表示第二次回到结束位置。
                    {
                        curAnimationEnd = true;//当前动画结束
                        transform.Reset();
                        transform.Translate(0, toY - fromY);
                    }
                    beginBounceBack = true;//开始回弹
                }
                else//不回弹
                {
                    curAnimationEnd = true;//当前动画结束
                    transform.Reset();
                    transform.Translate(0, toY - fromY);
                }
            }
            else
            {
                transform.Reset();
                transform.Translate(0, curY - fromY);//再平移
                if (bounceBack && !back)
                    transform.RotateAt(-angle, pivot);//先旋转
            }
        }

        private void Fall()
        {
            if (toY == fromY)
            {
                curY = toY;
            }
            else if (fromY < toY)
            {
                curY += CHANGE_VALUE;
                if (curY >= toY)
                    curY = toY;
            }
            else
            {
                curY -= CHANGE_VALUE;
                if (curY <= toY)
                    curY = toY;
            }
        }

        private void BounceBack()
        {
            if (toY == bounceY)
            {
                back = true;
                curY = toY;
            }
            else if (toY > bounceY)
            {
                if (!back)
                {
                    curY -= CHANGE_VALUE;
                    if (curY <= bounceY)
                    {
                        curY = bounceY;
                        back = true;
                    }
                }
                else
                {
                    curY += CHANGE_VALUE;
                    if (curY >= toY)
                    {
                        curY = toY;
                    }
                }
            }
            else
            {
                if (!back)
                {
                    curY += CHANGE_VALUE;
                    if (curY >= bounceY)
                    {
                        curY = bounceY;
                        back = true;
                    }
                }
                else
                {
                    curY -= CHANGE_VALUE;
                    if (curY <= toY)
                    {
                        curY = toY;
                    }
                }
            }
        }

        public override void Reset()
        {
            base.Reset();
            back = false;
            beginBounceBack = false;
            curY = fromY;
            curAnimationEnd = false;
        }

        public override void Cancel()
        {
            
        }

        public override bool IsAnimationEnd()
        {
            //依次动画，以最后一个动画结束为结束。
            isEnd = false;
            if (nextAnimation == null)
            {
                if (curAnimationEnd)
                    isEnd = true;
            }
            else
            {
                isEnd = nextAnimation.IsAnimationEnd();
            }

            return isEnd;
        }
    }
}
