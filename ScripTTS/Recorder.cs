using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Speech.Synthesis;

namespace ScriptSeparator
{
    class Script
    {
        public int Index { get; }
        public string Title { get; }
        public string Content { get; }

        public Script(int index, string title, string content)
        {
            Index = index;
            Title = title;
            Content = content;
        }
    }

    class Recorder
    {
        const string txtDirName = "txt";
        const string recordDirName = "records";

        private readonly string filePath;

        public Recorder(string filePath)
        {
            this.filePath = filePath;
        }

        public void Run()
        {
            List<Script> scripts = ReadFile();

            Directory.CreateDirectory(txtDirName);
            Directory.CreateDirectory(recordDirName);

            foreach (Script script in scripts)
            {
                WriteToFile(script);
                Record(script);
            }
        }

        private List<Script> ReadFile()
        {
            List<Script> list = new List<Script>();
            string[] lines = File.ReadAllLines(filePath, Encoding.Default);
            int curIndex = 0;
            string curTitle = string.Empty;
            
            foreach (string line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                if (line.StartsWith("-")) curTitle = line.Remove(0, 1);
                else list.Add(new Script(curIndex++, curTitle, line));
            }

            return list;
        }

        private void WriteToFile(Script script)
        { 
            string filePath = genFilePath(txtDirName, script) + ".txt";

            File.WriteAllText(filePath, script.Content);
        }

        private void Record(Script script)
        {
            string filePath = genFilePath(recordDirName, script) + ".wav";

            SpeechSynthesizer reader = new SpeechSynthesizer
            {
                Rate = 0
            };

            reader.SelectVoiceByHints(VoiceGender.Male, VoiceAge.NotSet, 0, new System.Globalization.CultureInfo("en-UK", false));
            reader.SetOutputToWaveFile(filePath);

            reader.Speak(script.Content);
        }

        private string genFilePath(string dirPath, Script script)
        {
            string fileName = script.Index + "." + script.Title;

            return Path.Combine(dirPath, fileName);
        }
    }
}
