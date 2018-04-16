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
  Spreadsheet(std::string name, std::string file_path);

  void AddSubscriber(int client_id);

  std::set<int> GetSubscribers() const;

  void RemoveSubscriber(int client_id);

  void ChangeCellContents(std::string cell_name, std::string new_contents);

  std::string GetFullStateString() const;

  std::string GetName() const;

  void WriteSpreadsheetToFile() const;

  static void CreateSpreadsheetsMapXmlFile(const std::string &folder);

  static std::map<std::string, Spreadsheet *> LoadSpreadsheetsMapFromXml(const std::string &folder);

  static Spreadsheet *LoadSpreadsheetFromFile(std::string name, std::string path);

private:
  std::map<std::string, std::vector<std::string>> spreadsheet_map;
  std::set<int> current_subscribers;

  std::string name;
  std::string file_path;
};


#endif //SPREADSHEET_H
