using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AoC2020.Application;

namespace AoC2020
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine("Arguments: --event <int> --file <string> --second?");
                return;
            }

            var argsEvent = GetArgsValue(args, "-e", "--event");
            
            var argsFile = GetArgsValue(args, "--file");
            var linesEnumerable = GetLines(argsFile);
            
            var second = GetArgsExist(args, "--second");

            IEventApp app;
            switch (argsEvent)
            {
                case "1": {
                    app = new One(linesEnumerable, 2020);
                    break;
                }
                case "2": {
                    app = new Two(linesEnumerable);
                    break;
                }
                case "3": {
                    app = new Three(linesEnumerable);
                    break;
                }
                case "4": {
                    app = new Four(linesEnumerable);
                    break;
                }
                case "5": {
                    app = new Five(linesEnumerable);
                    break;
                }
                case "6": {
                    app = new Six(linesEnumerable);
                    break;
                }
                default:
                    Console.WriteLine("No registered runner for event: " +argsEvent);
                    return;
            }
            
            app.Run(second);
            return;
        }

        private static string GetArgsValue(string[] args, params string[] flags) {
            var value = "";
            var index = Array.FindIndex(args, a => flags.Contains(a));
            if(index != -1) {
                value = args[index+1];
            }
            return value;
        }
        private static bool GetArgsExist(string[] args, params string[] flags) {
            return Array.FindIndex(args, a => flags.Contains(a)) != -1;
        }

        private static IEnumerable<string> GetLines(string fileSource) {
            return File.ReadLines(fileSource);
        }
    }
}
