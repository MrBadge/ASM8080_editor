using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;

namespace ASMgenerator8080
{
    class BinaryGenerator
    {
        private const int minAddr = 0x2100;
        private const int maxAddr = 0x10000;
        private static readonly Dictionary<string, char> ops0 = new Dictionary<string, char>()
        { 
            { "nop" , (char)0x00 },
            { "hlt" , (char)0x76 },
            { "ei"  , (char)0xfb },
            { "di"  , (char)0xf3 },
            { "sphl", (char)0xf9 },
            { "xchg", (char)0xeb },
            { "xthl", (char)0xe3 },
            { "daa" , (char)0x27 },
            { "cma" , (char)0x2f },
            { "stc" , (char)0x37 },
            { "cmc" , (char)0x3f },
            { "rlc" , (char)0x07 },
            { "rrc" , (char)0x0f },
            { "ral" , (char)0x17 },
            { "rar" , (char)0x1f },
            { "pchl", (char)0xe9 },
            { "ret" , (char)0xc9 },
            { "rnz" , (char)0xc0 },
            { "rz"  , (char)0xc8 },
            { "rnc" , (char)0xd0 },
            { "rc"  , (char)0xd8 },
            { "rpo" , (char)0xe0 },
            { "rpe" , (char)0xe8 },
            { "rp"  , (char)0xf0 },
            { "rm"  , (char)0xf8 }
        };

        private static readonly Dictionary<string, char> opsIm16 = new Dictionary<string, char> ()
        {
            { "lda" , (char)0x3a },
            { "sta" , (char)0x32 },
            { "lhld", (char)0x2a },
            { "shld", (char)0x22 },
            { "jmp" , (char)0xc3 },
            { "jnz" , (char)0xc2 },
            { "jz"  , (char)0xca },
            { "jnc" , (char)0xd2 },
            { "jc"  , (char)0xda },
            { "jpo" , (char)0xe2 },
            { "jpe" , (char)0xea },
            { "jp"  , (char)0xf2 },
            { "jm"  , (char)0xfa },
            { "call", (char)0xcd },
            { "cnz" , (char)0xc4 },
            { "cz"  , (char)0xcc },
            { "cnc" , (char)0xd4 },
            { "cc"  , (char)0xdc },
            { "cpo" , (char)0xe4 },
            { "cpe" , (char)0xec },
            { "cp"  , (char)0xf4 },
            { "cm"  , (char)0xfc }
        };

        private static readonly Dictionary<string, char> opsRpIm16 = new Dictionary<string, char>()
        {
            { "lxi", (char)0x01 }
        };

        private static readonly Dictionary<string, char> opsIm8 = new Dictionary<string, char>()
        {
            { "adi", (char)0xc6 },
            { "aci", (char)0xce },
            { "sui", (char)0xd6 },
            { "sbi", (char)0xde },
            { "ani", (char)0xe6 },
            { "xri", (char)0xee },
            { "ori", (char)0xf6 },
            { "cpi", (char)0xfe },
            { "in" , (char)0xdb },
            { "out", (char)0xd3 }
        };

        private static readonly Dictionary<string, char> opsRegIm8 = new Dictionary<string, char>()
        {
            { "mvi", (char)0x06 }
        };

        private static readonly Dictionary<string, char> opsRegReg = new Dictionary<string, char>()
        {
            { "mov", (char)0x40 }
        };

        private static readonly Dictionary<string, char> opsReg = new Dictionary<string, char>()
        {
            { "add", (char)0x80 },
            { "adc", (char)0x88 },
            { "sub", (char)0x90 },
            { "sbb", (char)0x98 },
            { "ana", (char)0xa0 },
            { "xra", (char)0xa8 },
            { "ora", (char)0xb0 },
            { "cmp", (char)0xb8 },
            { "inr", (char)0x04 },
            { "dcr", (char)0x05 }
        };

        private static readonly Dictionary<string, char> opsRp = new Dictionary<string, char>()
        {
            { "ldax", (char)0x0A },
            { "stax", (char)0x02 },
            { "dad" , (char)0x09 },
            { "inx" , (char)0x03 },
            { "dcx" , (char)0x0b },
            { "push", (char)0xc5 },
            { "pop" , (char)0xc1 }
        };

        private char[] binaryInstruction;
        private ArrayList mem;
        private int startAddr;
        private int LabelsCount = 0;
        private Object[] labels;
        
        public BinaryGenerator(int startAddress)
        {
            if (startAddress >= minAddr && startAddress < maxAddr)
            {
                startAddr = startAddress;
                mem = new ArrayList();
                binaryInstruction = new char[1];
            }
            else throw new BinaryGeneratorException();
        }

        public char[] getBinary(string instruction)
        {
            return binaryInstruction;
        }

        private void setmem16(char immediate) 
        {
            if (immediate >= 0) {
                mem.Add(immediate & 0xff);
                mem.Add(immediate >> 8);
            } else {
                mem.Add(immediate);
                mem.Add(immediate);
            }
        }
        
