using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.UI {

    /// <summary>
    /// Alignment. First nibble is vertical type, second is horizontal type.
    /// </summary>
    public enum Alignment : byte { 
        TopLeft = 0x00,
        Top = 0x01,
        TopRight = 0x02,
        Left = 0x10,
        Center = 0x11,
        Right = 0x12,
        BottomLeft = 0x20,
        Bottom = 0x21,
        BottomRight = 0x22
    }

    /// <summary>
    /// Position type.
    /// </summary>
    public enum PositionType : byte { 
        TopOrLeft = 0,
        Center = 1,
        BottomOrRight = 2
    }

}
