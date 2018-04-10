//
// Created by Austin Prete on 4/9/18.
//

#ifndef SPREADSHEET_H
#define SPREADSHEET_H


#include <string>
#include <map>

class Spreadsheet
{

public:
  static Spreadsheet *LoadSpreadsheetFromFile(std::string path);

  Spreadsheet();

private:
  std::map<std::string, std::string> spreadsheet_map;
  int id;
};


#endif //SPREADSHEET_H
