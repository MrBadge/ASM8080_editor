using System;
using System.Collections.Generic;
using System.Linq;

namespace ASMgenerator8080
{
    internal class DisAssembler
    {
        private static readonly Dictionary<byte, string> ops0 = new Dictionary<byte, string>
        {
            {0x00, "nop"},
            {0x76, "hlt"},
            {0xfb, "ei"},
            {0xf3, "di"},
            {0xf9, "sphl"},
            {0xeb, "xchg"},
            {0xe3, "xthl"},
            {0x27, "daa"},
            {0x2f, "cma"},
            {0x37, "stc"},
            {0x3f, "cmc"},
            {0x07, "rlc"},
            {0x0f, "rrc"},
            {0x17, "ral"},
            {0x1f, "rar"},
            {0xe9, "pchl"},
            {0xc9, "ret"},
            {0xc0, "rnz"},
            {0xc8, "rz"},
            {0xd0, "rnc"},
            {0xd8, "rc"},
            {0xe0, "rpo"},
            {0xe8, "rpe"},
            {0xf0, "rp"},
            {0xf8, "rm"}
        };

        private static readonly Dictionary<byte, string> opsIm16 = new Dictionary<byte, string>
        {
            {0x3a, "lda"},
            {0x32, "sta"},
            {0x2a, "lhld"},
            {0x22, "shld"},
            {0xc3, "jmp"},
            {0xc2, "jnz"},
            {0xca, "jz"},
            {0xd2, "jnc"},
            {0xda, "jc"},
            {0xe2, "jpo"},
            {0xea, "jpe"},
            {0xf2, "jp"},
            {0xfa, "jm"},
            {0xcd, "call"},
            {0xc4, "cnz"},
            {0xcc, "cz"},
            {0xd4, "cnc"},
            {0xdc, "cc"},
            {0xe4, "cpo"},
            {0xec, "cpe"},
            {0xf4, "cp"},
            {0xfc, "cm"}
        };

        private static readonly Dictionary<byte, string> opsRpIm16 = new Dictionary<byte, string>
        {
            {0x01, "lxi"}
        };

        private static readonly Dictionary<byte, string> opsIm8 = new Dictionary<byte, string>
        {
            {0xc6, "adi"},
            {0xce, "aci"},
            {0xd6, "sui"},
            {0xde, "sbi"},
            {0xe6, "ani"},
            {0xee, "xri"},
            {0xf6, "ori"},
            {0xfe, "cpi"},
            {0xdb, "in"},
            {0xd3, "out"}
        };

        private static readonly Dictionary<byte, string> opsRegIm8 = new Dictionary<byte, string>
        {
            {0x06, "mvi"}
        };

        private static readonly Dictionary<byte, string> opsRegReg = new Dictionary<byte, string>
        {
            {0x40, "mov"}
        };

        private static readonly Dictionary<byte, string> opsReg = new Dictionary<byte, string>
        {
            {0x80, "add"},
            {0x88, "adc"},
            {0x90, "sub"},
            {0x98, "sbb"},
            {0xa0, "ana"},
            {0xa8, "xra"},
            {0xb0, "ora"},
            {0xb8, "cmp"},
            {0x04, "inr"},
            {0x05, "dcr"}
        };

        private static readonly Dictionary<byte, string> opsRp = new Dictionary<byte, string>
        {
            {0x0a, "ldax"},
            {0x02, "stax"},
            {0x09, "dad"},
            {0x03, "inx"},
            {0x0b, "dcx"},
            {0xc5, "push"},
            {0xc1, "pop"}
        };

        private static readonly Dictionary<byte, string> RPnames = new Dictionary<byte, string>
        {
            {0, "BC"},
            {1, "DE"},
            {2, "HL"},
            {3, "AF"}
        };

        private static readonly Dictionary<byte, string> Rnames = new Dictionary<byte, string>
        {
            {0, "B"},
            {1, "C"},
            {2, "D"},
            {3, "E"},
            {4, "H"},
            {5, "L"},
            {6, "M"},
            {7, "A"}
        };

        private static readonly Dictionary<byte, string> CondNames = new Dictionary<byte, string>
        {
            {0, "NZ"},
            {1, "Z"},
            {2, "NC"},
            {3, "C"},
            {4, "PO"},
            {5, "PE"},
            {6, "P"},
            {7, "M"}
        };

        //private const byte maskOpsIm8 = 0xCC;
        private const byte maskOpsReg = 0xF8;
        private const byte maskOpsRegIm8 = 0xC7;
        private const byte maskOpsRegReg = 0xC0;
        private const byte maskOpsRp = 0xCF;
        private const byte maskOpsRpIm16 = 0xCF;

        private byte GetClearCommand(byte b, byte mask)
        {
            return Convert.ToByte(b & mask);
        }

