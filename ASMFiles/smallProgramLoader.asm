;timer and uart can be initialized by programm in Monitor
;with parameters 4800 bit/s, 7 bit, parity even, two stop bits
;
;init timer and uart
;used registers: A 
;input: none 
;output: none 
init_timer_and_uart: 
    MVI A, 40h ; сброс
    OUT 0E3h
    MVI A, 1Ah ; регистр сравнения 
    OUT 0E1h 
    MVI A, 7Eh ; (01 11 11 10) управляющее влово режима работы UART 
    OUT 0FBh 
    MVI A, 05h ; (00 00 01 01) включение приема / передачи 
    OUT 0FBh 
    XRA A 
    OUT 0FBh 
    OUT 0FBh 
    OUT 0FBh 
    MVI A, 40h ; сброс 
    OUT 0FBh

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
    IN 0FBh
    ANI 02h
    JZ readByte
    IN 0FAh
    MOV B, A
    IN 0FBh
    ANI 28h ; (0010 1000)
    JNZ readByte
    RET