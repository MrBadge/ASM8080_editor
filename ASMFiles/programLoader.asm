;init timer
;used registers: A
;input: none
;output: none
init_timer:
    MVI A, 56 ; режим работы
    OUT 0xE3
    MVI A, 0x1A ; регистр сравнения
    OUT 0xE1
    MVI A, 0x7A ; (01 11 10 10) управляющее влово режима работы UART
    OUT 0xFB
    MVI A, 0x05 ; (00 00 01 01) включение приема / передачи
    OUT 0xFB
    XRA A
    OUT 0xFB
    OUT 0xFB
    OUT 0xFB
    MVI A, 0x40 ; сброс
    OUT 0xFB

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

;write byte to terminal
;used registers: A, B
;input: byte to write in register B
;output: none
writeByte:
	IN 0xFB
	ANI 0x01
	JZ writeByte
	MOV A, B
	OUT 0xFA
	RET

;load programm from terminal
;used registers: A, B, C, HL
;input: first byte from terminal is a command
;commands:
;	0 - exit program;
;	1 - next to bytes is starting adress to write
;	2 - next byte is byte to write
;output: none
programLoader:
	CALL readByte
	MOV A, B
	ANA A
	RZ ; exit program if com zero
	SUI 0x01
	JZ programLoader_command_1
	SUI 0x01
	JZ programLoader_command_2
	programLoader_command_1:
	CALL readByte
	MOV H, B
	CALL readByte
	MOV L, B
	JMP programLoader
	programLoader_command_2:
	CALL readByte
	mov M, B
	INX H
	JMP programLoader