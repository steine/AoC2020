using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020.Application
{
    public class Five : IEventApp
    {
        public class BoardingPass {
            public int Row { get; set; }
            public int Column { get; set; }
            public int SeatId() {
                return Row * 8 + Column;
            }
        }

        private readonly IEnumerable<string> _lines;
        
        public Five(IEnumerable<string> lines)
        {
            _lines = lines;
        }

        public void Run(bool second = false) {
            if(second) {
                FindMissingSeatId();
            } else {
                FindHighestSeatId();
            }
        }
        
        private IEnumerable<BoardingPass> Parse(IEnumerable<string> lines) {
            return lines.Select(l => new BoardingPass() {
                Row = ParseBSP(l.Substring(0,7), 'B'),
                Column = ParseBSP(l.Substring(7,3), 'R'),
            });
        }

        private int ParseBSP(string bsp, char one)
        {
            var worth = (int) Math.Pow(2, bsp.Length-1);
            var row = 0;
            foreach(var c in bsp) {
                if(c == one) {
                    row += worth;
                }
                worth /= 2;
            }

            return row;
        }

        private void FindHighestSeatId() {
            var boardingPasses = Parse(_lines);
            var highest = -1;

            foreach (var boardingPass in boardingPasses)
            {
                if(boardingPass.SeatId() > highest) {
                    highest = boardingPass.SeatId();
                }
            }

            Console.WriteLine(highest);
        }

        private void FindMissingSeatId()
        {
            var dict = new Dictionary<int, bool>();
            var boardingPasses = Parse(_lines);
            foreach (var bp in boardingPasses)
            {
                dict[bp.SeatId()] = true;
            }
            for(var sid=0; sid < 1024; sid++) {
                if(sid < 8 && sid > 1015) {
                    continue;
                }
                if(!dict.ContainsKey(sid) && dict.ContainsKey(sid-1) && dict.ContainsKey(sid+1)) {
                    Console.WriteLine(sid);
                }
            }
        }
    }
}