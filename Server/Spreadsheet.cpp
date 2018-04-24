/**
 * Mathew Beseris, Cindy Liao, Cole Perschon, and Austin Prete
 * CS3505 - Spring 2018
 */

#include <cstdlib>
#include <iostream>
#include <mutex>
#include <regex>
#include <string>
#include <thread>
#include <vector>

#include <boost/algorithm/string.hpp>
#include <boost/regex.hpp>

#include "Dependencies/rapidxml-1.13/rapidxml.hpp"
#include "Dependencies/rapidxml-1.13/rapidxml_print.hpp"
#include "Dependencies/rapidxml-1.13/rapidxml_utils.hpp"

#include "Spreadsheet.h"

using namespace std;
using namespace rapidxml;
using namespace boost;

Spreadsheet::Spreadsheet(std::string name, std::string file_path) : spreadsheet_map(), name(name), file_path(file_path)
{}

void Spreadsheet::WriteSpreadsheetToFile(const string &directory)
{
  xml_document<> doc;
  xml_node<> *root_node = doc.allocate_node(node_element, "spreadsheet");
  doc.append_node(root_node);

  string undo_history_string;

  for (auto &undo_entry : undo_history) {

    string was_edit = undo_entry.first ? "true" : "false";

    undo_history_string.append(was_edit);
    undo_history_string.push_back((char) 1);

    undo_history_string.append(undo_entry.second.first);
    undo_history_string.push_back((char) 1);

    undo_history_string.append(undo_entry.second.second);
    undo_history_string.push_back((char) 1);

    undo_history_string.push_back((char) 2);
  }

  std::lock_guard<std::mutex> undo_guard(undo_history_mutex);

  // Pop final separator off
  if (!undo_history.empty()) {
    undo_history_string.pop_back();
  }

  xml_node<> *undo_history_node = doc.allocate_node(node_element, "undo_history", undo_history_string.c_str());
  root_node->append_node(undo_history_node);

  for (auto &cell : spreadsheet_map) {
    xml_node<> *cell_node = doc.allocate_node(node_element, "cell");

    xml_node<> *name_node = doc.allocate_node(node_element, "name", cell.first.c_str());

    string terminator_string;
    terminator_string.push_back((char) 2);

    auto *joined_contents_string = new string();
    joined_contents_string->append(boost::algorithm::join(cell.second, terminator_string));

    xml_node<> *contents_node = doc.allocate_node(node_element, "contents", joined_contents_string->c_str());

    cell_node->append_node(name_node);
    cell_node->append_node(contents_node);

    root_node->append_node(cell_node);
  }

  std::lock_guard<std::mutex> guard(file_mutex);

  ofstream outfile;
  outfile.open(directory + "/" + file_path, ios::out | ios::trunc);

  outfile << doc;

  outfile.close();
}

void Spreadsheet::ChangeCellContents(std::string cell_name, std::string new_contents)
{
  cout << "Changing cell contents of cell " << cell_name << " to: " << new_contents << endl;

  boost::to_upper(cell_name);

  boost::regex cell_name_pattern("^[A-Z]{1}[1-9]{1}[0-9]{0,1}$");

  if (!regex_match(cell_name, cell_name_pattern)) {
    return;
  }

  std::string previous_contents;

  std::lock_guard<std::mutex> guard(spreadsheet_map_mutex);

  auto search = spreadsheet_map.find(cell_name);

  if (search != spreadsheet_map.end()) {
    if (!search->second.empty()) {
      previous_contents = (*search).second.back();
    }
    (*search).second.push_back(new_contents);
  } else {
    vector<string> contents_history;
    contents_history.push_back(new_contents);
    spreadsheet_map.insert(std::make_pair(cell_name, contents_history));
  }

  std::lock_guard<std::mutex> undo_guard(undo_history_mutex);

  undo_history.emplace_back(true, std::make_pair(cell_name, previous_contents));
}

std::string Spreadsheet::GetFullStateString()
{
  ostringstream full_state_stream;

  full_state_stream << "full_state ";

  std::lock_guard<std::mutex> guard(spreadsheet_map_mutex);

  for (const auto &cell : spreadsheet_map) {
    string name = cell.first;

    if (cell.second.empty()) {
      continue;
    }

    string contents = cell.second.back();

    full_state_stream << name << ":" << contents << '\n';
  }

  return full_state_stream.str();
}

void Spreadsheet::CreateSpreadsheetsMapXmlFile(const string &folder)
{
  string path = folder + "/spreadsheets_map.xml";

  xml_document<> doc;
  xml_node<> *root_node = doc.allocate_node(node_element, "spreadsheets");
  doc.append_node(root_node);

  ofstream outfile;
  outfile.open(path, ios::out | ios::trunc);

  outfile << doc;

  outfile.close();
}

