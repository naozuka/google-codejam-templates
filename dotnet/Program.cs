using System;

namespace dotnet
{
    class Program
    {
        static void Main(string[] args)
        {
            using IOHelper helper = new IOHelper(args);            
            
            int testCases = helper.ReadLine<int>();
            for (int testCase = 1; testCase <= testCases; testCase++) 
            {
                string result = Resolve(helper);
                helper.WriteLine(String.Format("Case #{0}: {1}", testCase, result));
            }        
        }

        static string Resolve(IOHelper helper)
        {
            int[] numbers = helper.ReadLineAsArray<int>();
            int sum = 0;
            foreach (int num in numbers)
                sum += num;

            return sum.ToString();
        }
    }       

    public class IOHelper : IDisposable
    {
        private readonly System.IO.TextReader _readerInput;        
        private readonly System.IO.TextReader _readerOutput;
        private readonly System.IO.TextWriter _writer;
        private readonly bool _debug;
        public bool DebugOn { get; set; } // Use this to enable a debug on specific test case or scenario

        public IOHelper(string[] args)
        {            
            DebugOn = true;
            if (args.Length > 0)
            {                
                _debug = true;

                if (System.IO.File.Exists(args[0] + "small.in"))
                    _readerInput = new System.IO.StreamReader(args[0] + "small.in");

                if (System.IO.File.Exists(args[0] + "small.out"))
                    _readerOutput = new System.IO.StreamReader(args[0] + "small.out");                
            }
            else
            {
                _debug = false;                
                _readerInput = System.Console.In;
                _writer = System.Console.Out;
            }

            RecreateLogFile();
        }

        public string ReadLine() => _readerInput.ReadLine();        
        public string[] ReadLineAsArray() => _readerInput.ReadLine().Split(' ');        

        public T ReadLine<T>()
        {
            string value = ReadLine();
            return GenericParse<T>(value);
        }

        public T[] ReadLineAsArray<T>()
        {
            string[] strings = ReadLineAsArray();            
            return Array.ConvertAll<string, T>(strings, s => GenericParse<T>(s));
        }

        private T GenericParse<T>(string value)
        {
            Type _t = typeof(T);
            
            // Test for Nullable<T> and return the base type instead:
            Type undertype = Nullable.GetUnderlyingType(_t);
            Type basetype = undertype == null ? _t : undertype;
            return (T)Convert.ChangeType(value, basetype);
        }

        public void WriteLine(string text)
        {            
            if (_writer != null)
            {
                _writer.WriteLine(text);
                _writer.Flush();
            }
            else
            {
                string correctOutput = _readerOutput.ReadLine();

                if (text == correctOutput)
                    text += " [PASSED]";
                else
                    text += String.Format(" [ERROR - Expected: {0}]", correctOutput);

                Console.WriteLine(text);
            }
        } 

        // Clear log file
        private void RecreateLogFile()
        {
            if (System.IO.File.Exists("data.log")) 
            {
                using (System.IO.TextWriter log = new System.IO.StreamWriter("data.log"))
                    log.Close();
            }
        }     

        // You can use this to "debug" in interactive problems
        public void Log(object value)
        {
            // Create this file in your root project to safe log
            if (System.IO.File.Exists("data.log") && DebugOn) 
            {
                using (System.IO.TextWriter log = new System.IO.StreamWriter("data.log", true))
                {
                    log.WriteLine(string.Format("{0}", value));
                    log.Close();
                }
            }
        }

        // You can use this as a safe way to console your variables
        public void PrintLine(object value)
        {
            if (_debug && DebugOn)
                Console.WriteLine(value);
        }

        public void Dispose()
        {
            if (_readerInput != null) 
                _readerInput.Close();

            if (_readerOutput != null) 
                _readerOutput.Close();

            if (_writer != null) 
                _writer.Close();
        }
    }
}
