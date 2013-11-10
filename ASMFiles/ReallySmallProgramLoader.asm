;com port must be initialized with parameters: 4800 baud, 7 bit, parity even, two stop bits
MVI H, 021h ;the address must reference to the end of this loader, where main loader will start
MVI L, 023h
JMP smallPrLoad
readByte:
    IN 0FBh
    ANI 02h
    JZ readByte
    IN 0FAh
    MOV B, A
    IN 0FBh
    ANI 028h ; (0010 1000)
    JNZ readByte
    RET

smallPrLoad:
    call readByte
    MOV A, B
    MOV M, B
    INX H
    ANA A
    JNZ smallPrLoad