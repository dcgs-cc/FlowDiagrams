
;
;	#define Use16F876A
	#define Use16F876
;        #define Use16F886



#ifdef Use16F876A
	__CONFIG    _WDT_OFF  &  _XT_OSC & _DEBUG_OFF  & _BODEN_OFF  & _PWRTE_ON & _LVP_OFF & _CPD_OFF & _CP_OFF  & _WRT_OFF  & _CP_OFF
	LIST   P=PIC16F876A	; list directive to define processor
	#include <p16F876A.inc>	; processor specific variable definition
endif

#ifdef Use16F876
	__CONFIG    _WDT_OFF  &  _XT_OSC & _DEBUG_OFF  & _BODEN_OFF  & _PWRTE_ON & _LVP_OFF & _CPD_OFF & _CP_OFF  & _WRT_ENABLE_ON 
	LIST   P=PIC16F876	; list directive to define processor
	#include <p16F876.inc>	; processor specific variable definition
endif

#ifdef Use16F886
	__CONFIG    2007, _WDT_OFF  &  _XT_OSC & _DEBUG_OFF  & _BOR_OFF & _PWRTE_ON & _LVP_OFF & _CPD_OFF & _CP_OFF  &_EXTRC_OSC_CLKOUT
	__CONFIG    2008,   _WRT_OFF
	LIST   P=PIC16F886	; list directive to define processor
	#include <p16F886.inc>	; processor specific variable definition
endif




;**********************************************************************
;VARIABLE DEFINITIONS - the data memory starts at 0x20
R0		equ	0x20	;register use for OCR S0 etc
R1		equ	0x21
R2		equ	0x22
R3		equ	0x23
R4		equ	0x24		
R5		equ	0x25		
R6		equ	0x26		
R7		equ	0x27
R8		equ	0x28		;used for workspace for flow diagram stuff
R9		equ	0x29
w_temp		equ	0x2a       	; variable used for context saving 
status_temp	equ	0x2b        	; variable used for context saving
temp1		equ	0x2c		;following used for gen purpose in routines
temp2		equ	0x2d
A0		equ	0x2e		;used for the os code aas workspace
A1		equ	0x2f
A2		equ	0x30
A3		equ	0x31
A4		equ	0x32
A5		equ	0x33
w_Btemp		equ	0x34       	; variable used for context saving in break
status_Btemp	equ	0x35       	; variable used for context saving in break
next_free	equ	0x36		;holds inital value of PORTA on reset...


ADDRL		equ	0x70		;these registers in all banks
ADDRH		equ	0x71
DATAL		equ	0x72		;these registers in all banks
DATAH		equ	0x73
INTCONX		equ 	0x74		;for status saving in memory read/writes
SerBuffPtr	equ 	0x75		;pointer to serial buffer
SerFlags	equ 	0x76		;bit 0 = buffer overrun; b1 = msg complete
#ifdef Use16F876A
DATAbuffer	equ	0x77		;used for write buffer on 16f876A only
endif




;using the file registers in bank 2 for serial received buffer starting at 0x10
SerialBuffer 	 equ	 0x10	;bank 2!!!  
				; message for w is @@@@w0500 plus 64 chars so max of 0x49 chars    

#ifdef Use16F886
;using ram in bank 2
DATAbuffer	equ	0x10		;used for write buffer on 16f886 only - I think it is safe to overwrite the serial buffer?

endif

;**********************************************************************
		ORG	0x000		; processor reset vector
		goto	setup		; go to beginning of program
		goto	break_point	;vector for breakpoint
		nop			;here for user jump later...
		ORG	0x004		; interrupt vector location
		movwf	w_temp		; save off current W register contents
		movf	STATUS,w	; move status register into W register
		movwf	status_temp	; save off contents of STATUS register
					;int code goes here
		
		bcf 	STATUS, RP1 
		bcf	STATUS, RP0 	;Bank 0
		btfss	PIR1,RCIF
		goto	Exit_Int	;is us

		movf	SerBuffPtr,W	;store at next point in buffer
		movwf 	FSR
		bsf	STATUS,7	;irp bit so access bank 2
		movf	RCREG,W
		movwf	INDF
		sublw	0x0d
		btfsc	STATUS,Z	;is msg complete??
		bsf	SerFlags,1

		incf	SerBuffPtr,F	;inc ptr
		movlw	0x6f		;top of buffer
		subwf	SerBuffPtr,W
		btfss	STATUS,Z
		goto	Exit_Int
		decf	SerBuffPtr,W	;so flag overrun and dec
		bsf	SerFlags,0