        private string getExpr(string[] s) {
                string ex = String.Join(" ", s, 1, s.Length-1);
                if (ex[0] == '"' || ex[0] == '\'') {
                    return ex;
                }
                return ex.Split(';')[0];
            }
 
        private int useExpr(string[] s, int addr, int linenumber) {
            string expr = getExpr(s);
            if (expr.Length == 0) return 0;
 
            int immediate = markLabel(expr, addr, linenumber);
            referencesLabel(expr, linenumber);
            return immediate;
        }

        private int markLabel(string identifier, int address, int linenumber) {
            Regex rgx1 = new Regex(@"/\$([0-9a-fA-F]+)/");
            Regex rgx2 = new Regex(@"/(^|[^'])(\$|\.)/");

            identifier = rgx1.Replace(identifier, "0x$1");
            identifier = rgx2.Replace(identifier, " "+address+" ");
          var number = resolveNumber(identifier.Trim());
          if (number != -1) return number;
  
            LabelsCount++;
            address = -1 - LabelsCount;
 
            identifier = identifier.ToLower();
  
          var found = labels[identifier];
            if (found != undefined) {
                if (address >= 0) {
                    resolveTable[-found] = address;
                } else {
                    address = found;
                }
            }
 
          if (!found || override) {
                labels[identifier] = address;
          }
 
            if (linenumber != undefined) {
                textlabels[linenumber] = identifier;
            }
  
          return address;
        }

        private void referencesLabel(string identifier, int linenumber) {
            identifier = identifier.ToLower();
            if (references[linenumber] == undefined) {
                references[linenumber] = identifier;
            }
        }

        private int FromBinary(string val) {
            int x = 0;
            int n = 1;
            for (int i = val.Length - 1; i >= 0; i--) {
                if (val[i] == '1')
                    x += n;
                else if (val[i] != '0') 
                    return Convert.ToInt32("abc");
                n *= 2;
            }
            return Convert.ToInt32(x);
        }

        private int resolveNumber(string identifier)
        {
            if (identifier == "" || identifier.Length == 0) return;
    
            if ((identifier[0].Equals("'") || identifier[0].Equals("'"))
                && identifier.Length == 3)
            {
                return (0xff & identifier[1]);
            }
 
            if (identifier[0] == '$') {
                identifier = "0x" + identifier.Substring(1, identifier.Length-1);
            }
 
          if ("0123456789".IndexOf(identifier[0]) != -1)
          {
              int test;
              test = Convert.ToInt32(identifier);

              var suffix = Convert.ToString(identifier[identifier.Length - 1]).ToLower();
                switch (suffix) {
                case "d":
                    test = Convert.ToInt32(identifier.Substring(0, identifier.Length-1));
                    break;
                case "h":
              test = Convert.ToInt32("0x" + identifier.Substring(0, identifier.Length-1));
                    break;
                case "b":
              test = FromBinary(identifier.Substring(0, identifier.Length-1));
                    break;
                case "q":
                    try {
                        var oct = identifier.Substring(0, identifier.Length-1);
                        for (var i = oct.Length; --i >= 0;) {
                            if (oct[i] == '8' || oct[i] == '9') return -1;
                        }
                        return Convert.ToInt32('0' + identifier.Substring(0, identifier.Length-1));
                    } catch(Exception e) {}
                    break;
                }
          }
        return -1;
    }
        private int parseRegisterPair(string s) {
            if (s != "") {
                s = s.Split(';')[0].ToLower();
              if (s == "b" || s == "bc") return 0;
                if (s == "d" || s == "de") return 1;
               if (s == "h" || s == "hl") return 2;
                    if (s == "sp"|| s == "psw" || s == "a") return 3;
            }
          return -1;
        }

