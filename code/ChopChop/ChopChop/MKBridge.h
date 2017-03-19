#include "KeyAdapter.h"
#include <unordered_map>
#include <fstream>
#include "Parameters.h"
#include "WinKeyAdapter.h"

#pragma once
class MKBridge
{
public:
	MKBridge();
	MKBridge(KeyAdapter*);
	MKBridge(WinKeyAdapter*);
	~MKBridge();

	void readMidi(int, int);

private:
	KeyAdapter* adapter;
	
	void processKeyUp(int);
	void processKeyDown(int);
	int noteToPrime(int);
	boolean triggered;

	int startOfRange;
	int baseValue;
	int triggerValue;
	std::unordered_map<int, std::unordered_map<int, std::string>> baseMap;
};