Exit_Int
		movf    status_temp,w	; retrieve copy of STATUS register
		movwf	STATUS		;restore pre-isr STATUS register contents
		swapf   w_temp,f
		swapf   w_temp,w	; restore pre-isr W register contents
		retfie

;**********************************************************************
;
;		setup
;

; PORTB as OUTPUT  PORT C as INPUTS except RC6 (TX) and RC7(Rx)
setup
		bcf 	STATUS, RP1 	;
		bcf 	STATUS, RP0 	;Bank 0
		clrf	PORTC
		clrf	PORTB
		bsf	PORTC,6		;best to have serial tx hi at start
		bsf 	STATUS, RP0	;bank1
		movlw	B'10111111'
		movwf	TRISC
		movlw	B'00000000'
		movwf	TRISB
		movlw	B'11110111'	;use RA4/5 as inputs also  RA3 for "ticker!"RA2 for pgm decision
		movwf	TRISA

#ifdef Use16F886
		BANKSEL ANSEL
		movlw	0x01
		movwf	ANSEL 		;digital I/O except bit 0
		BANKSEL ANSELH
		CLRF	ANSELH

endif

		BANKSEL ADCON1 		;seems to be same for 876 886
		movlw   b'00001110'  	; set as ADRESH is left justified and use AN0 as analog intput with int ref
		movwf	ADCON1		;uses RA0 as the sole analogue input


		bcf 	STATUS, RP0 	;Bank 0
		call	SetupSerial
		goto	main

;**************************************************************************
;
;		routines for OCR emulate
;
;

READADC
;return result in R0

		banksel	ADCON0
#ifdef Use16F886

		MOVLW	b'11000011' ;ADC Frc clock,
		MOVWF 	ADCON0 ;AN0, On
		
		btfsc	ADCON0,1
		goto	$-1
		movf	ADRESH,W	;top 8 bits.....
		movwf	R0
		return
else
		movlw	b'00000101'	;set go
		movwf	ADCON0
		btfsc	ADCON0,2
		goto	$-1
		movf	ADRESH,W	;top 8 bits.....
		movwf	R0
		return
endif

READTABLE

		movf	R7,W		;R7 points to table low byte   0x400 for hi byte ;return in R0
		bsf 	STATUS, RP1
		bcf	STATUS, RP0	;Bank 2
		movwf	EEADR
		movlw	0x04		;table at 0x400
		movwf	EEADRH
		bsf	STATUS, RP0	;Bank 3
		bsf	EECON1,EEPGD
		bsf	EECON1,RD
		nop
		nop
		bcf	STATUS,RP0	;bank 2
		movf	EEDATA,W
		bcf	STATUS, RP1 	;bank 0
		movwf	R0
		bsf	STATUS, RP1 	;bank 2
		movf	EEDATH,W	;hi 6 bits
		bcf	STATUS, RP1 	;bank 0
		return


WAIT1MS
		movlw	0
		movwf	temp1
		decfsz	temp1,1
		goto	$-1
		return

READDATAEEPROM
;		not really an OCR call but added for flexibility...
;		value returned in S0
;		address in S7
		movf	R7,W
		movwf	ADDRL
		clrf	ADDRH
		call	ReadEEProm
		movwf	R0
		return

WRITEDATAEEPROM
;		not really an OCR call but added for flexibility...
;		value in S0
;		address in S7
		movf	R7,W
		movwf	ADDRL
		clrf	ADDRH
		movf	R0,W
		movwf	DATAL
		goto	writeEEProm



;**************************************************************************
;
;		memory reading/writing
;
;

SetBank0_return
		bcf 	STATUS, RP1
		bcf	STATUS, RP0 	;Bank 0
		return

ReadProgramMemory

		call	SetUpMemAdd	;returns in bank 2
		bsf	STATUS, RP0	;Bank 3
		bsf	EECON1,EEPGD
		bsf	EECON1,RD
		nop
		nop
		bcf	STATUS,RP0	;bank 2
		movf	EEDATA,W
		movwf	DATAL
		movf	EEDATH,W	;hi 6 bits
		movwf	DATAH
		goto	SetBank0_return

ReadEEProm
;returns value in W
;address ADDRL/ADDRH
		call	SetUpMemAdd	;returns in bank 2  note sets up EEADRH as well tho not needed
		bsf	STATUS, RP0 	;Bank 3
		bcf 	EECON1, EEPGD 	;Point to Data memory
		bsf	EECON1, RD 	;Start read operation
		bcf 	STATUS, RP0 	;Bank 2
		movf	EEDATA, W 	;W = EEDATA
		goto	SetBank0_return

