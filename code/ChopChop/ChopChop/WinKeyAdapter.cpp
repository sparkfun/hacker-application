#include "stdafx.h"
#include "WinKeyAdapter.h"
#include <iostream>

WinKeyAdapter::WinKeyAdapter()
{	
	//initialize input to receive virtual key strokes
	this->keyEvents.type = INPUT_KEYBOARD;
	this->keyEvents.ki.wScan = 0;
	this->keyEvents.ki.time = 0;
	this->keyEvents.ki.dwExtraInfo = 0;

	//Build unordered map for converting string to escape enum
	stringToEscape.emplace("BSP", BSP);
	stringToEscape.emplace("BRK", BRK);
	stringToEscape.emplace("CLK", CLK);
	stringToEscape.emplace("DEL", DEL);
	stringToEscape.emplace("DAR", DAR);
	stringToEscape.emplace("END", END);
	stringToEscape.emplace("ENT", ENT);
	stringToEscape.emplace("ESC", ESC);
	stringToEscape.emplace("HLP", HLP);
	stringToEscape.emplace("HOM", HOM);
	stringToEscape.emplace("INS", INS);
	stringToEscape.emplace("LAR", LAR);
	stringToEscape.emplace("NLK", NLK);
	stringToEscape.emplace("PGD", PGD);
	stringToEscape.emplace("PGU", PGU);
	stringToEscape.emplace("PSC", PSC);
	stringToEscape.emplace("RAR", RAR);
	stringToEscape.emplace("SLK", SLK);
	stringToEscape.emplace("TAB", TAB);
	stringToEscape.emplace("UAR", UAR);
	stringToEscape.emplace("F01", F01);
	stringToEscape.emplace("F02", F02);
	stringToEscape.emplace("F03", F03);
	stringToEscape.emplace("F04", F04);
	stringToEscape.emplace("F05", F05);
	stringToEscape.emplace("F06", F06);
	stringToEscape.emplace("F07", F07);
	stringToEscape.emplace("F08", F08);
	stringToEscape.emplace("F09", F09);
	stringToEscape.emplace("F10", F10);
	stringToEscape.emplace("F11", F11);
	stringToEscape.emplace("F12", F12);
	stringToEscape.emplace("F13", F13);
	stringToEscape.emplace("F14", F14);
	stringToEscape.emplace("F15", F15);
	stringToEscape.emplace("F16", F16);
	stringToEscape.emplace("KPA", KPA);
	stringToEscape.emplace("KPS", KPS);
	stringToEscape.emplace("KPM", KPM);
	stringToEscape.emplace("KPQ", KPQ);
	stringToEscape.emplace("QUO", QUO);
	stringToEscape.emplace("CTR", CTR);
	stringToEscape.emplace("SHT", SHT);
	stringToEscape.emplace("WIN", WIN);
	stringToEscape.emplace("ALT", ALT);
}

//destructor
WinKeyAdapter::~WinKeyAdapter()
{
}


void WinKeyAdapter::receiveCommand(char* cmd)
{
	int length = strlen(cmd);
	//std::cout << length << " : " << cmd << "\n";
	for (int i = 0; i < length; i++)
	{
		//Check for escape sequence ('&?')
		if (cmd[i] == '&' && cmd[i + 1] == '?')
		{
			std::string esc = {cmd[i + 2], cmd[i + 3], cmd[i + 4] };
			processEsc(esc);
			i += 5;
		}
		//interpret string literally and figure out it's virtual key code.
		else
		{
			boolean shifted = (mapInput(cmd[i]) & 0x100) >> 8;
			if (shifted)
			{
				this->keyEvents.ki.wVk = VK_LSHIFT;
				this->keyEvents.ki.dwFlags = 0;
				SendInput(1, &keyEvents, sizeof(INPUT));
			}
			

			this->keyEvents.ki.wVk = mapInput(cmd[i]);
			this->keyEvents.ki.dwFlags = 0;
			SendInput(1, &keyEvents, sizeof(INPUT));

			this->keyEvents.ki.dwFlags = KEYEVENTF_KEYUP;
			SendInput(1, &keyEvents, sizeof(INPUT));

			
			if (shifted)
			{
				keyEvents.ki.dwFlags = KEYEVENTF_KEYUP;
				keyEvents.ki.wVk = VK_LSHIFT;
				SendInput(1, &keyEvents, sizeof(INPUT));
			}
		}
	}

	return;
}


void WinKeyAdapter::receiveCommand(std::string cmd)
{

}

