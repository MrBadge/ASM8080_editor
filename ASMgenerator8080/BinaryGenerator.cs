﻿//using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text.RegularExpressions;

namespace ASMgenerator8080
{
    public class BinaryGenerator
    {
        

        private const string incorrect_ops = "Incorrect operands";
        private const string incorrect_const = "Incorrect constant";
        private const string miss_ops = "Missing operands";
        private const string to_much_ops = "Too many operands";
        private const string incorrect_cmd = "Unknown command";
        private const string incorrect_label = "Incorrect label";
        private const string unres_label = "Unresolved label";
        private const string unused_labels = "Unused labels";

        private const string long_num = "One register can't store value more than 0xFF";

        private static readonly ArrayList opsRegDst = new ArrayList {"inr", "dcr"};

        private static readonly Dictionary<string, byte> ops0 = new Dictionary<string, byte>
        {
            {"nop", 0x00},
            {"hlt", 0x76},
            {"ei", 0xfb},
            {"di", 0xf3},
            {"sphl", 0xf9},
            {"xchg", 0xeb},
            {"xthl", 0xe3},
            {"daa", 0x27},
            {"cma", 0x2f},
            {"stc", 0x37},
            {"cmc", 0x3f},
            {"rlc", 0x07},
            {"rrc", 0x0f},
            {"ral", 0x17},
            {"rar", 0x1f},
            {"pchl", 0xe9},
            {"ret", 0xc9},
            {"rnz", 0xc0},
            {"rz", 0xc8},
            {"rnc", 0xd0},
            {"rc", 0xd8},
            {"rpo", 0xe0},
            {"rpe", 0xe8},
            {"rp", 0xf0},
            {"rm", 0xf8}
        };

        private static readonly Dictionary<string, byte> opsIm16 = new Dictionary<string, byte>
        {
            {"lda", 0x3a},
            {"sta", 0x32},
            {"lhld", 0x2a},
            {"shld", 0x22},
            {"jmp", 0xc3},
            {"jnz", 0xc2},
            {"jz", 0xca},
            {"jnc", 0xd2},
            {"jc", 0xda},
            {"jpo", 0xe2},
            {"jpe", 0xea},
            {"jp", 0xf2},
            {"jm", 0xfa},
            {"call", 0xcd},
            {"cnz", 0xc4},
            {"cz", 0xcc},
            {"cnc", 0xd4},
            {"cc", 0xdc},
            {"cpo", 0xe4},
            {"cpe", 0xec},
            {"cp", 0xf4},
            {"cm", 0xfc}
        };

        private static readonly Dictionary<string, byte> opsRpIm16 = new Dictionary<string, byte>
        {
            {"lxi", 0x01}
        };

        private static readonly Dictionary<string, byte> opsIm8 = new Dictionary<string, byte>
        {
            {"adi", 0xc6},
            {"aci", 0xce},
            {"sui", 0xd6},
            {"sbi", 0xde},
            {"ani", 0xe6},
            {"xri", 0xee},
            {"ori", 0xf6},
            {"cpi", 0xfe},
            {"in", 0xdb},
            {"out", 0xd3}
        };

        private static readonly Dictionary<string, byte> opsRegIm8 = new Dictionary<string, byte>
        {
            {"mvi", 0x06}
        };

        private static readonly Dictionary<string, byte> opsRegReg = new Dictionary<string, byte>
        {
            {"mov", 0x40}
        };

        private static readonly Dictionary<string, byte> opsReg = new Dictionary<string, byte>
        {
            {"add", 0x80},
            {"adc", 0x88},
            {"sub", 0x90},
            {"sbb", 0x98},
            {"ana", 0xa0},
            {"xra", 0xa8},
            {"ora", 0xb0},
            {"cmp", 0xb8},
            {"inr", 0x04},
            {"dcr", 0x05}
        };