WriteProgramMemory
;address ADDRL/ADDRH
;data in DATAl/DATAH

		call	SetUpMemAdd	;returns in bank 2
		movf 	DATAL, W 	;Write value to
		movwf	EEDATA 		;program at
		movf	DATAH, W	;desired memory
		movwf	EEDATH 		;location
		bsf	STATUS, RP0 	;Bank 3
		bsf	EECON1, EEPGD 	;Point to Program memory
		bsf	EECON1, WREN 	;Enable writes
		movf	INTCON,W
		movwf	INTCONX		;save int state
		bcf	INTCON, GIE 	;disable interrupts
		movlw	0x55 		;Write 55h to EECON2
		movwf	EECON2
		movlw	0xAA 		;Write AAh to EECON2
		movwf	EECON2
		bsf	EECON1, WR 	;Start write operation
		nop 			;Two NOPs to allow micro
		nop			;to setup for write
		movf	INTCONX,W	;restore int state
		movwf	INTCON
		bcf	EECON1, WREN 	;Disable writes
		goto	SetBank0_return

writeEEProm
;address ADDRL
;data in DATAl

		call	SetUpMemAdd	;returns in bank 2
		movf	DATAL, W	;Data to
		movwf	EEDATA 		;write
		bsf	STATUS, RP0 	;Bank 3
		bcf	EECON1, EEPGD 	;Point to Data memory
		bsf	EECON1, WREN 	;Enable writes
		bcf	INTCON, GIE 	;disable int
		movlw	0x55 		;Write 55h to
		movwf	EECON2 		;EECON	
		movlw	0xAA 		;Write AAh to	
		movwf	EECON2 		;EECON2
		bsf	EECON1, WR 	;Start write operation
		bsf	STATUS, RP0 	;Bank 3
		btfsc	EECON1, WR 	;Wait for
		goto	$-1 		;write to finish
		bsf	INTCON, GIE 	;enable int	
		bcf	EECON1, WREN 	;Disable writes
		goto	SetBank0_return	



WriteProgramMemory4	;awful 4 word block write.... only used for 16F876A
#ifdef Use16F876A
;address ADDRL/ADDRH
;8 bytes (4 words) of data stored at (DATAbuffer)
		call	SetUpMemAdd	;returns in bank 2
		movlw	DATAbuffer	;Load initial buffer address into the FSR
		movwf	FSR ;
loopWR4	 	movf	INDF,W 		;Load first data byte into lower
		movwf	EEDATA 
		incf	FSR,F 		;Next byte
		movf	INDF,W		;Load second data byte into upper
		movwf	EEDATH
		incf	FSR,F 
		bsf	STATUS,RP0 	;Bank 3
		bsf	EECON1,EEPGD 	;Point to program memory
		bsf	EECON1,WREN 	;Enable writes
		bcf	INTCON,GIE 	;Disable interrupts (if using)
		movlw	0x55 		;Write 55h to EECON2
		movwf	EECON2
		movlw	0xAA 		;Write AAh to EECON2
		movwf	EECON2
		bsf	EECON1, WR 	;Start write operation
; Any instructions here are ignored as processor
; halts to begin write sequence
		nop
		nop
; processor will stop here and wait for write complete
; after write processor continues with 3rd instruction

		bcf	EECON1,WREN 	;Disable writes
		bsf	INTCON,GIE 	;Enable interrupts (if using)
		bcf	STATUS,RP0 	;Bank 2
		incf	EEADR,F 	;Increment address
		movf	EEADR,W 	;Check if lower two bits of address are ‘00’
		andlw	0x03 		;Indicates when four words have been programmed
		;xorlw	0x03 
		btfss	STATUS,Z	;Exit if more than four words,
		goto	loopWR4		;Continue if less than four words
		bcf 	STATUS, RP1
		bcf	STATUS, RP0 	;Bank 0

endif

		return

WriteProgramMemory8	;awful 8 word block write.... only used for 16F886
#ifdef Use16F886
;address ADDRL/ADDRH
;16 bytes (8 words) of data stored at (DATAbuffer)

		movlw	SerialBuffer+9		;buffer points to data
		movwf	SerBuffPtr
		call	SetUpMemAdd	;returns in bank 2

