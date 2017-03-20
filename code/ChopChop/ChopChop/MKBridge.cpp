#include "stdafx.h"
#include "MKBridge.h"
#include "KeyAdapter.h"
#include "MapLoader.h"
#include <fstream>
#include <iostream>
#include <string>

//Receive midimessages from MidiAdapter, translate it into a string from the layout, and send that string to the key adapter.
void MKBridge::readMidi(int midiValue, int action)
{
	if (action == 128) //keyup
	{
		processKeyUp(midiValue);
	}
	else if (action == 144) //keydown
	{
		processKeyDown(midiValue);
	}
	return;
}


void MKBridge::processKeyUp(int midiValue)
{
	//Key is in the base octave
	if (midiValue - startOfRange < 12)
	{
		baseValue /= noteToPrime(midiValue % 12);
	}
	else if (triggered == FALSE)
	{
		char* toSend = (char *)baseMap[baseValue][triggerValue].c_str();
		adapter->receiveCommand(toSend);

		triggerValue /= noteToPrime(midiValue % 12);
		if (triggerValue != 1)
		{
			triggered = TRUE;
		}
		else
		{
			triggered = FALSE;
		}
	}
	else
	{
		triggerValue /= noteToPrime(midiValue % 12);
		if (triggerValue == 1)
		{
			triggered = FALSE;
		}
	}
}
void MKBridge::processKeyDown(int midiValue)
{
	if (baseValue == 1)	//No baseValue defined
	{
		startOfRange = midiValue - (midiValue % 12);
		baseValue *= noteToPrime(midiValue % 12);
	}
	else if (midiValue - startOfRange < 12) //base octave
	{
		baseValue *= noteToPrime(midiValue % 12);
	}
	else
	{
		triggerValue *= noteToPrime(midiValue % 12);
	}
}

int MKBridge::noteToPrime(int noteValue)
{
	switch (noteValue)
	{
	case 0:
		return 2;
		break;
	case 1:
		return 3;
		break;
	case 2:
		return 5;
		break;
	case 3:
		return 7;
		break;
	case 4:
		return 11;
		break;
	case 5:
		return 13;
		break;
	case 6:
		return 17;
		break;
	case 7:
		return 19;
		break;
	case 8:
		return 23;
		break;
	case 9:
		return 29;
		break;
	case 10:
		return 31;
		break;
	case 11:
		return 37;
		break;
	default:
		return 1;
		break;

	};
}

//Default constructor
MKBridge::MKBridge()
{
	startOfRange = 0;
	baseValue = 1;
	triggerValue = 1;
}

//constructor
MKBridge::MKBridge(KeyAdapter* _adapter)
{
	startOfRange = 0;
	baseValue = 1;
	triggerValue = 1;
	adapter = _adapter;
	triggered = FALSE;

	baseMap = MapLoader::loadMap("testLayout.xml");
}


//Destructor
MKBridge::~MKBridge()
{
}
