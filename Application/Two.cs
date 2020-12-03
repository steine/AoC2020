using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020.Application
{
    public class Two : IEventApp
    {
        public class PolicyAndPass {
            public int Min { get; set; }
            public int Max { get; set; }
            public char Letter { get; set; }
            public string Password { get; set; }
        }

        private readonly IEnumerable<string> _lines;

        public Two(IEnumerable<string> lines)
        {
            _lines = lines;
        }

        public void Run(bool second = false) {
            if(second) {
                Second();
            } else {
                First();
            }
        }

        public PolicyAndPass ParseLine(string s) {
            var res = new PolicyAndPass();
            
            var r = new Regex(@"(\d+)-(\d+) (\w): (\w+)");
            var match = r.Match(s);

            res.Min = int.Parse(match.Groups[1].Value);
            res.Max = int.Parse(match.Groups[2].Value);
            res.Letter = char.Parse(match.Groups[3].Value);
            res.Password = match.Groups[4].Value;

            return res;
        }

        public void First() {
            var list = _lines.Select(l => ParseLine(l));
            var count = 0;
            foreach (var policyAndPass in list)
            {
                if(SledRental_IsValid(policyAndPass)) {
                    count++;
                }
            }
            Console.WriteLine(count);
        }

        public bool SledRental_IsValid(PolicyAndPass policyAndPass) {
            var count = 0;

            foreach (var letter in policyAndPass.Password)
            {
                if (letter == policyAndPass.Letter) {
                    count++;
                }
            }

            return policyAndPass.Min <= count && count <= policyAndPass.Max;
        }
        
        public void Second() {
            var list = _lines.Select(l => ParseLine(l));
            var count = 0;
            foreach (var policyAndPass in list)
            {
                if(Toboggan_IsValid(policyAndPass)) {
                    count++;
                }
            }
            Console.WriteLine(count);
        }

        public bool Toboggan_IsValid(PolicyAndPass policyAndPass) {
            return policyAndPass.Password[policyAndPass.Min-1] == policyAndPass.Letter ^
                policyAndPass.Password[policyAndPass.Max-1] == policyAndPass.Letter;
        }
    }
}