loopWR8
		call	GetDataWord	;data word in DATAH/L  ... returns in bank2
		movf	DATAL,W		;Load first data byte into lower
		movwf	EEDATA 
		movf	DATAH,W		;Load second data byte into upper
		movwf	EEDATH

		bsf	STATUS,RP0 	;Bank 3
		bsf	EECON1,EEPGD 	;Point to program memory
		bsf	EECON1,WREN 	;Enable writes
		bcf	INTCON,GIE 	;Disable interrupts (if using)
		btfsc 	INTCON,GIE ; See AN576
		goto	 $-2
		movlw	0x55 		;Write 55h to EECON2
		movwf	EECON2
		movlw	0xAA 		;Write AAh to EECON2
		movwf	EECON2
		bsf	EECON1, WR 	;Start write operation
; Any instructions here are ignored as processor
; halts to begin write sequence
		nop
		nop
; processor will stop here and wait for write complete
; after write processor continues with 3rd instruction

		bcf	EECON1,WREN 	;Disable writes
		bsf	INTCON,GIE 	;Enable interrupts (if using)
		bcf	STATUS,RP0 	;Bank 2
		incf	EEADR,F 	;Increment address
		movf	EEADR,W 	;Check if lower three bits of address are ‘00’
		andlw	0x0F		;Indicates when 16 words have been programmed

		btfss	STATUS,Z	;Exit if more than 16 words,
		goto	loopWR8		;Continue 
		bcf 	STATUS, RP1
		bcf	STATUS, RP0 	;Bank 0

endif
return

SetUpMemAdd
		bsf 	STATUS, RP1 
		bcf 	STATUS, RP0 	;Bank 2
		movf 	ADDRL, W 	;Write address
		movwf	EEADR 		;of desired
		movf	ADDRH, W 	;program memory
		movwf	EEADRH 		;location
		return


;**********************************************************************
;about 325 ms at 4MHz
delay_checkSerial
		movlw	0xf0
		movwf	A1
delay0		movwf	A0
delay1		decfsz	A0,F
		goto	delay1
		btfsc	SerFlags,1
		return
		decfsz	A1,F
		goto	delay0
		return


ConvertHexToNumber
;ascii hex symbol in W, returns number in W
;uses A0
;first is it >0x40
		movwf	A0
		movlw	0x40
		subwf	A0,W
		movlw	0x30	;doesn't affect C
;if c=1 then A0>W
		btfsc	STATUS,C
		addlw	0x07
		subwf	A0,W	;now a number 
		return

ConvertNumberToHex
;number in W,	returns ascii codes in A0(hi byte),A1(low byte)
		movwf	A1
		andlw	0xf0	;top nibble
		movwf	A0
		swapf	A0,F	;now lower nibble in A0
		movlw	0x0A
		subwf	A0,W	;c=1 if A0>W
		movlw	0x30	;c unaffected
		btfsc	STATUS,C
		addlw	0x07
		addwf	A0,F	;A0 has ascii code
		
		movf	A1,W
		andlw	0x0f	;lower nibble
		movwf	A1
		movlw	0x0A
		subwf	A1,W	;c=1 if A1>W
		movlw	0x30	;c unaffected
		btfsc	STATUS,C
		addlw	0x07
		addwf	A1,F	;A0 has ascii code
		return

;**********************************************************************
;
;		Serial routines
;
;

SetupSerial
		movlw	SerialBuffer
		movwf	SerBuffPtr
		bsf	STATUS, RP0 	;Bank 1
		MOVLW	b'00100100'
		MOVWF	TXSTA
		bsf	PIE1,RCIE	;enable ints for receive
		movlw	0x19		;9600 baud
		;movlw	0x0C		;19.2kbaud
		movwf	SPBRG		;should be 9.6kbaud at 4MHz  (also in bank 1)
		bcf	STATUS, RP0 	;Bank 0
		movlw	B'10010000'
		movwf	RCSTA
		bsf	INTCON,GIE	;going to enable int
		bsf	INTCON,PEIE
		clrf	SerFlags
		return


SendMessage
					;address ADDRL/ADDRH
		call 	ReadProgramMemory
		movf	DATAL,W		;data is packed as 7 bit ascii in DATAL/DATAH
		movwf	R2
		bcf	R2,7
		rlf	DATAL,F
		rlf	DATAH,F
		movf	DATAH,W
		btfss	PIR1,TXIF
		goto	$-1
		movwf	TXREG
		movf	R2,W
		btfsc	STATUS,Z
		return			;end on zero byte
		btfss	PIR1,TXIF
		goto	$-1
		movwf	TXREG
		incf	ADDRL,F
		movf	ADDRL,W
		btfsc	STATUS,Z
		incf	ADDRH,F
		goto	SendMessage