        private int parseInstruction(string s, int addr, int linenumber)
        {
            s = s.Trim();
            if (s.Length == 0) return -1;

            string pattern = "[ \t]*,[ \t]*|[ \t]+";
            string[] parts = Regex.Split(s, pattern, RegexOptions.None);
            int partsLen = parts.Length;

            for (var i = 0; i < partsLen; i++)
            {
                if (parts[i][0] == ';') {
                    partsLen = i;
                    break;
            }
        }
        
        //var labelTag;
        //var immediate;

        string mnemonic = parts[0].ToLower();
 
        // no operands
        try 
        {
            mem.Add(ops0[mnemonic]);
            return 1;
        } catch (KeyNotFoundException) {}
    
        // immediate word
            try
            {
                mem.Add(opsIm16[mnemonic]);
                int immediate = useExpr(parts.Slice(1), addr, linenumber);
                //if (!immediate) return -3;

                setmem16(addr + 1, immediate);
                return 3;
            }
            catch (Exception e)
            {
                
            }
    
        // register pair <- immediate
        try
        {
            char opcs = opsRpIm16[mnemonic];
            if (parts.Length < 2) return -3;
            rp = parseRegisterPair(subparts[0]);
            if (rp == -1) return -3;
 
            mem.Add(("0x" + opcs) | (rp << 4));
 
                immediate = useExpr(subparts.slice(1), addr, linenumber);
 
            setmem16(addr+1, immediate);
            return 3;
        } catch (KeyNotFoundException) {}
 
        // immediate byte    
        if ((opcs = opsIm8[mnemonic]) != undefined) {
            mem[addr] = new Number("0x" + opcs);
                immediate = useExpr(parts.slice(1), addr, linenumber);
                //if (!immediate) return -2;
                setmem8(addr+1, immediate);
                return 2;
        }
 
        // single register, im8
        if ((opcs = opsRegIm8[mnemonic]) != undefined) {
            subparts = parts.slice(1).join(" ").split(",");
            if (subparts.length < 2) return -2;
            reg = parseRegister(subparts[0]);
            if (reg == -1) return -2;
 
            mem[addr] = new Number("0x" + opcs) | reg << 3;
 
                immediate = useExpr(subparts.slice(1), addr, linenumber);
 
                setmem8(addr+1, immediate);
            return 2;      
        }
        
        // dual register (mov)
        if ((opcs = opsRegReg[mnemonic]) != undefined) {
            subparts = parts.slice(1).join(" ").split(",");
            if (subparts.length < 2) return -1;
            reg1 = parseRegister(subparts[0].trim());
            reg2 = parseRegister(subparts[1].trim());
            if (reg1 == -1 || reg2 == -1) return -1;
            mem[addr] = new Number("0x" + opcs) | reg1 << 3 | reg2;
            return 1;
        }
 
        // single register
        if ((opcs = opsReg[mnemonic]) != undefined) {
            reg = parseRegister(parts[1]);
            if (reg == -1) return -1;
      
            if (opsRegDst.indexOf(mnemonic) != -1) {
            reg <<= 3;
            }
      
            mem[addr] = new Number("0x" + opcs) | reg;
            return 1;
        }
    
        // single register pair
        if ((opcs = opsRp[mnemonic]) != undefined) {
            rp = parseRegisterPair(parts[1]);
            if (rp == -1) return -1;
            mem[addr] = new Number("0x" + opcs) | rp << 4;
            return 1;
        }    
    
        // rst
        if (mnemonic == "rst") {
            n = resolveNumber(parts[1]);
            if (n >= 0 && n < 8) {
            mem[addr] = 0xC7 | n << 3;
            return 1;
            }
            return -1;
        }
    
        if (mnemonic == ".org" || mnemonic == "org") {
            n = resolveNumber(parts[1]);
            if (n >= 0) {
            return -100000-n;
            }
            return -1;
        }
 
                if (mnemonic == ".binfile") {
                    if (parts[1] != undefined && parts[1].trim().length > 0) {
                        binFileName = parts[1];
                    }
                    return -100000;
                }
 
                if (mnemonic == ".hexfile") {
                    if (parts[1] != undefined && parts[1].trim().length > 0) {
                        hexFileName = parts[1];
                    }
                    return -100000;
                }
 
                if (mnemonic == ".objcopy") {
                   objCopy = parts.slice(1).join(' '); 
                   return -100000;
                }
 
                if (mnemonic == ".postbuild") {
                    postbuild = parts.slice(1).join(' ');
                    return -100000;
                }
 
                if (mnemonic == ".nodump") {
                    doHexDump = false;
                    return -100000;
                }
 
                // assign immediate value to label
                if (mnemonic == ".equ" || mnemonic == "equ") {
                    if (labelTag == undefined) return -1;
                    var value = evaluateExpression(parts.slice(1).join(' '), addr);
                    markLabel(labelTag, value, linenumber, true);
                    return 0;
                }
 
                if (mnemonic == "cpu" ||
                    mnemonic == "aseg" ||
                    mnemonic == ".aseg") return 0;
 
                if (mnemonic == "db" || mnemonic == ".db" || mnemonic == "str") {
                    return parseDeclDB(parts, addr, linenumber, 1);
                }
                if (mnemonic == "dw" || mnemonic == ".dw") {
                    return parseDeclDB(parts, addr, linenumber, 2);
                }
                if (mnemonic == "ds" || mnemonic == ".ds") {
                    var size = evaluateExpression(parts.slice(1).join(' '), addr);
                    if (size >= 0) {
                        for (var i = 0; i < size; i++) {
                            setmem8(addr+i, 0);
                        }
                        return size;
                    }
                    return -1;
                }
    
                if (parts[0][0] == ";") {
                return 0;
            }
 
                // nothing else works, it must be a label
                if (labelTag == undefined) {
                    var splat = mnemonic.split(':');
                    labelTag = splat[0];
                    markLabel(labelTag, addr, linenumber);
 
                    parts.splice(0, 1, splat.slice(1).join(':'));
                    continue;
                }
    
                mem[addr] = -2;
            return -1; // error
          }
  
          return 0; // empty
    }
}
