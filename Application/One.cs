using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020.Application
{
    public class One : IEventApp
    {
        private readonly IEnumerable<string> _lines;
        
        private readonly int _target;
        private readonly Dictionary<int, bool> _dict = new Dictionary<int, bool>();

        public One(IEnumerable<string> lines, int target)
        {
            _lines = lines;

            _target = target;
        }

        public void Run(bool second = false) {
            if(second) {
                Second();
            } else {
                First();
            }
        }

        public int ParseLine(string s) {
            return int.Parse(s);
        }
        
        public void First() {
            var numbers = _lines.Select(l => ParseLine(l));
            var target = _target;

            foreach(var number in numbers) {
                var complement = target - number;
                if(_dict.ContainsKey(complement)) {
                    Console.WriteLine(number * complement);
                    return;
                }
                _dict[number] = true;
            }
            throw new ArgumentException();
        }
        
        public void Second() {
            var numbers = _lines.Select(l => ParseLine(l));
            var target = _target;

            foreach(var number in numbers) {
                var complement = target - number;
                var sum = ContainsPairAndSum(complement);
                if(sum != -1) {
                    Console.WriteLine(number * sum);
                    return;
                }
                _dict[number] = true;
            }
            throw new ArgumentException();
        }

        private int ContainsPairAndSum(int target) {
            foreach(int stored in _dict.Keys)
            {
                if(_dict.ContainsKey(target-stored)) {
                    return stored * (target-stored);
                }
            }
            return -1;
        }
    }
}