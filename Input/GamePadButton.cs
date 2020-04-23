using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Input {

    /// <summary>
    /// Game pad button input.
    /// </summary>
    public enum GamePadButton : ulong {

        //NULL.
        NULL,

        //Left stick. These are axis so for now I'll just split the bit flag.
        LeftStick_Left = (ulong)1 << 60,
        LeftStick_Right = (ulong)1 << 56,
        LeftStick_Up = (ulong)1 << 52,
        LeftStick_Down = (ulong)1 << 48,

        //Right stick. These are axis so for now I'll just split the bit flag.
        RightStick_Left = (ulong)1 << 44,
        RightStick_Right = (ulong)1 << 40,
        RightStick_Up = (ulong)1 << 36,
        RightStick_Down = (ulong)1 << 32,

        //Triggers.
        Trigger_Left = (ulong)1 << 24,
        Trigger_Right = (ulong)1 << 16,

        //Unused bit here.

        //Stick presses.
        LeftStick_Button = (ulong)1 << 14,
        RightStick_Button = (ulong)1 << 13,

        //DPAD.
        DPAD_Left = (ulong)1 << 12,
        DPAD_Right = (ulong)1 << 11,
        DPAD_Up = (ulong)1 << 10,
        DPAD_Down = (ulong)1 << 9,

        //Bumpers.
        Bumper_Left = (ulong)1 << 8,
        Bumper_Right = (ulong)1 << 7,

        //Start and select buttons.
        Start = (ulong)1 << 6,
        Select = (ulong)1 << 5,

        //ABXY.
        A = (ulong)1 << 4,
        B = (ulong)1 << 3,
        X = (ulong)1 << 2,
        Y = (ulong)1 << 1,

        //Guide.
        Guide = (ulong)1 << 0

    }

}
