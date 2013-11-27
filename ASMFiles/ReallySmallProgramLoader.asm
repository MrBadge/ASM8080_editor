;com port must be initialized with parameters: 4800 baud, 7 bit, parity even, two stop bits
MVI A, 0xFC
OUT 0xFF
MVI H, 021h ;the address must reference to the end of this loader, where main loader will start
MVI L, 33h
init_timer_and_uart: 
    MVI A, 40h ; сброс
    OUT 0FBh
    MVI A, 0FEh ; (11 11 11 10) управляющее слово режима работы UART 
    OUT 0FBh
    MVI A, 05h ; (00 00 01 01) включение приема / передачи 
    OUT 0FBh

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
 
