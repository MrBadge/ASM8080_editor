init_timer_and_uart: 
    ;сброс
    XRA A
    OUT 0FBh
    OUT 0FBh
    OUT 0FBh
    MVI A, 40h ; сброс 
    OUT 0FBh

    MVI A, 56h ; (01 01 011 0) режим работы 
    OUT 0E3h
    MVI A, 1Ah ; регистр сравнения 
    OUT 0E1h

    MVI A, 7Eh ; (01 11 11 10) управляющее слово режима работы UART 
    OUT 0FBh
    MVI A, 05h ; (00 00 01 01) включение приема / передачи 
    OUT 0FBh
 
;load program from terminal
;used registers: A, B, C, HL, DE
;input: first byte from terminal is a command
;commands:
;   0 - nop
;   1 - next two bytes is starting address to write or read
;   2 - next byte is byte to write
;   3 - read current byte end send to terminal
;output: none 
programLoader: 
    CALL 2117h;readByte 
    MOV A, B 
    ANA A 
    JZ programLoader; nop 
    SUI 01h 
    JZ programLoader_command_1 
    SUI 01h 
    JZ programLoader_command_2 
    SUI 01h 
    JZ programLoader_command_3 
    JMP programLoader
    programLoader_command_1: 
        CALL 2117h;readByte 
        MOV H, B 
        CALL 2117h;readByte 
        MOV L, B 
        SHLD Addr1
        JMP programLoader
    programLoader_command_2: 
        CALL 2117h;readByte 
        MOV M, B 
        INX H
        JMP programLoader 
    programLoader_command_3:
        MOV B, M
        INX H
        CALL writeByte
        JMP programLoader
    Addr1 DW 2123h
;read byte from terminal 
;used registers: A, B 
;input: none 
;output: read byte in register B 
;readByte: 
;    IN 0FBh 
;    ANI 02h 
;    JZ readByte 
;    IN 0FAh 
;    MOV B, A 
;    IN 0FBh 
;    ANI 28h ; (0010 1000) 
;    JNZ readByte 
;    RET 
 
;write byte to terminal 
;used registers: A, B 
;input: byte to write in register B 
;output: none 
writeByte: 
    IN 0FBh 
    ANI 01h 
    JZ writeByte 
    MOV A, B 
    OUT 0FAh 
    RET 