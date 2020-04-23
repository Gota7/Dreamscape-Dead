using Dreamscape.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Cutscene {

    /// <summary>
    /// A cutscene command.
    /// </summary>
    public class CutsceneCommand : IReadable, IWriteable {

        /// <summary>
        /// Command type.
        /// </summary>
        public CommandType CommandType {
            get {
                if (CustomCommandType == "") {
                    return m_CommandType;
                } else {
                    return CommandType.Custom;
                }
            }
            set {
                if (value != CommandType.Custom) {
                    CustomCommandType = "";
                    m_CommandType = value;
                }
            }
        }
        private CommandType m_CommandType;

        /// <summary>
        /// If the command type is custom.
        /// </summary>
        public string CustomCommandType = "";

        /// <summary>
        /// Boolean parameters.
        /// </summary>
        public bool[] BooleanParameters = new bool[0];

        /// <summary>
        /// Char parameters.
        /// </summary>
        public char[] CharParameters = new char[0];

        /// <summary>
        /// String parameters.
        /// </summary>
        public string[] StringParameters = new string[0];

        /// <summary>
        /// Decimal parameters.
        /// </summary>
        public decimal[] DecimalParameters = new decimal[0];
        
        /// <summary>
        /// Double parameters.
        /// </summary>
        public double[] DoubleParameters = new double[0];

        /// <summary>
        /// Float parameters.
        /// </summary>
        public float[] FloatParameters = new float[0];

        /// <summary>
        /// Integer parameters.
        /// </summary>
        public int[] IntParameters = new int[0];

        /// <summary>
        /// Unsigned integer parameters.
        /// </summary>
        public uint[] UIntParameters = new uint[0];

        /// <summary>
        /// Blank constructor.
        /// </summary>
        public CutsceneCommand() {}

        /// <summary>
        /// Create a cutscene command and read it.
        /// </summary>
        /// <param name="br">The reader.</param>
        public CutsceneCommand(FileReader br) {
            Read(br);
        }

        /// <summary>
        /// Get a number parameter at an index.
        /// </summary>
        /// <param name="index">Index to get the number.</param>
        /// <returns>The number at the index.</returns>
        public double GetNumber(int index) {
            if (UIntParameters.Length - 1 >= index) { return UIntParameters[index]; }
            if (IntParameters.Length - 1 >= index) { return IntParameters[index]; }
            if (FloatParameters.Length - 1 >= index) { return FloatParameters[index]; }
            if (DoubleParameters.Length - 1 >= index) { return DoubleParameters[index]; }
            if (DecimalParameters.Length - 1 >= index) { return (double)DecimalParameters[index]; }
            throw new ArgumentOutOfRangeException("No number given at the specified index " + index + "!");
        }

        /// <summary>
        /// Read the command.
        /// </summary>
        /// <param name="br">The reader.</param>
        public void Read(FileReader br) {

            //Read command type.
            CommandType = (CommandType)br.ReadByte();

            //Custom command type.
            if (CommandType == CommandType.Custom) {
                CustomCommandType = br.ReadString();
            }

            //Get parameter bits. Structure: 0xBHSCDFIU. B - Bool, H - Char, S = String, C = Decimal, D = Double, F = Float, I = Int, U = UInt.
            byte parameters = br.ReadByte();

            //Booleans.
            if ((parameters & 0b10000000) > 0) {
                BooleanParameters = br.ReadBooleans(br.ReadByte());
            }

            //Chars.
            if ((parameters & 0b01000000) > 0) {
                CharParameters = br.ReadChars(br.ReadByte());
            }

            //Strings.
            if ((parameters & 0b00100000) > 0) {
                StringParameters = br.ReadStrings(br.ReadByte());
            }

            //Decimals.
            if ((parameters & 0b00010000) > 0) {
                DecimalParameters = br.ReadDecimals(br.ReadByte());
            }

            //Doubles.
            if ((parameters & 0b00001000) > 0) {
                DoubleParameters = br.ReadDoubles(br.ReadByte());
            }

            //Floats.
            if ((parameters & 0b00000100) > 0) {
                FloatParameters = br.ReadSingles(br.ReadByte());
            }

            //Ints.
            if ((parameters & 0b00000010) > 0) {
                IntParameters = br.ReadInt32s(br.ReadByte());
            }

            //UInts.
            if ((parameters & 0b00000001) > 0) {
                UIntParameters = br.ReadUInt32s(br.ReadByte());
            }

        }

        /// <summary>
        /// Write the command.
        /// </summary>
        /// <param name="bw">The writer.</param>
        public void Write(FileWriter bw) {

            //Write the command type.
            bw.Write((byte)CommandType);

            //Custom command type.
            if (CommandType == CommandType.Custom) {
                bw.Write(CustomCommandType);
            }

            //Get parameter bitflags. Structure: 0xBHSCDFIU. B - Bool, H - Char, S = String, C = Decimal, D = Double, F = Float, I = Int, U = UInt.
            byte parameters = 0;
            parameters |= (byte)(BooleanParameters.Length > 0 ? 0b10000000 : 0);
            parameters |= (byte)(CharParameters.Length > 0 ?    0b01000000 : 0);
            parameters |= (byte)(StringParameters.Length > 0 ?  0b00100000 : 0);
            parameters |= (byte)(DecimalParameters.Length > 0 ? 0b00010000 : 0);
            parameters |= (byte)(DoubleParameters.Length > 0 ?  0b00001000 : 0);
            parameters |= (byte)(FloatParameters.Length > 0 ?   0b00000100 : 0);
            parameters |= (byte)(IntParameters.Length > 0 ?     0b00000010 : 0);
            parameters |= (byte)(UIntParameters.Length > 0 ?    0b00000001 : 0);

            //Write bitflags.
            bw.Write(parameters);

            //Booleans.
            if ((parameters & 0b10000000) > 0) {
                bw.Write((byte)BooleanParameters.Length);
                bw.Write(BooleanParameters);
            }

            //Chars.
            if ((parameters & 0b01000000) > 0) {
                bw.Write((byte)CharParameters.Length);
                bw.Write(CharParameters);
            }

            //Strings.
            if ((parameters & 0b00100000) > 0) {
                bw.Write((byte)StringParameters.Length);
                bw.Write(StringParameters);
            }

            //Decimals.
            if ((parameters & 0b00010000) > 0) {
                bw.Write((byte)DecimalParameters.Length);
                bw.Write(DecimalParameters);
            }

            //Doubles.
            if ((parameters & 0b00001000) > 0) {
                bw.Write((byte)DoubleParameters.Length);
                bw.Write(DoubleParameters);
            }

            //Floats.
            if ((parameters & 0b00000100) > 0) {
                bw.Write((byte)FloatParameters.Length);
                bw.Write(FloatParameters);
            }

            //Ints.
            if ((parameters & 0b00000010) > 0) {
                bw.Write((byte)IntParameters.Length);
                bw.Write(IntParameters);
            }

            //UInts.
            if ((parameters & 0b00000001) > 0) {
                bw.Write((byte)UIntParameters.Length);
                bw.Write(UIntParameters);
            }

        }

    }

}
