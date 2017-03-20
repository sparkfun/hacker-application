#include "stdafx.h"
#include "MapLoader.h"
#include <fstream>
#include <iostream>
#include <string>


std::unordered_map<int, std::unordered_map<int, std::string>> MapLoader::loadMap(char* file)
{
	std::unordered_map<int, std::unordered_map<int, std::string>> baseMap;
	std::ifstream ccSettings(file);

	int mapID = 0;
	std::unordered_map<int, std::string> triggerMap;


	char* buffer = (char *)malloc(64);
	while (ccSettings.getline(buffer, 64))
	{
		std::string line = std::string(buffer);
		//line contains trigger
		if (line.find("trigger") != std::string::npos)
		{
			int position = line.find("key");
			int first = line.find("\"", position + 1);
			int second = line.find("\"", first + 1);
			int key;
			key = std::stoi(line.substr(first + 1, second - first - 1));

			position = line.find("value");
			first = line.find("\"", position + 1);
			second = line.find("\"", first + 1);
			std::string value = line.substr(first + 1, second - first - 1);
			triggerMap[key] = value;
		}

		//line contains /basemap
		else if (line.find("/basemap") != std::string::npos)
		{
			baseMap[mapID] = triggerMap;
			triggerMap.clear();
		}
		//line contains basemap
		else if (line.find("basemap") != std::string::npos)
		{
			//Create a map, and store the int
			int position = line.find("value");
			int first = line.find("\"", position + 1);
			int second = line.find("\"", first + 1);
			mapID = std::stoi(line.substr(first + 1, second - first - 1));
		}
		//line contains /layout
	}
	return baseMap;
}

MapLoader::MapLoader()
{
}


MapLoader::~MapLoader()
{
}
