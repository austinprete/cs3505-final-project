//
// Created by Austin Prete on 4/9/18.
//

#include <string>
#include <iostream>

#include "Spreadsheet.h"
#include "Dependencies/rapidxml-1.13/rapidxml.hpp"
#include "Dependencies/rapidxml-1.13/rapidxml_print.hpp"
#include "Dependencies/rapidxml-1.13/rapidxml_utils.hpp"

using namespace std;
using namespace rapidxml;

Spreadsheet::Spreadsheet() : spreadsheet_map()
{}

void Spreadsheet::WriteSpreadsheetToFile(std::string path) const
{
  xml_document<> doc;
  xml_node<> *root_node = doc.allocate_node(node_element, "spreadsheet");
  doc.append_node(root_node);

  root_node->append_node(doc.allocate_node(node_element, "spreadsheet2"));

  for (auto &cell : spreadsheet_map) {
    xml_node<> *cell_node = doc.allocate_node(node_element, "cell");

    xml_node<> *name_node = doc.allocate_node(node_element, "name", cell.first.c_str());
    xml_node<> *contents_node = doc.allocate_node(node_element, "contents", cell.second.c_str());

    cell_node->append_node(name_node);
    cell_node->append_node(contents_node);

    root_node->append_node(cell_node);
  }

  ofstream outfile;
  outfile.open(path, ios::out | ios::trunc);

  outfile << doc;
}

Spreadsheet *Spreadsheet::LoadSpreadsheetFromFile(string path)
{
//  file<> file = new file(path);
  rapidxml::file<> xmlFile(path.c_str());
  xml_document<> doc;
  doc.parse<0>(xmlFile.data());

  Spreadsheet *sheet = new Spreadsheet();

  xml_node<> *current_cell = doc.first_node("spreadsheet")->first_node("cell");

  while (current_cell) {

    string name = current_cell->first_node("name")->value();
    string contents = current_cell->first_node("contents")->value();

    sheet->spreadsheet_map.insert(std::make_pair(name, contents));


    current_cell = current_cell->next_sibling();
  }

  return sheet;
}
