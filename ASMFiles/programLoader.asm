;settings 4800 baud, 8 bit, parity even, one stop bit 
;init timer and uart
;used registers: A
;input: none
;output: none
init_timer_and_uart: 
    MVI A, 56h ; (01 01 011 0) режим работы 
    OUT 0E3h
    MVI A, 1Ah ; регистр сравнения 
    OUT 0E1h
    MVI A, 7Eh ; (01 11 11 10) управляющее слово режима работы UART 
    OUT 0FBh
    MVI A, 05h ; (00 00 01 01) включение приема / передачи 
    OUT 0FBh
    XRA A
    OUT 0FBh
    OUT 0FBh
    OUT 0FBh
    MVI A, 40h ; сброс 
    OUT 0FBh
 
;load program from terminal
;used registers: A, B, C, HL, DE
;input: first byte from terminal is a command
;commands:
;   0 - jump to the latest starting adress
;   1 - next two bytes is starting address to write or read
;   2 - next byte is byte to write
;   3 - read current byte end send to terminal
;output: none 
programLoader: 
    CALL readByte 
    MOV A, B 
    ANA A 
    JZ Addr1; go to start of user''s program if command zero 
    SUI 01h 
    JZ programLoader_command_1 
    SUI 01h 
    JZ programLoader_command_2 
    SUI 01h 
    JZ programLoader_command_3 
    JMP programLoader
    programLoader_command_1: 
        CALL readByte 
        MOV H, B 
        CALL readByte 
        MOV L, B 
        SHLD Addr1
        JMP programLoader
    programLoader_command_2: 
        CALL readByte 
        MOV M, B 
        INX H
        JMP programLoader 
    programLoader_command_3:
        MOV B, M
        INX H
        CALL writeByte
        JMP programLoader
    Addr1 DW 2101h
 
;read byte from terminal 
;used registers: A, B 
;input: none 
;output: read byte in register B 
readByte: 
    IN 0FBh 
    ANI 02h 
    JZ readByte 
    IN 0FAh 
    MOV B, A 
    IN 0FBh 
    ANI 28h ; (0010 1000) 
    JNZ readByte 
    RET 
 
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