        private static readonly Dictionary<string, byte> opsRp = new Dictionary<string, byte>
        {
            {"ldax", 0x0a},
            {"stax", 0x02},
            {"dad", 0x09},
            {"inx", 0x03},
            {"dcx", 0x0b},
            {"push", 0xc5},
            {"pop", 0xc1}
        };

        //private ArrayList textlabels;
        private readonly Dictionary<string, int> labels;
        private readonly ArrayList mem;
        //private ArrayList references;
        //private ArrayList resolveTable;
        private readonly Dictionary<string, ArrayList> unresolvedLabels;
        private Dictionary<int, string> warnings;
        private int LabelsCount;
        private int currentAddr;
        private int startAddr = 0x2100;

        public BinaryGenerator()
        {
            //references = new ArrayList();
            //resolveTable = new ArrayList();
            //textlabels = new ArrayList();
            labels = new Dictionary<string, int>();
            warnings = new Dictionary<int, string>();
            unresolvedLabels = new Dictionary<string, ArrayList>();
            mem = new ArrayList();
        }

        public void generateBinary(string s, int addr = 0x2100)
        {
            clear();
            if (s == null) throw new BinaryGeneratorException("The string is empty");
            setStartAddress(addr);
            var rgx = new Regex("\r\n");
            string[] lines = rgx.Split(s);
            int size = 0;
            for (int i = 0; i < lines.Length; ++i)
            {
                size = parseInstruction(lines[i], currentAddr, i);
                currentAddr += size > 0 ? size : 0;
                //if (size < 0) 
                //throw new BinaryGeneratorException("Something went wrong here:(", i);
            }
            if (unresolvedLabels.Count != 0)
            {
                string label = unresolvedLabels.Keys.ElementAt(0);
                if (Char.IsDigit(label[0])) throw new BinaryGeneratorException(incorrect_const + ": \"" + label + "\"",
                    ((locationCode)unresolvedLabels[label][0]).linenumber);
                else throw new BinaryGeneratorException(unres_label + ": \"" + label + "\"",
                    ((locationCode) unresolvedLabels[label][0]).linenumber);
            }
        }

        public string[] getBinaryDumpToString(int len = 0)
        {
            int size = mem.Count/len + (mem.Count%len == 0 ? 0 : 1);
            var res = new string[size];
            if (len < 0) return null;

            int j = 0;
            for (int i = 0; i < mem.Count; ++i)
                if (mem[i] != null)
                {
                    if (len > 0 && i > 0 && i%len == 0) ++j;
                    res[j] += (byte)mem[i] < 0xf ? "0" + ((byte)mem[i]).ToString("X") : ((byte)mem[i]).ToString("X");
                    res[j] += " ";
                }
            return res;
        }

        public string[] getACIIDumpToString(int len = 0)
        {
            int size = mem.Count/len + (mem.Count%len == 0 ? 0 : 1);
            var res = new string[size];
            if (len < 0) return null;

            int j = 0;
            for (int i = 0; i < mem.Count; ++i)
                if (mem[i] != null)
                {
                    if (len > 0 && i > 0 && i%len == 0) ++j;
                    res[j] += (byte) mem[i] < 32 || (byte) mem[i] > 127 ? '.' : (char) (byte) mem[i];
                }
            return res;
        }

        public ArrayList getBinaryDump()
        {
            return mem;
        }

        public Dictionary<int, string> getWarnings()
        {
            return warnings;
        }

        public void clear()
        {
            mem.Clear();
            LabelsCount = 0;
            //textlabels.Clear();
            labels.Clear();
            warnings.Clear();
            //references.Clear();
            //resolveTable.Clear();
            unresolvedLabels.Clear();
        }

        private void setStartAddress(int startAddress)
        {
            if (startAddress >= Constants.minAddr && startAddress < Constants.maxAddr)
            {
                startAddr = startAddress;
                currentAddr = startAddr;
            }
            else throw new BinaryGeneratorException("Start adress is incorrect");
        }

        public int getStartAddress()
        {
            return startAddr;
        }

