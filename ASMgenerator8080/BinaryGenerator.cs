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

        private static readonly ArrayList opsRegDst = new ArrayList() { "inr", "dcr" };
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

        private ArrayList mem;
        private int startAddr = 0x2100;
        private int LabelsCount = 0;
        private ArrayList textlabels;
        private Dictionary<string, int> labels;
        private ArrayList references;
        private ArrayList resolveTable;

        public BinaryGenerator()
        {
            references = new ArrayList();
            resolveTable = new ArrayList();
            textlabels = new ArrayList();
            labels = new Dictionary<string,int>();
            mem = new ArrayList();
        }

        public void generateBinary(string s, int addr, int line)
        {
            if (line < 0) return;
            setStartAddress(addr);
            mem.Clear();
            parseInstruction(s, addr, line);
        }

        public ArrayList getBinaryDump()
        {
            return mem;
        }

        public void setStartAddress(int startAddress)
        {
            if (startAddress >= minAddr && startAddress < maxAddr)
            {
                startAddr = startAddress;
            } else throw new BinaryGeneratorException();
        }

        public int getStartAddress()
        {
            return startAddr;
        }

        private void setmem16(int addr, int immediate) 
        {
            if (immediate >= 0) {
                if (mem.Capacity < addr - startAddr + 1) mem.Capacity = addr - startAddr + 2;
                mem.Insert(addr - startAddr, (char)(immediate & 0xff));
                mem.Insert(addr - startAddr + 1, (char)(immediate >> 8));
            } else {
                if (mem.Capacity < addr - startAddr + 1) mem.Capacity = addr - startAddr + 2;
                mem.Insert(addr - startAddr, (char)immediate);
                mem.Insert(addr - startAddr + 1, (char)immediate);
            }
        }
        
        private void setmem8(int addr, int immediate) {
            if (mem.Capacity < addr - startAddr) mem.Capacity = addr - startAddr + 1;
            mem.Insert(addr - startAddr, (char)(immediate < 0 ? immediate : immediate & 0xff));
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

        private int markLabel(string identifier, int address, int linenumber, bool _override = false) {
            Regex rgx1 = new Regex(@"/\$([0-9a-fA-F]+)/");
            Regex rgx2 = new Regex(@"/(^|[^'])(\$|\.)/");

            identifier = rgx1.Replace(identifier, "0x$1");
            identifier = rgx2.Replace(identifier, " "+address+" ");
            var number = resolveNumber(identifier.Trim());
            if (number != -1) return number;
  
            LabelsCount++;
            address = -1 - LabelsCount;
 
            identifier = identifier.ToLower();
            
            try 
            {
                int found = labels[identifier];
                if (address >= 0) {
                    resolveTable[-found] = address;
                } else {
                    address = found;
                }

                if (found == 0 || _override) {
                    labels[identifier] = address;
                }


            } catch (KeyNotFoundException) {};
            
            textlabels[linenumber] = identifier;
  
            return address;
        }

        private void referencesLabel(string identifier, int linenumber) {
            identifier = identifier.ToLower();

            if (references.Capacity < linenumber)
                references.Insert(linenumber, identifier);
        }

        private int FromBinary(string val) {
            int x = 0;
            int n = 1;
            for (int i = val.Length - 1; i >= 0; i--) {
                if (val[i] == '1')
                    x += n;
                else if (val[i] != '0') 
                    //return Convert.ToInt32("abc");
                    throw new BinaryGeneratorException();
                n *= 2;
            }
            return Convert.ToInt32(x);
        }

        private int parseDeclDB(string[] args, int addr, int linenumber, int dw) {
            string text = String.Join(" ", args, 1, args.Length-1);
            string arg = "";
            int mode = 0;
            char cork = (char)0;
            int nbytes = 0;
            int len = 0;

            for (int i = 0; i < text.Length; i++) {
                switch (mode) {
                case 0:
                    if (text[i] == '"' || text[i] == '\'') {
                        mode = 1; cork = text[i];
                        break;
                    } else if (text[i] == ',') {
                        len = tokenDBDW(arg, addr+nbytes, dw, linenumber);
                        if (len < 0) {
                            return -1;
                        }
                        nbytes += len;
                        arg = "";
                    } else if (text[i] == ';') {
                        i = text.Length;
                        break;
                    } else {
                        arg += text[i];
                    }
                    break;
                case 1:
                    if (text[i] != cork) {
                        arg += text[i]; 
                    } else {
                        cork = (char)0;
                        mode = 0;
                        len = tokenString(arg, addr+nbytes, linenumber);
                        if (len < 0) {
                            return -1;
                        }
                        nbytes += len;
                        arg = "";
                    }
                    break; 
                }
            }
            if (mode == 1) return -1;    // unterminated string
            len = tokenDBDW(arg, addr+nbytes, dw, linenumber);
            if (len < 0) return -1;
                nbytes += len;
 
            return nbytes;
        }

        private int tokenString(string s, int addr, int linenumber) {
            for (int i = 0; i < s.Length; i++) {
                setmem8(addr+i, s[i]);
            }
            return s.Length;
        }

        private int tokenDBDW(string s, int addr, int len, int linenumber) {
            int size = -1;
 
            if (s.Length == 0) return 0;
 
            int n = markLabel(s, addr, 0);
            referencesLabel(s, linenumber);
 
            //if (len == undefined) len = 1;
 
            if (len == 1 && n < 256) {
                setmem8(addr, n);
                size = 1;
            } else if (len == 2 && n < 65536) {
                setmem16(addr, n); 
                size = 2;
            }
 
            return size;
        }

        private int resolveNumber(string identifier)
        {
            if (identifier == "" || identifier.Length == 0) return -1;
    
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
                        for (int i = oct.Length; --i >= 0;) {
                            if (oct[i] == '8' || oct[i] == '9') return -1;
                        }
                        return Convert.ToInt32('0' + identifier.Substring(0, identifier.Length-1));
                    } catch(Exception) {}
                    break;
                }
            }
            return -1;
        }

        /*private int evaluateExpression(string input, int addr) {
            string[] q;
            string originput = input;
            // console.log("input=" + input + " addr=" + addr);
            Regex rgx1 = new Regex(@"/\$([0-9a-fA-F]+)/");
            Regex rgx2 = new Regex(@"/(^|[^'])\$|\./gi");
            Regex rgx3 = new Regex(@"/([\d\w]+)\s(shr|shl|and|or|xor)\s([\d\w]+)/gi");

            //identifier = rgx1.Replace(identifier, "0x$1");

            try {
                input = rgx1.Replace(input, "0x$1");
                input = rgx2.Replace(input, " "+addr+" ");
                input = rgx3.Replace(/([\d\w]+)\s(shr|shl|and|or|xor)\s([\d\w]+)/gi,"($1 $2 $3)");
                input = input.replace(/\b(shl|shr|xor|or|and|[+\-*\/()])\b/gi,
                    function(m) {
                        switch (m) {
                        case 'and':
                            return '&';
                        case 'or':
                            return '|';
                        case 'xor':
                            return '^';
                        case 'shl':
                            return '<<';
                        case 'shr':
                            return '>>';
                        default:
                            return m;
                        }
                    });
                q = input.split(/<<|>>|[+\-*\/()\^\&]/);
            } catch (e) {
                return -1;
            }
            input = input;
            var expr = '';
            for (var ident = 0; ident < q.length; ident++) {
                var qident = q[ident].trim();
                if (-1 != resolveNumber(qident)) continue;
                var addr = labels[qident];//.indexOf(qident);
                if (addr != undefined) {
                    //addr = labels[idx+1];
                    if (addr >= 0) {
                        expr += 'var _' + qident + '=' + addr +';\n';
                        var rx = new RegExp('\\b'+qident+'\\b', 'gm');
                        input = input.replace(rx, '_' + qident);
                    } else {
                        expr = false;
                        break;
                    }
                }
            }
            // console.log('0 input=',  input);
            // console.log('1 expr=', expr);
            expr += input.replace(/[0-9][0-9a-fA-F]*[hbqdHBQD]|'.'/g,
                function(m) {
                    return resolveNumber(m);
                });
            // console.log('expr=', expr);
            try {
                return eval(expr.toLowerCase());
            } catch (err) {
                // console.log('expr was:',expr.toLowerCase(), originput);
                // console.log(err);
            }
 
            return -1;
        }*/

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

        private int parseRegister(string s) {
            if (s.Length == 0) return -1;
                s = s.ToLower();
            return "bcdehlma".IndexOf(s[0]);
        }

        private int parseInstruction(string s, int addr, int linenumber)
        {
            s = s.Trim();
            if (s.Length == 0) return 0;

            string pattern = "[ \t]*,[ \t]*|[ \t]+";
            string[] parts = Regex.Split(s, pattern, RegexOptions.None);
            int partsLen = parts.Length;

            for (var i = 0; i < partsLen; i++)
            {
                if (parts[i][0] == ';')
                {
                    partsLen = i;
                    break;
                }
            }

            string labelTag = "";
            int immediate = 0;

            for (; partsLen > 0; )
            {
                string mnemonic = parts[0].ToLower();
                if (mem.Capacity < addr - startAddr) mem.Capacity = addr - startAddr + 1;

                // no operands
                try
                {
                    mem.Insert(addr - startAddr, ops0[mnemonic]);
                    return 1;
                }
                catch (KeyNotFoundException) { }

                // immediate word
                try
                {
                    mem.Insert(addr - startAddr, opsIm16[mnemonic]);
                    immediate = useExpr(parts, addr, linenumber);
                    setmem16(addr + 1, immediate);
                    return 3;
                }
                catch (KeyNotFoundException) { }

                // register pair <- immediate
                try
                {
                    if (partsLen < 2) return -3;
                    int rp = parseRegisterPair(parts[1]);
                    if (rp == -1) return -3;
                    mem.Insert(addr - startAddr, opsRpIm16[mnemonic] | (rp << 4));
                    immediate = useExpr(parts, addr, linenumber);
                    setmem16(addr + 1, immediate);
                    return 3;
                }
                catch (KeyNotFoundException) { }

                // immediate byte    
                try
                {
                    mem.Insert(addr - startAddr, opsIm8[mnemonic]);
                    immediate = useExpr(parts, addr, linenumber);
                    setmem8(addr + 1, immediate);
                    return 2;
                }
                catch (KeyNotFoundException) { }

                // single register, im8
                try
                {
                    if (partsLen < 2) return -2;
                    int reg = parseRegister(parts[1]);
                    if (reg == -1) return -2;
                    mem.Insert(addr - startAddr, opsRegIm8[mnemonic] | reg << 3);
                    immediate = useExpr(parts, addr, linenumber);
                    setmem8(addr + 1, immediate);
                    return 2;
                }
                catch (KeyNotFoundException) { };

                // dual register (mov)
                try
                {
                    if (partsLen < 2) return -1;
                    int reg1 = parseRegister(parts[1].Trim());
                    int reg2 = parseRegister(parts[2].Trim());
                    if (reg1 == -1 || reg2 == -1) return -1;
                    mem.Insert(addr - startAddr, opsRegReg[mnemonic] | reg1 << 3 | reg2);
                    return 1;
                }
                catch (KeyNotFoundException) { };

                // single register
                try
                {
                    int reg = parseRegister(parts[1]);
                    if (reg == -1) return -1;
                    if (opsRegDst.IndexOf(mnemonic) != -1) reg <<= 3;
                    mem.Insert(addr - startAddr, opsReg[mnemonic] | reg);
                    return 1;
                }
                catch (KeyNotFoundException) { };

                // single register pair
                try
                {
                    int rp = parseRegisterPair(parts[1]);
                    if (rp == -1) return -1;
                    mem.Insert(addr - startAddr, opsRp[mnemonic] | rp << 4);
                    return 1;
                }
                catch (KeyNotFoundException) { };

                // rst
                if (mnemonic == "rst")
                {
                    int n = resolveNumber(parts[1]);
                    if (n >= 0 && n < 8)
                    {
                        mem.Insert(addr - startAddr, 0xC7 | n << 3);
                        return 1;
                    }
                    return -1;
                }

                if (mnemonic == ".org" || mnemonic == "org")
                {
                    int n = resolveNumber(parts[1]);
                    if (n >= 0)
                    {
                        return -100000 - n;
                    }
                    return -1;
                }


                // assign immediate value to label
                /*if (mnemonic == ".equ" || mnemonic == "equ") {
                    if (labelTag == "") return -1;
                    var value = evaluateExpression(String.Join(" ", parts, 1, parts.Length-1), addr);
                    markLabel(labelTag, value, linenumber, true);
                    return 0;
                }*/

                if (mnemonic == "cpu" ||
                    mnemonic == "aseg" ||
                    mnemonic == ".aseg") return 0;

                if (mnemonic == "db" || mnemonic == ".db" || mnemonic == "str")
                {
                    return parseDeclDB(parts, addr, linenumber, 1);
                }
                if (mnemonic == "dw" || mnemonic == ".dw")
                {
                    return parseDeclDB(parts, addr, linenumber, 2);
                }
                /*if (mnemonic == "ds" || mnemonic == ".ds") {
                    var size = evaluateExpression(String.Join(" ", parts, 1, parts.Length-1), addr);
                    if (size >= 0) {
                        for (var i = 0; i < size; i++) {
                            setmem8(addr+i, 0);
                        }
                        return size;
                    }
                    return -1;
                }*/

                if (parts[0][0] == ';') return 0;

                // nothing else works, it must be a label
                if (labelTag == "")
                {
                    string[] splat = mnemonic.Split(':');
                    labelTag = splat[0];
                    markLabel(labelTag, addr, linenumber);

                    parts[0] = String.Join(":", parts, 1, parts.Length - 1);
                    continue;
                }

                mem.Insert(addr - startAddr, -2);
                return -1; // error
            }
            return 0;
        }
    }
}