void Spreadsheet::WriteSpreadsheetsMapXmlFile(const std::string &folder,
                                              std::map<std::string, Spreadsheet *> *spreadsheets_map)
{
  xml_document<> doc;
  xml_node<> *root_node = doc.allocate_node(node_element, "spreadsheets");
  doc.append_node(root_node);

  string map_file_path = folder + "/spreadsheets_map.xml";

  for (auto &spreadsheet_info : (*spreadsheets_map)) {
    xml_node<> *spreadsheet_node = doc.allocate_node(node_element, "spreadsheet");

    xml_node<> *name_node = doc.allocate_node(node_element, "name", spreadsheet_info.first.c_str());

    string *file = new string();
    Spreadsheet *sheet = spreadsheet_info.second;

    file->append(sheet->GetFile());

    xml_node<> *file_path_node = doc.allocate_node(node_element, "file", file->c_str());

    spreadsheet_node->append_node(name_node);
    spreadsheet_node->append_node(file_path_node);

    root_node->append_node(spreadsheet_node);
  }

  ofstream outfile;
  outfile.open(map_file_path, ios::out | ios::trunc);

  outfile << doc;

  outfile.close();
}

Spreadsheet *Spreadsheet::LoadSpreadsheetFromFile(std::string name, std::string path, std::string directory)
{
  string spreadsheet_path = directory + "/" + path;

  rapidxml::file<> xmlFile(spreadsheet_path.c_str());
  xml_document<> doc;
  doc.parse<0>(xmlFile.data());

  Spreadsheet *sheet = new Spreadsheet(name, path);


  string undo_history_string = doc.first_node("spreadsheet")->first_node("undo_history")->value();

  if (!undo_history_string.empty()) {

    vector<string> undo_entries;
    split(undo_entries, undo_history_string, is_from_range(2, 2));

    for (auto &undo_entry : undo_entries) {
      vector<string> undo_values;
      split(undo_values, undo_entry, is_from_range(1, 1));

      bool is_edit = (undo_values.at(0) == "true");

      string cell_name = undo_values.at(1);

      string cell_contents;

      if (undo_values.size() == 3) {
        cell_contents = undo_values.at(2);
      }

      sheet->undo_history.emplace_back(is_edit, std::make_pair(cell_name, cell_contents));
    }

  }

  xml_node<> *current_cell = doc.first_node("spreadsheet")->first_node("cell");

  while (current_cell) {

    string name = current_cell->first_node("name")->value();
    string contents = current_cell->first_node("contents")->value();

    vector<string> contents_history;

    split(contents_history, contents, is_from_range(2, 2));

    sheet->spreadsheet_map.insert(std::make_pair(name, contents_history));


    current_cell = current_cell->next_sibling();
  }

  return sheet;
}

map<string, Spreadsheet *> Spreadsheet::LoadSpreadsheetsMapFromXml(const std::string &folder)
{
  auto *spreadsheets = new map<string, Spreadsheet *>();

  string path = folder + "/spreadsheets_map.xml";

  rapidxml::file<> xmlFile(path.c_str());
  xml_document<> doc;
  doc.parse<0>(xmlFile.data());

  xml_node<> *current_sheet = doc.first_node("spreadsheets")->first_node("spreadsheet");

  while (current_sheet) {

    string name = current_sheet->first_node("name")->value();
    string file = current_sheet->first_node("file")->value();

    string file_path = file;

    Spreadsheet *sheet = LoadSpreadsheetFromFile(name, file_path, folder);

    spreadsheets->insert(std::make_pair(name, sheet));

    current_sheet = current_sheet->next_sibling();
  }

  return (*spreadsheets);
}

void Spreadsheet::AddSubscriber(int client_id)
{
  std::lock_guard<std::mutex> guard(current_subscribers_mutex);
  current_subscribers.insert(client_id);
}

void Spreadsheet::RemoveSubscriber(int client_id)
{
  std::lock_guard<std::mutex> guard(current_subscribers_mutex);
  current_subscribers.erase(client_id);
}

std::set<int> Spreadsheet::GetSubscribers() const
{
  return current_subscribers;
}

string Spreadsheet::GetName() const
{
  return name;
}

std::string Spreadsheet::GetFile() const
{
  return file_path;
}

string Spreadsheet::RevertCellContents(string cell_name)
{
  std::lock_guard<std::mutex> guard(spreadsheet_map_mutex);

  auto search = spreadsheet_map.find(cell_name);

  if (search != spreadsheet_map.end()) {
    if (!(*search).second.empty()) {
      string previous_contents = (*search).second.back();

      std::lock_guard<std::mutex> undo_guard(undo_history_mutex);
      undo_history.emplace_back(false, std::make_pair(cell_name, previous_contents));

      (*search).second.pop_back();

      if (!(*search).second.empty()) {
        return (*search).second.back();
      }
    }
  }

  return "";
}

std::pair<string, string> Spreadsheet::UndoLastChange()
{
  std::lock_guard<std::mutex> undo_guard(undo_history_mutex);

  if (undo_history.empty()) {
    return make_pair("", "");
  }

  auto undo = undo_history.back();
  undo_history.pop_back();

  std::lock_guard<std::mutex> guard(spreadsheet_map_mutex);

  if (undo.first) {
    string cell_name = undo.second.first;
    auto cell_search = spreadsheet_map.find(cell_name);

    if (cell_search != spreadsheet_map.end()) {
      if (!cell_search->second.empty()) {
        cell_search->second.pop_back();
      }
    }
  }

  return make_pair(undo.second.first, undo.second.second);
}

