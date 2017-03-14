using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordArt.Animation
{
    public enum AnimationType
    {
        /// <summary>
        /// 向左飞入
        /// </summary>
        RIGHT_IN,

        /// <summary>
        /// 向右飞入
        /// </summary>
        LEFT_IN,

        /// <summary>
        /// 向上飞入
        /// </summary>
        BOTTOM_IN,

        /// <summary>
        /// 向下飞入
        /// </summary>
        TOP_IN,

        /// <summary>
        /// 逐字输出
        /// </summary>
        VERBATIM,

        /// <summary>
        /// 波浪
        /// </summary>
        WAVE,

        /// <summary>
        /// 左右摇摆
        /// </summary>
        SWAY,

        /// <summary>
        /// 前后缩放
        /// </summary>
        ZOOM_IN_OUT,

        /// <summary>
        /// 逐字放大
        /// </summary>
        VERBATIM_AMPLIFICATION,

        /// <summary>
        /// 逐字缩小
        /// </summary>
        VERBATIM_NARROW,

        /// <summary>
        /// 向上放大缩小
        /// </summary>
        UPWARD_ZOOM_IN_OUT,

        /// <summary>
        /// 依次翻转
        /// </summary>
        VERBATIM_FLIP,

        /// <summary>
        /// 向右放大
        /// </summary>
        SCALE_TO_RIGHT,

        /// <summary>
        /// 颤抖
        /// </summary>
        TREMBLE,

        /// <summary>
        /// 旋转
        /// </summary>
        ROTATE,

        /// <summary>
        /// 旋转滚动
        /// </summary>
        ROLLING,

        /// <summary>
        /// 角度旋转
        /// </summary>
        ROTATE_AT_ANGLE,

        /// <summary>
        /// 依次下落
        /// </summary>
        FALL_IN_TURN,

        /// <summary>
        /// 45度角下落回弹一次
        /// </summary>
        FALL_IN_TURN_BOUNCE,

        /// <summary>
        /// 左右摇晃
        /// </summary>
        SWAY_LEFT_RIGHT,

        /// <summary>
        /// 重复下落
        /// </summary>
        REPEAT_FALL,

        /// <summary>
        /// 右上方移出
        /// </summary>
        TOP_RIGHT_MOVE_IN,

        /// <summary>
        /// 右上角逐次放大下落
        /// </summary>
        TOP_RIGHT_MOVE_IN_WIDTH_SCALE,

        /// <summary>
        /// 黑洞
        /// </summary>
        BLACK_HOLE,

        /// <summary>
        /// 水平旋转
        /// </summary>
        HORIZONTAL_ROTATION,

        /// <summary>
        /// 翻转挤入
        /// </summary>
        TURN_INTO,

        /// <summary>
        /// 旋转挤入
        /// </summary>
        ROTATION_INTO,

        /// <summary>
        /// 文字缩放
        /// </summary>
        TEXT_SCALING,

        /// <summary>
        /// 蛙跳带出
        /// </summary>
        FROG_CARRY_OUT,

        /// <summary>
        /// 蛙跳压缩
        /// </summary>
        FROG_COMPRESS,

        /// <summary>
        /// 连续左移
        /// </summary>
        CONTINUOUSE_MOVE_LEFT,

        /// <summary>
        /// 连续右移
        /// </summary>
        CONTINUOUSE_MOVE_RIGHT,

        /// <summary>
        /// 连续下移
        /// </summary>
        CONTINUOUSE_MOVE_BOTTOM,

        /// <summary>
        /// 连续上移
        /// </summary>
        CONTINUOUSE_MOVE_TOP
    }

    public enum MoveType
    { 
        /// <summary>
        /// 连续左移
        /// </summary>
        CONTINUOUS_LEFT_SHIFT,
        /// <summary>
        /// 连续右移
        /// </summary>
        CONTINUOUS_RIGHT_SHIFT,
        /// <summary>
        /// 连续上移
        /// </summary>
        CONTINUOUS_UPPER_SHIFT,
        /// <summary>
        /// 连续下移
        /// </summary>
        CONTINUOUS_DOWN_SHIFT
    }
}