//Take a char, and find out its virtual code.  Is this necessary?
int WinKeyAdapter::mapInput(char in)
{
	int output = VkKeyScan(in);
	return output;
}

//Send the virtual key for the associated escape sequence.
void WinKeyAdapter::processEsc(std::string esc)
{

	escape _esc = stringToEscape[esc];

	switch (_esc)
	{
	case BSP:							//Backspace
		this->keyEvents.ki.wVk = 0x08;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case BRK:							//Break
		break;
	case CLK:				 			//Caps Lock
		this->keyEvents.ki.wVk = 0x14;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case DEL:							//Delete
		this->keyEvents.ki.wVk = 0x2E;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case DAR:							//Down Arrow
		this->keyEvents.ki.wVk = 0x28;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case END:							//End
		this->keyEvents.ki.wVk = 0x23;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case ENT:							//Enter
		this->keyEvents.ki.wVk = 0x0D;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case ESC:							//Escape
		this->keyEvents.ki.wVk = 0x1B;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case HLP:							//Help
		this->keyEvents.ki.wVk = 0x2F;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case HOM:							//Home
		this->keyEvents.ki.wVk = 0x24;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case INS:							//Insert
		this->keyEvents.ki.wVk = 0x2D;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case LAR:							//Left Arrow
		this->keyEvents.ki.wVk = 0x25;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case NLK:							//Num Lock
		this->keyEvents.ki.wVk = 0x90;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case PGD:							//Page Down
		this->keyEvents.ki.wVk = 0x22;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case PGU:							//Page Up
		this->keyEvents.ki.wVk = 0x21;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case PSC:							//Print Screen
		this->keyEvents.ki.wVk = 0x2C;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case RAR:							//Right Arrow
		this->keyEvents.ki.wVk = 0x27;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case SLK:							//Scroll Lock
		this->keyEvents.ki.wVk = 0x91;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case TAB:							//Tab
		this->keyEvents.ki.wVk = 0x09;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case UAR:							//Up Arrow
		this->keyEvents.ki.wVk = 0x27;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F01:							//F1
		this->keyEvents.ki.wVk = 0x70;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F02:							//F2
		this->keyEvents.ki.wVk = 0x71;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F03:							//F3
		this->keyEvents.ki.wVk = 0x72;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F04:							//F4
		this->keyEvents.ki.wVk = 0x73;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F05:							//F5
		this->keyEvents.ki.wVk = 0x74;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F06:							//F6
		this->keyEvents.ki.wVk = 0x75;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F07:							//F7
		this->keyEvents.ki.wVk = 0x76;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F08:							//F8
		this->keyEvents.ki.wVk = 0x77;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F09:							//F9
		this->keyEvents.ki.wVk = 0x78;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F10:
		this->keyEvents.ki.wVk = 0x79;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F11:
		this->keyEvents.ki.wVk = 0x7A;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F12:
		this->keyEvents.ki.wVk = 0x7B;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F13:
		this->keyEvents.ki.wVk = 0x7C;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F14:
		this->keyEvents.ki.wVk = 0x7D;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F15:
		this->keyEvents.ki.wVk = 0x7E;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case F16:
		this->keyEvents.ki.wVk = 0x7F;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case KPA:									//Keypad Add
		this->keyEvents.ki.wVk = 0x6B;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case KPS:									//Keypad Subtract
		this->keyEvents.ki.wVk = 0x6D;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case KPM:									//Keypad Multiply
		this->keyEvents.ki.wVk = 0x6A;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case KPQ:									//Keypad divide
		this->keyEvents.ki.wVk = 0x6F;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
		break;
	case QUO:
		this->keyEvents.ki.wVk = VK_LSHIFT;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));

		this->keyEvents.ki.wVk = 0xDE;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));

		keyEvents.ki.dwFlags = KEYEVENTF_KEYUP;
		keyEvents.ki.wVk = VK_LSHIFT;
		SendInput(1, &keyEvents, sizeof(INPUT));
	case CTR:									//CTRL
		this->keyEvents.ki.wVk = 0x11;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
	case SHT:									//Shift
		this->keyEvents.ki.wVk = 0x10;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
	case WIN:									//Windows Key
		this->keyEvents.ki.wVk = 0x5B;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
	case ALT:									//Alt key
		this->keyEvents.ki.wVk = 0x12;
		this->keyEvents.ki.dwFlags = 0;
		SendInput(1, &keyEvents, sizeof(INPUT));
	default:
		break;
	}
}