        private void setmem16(int addr, int immediate)
        {
            if (immediate >= 0)
            {
                if (mem.Count <= addr - startAddr + 1) fillArray(mem, addr - startAddr + 1);
                mem[addr - startAddr] = (byte) (immediate & 0xff);
                mem[addr - startAddr + 1] = (byte) (immediate >> 8);
            }
            else
            {
                if (mem.Count <= addr - startAddr + 1) fillArray(mem, addr - startAddr + 1);
                mem[addr - startAddr] = (byte) immediate;
                mem[addr - startAddr] = (byte) immediate;
            }
        }

        private void setmem8(int addr, int immediate)
        {
            if (mem.Count <= addr - startAddr) fillArray(mem, addr - startAddr);
            mem[addr - startAddr] = (byte) (immediate < 0 ? immediate : immediate & 0xff);
        }

        private string getExpr(string[] s, int index)
        {
            string ex = String.Join(" ", s, index, s.Length - index);
            if (ex.Length == 0) return null;

            if (ex[0] == '"' || ex[0] == '\'')
            {
                return ex;
            }
            return ex.Split(';')[0];
        }

        private int useExpr(string[] s, int addr, int linenumber, int index = 1)
        {
            string expr = getExpr(s, index);
            if (expr == null) throw new BinaryGeneratorException(miss_ops, linenumber);
            if (expr.Length == 0) return 0;

            int immediate = markLabel(expr, addr, linenumber);
            //referencesLabel(expr, linenumber);
            return immediate;
        }

        private int markLabel(string identifier, int address, int codeLine, int linenumber = -1, bool _override = false,
            bool isUseLabel = true)
        {
            var rgx1 = new Regex(@"/\$([0-9a-fA-F]+)/");
            var rgx2 = new Regex(@"/(^|[^'])(\$|\.)/");

            identifier = rgx1.Replace(identifier, "0x$1");
            identifier = rgx2.Replace(identifier, " " + address + " ");
            int number = resolveNumber(identifier.Trim(), codeLine);
            int instAddr = address;

            if (number != -1) return number;

            if (linenumber < 0)
            {
                LabelsCount++;
                address = -1 - LabelsCount;
            }

            identifier = identifier.ToLower();

            try
            {
                address = labels[identifier];
                //if (address >= 0)
                //{
                //    if (resolveTable.Count <= -found) fillArray(resolveTable, -found);
                //        resolveTable[-found] = address;

                //}
                //else
                //{
                //    address = found;
                //}
            }
            catch (KeyNotFoundException)
            {
                if (isUseLabel)
                {
                    if (unresolvedLabels.ContainsKey(identifier))
                        unresolvedLabels[identifier].Add(new locationCode(instAddr, codeLine));
                    else unresolvedLabels.Add(identifier, new ArrayList {new locationCode(instAddr, codeLine)});
                }
                else
                {
                    labels.Add(identifier, address);

                    try
                    {
                        ArrayList locations = unresolvedLabels[identifier];
                        foreach (locationCode locs in locations) setmem16(locs.address + 1, address);
                        unresolvedLabels.Remove(identifier);
                    }
                    catch (KeyNotFoundException)
                    {
                    }
                }
            }
            ;

            //if (linenumber >= 0)
            //{
            //    if (textlabels.Count <= linenumber) fillArray(textlabels, linenumber);
            //    textlabels[linenumber] = identifier;
            //}

            return address;
        }

        //private void referencesLabel(string identifier, int linenumber) {
        //    identifier = identifier.ToLower();

        //    if (references.Count <= linenumber)
        //    {
        //        fillArray(references, linenumber);
        //        references[linenumber] = identifier;
        //    }
        //}

        private void fillArray(ArrayList arr, int index)
        {
            int n = index - arr.Count + 1;
            if (n <= 0) return;

            for (int i = 0; i < n; ++i) arr.Add(null);
        }

        /*private int FromBinary(string val) {
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
        }*/

