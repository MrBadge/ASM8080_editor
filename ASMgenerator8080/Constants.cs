﻿using System.Collections.Generic;

namespace ASMgenerator8080
{
    internal class Constants
    {
        public static readonly int defaultStartingAdress = 0x2100;
        public static int smallProgramLoaderSize = 0x33;
        public static int sleepDelay = 20;
        public static List<string> Commands = new List<string>
        {
            "MOV",
            "MVI",
            "LXI",
            "LDA",
            "LHLD",
            "LDAX",
            "STA",
            "SHLD",
            "STAX",
            "PUSH",
            "POP",
            "XTHL",
            "IN",
            "OUT",
            "ADD",
            "ADI",
            "ADC",
            "ACI",
            "SUB",
            "SUI",
            "SBB",
            "SBI",
            "ANA",
            "ANI",
            "ORA",
            "ORI",
            "XRA",
            "XRI",
            "CMP",
            "CPI",
            "INR",
            "INX",
            "DCR",
            "DCX",
            "DAD",
            "RLC",
            "RRC",
            "RAL",
            "RAR",
            "CMA",
            "JMP",
            "JNZ",
            "JNC",
            "JPO",
            "JP",
            "JZ",
            "JC",
            "JPE",
            "JM",
            "CALL",
            "PCHL",
            "RNZ",
            "RNC",
            "RPO",
            "RP",
            "RZ",
            "RC",
            "RPE",
            "RM",
            "RET",
            "CNZ",
            "CNC",
            "CPO",
            "CP",
            "CZ",
            "CC",
            "CPE",
            "CM",
            "HLT",
            "NOP",
            "END"
        };

        public static byte[] BigProgramLoader =
        {
            0xAF, 0xD3, 0xFB, 0xD3, 0xFB, 0xD3, 0xFB, 0x3E, 0x40, 0xD3, 0xFB, 0x3E, 0x56, 0xD3, 0xE3, 0x3E, 
            0x1A, 0xD3, 0xE1, 0x3E, 0x7E, 0xD3, 0xFB, 0x3E, 0x05, 0xD3, 0xFB, 0xCD, 0x17, 0x21, 0x78, 0xA7, 
            0xCA, 0x4E, 0x21, 0xD6, 0x01, 0xCA, 0x68, 0x21, 0xD6, 0x01, 0xCA, 0x76, 0x21, 0xD6, 0x01, 0xCA, 
            0x7E, 0x21, 0xC3, 0x4E, 0x21, 0xCD, 0x17, 0x21, 0x60, 0xCD, 0x17, 0x21, 0x68, 0x22, 0x86, 0x21, 
            0xC3, 0x4E, 0x21, 0xCD, 0x17, 0x21, 0x70, 0x23, 0xC3, 0x4E, 0x21, 0x46, 0x23, 0xCD, 0x88, 0x21, 
            0xC3, 0x4E, 0x21, 0x23, 0x21, 0xDB, 0xFB, 0xE6, 0x01, 0xCA, 0x88, 0x21, 0x78, 0xD3, 0xFA, 0xC9,
            0x00
            //readByte used from smallLoader, with MUST be placed from 2100h
            /*0x3E, 0x56, 0xD3, 0xE3, 0x3E, 0x1A, 0xD3, 0xE1, 0x3E, 0x7E, 0xD3, 0xFB, 0x3E, 0x05, 0xD3, 0xFB, 
            0xAF, 0xD3, 0xFB, 0xD3, 0xFB, 0xD3, 0xFB, 0x3E, 0x40, 0xD3, 0xFB, 0xCD, 0x78, 0x21, 0x78, 0xA7, 
            0xCA, 0x76, 0x21, 0xD6, 0x01, 0xCA, 0x58, 0x21, 0xD6, 0x01, 0xCA, 0x66, 0x21, 0xD6, 0x01, 0xCA, 
            0x6E, 0x21, 0xC3, 0x3E, 0x21, 0xCD, 0x78, 0x21, 0x60, 0xCD, 0x78, 0x21, 0x68, 0x22, 0x76, 0x21, 
            0xC3, 0x3E, 0x21, 0xCD, 0x78, 0x21, 0x70, 0x23, 0xC3, 0x3E, 0x21, 0x46, 0x23, 0xCD, 0x8A, 0x21, 
            0xC3, 0x3E, 0x21, 0x01, 0x21, 0xDB, 0xFB, 0xE6, 0x02, 0xCA, 0x78, 0x21, 0xDB, 0xFA, 0x47, 0xDB, 
            0xFB, 0xE6, 0x28, 0xC2, 0x78, 0x21, 0xC9, 0xDB, 0xFB, 0xE6, 0x01, 0xCA, 0x8A, 0x21, 0x78, 0xD3, 
            0xFA, 0xC9, 0x00*/ //with readByte 2113
        };
    }
}