SendReady
		movlw 	low Msg_Ready
		movwf	ADDRL
		movlw 	high Msg_Ready
		movwf	ADDRH
		call	SendMessage
		MOVLW	0X40
		btfss	PIR1,TXIF
		goto	$-1
		movwf	TXREG
		return

Sendproctype
		movlw 	low Msg_Proc
		movwf	ADDRL
		movlw 	high Msg_Proc
		movwf	ADDRH
		call	SendMessage
		MOVLW	0X40
		btfss	PIR1,TXIF
		goto	$-1
		movwf	TXREG
		return

SendVersion
		movlw 	low Msg_Version
		movwf	ADDRL
		movlw 	high Msg_Version
		movwf	ADDRH
		call	SendMessage
		MOVLW	0X40
		btfss	PIR1,TXIF
		goto	$-1
		movwf	TXREG
		return


TransmitByte	btfss	PIR1,TXIF
		goto	$-1
		movwf	TXREG
		return


EchoSerial				; echo back
		movlw	SerialBuffer
		movwf	SerBuffPtr
Echoloop1	movf	SerBuffPtr,W
		movwf 	FSR
		bsf	STATUS,7	;irp bit so access bank 2
		movf	INDF,W
		movwf	A0
		sublw	0x0d		;msg terminted with 0x0d
		btfsc	STATUS,Z
		return
		movf	A0,W
		btfss	PIR1,TXIF
		goto	$-1
		movwf	TXREG
		incf	SerBuffPtr,F
		goto 	Echoloop1

ChkMsgValid
		movlw	SerialBuffer
		movwf	SerBuffPtr
		movlw	0x03
		movwf	A1		;loop counter
ChkMsgValid1	movf	SerBuffPtr,W
		movwf 	FSR
		bsf	STATUS,7	;irp bit so access bank 2
		movf	INDF,W
		sublw	0x40		;ascii @
		btfss	STATUS,Z
		return			;wrong   z=0
		incf	SerBuffPtr,F
		decfsz	A1,F
		goto	ChkMsgValid1
		bsf	STATUS,Z
		return			;OK z =1

GetAddr
		bsf 	STATUS, RP1 		;bank2
		movf	SerialBuffer+5,W
		bcf 	STATUS, RP1 		;bank0
		call	ConvertHexToNumber
		movwf	A1
		swapf	A1,F
		bsf 	STATUS, RP1 		;bank2
		movf	SerialBuffer+6,W
		bcf 	STATUS, RP1 		;bank0
		call	ConvertHexToNumber
		addwf	A1,W
		movwf	ADDRH

		bsf 	STATUS, RP1 		;bank2
		movf	SerialBuffer+7,W
		bcf 	STATUS, RP1 		;bank0
		call	ConvertHexToNumber
		movwf	A1
		swapf	A1,F

		bsf 	STATUS, RP1 		;bank2
		movf	SerialBuffer+8,W
		bcf 	STATUS, RP1 		;bank0
		call	ConvertHexToNumber
		addwf	A1,W
		movwf	ADDRL
		return

GetDataWord	movf	SerBuffPtr,W	;SerBuffPtr setup on entry
		movwf 	FSR
		bsf	STATUS,7	;irp bit so access bank 2 where serbuffer is..
		movf	INDF,W
		bcf 	STATUS, RP1 		;bank0
		call	ConvertHexToNumber
		movwf	A1
		swapf	A1,F
		bsf 	STATUS, RP1 		;bank2
		incf	SerBuffPtr,F
		movf	SerBuffPtr,W
		movwf 	FSR
		movf	INDF,W
		bcf 	STATUS, RP1 		;bank0
		call	ConvertHexToNumber
		addwf	A1,W
		movwf	DATAH
		bsf 	STATUS, RP1 		;bank2
		incf	SerBuffPtr,F
		movf	SerBuffPtr,W	
		movwf 	FSR
		movf	INDF,W
		bcf 	STATUS, RP1 		;bank0
		call	ConvertHexToNumber
		movwf	A1
		swapf	A1,F
		bsf 	STATUS, RP1 		;bank2
		incf	SerBuffPtr,F
		movf	SerBuffPtr,W
		movwf 	FSR
		movf	INDF,W
		bcf 	STATUS, RP1 		;bank0
		call	ConvertHexToNumber
		addwf	A1,W
		movwf	DATAL
		incf	SerBuffPtr,F
		bsf 	STATUS, RP1 		;bank2
		return			;data word in DATAH/L
		;still in bank 2
		

