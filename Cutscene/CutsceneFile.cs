using Dreamscape.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamscape.Cutscene {

    /// <summary>
    /// Cutscene file loading. Binary form is DCUT (Dreamscape CUTscence). Please use the .dream extension for text cutscenes for organizational purposes.
    /// </summary>
    public class CutsceneFile : IOFile {

        /// <summary>
        /// If the file is in binary form.
        /// </summary>
        public bool IsBinary;

        /// <summary>
        /// List of cutscene commands.
        /// </summary>
        public List<CutsceneCommand> Commands = new List<CutsceneCommand>();

        /// <summary>
        /// Retrieve cutscene commands from a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The cutscene commands.</returns>
        public static List<CutsceneCommand> RetrieveCommands(string filePath) {

            //Read a file and return its commands.
            CutsceneFile b = new CutsceneFile();
            b.Read(FileSystem.OpenFileStream(filePath));
            return b.Commands;

        }

        /// <summary>
        /// Read the file.
        /// </summary>
        /// <param name="r">The reader.</param>
        public override void Read(FileReader r) {

            //New commands list.
            Commands = new List<CutsceneCommand>();

            //If binary.
            IsBinary = new string(r.ReadChars(4)).Equals("DCUT");

            //Binary form.
            if (IsBinary) {

                //Read commands.
                uint numCommands = r.ReadUInt32();
                for (uint i = 0; i < numCommands; i++) {
                    Commands.Add(r.Read<CutsceneCommand>());
                }

            }

            //Text form.
            else {

                //Move back.
                r.Position -= 4;

                //Read text.
                List<string> lines = new List<string>();
                using (StreamReader s = new StreamReader(r.BaseStream)) {
                    while (!s.EndOfStream) {
                        lines.Add(s.ReadLine());
                    }
                }

                //Parse.
                Commands = DreamAssembler.ParseCommands(lines.ToArray());

            }

        }

        /// <summary>
        /// Write the file.
        /// </summary>
        /// <param name="w">The writer.</param>
        public override void Write(FileWriter w) {

            //Binary form.
            if (IsBinary) {
                w.Write("DCUT".ToCharArray());
                w.Write((uint)Commands.Count);
                foreach (var c in Commands) {
                    w.Write(c);
                }
            }

            //Text form.
            else {
                List<string> lines = DreamAssembler.DisassembleCommands(Commands);
                using (StreamWriter s = new StreamWriter(w.BaseStream)) {
                    foreach (var l in lines) {
                        s.WriteLine(l);
                    }
                }
            }

        }

    }

}
