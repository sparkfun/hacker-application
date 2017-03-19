#include <unordered_map>

#pragma once

class MapLoader
{
public:
	~MapLoader();

	static std::unordered_map<int, std::unordered_map<int, std::string>> loadMap(char* file);

private:
	MapLoader();
};

