//
// Created by Austin Prete on 4/9/18.
//

#ifndef SPREADSHEET_H
#define SPREADSHEET_H


#include <set>
#include <string>
#include <map>

class Spreadsheet
{

public:
  Spreadsheet();

  void AddSubscriber(int client_id);

  std::set<int> GetSubscribers() const;

  void RemoveSubscriber(int client_id);

  void ChangeCellContents(std::string cell_name, std::string new_contents);

  std::string GetFullStateString() const;

  void WriteSpreadsheetToFile(std::string path) const;

  static void CreateSpreadsheetsMapXmlFile(const std::string &folder);

  static std::map<std::string, Spreadsheet *> LoadSpreadsheetsMapFromXml(const std::string &folder);

  static Spreadsheet *LoadSpreadsheetFromFile(std::string path);

private:
  std::map<std::string, std::vector<std::string>> spreadsheet_map;
  int id;
  std::set<int> current_subscribers;
};


#endif //SPREADSHEET_H