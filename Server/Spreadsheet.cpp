//
// Created by Austin Prete on 4/9/18.
//

#include <string>
#include <iostream>

#include "Spreadsheet.h"
#include "Dependencies/rapidxml-1.13/rapidxml_utils.hpp"

using namespace std;

Spreadsheet *Spreadsheet::LoadSpreadsheetFromFile(string path)
{
//  rapidxml::file<> file = new rapidxml::file(path);
  rapidxml::file<> xmlFile(path.c_str());
  rapidxml::xml_document<> doc;
  doc.parse<0>(xmlFile.data());

  Spreadsheet *sheet = new Spreadsheet();

  rapidxml::xml_node<> *current_cell = doc.first_node("spreadsheet")->first_node("cell");

  while (current_cell) {

    string name = current_cell->first_node("name")->value();
    string contents = current_cell->first_node("contents")->value();

    sheet->spreadsheet_map.insert(std::make_pair(name, contents));


    current_cell = current_cell->next_sibling();
  }

  return sheet;
}

Spreadsheet::Spreadsheet() : spreadsheet_map()
{}