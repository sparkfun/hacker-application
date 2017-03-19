#include "KeyAdapter.h"
#include <Windows.h>
#include <cstdlib>
#include <unordered_map>

#pragma once
class WinKeyAdapter: public KeyAdapter
{
public:
	WinKeyAdapter();
	~WinKeyAdapter();
	void receiveCommand(char *);
	void receiveCommand(std::string);

private:
	int mapInput(char);
	void processEsc(std::string);
	INPUT keyEvents;
	enum escape {
		BSP,
		BRK,
		CLK,
		DEL,
		DAR,
		END,
		ENT,
		ESC,
		HLP,
		HOM,
		INS,
		LAR,
		NLK,
		PGD,
		PGU,
		PSC,
		RAR,
		SLK,
		TAB,
		UAR,
		F01,
		F02,
		F03,
		F04,
		F05,
		F06,
		F07,
		F08,
		F09,
		F10,
		F11,
		F12,
		F13,
		F14,
		F15,
		F16,
		KPA,
		KPS,
		KPM,
		KPQ,
		QUO,
		CTR,
		SHT,
		WIN,
		ALT
	};
	std::unordered_map<std::string, escape> stringToEscape;
};