        private int parseDeclDB(string[] args, int addr, int linenumber, int dw)
        {
            string text = String.Join(" ", args, 1, args.Length - 1);
            if (text.Length == 0) throw new BinaryGeneratorException(miss_ops, linenumber);
            string arg = "";
            int mode = 0;
            var cork = (char) 0;
            int nbytes = 0;
            int len = 0;

            for (int i = 0; i < text.Length; i++)
            {
                switch (mode)
                {
                    case 0:
                        if (text[i] == '"' || text[i] == '\'')
                        {
                            mode = 1;
                            cork = text[i];
                        }
                        if (text[i] == ',')
                        {
                            len = tokenDBDW(arg, addr + nbytes, dw, linenumber);
                            if (len < 0)
                            {
                                return -1;
                            }
                            nbytes += len;
                            arg = "";
                        }
                        else if (text[i] == ';')
                        {
                            i = text.Length;
                        }
                        else
                        {
                            arg += text[i];
                        }
                        break;
                    case 1:
                        if (text[i] != cork)
                        {
                            arg += text[i];
                        }
                        else
                        {
                            cork = (char) 0;
                            mode = 0;
                            len = tokenString(arg, addr + nbytes, linenumber);
                            if (len < 0)
                            {
                                return -1;
                            }
                            nbytes += len;
                            arg = "";
                        }
                        break;
                }
            }
            if (mode == 1) return -1; // unterminated string
            len = tokenDBDW(arg, addr + nbytes, dw, linenumber);
            if (len < 0) return -1;
            nbytes += len;

            return nbytes;
        }

        private int tokenString(string s, int addr, int linenumber)
        {
            for (int i = 0; i < s.Length; i++)
            {
                setmem8(addr + i, s[i]);
            }
            return s.Length;
        }

        private int tokenDBDW(string s, int addr, int len, int linenumber)
        {
            int size = -1;

            if (s.Length == 0) return 0;

            int n = markLabel(s, addr, linenumber, 0);
            //referencesLabel(s, linenumber);

            //if (len == undefined) len = 1;

            if (len == 1 && n < 256)
            {
                setmem8(addr, n);
                size = 1;
            }
            else if (len == 2 && n < 65536)
            {
                setmem16(addr, n);
                size = 2;
            }

            return size;
        }

