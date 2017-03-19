#include "MKBridge.h"

#pragma once
class WinMIDIAdapter
{
public:
	WinMIDIAdapter();
	WinMIDIAdapter(int, MKBridge*);
	static int sendEvent(int);
	~WinMIDIAdapter();
};

