using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Application
{
    public class Three : IEventApp
    {
        private readonly IEnumerable<string> _lines;
        private readonly int _lineWidth;
        private (int X, int Y)[] _steppingRules;
        
        public Three(IEnumerable<string> lines)
        {
            _lineWidth = lines.First().Length;
            _lines = lines.Skip(1);
        }

        public void Run(bool second = false) {
            if(second) {
                _steppingRules = new (int, int)[] { (1, 1), (3, 1), (5, 1), (7, 1), (1, 2) };
                CalculateTreesHit();
            } else {
                _steppingRules = new (int, int)[] { (3, 1) };
                CalculateTreesHit();
            }
        }

        public void CalculateTreesHit() {
            var collisionCounters = new int[_steppingRules.Length];
            var xPositions = new int[_steppingRules.Length];
            var yCounters = _steppingRules.Select(r => r.Y).ToArray();

            foreach (var line in _lines)
            {
                var ruleIndex = 0;
                foreach (var rule in _steppingRules) {
                    var y = yCounters[ruleIndex] -1;
                    
                    if(y == 0) {
                        var x = (xPositions[ruleIndex] + rule.X) % _lineWidth;

                        y = rule.Y;
                        if(line[x] == '#') {
                            collisionCounters[ruleIndex]++;
                        }

                        xPositions[ruleIndex] = x;
                    }

                    yCounters[ruleIndex] = y;
                    ruleIndex++;
                }
            }

            Console.WriteLine(collisionCounters.Aggregate((long) 1, (sum, rc) => sum * rc));
        }
    }
}