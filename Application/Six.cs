using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020.Application
{
    public class Six : IEventApp
    {
        private readonly IEnumerable<string> _lines;
        
        public Six(IEnumerable<string> lines)
        {
            _lines = lines;
        }

        public void Run(bool second = false) {
            if(second) {
                SumYes(true);
            } else {
                SumYes();
            }
        }
        
        private IEnumerable<string> Parse(IEnumerable<string> lines, bool mustExistInAllLines) {
            var ret = new List<string>();
            var groupMembers = 0;
            var reset = false;
            foreach (var line in lines)
            {
                if (reset) {
                    ret = new List<string>();
                    groupMembers = 0;
                    reset = false;
                }
                if(line == "") {
                    reset = true;
                    if(mustExistInAllLines) {
                        var intersected = ret.First();
                        foreach (var item in ret.Skip(1))
                        {
                            intersected = new string(intersected.Intersect(item).ToArray());
                        }
                        yield return intersected;
                    } else {
                        yield return string.Join("", ret);
                    }
                } else {
                    ret.Add(line);
                    groupMembers++;
                }
            }
            if(mustExistInAllLines) {
                var intersected = ret.First();
                foreach (var item in ret.Skip(1))
                {
                    intersected = new string(intersected.Intersect(item).ToArray());
                }
                yield return intersected;
            } else {
                yield return string.Join("", ret);
            }
        }

        private void SumYes(bool mustExistInAllLines = false) {
            var strings = Parse(_lines, mustExistInAllLines);
            
            var sum = strings.Select(s => s.Distinct().Count()).Sum();

            Console.WriteLine(sum);
        }
    }
}