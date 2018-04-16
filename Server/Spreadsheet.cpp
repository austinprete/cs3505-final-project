//
// Created by Austin Prete on 4/9/18.
//

#include <string>
#include <iostream>
#include <vector>
#include <boost/algorithm/string.hpp>
#include <boost/algorithm/string/classification.hpp>
#include <regex>

#include "Spreadsheet.h"
#include "Dependencies/rapidxml-1.13/rapidxml.hpp"
#include "Dependencies/rapidxml-1.13/rapidxml_print.hpp"
#include "Dependencies/rapidxml-1.13/rapidxml_utils.hpp"

using namespace std;
using namespace rapidxml;
using namespace boost;

Spreadsheet::Spreadsheet() : spreadsheet_map()
{}

void Spreadsheet::WriteSpreadsheetToFile(std::string path) const
{
  xml_document<> doc;
  xml_node<> *root_node = doc.allocate_node(node_element, "spreadsheet");
  doc.append_node(root_node);

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

  ofstream outfile;
  outfile.open(path, ios::out | ios::trunc);

  outfile << doc;
}

void Spreadsheet::ChangeCellContents(std::string cell_name, std::string new_contents)
{
  boost::to_upper(cell_name);

  regex cell_name_pattern("^[A-Z]{1}[1-9]{1}[0-9]{0,1}$");

  if (!regex_match(cell_name, cell_name_pattern)) {
    return;
  }

  auto search = spreadsheet_map.find(cell_name);

  if (search != spreadsheet_map.end()) {
    (*search).second.push_back(new_contents);
  } else {
    vector<string> contents_history;
    contents_history.push_back(new_contents);
    spreadsheet_map.insert(std::make_pair(cell_name, contents_history));
  }
}

std::string Spreadsheet::GetFullStateString() const
{
  ostringstream full_state_stream;

  full_state_stream << "full_state ";

  for (const auto &cell : spreadsheet_map) {
    string name = cell.first;
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

  xml_node<> *id_node = doc.allocate_node(node_element, "current_id", "0");

  root_node->append_node(id_node);

  ofstream outfile;
  outfile.open(path, ios::out | ios::trunc);

  outfile << doc;

  outfile.close();
}

Spreadsheet *Spreadsheet::LoadSpreadsheetFromFile(string path)
{
  rapidxml::file<> xmlFile(path.c_str());
  xml_document<> doc;
  doc.parse<0>(xmlFile.data());

  Spreadsheet *sheet = new Spreadsheet();

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

    Spreadsheet *sheet = LoadSpreadsheetFromFile(folder + "/" + file);

    spreadsheets->insert(std::make_pair(name, sheet));

    current_sheet = current_sheet->next_sibling();
  }

  return (*spreadsheets);
}

void Spreadsheet::AddSubscriber(int client_id)
{
  current_subscribers.insert(client_id);
}

void Spreadsheet::RemoveSubscriber(int client_id)
{
  current_subscribers.erase(client_id);
}

std::set<int> Spreadsheet::GetSubscribers() const
{
  return current_subscribers;
}
