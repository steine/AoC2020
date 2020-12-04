using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020.Application
{
    public class Four : IEventApp
    {
        public class Passport {
            public string Byr { get; set; }
            public string Iyr { get; set; }
            public string Eyr { get; set; }
            public string Hgt { get; set; }
            public string Hcl { get; set; }
            public string Ecl { get; set; }
            public string Pid { get; set; }
            public string Cid { get; set; }

            public void Set(string property, string value) {
                var capitalisedProperty = Char.ToUpper(property[0]) + property.Substring(1);
                GetType().GetProperty(capitalisedProperty).GetSetMethod().Invoke(this, new object[] { value });
            }
        }

        private readonly IEnumerable<string> _lines;
        
        public Four(IEnumerable<string> lines)
        {
            _lines = lines;
        }
        
        public IEnumerable<Passport> ParsePassports(IEnumerable<string> lines) {
            var r = new Regex(@"(\w+):(#?\w+)\s");
            
            var currentPassport = new Passport();
            var resetCurrent = false;
            foreach(var line in lines) {
                if(resetCurrent) {
                    resetCurrent = false;
                    currentPassport = new Passport();
                }

                var matches = r.Matches(line+" ");
                foreach (Match match in matches)
                {
                    currentPassport.Set(match.Groups[1].Value, match.Groups[2].Value);
                }

                if(line == "") {
                    resetCurrent = true;
                    yield return currentPassport;
                }
            }
            yield return currentPassport;
        }

        public void Run(bool second = false) {
            if(second) {
                ValidatePassports(SmarterPassportValidator);
            } else {
                ValidatePassports(NaivePassportValidator);
            }
        }

        public void ValidatePassports(Func<Passport, bool> validator) {
            var count = 0;
            var passports = ParsePassports(_lines);
            foreach (var passport in passports)
            {
                if(validator(passport)) {
                    count++;
                }
            }

            Console.WriteLine(count);
        }

        public bool NaivePassportValidator(Passport pass) {
            return (
                pass.Byr != null &&
                pass.Iyr != null &&
                pass.Eyr != null &&
                pass.Hgt != null &&
                pass.Hcl != null &&
                pass.Ecl != null &&
                pass.Pid != null 
            );
        }

        public bool SmarterPassportValidator(Passport pass) {
            if(
                !NaivePassportValidator(pass) ||
                !ByrIsValid(pass) ||
                !IyrIsValid(pass)
            ) {
                return false;
            }
            
            var eyr = int.Parse(pass.Eyr);
            if(2020 > eyr || eyr > 2030) {
                return false;
            }
            
            var hgtRegex = new Regex(@"(\d+)(cm|in)\b");
            var hgtPass = hgtRegex.IsMatch(pass.Hgt);
            if(!hgtPass) {
                return false;
            }
            var hgtMatch = hgtRegex.Match(pass.Hgt);
            var hgt = int.Parse(hgtMatch.Groups[1].Value);
            var unit = hgtMatch.Groups[2].Value;
            if(unit == "cm") {
                if(150 > hgt || hgt > 193) {
                    return false;
                }
            }
            if(unit == "in") {
                if(59 > hgt || hgt > 76) {
                    return false;
                }
            }
            
            var hclRegex = new Regex(@"(#[0-9a-f]{6})");
            var hclPass = hclRegex.IsMatch(pass.Hcl);
            if(!hclPass || pass.Hcl.Length != 7) {
                return false;
            }

            var eclRegex = new Regex(@"(amb|blu|brn|gry|grn|hzl|oth)");
            var eclPass = eclRegex.IsMatch(pass.Ecl);
            if(!eclPass || pass.Ecl.Length != 3) {
                return false;
            }

            var pidRegex = new Regex(@"[0-9]{9}");
            var pidPass = pidRegex.IsMatch(pass.Pid);
            if(!pidPass || pass.Pid.Length != 9) {
                return false;
            }
            return true;
        }

        public bool ByrIsValid(Passport pass) {
            var byr = int.Parse(pass.Byr);
            return 1920 <= byr && byr <= 2002;
        }
        public bool IyrIsValid(Passport pass) {
            var iyr = int.Parse(pass.Iyr);
            return 2010 <= iyr && iyr <= 2020;
        }
    }
}