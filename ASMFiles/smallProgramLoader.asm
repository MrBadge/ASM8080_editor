;init timer and uart
;used registers: A 
;input: none 
;output: none 
init_timer_and_uart: 
    MVI A, 56 ; режим работы 
    OUT 0xE3 
    MVI A, 0x1A ; регистр сравнения 
    OUT 0xE1 
    MVI A, 0x7E ; (01 11 11 10) управляющее влово режима работы UART 
    OUT 0xFB 
    MVI A, 0x05 ; (00 00 01 01) включение приема / передачи 
    OUT 0xFB 
    XRA A 
    OUT 0xFB 
    OUT 0xFB 
    OUT 0xFB 
    MVI A, 0x40 ; сброс 
    OUT 0xFB

;loader for programLoader
;used registers: A, B, H, L
;input:
;	first two bytes are starting address in memory for programLoader
;	next byte is state byte
;	if state byte is zero - end of loading
;	else - next byte is byte of program and so on
smallProgramLoader:
	CALL readByte
	MOV H, B
	CALL readByte
	MOV L, B
	programLoaderAddr DW 0x00
	SHLD programLoaderAddr
	smallProgramLoader_mainLoop:
		CALL readByte
		MOV A, B
		ANA A
		JZ programLoaderAddr
		CALL readByte
		MOV M, B
		INX H
		JMP smallProgramLoader_mainLoop


;read byte from terminal 
;used registers: A, B 
;input: none 
;output: read byte in register B 
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