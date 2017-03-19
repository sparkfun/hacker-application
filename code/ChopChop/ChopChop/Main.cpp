// ConsoleApplication2.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Windows.h"
#include "mmsystem.h"
#include <iostream>
#include <stdio.h>
#include "WinMIDIAdapter.h"
#include "KeyAdapter.h"
#include "WinKeyAdapter.h"



#pragma comment(lib, "winmm.lib")

using namespace System;

WinMIDIAdapter* winMidi;
KeyAdapter* adapter;

int connect(int devId, MKBridge* bridge)
{
	winMidi = new WinMIDIAdapter(devId, bridge);
	return 0;
}


int main() {
	WinKeyAdapter winAdapter = WinKeyAdapter();
	adapter = &winAdapter;
	MKBridge bridge = MKBridge(adapter);
	connect(0, &bridge);
	 

	//loop and wait for events.
	int active = 0;
	while (active < 60) 
	{
		
		//active++;
		Sleep(500);
	}

	printf("Process terminating.\n");
	delete(winMidi);
	return 0;
}

