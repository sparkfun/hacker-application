#include "stdafx.h"
#include "WinMIDIAdapter.h"
#include "Windows.h"
#include "mmsystem.h"


HMIDIIN midiIn;
LPMIDIHDR midiInHdr;
MKBridge bridge;

//Listen for MIDI Events
static void CALLBACK midiInputCallback(HMIDIIN hMidiIn, UINT wMsg, DWORD_PTR dwInstance, DWORD_PTR dwParam1, DWORD_PTR dwParam2) 
{
	bridge.readMidi(HIBYTE(dwParam1), LOBYTE(dwParam1));
	dwInstance;
	wMsg;	hMidiIn;
	return;
}

int WinMIDIAdapter::sendEvent(int byte)
{
	return byte;
}

//Default constructor
WinMIDIAdapter::WinMIDIAdapter()
{
	midiIn = NULL;
	midiInHdr = NULL;
}

//Constructor
WinMIDIAdapter::WinMIDIAdapter(int devID, MKBridge *_bridge)
{
	//Associate a bridge with adapter
	bridge = *_bridge;

	//Check to see if selected device is in range
	midiIn = NULL;
	if (devID > (int)midiInGetNumDevs())
	{
		return;
	}

	//attempt to open midi connection on selected port
	int status = midiInOpen((LPHMIDIIN)&midiIn, devID, (DWORD_PTR)&midiInputCallback, 0, CALLBACK_FUNCTION);

	//make sure connection is succesful. 
	if (status != MMSYSERR_NOERROR)
	{
		return;
	}

	//Start the midi stream
	midiInStart(midiIn);

	//Initialize buffer
	midiInHdr = (LPMIDIHDR)malloc(1024);
	midiInHdr->lpData = (LPSTR)malloc(512);
	midiInHdr->dwBufferLength = 512;
	MMRESULT flag = midiInPrepareHeader(midiIn, midiInHdr, (UINT) sizeof(MIDIHDR));
	flag = midiInAddBuffer(midiIn, midiInHdr, sizeof(MIDIHDR));

	return;
}


//Deconstructor
WinMIDIAdapter::~WinMIDIAdapter()
{
	midiInUnprepareHeader(midiIn, midiInHdr, 1024);
	midiInClose(midiIn);
}
