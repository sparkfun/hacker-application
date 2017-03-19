#pragma once
class KeyAdapter
{
public:
	KeyAdapter();
	~KeyAdapter();

	virtual void receiveCommand(char*) = 0;
};

