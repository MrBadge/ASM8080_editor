;com port must be initialized with parameters: 4800 bit/s, 8 bit, parity even, two stop bits
;start address must be 2100h
readByte:
    IN 0xFB 
    ANI 0x02
    JZ readByte
    IN 0xFA
    MOV B, A
    IN 0xFB
    ANI 0x28 ; (0010 1000)
    JNZ readByte
    RET

MVI H, 21h ;the address must reference to the end of this loader, where main loader will start
MVI L, 33h
smallPrLoad:
    call readByte
    MOV A, B
    MOV M, B
    INX H
    ANA A
    JNZ smallPrLoad