        public List<string> GetAsmCode(byte[] bytes, int startAddr = -1)
        {
            var AsmCode = new List<string>();
            var LineAddres = new Dictionary<int, string>();
            var LineAddresTmp = new Dictionary<int, string>();
            var lastDataIndex = 0;
            var DBList = new Dictionary<int, string>();
            var DWList = new Dictionary<int, string>();
            var AddrTmp = 0;
            var CurLabel = -1;
            if (startAddr != -1)
            {
                LineAddres.Add(startAddr, null);
                LineAddresTmp.Add(startAddr, null);
                for (var i = 0; i < bytes.Length; ++i)
                {
                    LineAddresTmp.Add(LineAddresTmp.Keys.Last() + 1, null);
                }
                CurLabel = 0;
            }
            int CurPos = 0;
            var tmpString = "";
            var command = "";
            byte reg;
            //while (CurPos < bytes.Length)
            //{
                //if (ops0.ContainsKey(bytes[CurPos]))
                    while (CurPos < bytes.Length)
                    {
                        if (startAddr != -1)
                        {
                            if (DWList.ContainsKey(startAddr + CurPos))
                            {
                                LineAddres.Add(LineAddres.Keys.Last() + 2, null);
                                tmpString = DWList[startAddr + CurPos] + " DW 0x" + bytes[CurPos].ToString("X") +
                                            bytes[CurPos + 1].ToString("X");
                                AsmCode.Add(tmpString);
                                CurPos += 2;
                                continue;
                            }
                            if (DBList.ContainsKey(startAddr + CurPos))
                            {
                                LineAddres.Add(LineAddres.Keys.Last() + 1, null);
                                tmpString = DBList[startAddr + CurPos] + " DB 0x" + bytes[CurPos].ToString("X");
                                AsmCode.Add(tmpString);
                                CurPos += 1;
                                continue;
                            }
                        }
                        if (ops0.ContainsKey(bytes[CurPos]))
                        {
                            AsmCode.Add(ops0[bytes[CurPos]].ToUpper());
                            LineAddres.Add(LineAddres.Keys.Last() + 1, null);
                            ++CurPos;
                        }
                        else if (opsIm16.ContainsKey(bytes[CurPos]))
                        {
                            AddrTmp = (bytes[CurPos + 2] << 8) | (bytes[CurPos + 1]);
                            LineAddres.Add(LineAddres.Keys.Last() + 3, null);
                            //if (LineAddresTmp.ContainsKey(AddrTmp))
                                //AddrTmp = (bytes[CurPos + 2] << 8) | (bytes[CurPos + 1]);
                            string opStr = opsIm16[bytes[CurPos]].ToLower();
                            if (startAddr != -1)
                            {
                                if (opStr == "sta")
                                    DBList.Add(AddrTmp, "Data" + lastDataIndex++);
                                else if (opStr == "shld")
                                    DWList.Add(AddrTmp, "Data" + lastDataIndex++);
                            }
                            if (LineAddresTmp.ContainsKey(AddrTmp) && LineAddresTmp[AddrTmp] == null)
                            {
                                LineAddresTmp[AddrTmp] = "Label" + Convert.ToString(CurLabel);
                                ++CurLabel;
                            }
                            tmpString = opsIm16[bytes[CurPos]].ToUpper() + " 0x" + (bytes[CurPos + 2]).ToString("X") +
                                        (bytes[CurPos + 1]).ToString("X");
                            AsmCode.Add(tmpString);
                            CurPos += 3;
                        }
                        else if (opsIm8.ContainsKey(bytes[CurPos]))
                        {
                            tmpString = opsIm8[bytes[CurPos]].ToUpper() + " 0x" + (bytes[CurPos + 1]).ToString("X");
                            LineAddres.Add(LineAddres.Keys.Last() + 2, null);
                            AsmCode.Add(tmpString);
                            CurPos += 2;
                        }
                        else if (opsReg.ContainsKey(Convert.ToByte(bytes[CurPos] & maskOpsReg))) //add
                        {
                            command = opsReg[Convert.ToByte(bytes[CurPos] & maskOpsReg)].ToUpper();
                            reg = Convert.ToByte(bytes[CurPos] & ~maskOpsReg);
                            if (Rnames.ContainsKey(reg))
                            {
                                tmpString = command + " " + Rnames[reg];
                                LineAddres.Add(LineAddres.Keys.Last() + 1, null);
                                AsmCode.Add(tmpString);
                                CurPos += 1;
                            }
                        }
                        else if (opsRegIm8.ContainsKey(Convert.ToByte(bytes[CurPos] & maskOpsRegIm8)))
                        {
                            command = opsRegIm8[Convert.ToByte(bytes[CurPos] & maskOpsRegIm8)].ToUpper();
                            reg = Convert.ToByte((bytes[CurPos] & ~maskOpsRegIm8) >> 3);
                            if (Rnames.ContainsKey(reg))
                            {
                                tmpString = command + " " + Rnames[reg] + ", 0x" + (bytes[CurPos + 1]).ToString("X");
                                LineAddres.Add(LineAddres.Keys.Last() + 2, null);
                                AsmCode.Add(tmpString);
                                CurPos += 2;
                            }
                        }
                        else if (opsRegReg.ContainsKey(Convert.ToByte(bytes[CurPos] & maskOpsRegReg))) //mov
                        {
                            command = opsRegReg[Convert.ToByte(bytes[CurPos] & maskOpsRegReg)].ToUpper();
                            var regs = Convert.ToByte(bytes[CurPos] & ~maskOpsRegReg);
                            var reg1 = Convert.ToByte(regs >> 3);
                            var reg2 = Convert.ToByte(regs & 7);
                            if (Rnames.ContainsKey(reg1) && Rnames.ContainsKey(reg2))
                            {
                                tmpString = command + " " + Rnames[reg1] + ", " + Rnames[reg2];
                                LineAddres.Add(LineAddres.Keys.Last() + 1, null);
                                AsmCode.Add(tmpString);
                                CurPos += 1;
                            }
                        }
                        else if (opsRp.ContainsKey(Convert.ToByte(bytes[CurPos] & maskOpsRp))) //push
                        {
                            command = opsRp[Convert.ToByte(bytes[CurPos] & maskOpsRp)].ToUpper();
                            reg = Convert.ToByte((bytes[CurPos] & ~maskOpsRp) >> 4);
                            if (RPnames.ContainsKey(reg))
                            {
                                tmpString = command + " " + RPnames[reg];
                                LineAddres.Add(LineAddres.Keys.Last() + 1, null);
                                AsmCode.Add(tmpString);
                                CurPos += 1;
                            }
                        }
                        else if (opsRpIm16.ContainsKey(Convert.ToByte(bytes[CurPos] & maskOpsRpIm16)))
                        {
                            command = opsRpIm16[Convert.ToByte(bytes[CurPos] & maskOpsRpIm16)].ToUpper();
                            reg = Convert.ToByte(bytes[CurPos] & ~maskOpsRpIm16);
                            if (RPnames.ContainsKey(reg))
                            {
                                tmpString = command + " " + RPnames[reg] + ", 0x" +
                                            (bytes[CurPos + 2]).ToString("X") + (bytes[CurPos + 1]).ToString("X");
                                LineAddres.Add(LineAddres.Keys.Last() + 3, null);
                                AsmCode.Add(tmpString);
                                CurPos += 3;
                            }
                        }
                        //tmpString = "";
                    }
                //var tmp = new ArrayList();
                foreach (var addrTmp in LineAddresTmp)
                {
                    if (addrTmp.Value != null)
                    {
                        LineAddres[addrTmp.Key] = addrTmp.Value;
                    }
                }
                var tmpByte = "";
                foreach (var addr in LineAddres)
                {
                    if (addr.Value != null)
                    {
                        tmpByte = (Convert.ToByte((addr.Key & 0xff00) >> 8).ToString("X") +
                                   Convert.ToByte(addr.Key & 0xff).ToString("X"));
                        if (!(DWList.ContainsKey(Convert.ToInt16(tmpByte, 16)) || DBList.ContainsKey(Convert.ToInt16(tmpByte, 16))))
                            for (int i = 0; i < AsmCode.Count; ++i)
                            {
                                AsmCode[i] = AsmCode[i].Replace("0x" + tmpByte,
                                    (addr.Value) + " ;" + "0x" + tmpByte);
                            }
                        else if (DWList.ContainsKey(Convert.ToInt16(tmpByte, 16)))
                            for (int i = 0; i < AsmCode.Count; ++i)
                            {
                                AsmCode[i] = AsmCode[i].Replace("0x" + tmpByte,
                                    (DWList[addr.Key]) + " ;" + "0x" + tmpByte);
                            }
                        else
                        {
                            for (int i = 0; i < AsmCode.Count; ++i)
                            {
                                AsmCode[i] = AsmCode[i].Replace("0x" + tmpByte,
                                    (DBList[addr.Key]) + " ;" + "0x" + tmpByte);
                            }
                        }
                        var index = 0;
                        foreach (var addres in LineAddres)
                        {
                            if ((addres.Key == addr.Key) )
                            {
                                //if (DWList.ContainsKey(addres.Key))
                                //    AsmCode[index] = addres.Value + " " + AsmCode[index];
                                //else
                                if ((index < AsmCode.Count) && !(DWList.ContainsKey(addres.Key) || DBList.ContainsKey(addres.Key)))
                                    AsmCode[index] = addres.Value + ":\n" + AsmCode[index];
                                break;
                            }
                            else
                            {
                                ++index;
                            }
                        }
                    }
                }
            return AsmCode;
        }
    }
}