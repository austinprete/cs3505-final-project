/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#ifndef SPREADSHEET_H
#define SPREADSHEET_H


#include <cstdlib>
#include <set>
#include <string>
#include <map>
#include <mutex>


class Spreadsheet
{

public:
  Spreadsheet(std::string name, std::string file_path);

  void AddSubscriber(int client_id);

  std::set<int> GetSubscribers() const;

  void RemoveSubscriber(int client_id);

  void ChangeCellContents(std::string cell_name, std::string new_contents);

  std::string RevertCellContents(std::string cell_name);

  std::pair<std::string, std::string> UndoLastChange();

  std::string GetFullStateString() const;

  std::string GetName() const;

  std::string GetFile() const;

  void WriteSpreadsheetToFile(const std::string &directory);

  static void CreateSpreadsheetsMapXmlFile(const std::string &folder);

  static std::map<std::string, Spreadsheet *> LoadSpreadsheetsMapFromXml(const std::string &folder);

  static Spreadsheet *LoadSpreadsheetFromFile(std::string name, std::string path, std::string directory);

  static void
  WriteSpreadsheetsMapXmlFile(const std::string &folder, std::map<std::string, Spreadsheet *> *spreadsheets_map);

//  void UndoChange(bool is_edit, std::string cell_name, std::string cell_contents);

private:
  std::map<std::string, std::vector<std::string>> spreadsheet_map;
  std::vector<std::pair<bool, std::pair<std::string, std::string>>> undo_history;
  std::set<int> current_subscribers;

  std::mutex file_mutex;

  std::string name;
  std::string file_path;
};


#endif //SPREADSHEET_H
