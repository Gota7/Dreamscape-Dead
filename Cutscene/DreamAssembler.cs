using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Cutscene {

    /// <summary>
    /// Dreamscape cutscene assembler assembler.
    /// </summary>
    public static class DreamAssembler {

        /// <summary>
        /// Command type map.
        /// </summary>
        private static Dictionary<string, CommandType> commandTypeMap = new Dictionary<string, CommandType>();

        /// <summary>
        /// Static constructor.
        /// </summary>
        static DreamAssembler() {

            //Get the enum values.
            var keys = Enum.GetNames(typeof(CommandType));
            var values = Enum.GetValues(typeof(CommandType)).Cast<CommandType>();
            for (int i = 0; i < keys.Length; i++) {
                commandTypeMap.Add(keys[i].ToLower(), values.ElementAt(i));
            }

        }

        /// <summary>
        /// Convert a command list back to the dream format.
        /// </summary>
        /// <param name="commands">List of commands to disassemble.</param>
        /// <returns>The commands in the dream format.</returns>
        public static List<string> DisassembleCommands(List<CutsceneCommand> commands) {

            //List.
            List<string> ret = new List<string>();

            //Add each command.
            foreach (var c in commands) {

                //The command.
                string command = "";

                //Get the command type.
                string type = c.CustomCommandType;
                if (c.CommandType != CommandType.Custom) {
                    type = c.CommandType.ToString();
                }
                type = type.Substring(0, 1).ToLower() + type.Substring(1);

                //Add type to command.
                command += type;

                //Booleans.
                foreach (var v in c.BooleanParameters) {
                    command += " " + (v ? "true" : "false");
                }

                //Chars.
                foreach (var v in c.CharParameters) {
                    command += " '" + v + "'";
                }

                //Strings.
                foreach (var v in c.StringParameters) {
                    if (v.Contains(" ")) {
                        command += " \"" + v.Replace("#", "\\#").Replace("\"", "\\\"") + "\"";
                    } else {
                        command += " " + v.Replace("#", "\\#").Replace("\"", "\\\"");
                    }
                }

                //Decimals.
                foreach (var v in c.DecimalParameters) {
                    command += " " + v.ToString() + "m";
                }

                //Doubles.
                foreach (var v in c.DoubleParameters) {
                    command += " " + v.ToString() + "d";
                }

                //Floats.
                foreach (var v in c.FloatParameters) {
                    command += " " + v.ToString() + "f";
                }

                //Ints.
                foreach (var v in c.IntParameters) {
                    command += " " + v.ToString();
                }

                //UInts.
                foreach (var v in c.UIntParameters) {
                    command += " " + v.ToString() + "u";
                }

                //Add the command.
                ret.Add(command);

            }

            //Return the commands.
            return ret;

        }

        /// <summary>
        /// Parse a commands list.
        /// </summary>
        /// <param name="lines">The lines to parse.</param>
        /// <returns>A list of cutscene commands.</returns>
        public static List<CutsceneCommand> ParseCommands(string[] lines) {

            //Commands.
            List<CutsceneCommand> c = new List<CutsceneCommand>();
            int lineNum = 0;

            //For each line.
            foreach (var s in lines) {

                //Remove additional parts.
                string fin = s.Replace("\n", "").Replace("\t", "").Replace("\r", "");

                //Get comment has locations.
                char prev = ' ';
                List<int> hashPtrs = new List<int>();
                for (int i = 0; i < fin.Length; i++) {

                    //Make sure previous is not a backslash.
                    if (prev.Equals('\\')) {
                        prev = fin[i];
                        continue;
                    }

                    //If this is a hash.
                    if (fin[i].Equals('#')) {
                        hashPtrs.Add(i);
                    }

                    //Set previous char.
                    prev = fin[i];

                }

                //The final string.
                string f = "";
                bool commentMode = false;
                for (int i = 0; i < fin.Length; i++) {

                    //Swap.
                    if (hashPtrs.Contains(i)) {
                        commentMode = !commentMode;
                        continue;
                    }

                    //Add if not comment.
                    if (!commentMode) {
                        f += fin[i];
                    }

                }

                //Make sure the command is valid.
                if (f != "") {

                    //Parse the command.
                    c.Add(ParseCommand(f, lineNum));
                
                }

                //Line number.
                lineNum++;

            }

            //Return commands.
            return c;

        }

        /// <summary>
        /// Parse a cutscene command.
        /// </summary>
        /// <param name="command">The command to parse.</param>
        /// <param name="commandNumber">Command number.</param>
        /// <returns>The cutscene command.</returns>
        public static CutsceneCommand ParseCommand(string command, int commandNumber) {

            //New command.
            CutsceneCommand c = new CutsceneCommand();

            //Get the type.
            string type = command.Split(' ')[0].ToLower();
            if (commandTypeMap.ContainsKey(type)) {
                c.CommandType = commandTypeMap[type];
            } else {
                c.CustomCommandType = type;
            }

            //Parse stuff.
            string sub = "";
            bool escapeMode = false;
            bool quoteMode = false;
            bool charMode = false;
            List<string> strs = new List<string>();
            List<char> chars = new List<char>();
            List<bool> bools = new List<bool>();
            List<decimal> decimals = new List<decimal>();
            List<double> doubles = new List<double>();
            List<float> floats = new List<float>();
            List<int> ints = new List<int>();
            List<uint> uints = new List<uint>();
            for (int i = type.Length + 1; i < command.Length; i++) {

                //Escape mode.
                if (!escapeMode && command[i].Equals('\\')) {
                    escapeMode = true;
                    continue;
                }

                //Quotes.
                if (!escapeMode && command[i].Equals('"')) {
                    quoteMode = !quoteMode;
                    if (!quoteMode) {
                        strs.Add(sub);
                        sub = "";
                    }
                    continue;
                }

                //Quote mode.
                if (quoteMode) {
                    sub += command[i];
                    if (escapeMode) {
                        escapeMode = false;
                    }
                    continue;
                }

                //Chars.
                if (!escapeMode && command[i].Equals('\'')) {
                    charMode = !charMode;
                    if (!charMode) {
                        chars.Add(sub[0]);
                        sub = "";
                    }
                    continue;
                }

                //Char mode.
                if (charMode) {
                    sub += command[i];
                    if (escapeMode) {
                        escapeMode = false;
                    }
                    continue;
                }

                //Just add the item to the sub if not space or end of string.
                if (command[i].Equals(' ') || i == command.Length - 1) {

                    //Last character.
                    if (i == command.Length - 1) {
                        sub += command[i];
                    }

                    //Break if too small.
                    if (sub.Length < 1) {
                        continue;
                    }

                    //Boolean.
                    if (sub.ToLower().Equals("true")) {
                        bools.Add(true);
                        sub = "";
                        continue;
                    } else if (sub.ToLower().Equals("false")) {
                        bools.Add(false);
                        sub = "";
                        continue;
                    }

                    //If number.
                    bool isNumber = true;
                    for (int j = 0; j < sub.Length - 1; j++) {
                        if (!(sub[j].Equals('0') || sub[j].Equals('1') || sub[j].Equals('2') || sub[j].Equals('3') || sub[j].Equals('4') || sub[j].Equals('5') || sub[j].Equals('6') || sub[j].Equals('7') || sub[j].Equals('8') || sub[j].Equals('9') || sub[j].Equals('.'))) {
                            isNumber = false;
                        }
                    }

                    //If a one length number, add int.
                    if (sub.Length == 1 && (sub.Equals("0") || sub.Equals("1") || sub.Equals("2") || sub.Equals("3") || sub.Equals("4") || sub.Equals("5") || sub.Equals("6") || sub.Equals("7") || sub.Equals("8") || sub.Equals("9"))) {
                        ints.Add(int.Parse(sub));
                        sub = "";
                        continue;
                    }

                    //If not number, then add a string.
                    if (!isNumber || sub.Length == 1) {
                        strs.Add(sub);
                        sub = "";
                        continue;
                    }

                    //Decimal.
                    if (sub.ToLower()[sub.Length - 1].Equals('m')) {
                        decimals.Add(decimal.Parse(sub.Substring(0, sub.Length - 1)));
                        sub = "";
                        continue;
                    }

                    //Double.
                    if (sub.ToLower()[sub.Length - 1].Equals('d')) {
                        doubles.Add(double.Parse(sub.Substring(0, sub.Length - 1)));
                        sub = "";
                        continue;
                    }

                    //Float.
                    if (sub.ToLower()[sub.Length - 1].Equals('f')) {
                        floats.Add(float.Parse(sub.Substring(0, sub.Length - 1)));
                        sub = "";
                        continue;
                    }

                    //UInt.
                    if (sub.ToLower()[sub.Length - 1].Equals('u')) {
                        uints.Add(uint.Parse(sub.Substring(0, sub.Length - 1)));
                        sub = "";
                        continue;
                    }

                    //Int.
                    ints.Add(int.Parse(sub));
                    sub = "";
                    continue;

                }

                //Add the sub.
                else {
                    sub += command[i];
                }

            }

            //Set parameters.
            c.StringParameters = strs.ToArray();
            c.CharParameters = chars.ToArray();
            c.BooleanParameters = bools.ToArray();
            c.DecimalParameters = decimals.ToArray();
            c.DoubleParameters = doubles.ToArray();
            c.FloatParameters = floats.ToArray();
            c.UIntParameters = uints.ToArray();
            c.IntParameters = ints.ToArray();

            //Return the command.
            return c;

        }

    }

}
