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
  Spreadsheet();

  void WriteSpreadsheetToFile(std::string path) const;

  static Spreadsheet *LoadSpreadsheetFromFile(std::string path);

private:
  std::map<std::string, std::string> spreadsheet_map;
  int id;
};


#endif //SPREADSHEET_H