        private int resolveNumber(string identifier, int linenumber)
        {
            if (identifier == "" || identifier.Length == 0) return -1;

            if ((identifier[0].Equals("'") || identifier[0].Equals("'"))
                && identifier.Length == 3)
            {
                return (0xff & identifier[1]);
            }

            int num = -1;
            // support 0x numbers

            if (identifier.Length > 2 && identifier[0] == '0' && (identifier[1] == 'x' || identifier[1] == 'X'))
            {
                try
                {
                    num = Convert.ToInt32(identifier, 16);
                }
                catch (Exception)
                {
                    //if (Regex.Split(identifier, "  *").Length > 1)
                    //    throw new BinaryGeneratorException(to_much_ops, linenumber);
                    //throw new BinaryGeneratorException(incorrect_ops, linenumber);
                    return -1;
                }
            } else 

            // support 0 numbers 

            if (identifier.Length > 2 && identifier[0] == '0')
            {
                if ("qhbd".IndexOf(identifier[identifier.Length - 1]) == -1)
                    for (int i = identifier.Length; --i >= 0;)
                    {
                        if (identifier[i] == 'a' || identifier[i] == 'b' || identifier[i] == 'c' || identifier[i] == 'd' ||
                            identifier[i] == 'e') 
                            return -1;
                        return Convert.ToInt32(identifier, 10);
                    } 
                try
                {
                    switch (identifier[identifier.Length - 1])
                    {
                        case 'q':
                            num = Convert.ToInt32(identifier.Substring(0, identifier.Length - 1), 8);
                            break;
                        case 'h':
                            num = Convert.ToInt32(identifier.Substring(0, identifier.Length - 1), 16);
                            break;
                        case 'b':
                            num = Convert.ToInt32(identifier.Substring(0, identifier.Length - 1), 2);
                            break;
                        case 'd':
                            num = Convert.ToInt32(identifier.Substring(0, identifier.Length - 1), 10);
                            break;
                    }

                }
                catch (Exception)
                {
                    return -1;
                }
            } else 

            if (identifier[0] == '$')
            {
                identifier = "0x" + identifier.Substring(1, identifier.Length - 1);
            } else

            if ("0123456789".IndexOf(identifier[0]) != -1)
            {
                try
                {
                    num = Convert.ToInt32(identifier);
                }
                catch (Exception)
                {
                    try
                    {
                        string suffix = Convert.ToString(identifier[identifier.Length - 1]).ToLower();
                        switch (suffix)
                        {
                            case "d":
                                num = Convert.ToInt32(identifier.Substring(0, identifier.Length - 1));
                                break;
                            case "h":
                                num = Convert.ToInt32("0x" + identifier.Substring(0, identifier.Length - 1), 16);
                                break;
                            case "b":
                                num = Convert.ToInt32(identifier.Substring(0, identifier.Length - 1), 2);
                                break;
                            case "q":
                                string oct = identifier.Substring(0, identifier.Length - 1);
                                for (int i = oct.Length; --i >= 0;)
                                {
                                    if (oct[i] == '8' || oct[i] == '9') return -1;
                                }
                                num = Convert.ToInt32(identifier.Substring(0, identifier.Length - 1), 8);
                                break;
                        }
                    }
                    catch (Exception)
                    {
                    }
                    ;
                }
            }

            //if (num > 0xFF) warnings.Add(linenumber, /*"Number " + "\"" + identifier + "\" " + */long_num);
            return num;
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

        private int parseRegisterPair(string s)
        {
            if (s != "")
            {
                s = s.Split(';')[0].ToLower();
                if (s == "b" || s == "bc") return 0;
                if (s == "d" || s == "de") return 1;
                if (s == "h" || s == "hl") return 2;
                if (s == "sp" || s == "psw" || s == "a") return 3;
            }
            return -1;
        }

        private int parseRegister(string s)
        {
            if (s.Length == 0 || s.Length > 1) return -1;
            s = s.ToLower();
            return "bcdehlma".IndexOf(s[0]);
        }

        private string[] subArray(string[] s, int begin, int end)
        {
            if (begin < 0 || end > s.Length) throw new IndexOutOfRangeException();
            if (end < begin) return null;
            var res = new string[end - begin];

            for (int i = 0; i < res.Length; ++i)
            {
                res[i] = s[i + begin];
            }

            return res;
        }

        private int parseInstruction(string s, int addr, int linenumber)
        {
            s = s.Trim();
            if (s.Length == 0) return 0;

            s = Regex.Split(s, @"\s*;\s*", RegexOptions.None)[0];

            string pattern = "[ \t]*,[ \t]*|[ \t]+";
            string[] parts = Regex.Split(s, pattern, RegexOptions.None);
            int partsLen = parts.Length;

            //for (var i = 0; i < partsLen; i++)
            //{
            //    if (parts[i].Length > 0 && parts[i][0] == ';')
            //    {
            //        partsLen = i;
            //        break;
            //    }
            //}

            string labelTag = "";
            int immediate = 0;
            var opcs = (byte) 0;

            for (; partsLen > 0;)
            {
                string mnemonic = parts[0].ToLower();
                if (mnemonic.Length == 0)
                {
                    parts = subArray(parts, 1, parts.Length);
                    --partsLen;
                    continue;
                }

                if (mem.Count <= addr - startAddr)
                    fillArray(mem, addr - startAddr);

                // no operands
                try
                {
                    mem[addr - startAddr] = ops0[mnemonic];
                    if (partsLen > 1) throw new BinaryGeneratorException(to_much_ops, linenumber);
                    return 1;
                }
                catch (KeyNotFoundException)
                {
                }

                // immediate word
                try
                {
                    mem[addr - startAddr] = opsIm16[mnemonic];
                    if (partsLen > 2) throw new BinaryGeneratorException(to_much_ops, linenumber);
                    immediate = useExpr(parts, addr, linenumber);
                    setmem16(addr + 1, immediate);
                    return 3;
                }
                catch (KeyNotFoundException)
                {
                }

                // register pair <- immediate
                try
                {
                    opcs = opsRpIm16[mnemonic];
                    if (partsLen > 3) throw new BinaryGeneratorException(to_much_ops, linenumber);
                    //if (partsLen < 2) return -3;
                    if (partsLen < 3) throw new BinaryGeneratorException(miss_ops, linenumber);
                    int rp = parseRegisterPair(parts[1]);
                    //if (rp == -1) return -3;
                    if (rp == -1) throw new BinaryGeneratorException(incorrect_ops, linenumber);
                    mem[addr - startAddr] = (byte) (opcs | (rp << 4));
                    immediate = useExpr(parts, addr, linenumber, 2);
                    setmem16(addr + 1, immediate);
                    return 3;
                }
                catch (KeyNotFoundException)
                {
                }

                // immediate byte    
                try
                {
                    mem[addr - startAddr] = opsIm8[mnemonic];
                    if (partsLen > 2) throw new BinaryGeneratorException(to_much_ops, linenumber);
                    immediate = useExpr(parts, addr, linenumber);
                    if (immediate > 0xFF) warnings.Add(linenumber, /*"Number " + "\"" + identifier + "\" " + */long_num);
                    setmem8(addr + 1, immediate);
                    return 2;
                }
                catch (KeyNotFoundException)
                {
                }

                // single register, im8
                try
                {
                    opcs = opsRegIm8[mnemonic];
                    if (partsLen > 3) throw new BinaryGeneratorException(to_much_ops, linenumber);
                    //if (partsLen < 2) return -2;
                    if (partsLen < 2) throw new BinaryGeneratorException(miss_ops, linenumber);
                    int reg = parseRegister(parts[1]);
                    //if (reg == -1) return -2;
                    if (reg == -1) throw new BinaryGeneratorException(incorrect_ops, linenumber);
                    mem[addr - startAddr] = (byte) (opcs | reg << 3);
                    immediate = useExpr(parts, addr, linenumber, 2);
                    if (immediate > 0xFF) warnings.Add(linenumber, /*"Number " + "\"" + identifier + "\" " + */long_num);
                    setmem8(addr + 1, immediate);
                    return 2;
                }
                catch (KeyNotFoundException)
                {
                }
                ;

                // dual register (mov)
                try
                {
                    opcs = opsRegReg[mnemonic];
                    if (partsLen > 3) throw new BinaryGeneratorException(to_much_ops, linenumber);
                    //if (partsLen <= 2) return -1;
                    if (partsLen < 3) throw new BinaryGeneratorException(miss_ops, linenumber);
                    int reg1 = parseRegister(parts[1].Trim());
                    int reg2 = parseRegister(parts[2].Trim());
                    //if (reg1 == -1 || reg2 == -1) return -1;
                    if (reg1 == -1 || reg2 == -1) throw new BinaryGeneratorException(incorrect_ops, linenumber);
                    mem[addr - startAddr] = (byte) (opcs | reg1 << 3 | reg2);
                    return 1;
                }
                catch (KeyNotFoundException)
                {
                }
                ;

                // single register
                try
                {
                    opcs = opsReg[mnemonic];
                    if (partsLen > 2) throw new BinaryGeneratorException(to_much_ops, linenumber);
                    if (partsLen < 2) throw new BinaryGeneratorException(miss_ops, linenumber);
                    //if (parts[1].ToLower() == "hl")
                    //{
                    //    parts[1] = "m";
                    //}
                    int reg = parseRegister(parts[1]);
                    //if (reg == -1) return -1;
                    if (reg == -1) throw new BinaryGeneratorException(incorrect_ops, linenumber);
                    if (opsRegDst.IndexOf(mnemonic) != -1) reg <<= 3;
                    mem[addr - startAddr] = (byte) (opcs | reg);
                    return 1;
                }
                catch (KeyNotFoundException)
                {
                }
                ;

                // single register pair
                try
                {
                    opcs = opsRp[mnemonic];
                    if (partsLen > 2) throw new BinaryGeneratorException(to_much_ops, linenumber);
                    if (partsLen < 2) throw new BinaryGeneratorException(miss_ops, linenumber);
                    int rp = parseRegisterPair(parts[1]);
                    //if (rp == -1) return -1;
                    if (rp == -1) throw new BinaryGeneratorException(incorrect_ops, linenumber);
                    mem[addr - startAddr] = (byte) (opcs | rp << 4);
                    return 1;
                }
                catch (KeyNotFoundException)
                {
                }
                ;

                // rst
                if (mnemonic == "rst")
                {
                    if (partsLen > 2) throw new BinaryGeneratorException(to_much_ops, linenumber);
                    if (partsLen < 2) throw new BinaryGeneratorException(miss_ops, linenumber);
                    int n = resolveNumber(parts[1], linenumber);
                    if (n >= 0 && n < 8)
                    {
                        mem[addr - startAddr] = (byte) (0xC7 | n << 3);
                        return 1;
                    }
                    //return -1;
                    throw new BinaryGeneratorException(incorrect_ops, linenumber);
                }

                if (mnemonic == ".org" || mnemonic == "org")
                {
                    if (partsLen > 2) throw new BinaryGeneratorException(to_much_ops, linenumber);
                    if (partsLen < 2) throw new BinaryGeneratorException(miss_ops, linenumber);
                    int n = resolveNumber(parts[1], linenumber);
                    if (n >= 0)
                    {
                        return -100000 - n;
                    }
                    //return -1;
                    throw new BinaryGeneratorException(incorrect_ops, linenumber);
                }


                // assign immediate value to label
                /*if (mnemonic == ".equ" || mnemonic == "equ") {
                    if (labelTag == "") return -1;
                    var value = evaluateExpression(String.Join(" ", parts, 1, parts.Length-1), addr);
                    markLabel(labelTag, value, linenumber, linenumber, true);
                    return 0;
                }*/

                if (mnemonic == "cpu" ||
                    mnemonic == "aseg" ||
                    mnemonic == ".aseg") return 0;

                if (mnemonic == "db" || mnemonic == ".db" || mnemonic == "str")
                {
                    int res = parseDeclDB(parts, addr, linenumber, 1);
                    if (res == -1) throw new BinaryGeneratorException(incorrect_ops, linenumber);
                    return res;
                }
                if (mnemonic == "dw" || mnemonic == ".dw")
                {
                    int res = parseDeclDB(parts, addr, linenumber, 2);
                    if (res == -1) throw new BinaryGeneratorException(incorrect_ops, linenumber);
                    return res;
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

                if (parts[0].Length > 0 && parts[0][0] == ';') return 0;

                // nothing else works, it must be a label
                if (labelTag == "")
                {
                    //if (partsLen > 1) throw new BinaryGeneratorException(incorrect_label, linenumber);
                    string[] splat = mnemonic.Split(':');
                    labelTag = splat[0];
                    if (labelTag.Length > 0 && Char.IsDigit(labelTag[0]))
                        throw new BinaryGeneratorException(incorrect_label, linenumber);
                    markLabel(labelTag, addr, linenumber, linenumber, false, false);

                    parts[0] = String.Join(":", splat, 1, splat.Length - 1);
                    continue;
                }

                mem[addr - startAddr] = (byte) 0xFE;
                //return -1; // error
                throw new BinaryGeneratorException(incorrect_cmd, linenumber);
            }
            return 0;
        }

        private class locationCode
        {
            public locationCode(int addr, int lineNum)
            {
                address = addr;
                linenumber = lineNum;
            }

            public int address { set; get; }
            public int linenumber { set; get; }
        }
    }
}