SendDataWord
		movf	DATAH,W
		call	ConvertNumberToHex	;result in A0/A1 (A0-hi)
		movf	A0,W
		call	TransmitByte
		movf	A1,W
		call	TransmitByte
		movf	DATAL,W
		call	ConvertNumberToHex	;result in A0/A1 (A10-hi)
		movf	A0,W
		call	TransmitByte
		movf	A1,W
		call	TransmitByte
		return
;**********************************************************************
;main pogram follows here


test1		
		movlw 	low Msg_User
		movwf	ADDRL
		movlw 	high Msg_User
		movwf	ADDRH
		call	SendMessage
		goto	msgend

main	
		;GOTO	Jon_Cowley_recieve

;going to read RA2 .. if hi go into pgm mode.. if not try to run user pgm at 0x500


main2		btfsc	PORTA,2
		goto	main1
;going to check to see that there is something at 0x500... memory is 3FFF when blank
		movlw	0x05
		movwf	ADDRH
		movlw	0x00
		movwf	ADDRH
		call	ReadProgramMemory	;Data in DATAL/H
		movf	DATAH,W
		sublw	0x3F
		btfsc	STATUS,2
		goto	main1
		movf	DATAL,W
		sublw	0xFF
		btfsc	STATUS,2
		goto	main1
		goto	0x500

main1
		call	SendReady
loop1
		call	delay_checkSerial
		movlw	0x08
		xorwf	PORTA,F		;flash led
		btfsc	SerFlags,1
		goto	msgReceived
		goto	loop1


;;;


msgend		movlw	SerialBuffer	;reset buffer
		movwf	SerBuffPtr
		clrf	SerFlags
		bsf	INTCON,PEIE	;enable int
		goto	loop1

msgReceived	
			;we have a message 		
