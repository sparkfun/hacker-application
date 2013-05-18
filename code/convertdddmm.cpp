#!/bin/sh
awk 'FNR>4' convertdddmm.cpp | g++ -o convertdddmm -lm -x c++ -
exit;

#include <iostream>
#include <cstdlib>
#include <cmath>
#include <iomanip>
#include <sstream>

using namespace std;

int main(int argc, char *argv[])
{
	double ddd = 0.0, input = 0.0, mm = 0;
	char tx = '\0';

	if(argc != 3)
	{
		cout << "Usage: " << argv[0] << "InputNumber N/W/S/E\n";
		return 1;
	}

	stringstream ss(argv[1]);
	ss >> input;
	tx = argv[2][0];

	ddd = floor(input/100.0);
	mm = modf(input/100.0, &ddd);
	input = ddd + (mm * 100.0) / 60.0;

	if((tx == 's') || (tx == 'S'))
	{
		if(input > 0)
			input = -input;
		tx = 'S';
	}
	if(tx == 'e') tx = 'E';
	if((tx == 'w') || (tx == 'W'))
	{
		if(input > 0)
			input = -input;
		tx = 'W';
	}
	if(tx == 'n') tx = 'N';

	cout << setprecision(12) << input << endl;

	return 0;
}
