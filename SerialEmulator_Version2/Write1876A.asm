




	BSF STATUS,RP1 ;
	BCF STATUS,RP0 ; Bank 2
	MOVF ADDRH,W ; Load initial address
	MOVWF EEADRH ;
	MOVF ADDRL,W ;
	MOVWF EEADR ;

	MOVF DATAADDR,W ; Load initial data address
	MOVWF FSR ;
LOOP 	MOVF INDF,W ; Load first data byte into lower
	MOVWF EEDATA ;
	INCF FSR,F ; Next byte
	MOVF INDF,W ; Load second data byte into upper
	MOVWF EEDATH ;
	INCF FSR,F ;
	BSF STATUS,RP0 ; Bank 3
	BSF EECON1,EEPGD ; Point to program memory
	BSF EECON1,WREN ; Enable writes
	BCF INTCON,GIE ; Disable interrupts (if using)
	MOVLW 55h ; Start of required write sequence:
	MOVWF EECON2 ; Write 55h
	MOVLW AAh ;
	MOVWF EECON2 ; Write AAh
	BSF EECON1,WR ; Set WR bit to begin write
	NOP ; Any instructions here are ignored as processor
	; halts to begin write sequence
	NOP ; processor will stop here and wait for write complete
	; after write processor continues with 3rd instruction
	BCF EECON1,WREN ; Disable writes
	BSF INTCON,GIE ; Enable interrupts (if using)
	BCF STATUS,RP0 ; Bank 2
	INCF EEADR,F ; Increment address
	MOVF EEADR,W ; Check if lower two bits of address are ‘00’
	ANDLW 0x03 ; Indicates when four words have been programmed
	XORLW 0x03 ;
	BTFSC STATUS,Z ; Exit if more than four words,
	GOTO LOOP ; Continue if less than four words