msg22		bcf	INTCON,PEIE	;disable int

		call	ChkMsgValid	;check valid (starts with @@@@)
		btfss	STATUS,Z
		goto	msgend


		bsf 	STATUS, RP1 		;bank2
		movf	SerialBuffer+4,W	;this is cmd
		bcf 	STATUS, RP1 		;bank0
		movwf	A0
		sublw	0x52			; R
		btfsc	STATUS,Z
		goto	cmd_R
		
		movf	A0,W
		sublw	0x72			; r
		btfsc	STATUS,Z
		goto	cmd_r

		movf	A0,W
		sublw	0x57			; W
		btfsc	STATUS,Z
		goto	cmd_W


		movf	A0,W
		sublw	0x77			; w  4 words..
		btfsc	STATUS,Z
		goto	cmd_w

		movf	A0,W
		sublw	0x4A			; J
		btfsc	STATUS,Z
		goto	cmd_J


		movf	A0,W
		sublw	0x5A			; Z
		btfsc	STATUS,Z
		goto	cmd_Z

		movf	A0,W
		sublw	0x43			; C
		btfsc	STATUS,Z
		goto	cmd_C

		movf	A0,W
		sublw	0x58			; X (return PIC type)
		btfsc	STATUS,Z
		goto	cmd_X

		movf	A0,W
		sublw	0x56			; V (return firmware version)
		btfsc	STATUS,Z
		goto	cmd_V

		movf	A0,W
		sublw	0x55			; U - reset processor...
		btfsc	STATUS,Z
		goto	cmd_U


		movf	A0,W
		sublw	0x45			;E - Eprom commands
		btfsc	STATUS,Z		; command is E00aaC00dd where aa is address
		goto	cmd_E			;C is R(read) or W(write and dd is data


		goto	msgend


cmd_end		movlw	0x40
		call	TransmitByte		;to show done....
		goto	msgend

cmd_E		;this reads//writes the EEPROM memory
		call	GetAddr
		bsf 	STATUS, RP1 		;bank2
		movf	SerialBuffer+9,W	;this is cmd either W or R
		bcf 	STATUS, RP1 		;bank0
		bcf 	STATUS, RP1 		;bank0
		bcf	STATUS, RP0
		sublw	0x52			; R
		btfsc	STATUS,Z
		goto	cmd_ER
;it is write
		movlw	SerialBuffer+0x0a		;buffer points to data
		movwf	SerBuffPtr
		call	GetDataWord

	;	bcf 	STATUS, RP1 		;bank0
	;	bcf	STATUS, RP0
	;	movlw	0x33
		;;;movwf	DATAL


		call	writeEEProm
		goto	cmd_end

cmd_ER		call	ReadEEProm
		call	ConvertNumberToHex
		movf	A0,W
		call	TransmitByte
		movf	A1,W
		call	TransmitByte
		goto	cmd_end

cmd_R		;this reads one byte from the address given...

		call	GetAddr			;address in ADDRL/H
		call	ReadProgramMemory	;Data in DATAL/H
		call	SendDataWord
		goto	cmd_end

cmd_r		call	GetAddr			;address in ADDRL/H
		movlw	0x10			;16 bytes 
		movwf	R2
cmd_r1		call	ReadProgramMemory	;Data in DATAL/H
		call	SendDataWord
		incf	ADDRL,F
		movf	ADDRL,W
		btfsc	STATUS,Z
		incf	ADDRH,F
		decfsz	R2,F
		goto	cmd_r1
		goto	cmd_end

cmd_W		call	GetAddr			;address in ADDRL/H
		movlw	SerialBuffer+9		;buffer points to data
		movwf	SerBuffPtr
		call	GetDataWord
		call	WriteProgramMemory
		goto	cmd_end


	

;write 4 bytes...
cmd_w		call	GetAddr			;address in ADDRL/H
		movlw	0x04			;4 bytes 
		movwf	R2	

#ifdef  Use16F876
		movlw	SerialBuffer+9		;buffer points to data				;call write word 4 times
		movwf	SerBuffPtr
cmd_w1		call	GetDataWord
		call	WriteProgramMemory
		incf	ADDRL,F
		movf	ADDRL,W
		btfsc	STATUS,Z
		incf	ADDRH,F
		decfsz	R2,F
		goto	cmd_w1
endif

#ifdef  Use16F876A	;use the skanky write 4 words...

		movlw	SerialBuffer+9		;buffer points to data
		movwf	SerBuffPtr
		call	GetDataWord	;data word in DATAH/L
		movf	DATAL,W
		movwf	DATAbuffer
		movf	DATAH,W
		movwf	DATAbuffer+1

		call	GetDataWord	;data word in DATAH/L
		movf	DATAL,W
		movwf	DATAbuffer+2
		movf	DATAH,W
		movwf	DATAbuffer+3


		call	GetDataWord	;data word in DATAH/L
		movf	DATAL,W
		movwf	DATAbuffer+4
		movf	DATAH,W
		movwf	DATAbuffer+5


		call	GetDataWord	;data word in DATAH/L
		movf	DATAL,W
		movwf	DATAbuffer+6
		movf	DATAH,W
		movwf	DATAbuffer+7

		call	WriteProgramMemory4
endif

#ifdef  Use16F886		;use the skanky write 16 Words...
		BANKSEL	PORTA			;bank0
		movlw	SerialBuffer+9		;buffer points to data
		movwf	SerBuffPtr
		call	WriteProgramMemory8
endif
		goto	cmd_end

cmd

cmd_X
		call	Sendproctype
		goto	cmd_end

cmd_V
		call	SendVersion
		goto	cmd_end




cmd_J		
		movlw	SerialBuffer		;reset buffer
		movwf	SerBuffPtr
		clrf	SerFlags
		movlw	0x40
		call	TransmitByte		;to show done....
		bsf	INTCON,PEIE		;enable int

		goto	0x500			; jump for code for FlowDaigrams...


;CODE BELOW DOES PROPER JUMP TO GIVEN ADDRESS.... onli works with 16F876 at present..
		call	GetAddr			;address in ADDRL/H
		movf	ADDRL,W
		movwf	DATAL
		movf	ADDRH,W
		movwf	DATAH
		bsf	DATAH,5
		bcf	DATAH,4
		bsf	DATAH,3			;goto is 10 1xxx xxxx xxxx

;need to set bitw3/4 of PCLATH
		bcf	PCLATH,4
		bcf	PCLATH,3
		btfsc	ADDRH,3
		bsf	PCLATH,3
		btfsc	ADDRH,4
		bsf	PCLATH,4
		movlw 	low cmd_J1
		movwf	ADDRL
		movlw 	high cmd_J1
		movwf	ADDRH
		call	WriteProgramMemory	;writes goto instruction
		movlw	SerialBuffer		;reset buffer
		movwf	SerBuffPtr
		clrf	SerFlags
		movlw	0x40
		call	TransmitByte		;to show done....
		bsf	INTCON,PEIE		;enable int
cmd_J1		goto	0000

cmd_Z		call	SendReady		;to check ready
		goto	cmd_end
	

break_point	;the return address to continue is on top of stack
		;W will contain the break number
		; we just need to send a message saying we have reached break..
		;will send OCR working regs, and Z flag as well?
		;send W
		movwf	w_Btemp		; save off current W register contents
		movf	STATUS,w	; move status register into W register
		movwf	status_Btemp	; save off contents of STATUS register

		call	ConvertNumberToHex	;send Status
		movf	A0,W
		call	TransmitByte
		movf	A1,W
		call	TransmitByte

		movf	w_Btemp,W		;send W ( which has break number...)
		call	ConvertNumberToHex
		movf	A0,W
		call	TransmitByte
		movf	A1,W
		call	TransmitByte

		movlw	0x20			;sends working regs
		movwf 	FSR
		bcf	STATUS,7		;irp bit so access bank 0
		movlw	0x08
		movwf	temp1
cmd_S1		movf	INDF,W
		call	ConvertNumberToHex
		movf	A0,W
		call	TransmitByte
		movf	A1,W
		call	TransmitByte
		incf	FSR
		decfsz	temp1
		goto	cmd_S1

		;send input port

		movf	PORTC,W
		andlw	0x3F
		movwf	A1

		movf	PORTA,W
		andlw	0x30
		movwf	A0
		rlf	A0,F
		rlf	A0,F
		movf	A0,W
		andlw	0xC0
		addwf	A1,W


		call	ConvertNumberToHex
		movf	A0,W
		call	TransmitByte
		movf	A1,W
		call	TransmitByte

		;send output port

		movf	PORTB,W
		call	ConvertNumberToHex
		movf	A0,W
		call	TransmitByte
		movf	A1,W
		call	TransmitByte

		
		goto	cmd_end


cmd_C		;continue after break   address should be on stack!


		movf    status_Btemp,w	; retrieve copy of STATUS register
		movwf	STATUS		;restore pre-isr STATUS register contents
		swapf   w_Btemp,f
		swapf   w_Btemp,w	; restore pre-isr W register contents
		return

cmd_U
		movlw	0x40
		call	TransmitByte		;to show done....		
		goto	0000



	

Msg_Ready	da  "Ready",0x0a0d,0x0000
ifdef  Use16F876
Msg_Proc	da  "16F876",0x0a0d,0x0000
endif
ifdef  Use16F876A
Msg_Proc	da  "16F876A",0x0a0d,0x0000
endif
ifdef  Use16F886
Msg_Proc	da  "16F886",0x0a0d,0x0000
endif

;ver 1.2 has some bugs corrected and the hooks for 16F886 chipa
;ver 1.3 adds the code for simpe break option in Flow diagrams and basic printing support
;ver 1.4 correct the bug on PORT B for 16F886 chips and add the self-test code 
;ver 1.4  which requires input port coupled to output port.
;ver 1.5  minor corrections
;ver 2.1   swops input and output ports so inpuit on Port C and output onport B
;          also removed the self test stuff
;ver 2.2   correct jon_cowley stuff to use Port B for output...
;ver 2.3   add cmd E to read and write EEProm memory also added the psuedo OCR calls READDATAEEPROM, WRITEDATAEEPROM
;ver 2.4   speed up serial stuff by checking for serial msg in delay routine

Msg_Version	da "Version 2.4",0x0a0d,0x0000

Msg_Received	da  "Received",0x0d0a,0x0000
Msg_Valid	da  "Valid ",0x0d0a,0x0000
Msg_CMDR	da  "CMD R:   "0x0d0a,0x0000
Msg_R		da  "next:   ",0x0d00
Msg_User	da  "In user Code!  ",0x0d00


;jump table
	ORG	0x300
		goto	WAIT1MS
		goto	READADC
		goto	READTABLE
		goto	break_point
		goto	TransmitByte
		goto	Jon_Cowley_recieve
		goto	Jon_Cowley_TRANSMIT
		goto	READDATAEEPROM
		goto	WRITEDATAEEPROM


Jon_Cowley_TRANSMIT
		call	READADC
		movf	R0,W
		call	TransmitByte
		call	WAIT1MS
		call	WAIT1MS
		goto 	Jon_Cowley_TRANSMIT

Jon_Cowley_recieve

		movlw	SerialBuffer
		movwf	SerBuffPtr
		movf	SerBuffPtr,W
		movwf 	FSR
		bsf	STATUS,7	;irp bit so access bank 2
		movf	INDF,W
;have the byte from serial
		BANKSEL	PORTB
		MOVWF	PORTB
		goto	Jon_Cowley_recieve
		

    END