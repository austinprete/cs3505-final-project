Written by Austin Prete and Aros Aziz for CS3500, Date Modified: 10.26.17

All design decisions have been implemented as specified by the assignment description. Two
external libraries are used in this assignment: Formula.dll and Spreadsheet.dll. These 
libraries allow for evaluation of numbers within a cell as well as checks on circular 
dependencies and invalid contents of a cell (much like excel). 

To start using this spreadsheet, build the SpreadsheetGUI project and run it. This project 
has been built using multiple forms. The SpreadsheetForm is considered the main form.
The HelpForm (always called by the SpreadsheetForm) is found within the SpreadsheetForm. 
The ExtraFeatureForm is our extra feature. If you try looking at the ExtraFeatureForm using
the designer, you may not know what it is exactly. To find out, run the SpreadsheetGUI 
project and click on help, this will guide you to gaining access to the ExtraFeatureForm.

ADDITIONAL FEATURES:

Our additional feature for this assignment is a code that you can enter into the contents
text box that will then pull up a new form (easter egg). The code is found on the help form:

UP,UP,DOWN,DOWN,LEFT,RIGHT,LEFT,RIGHT,B,A, ENTER

This code is known as the Konami Code. There is a link in the Help box on the spreadsheet
that contains a link for the history of this code.

The Konami Code itself is our extra feature. We consider this an extra feature because
you need to keep track of the buttons that are pressed at all times and make sure that
the previously pressed buttons match the Konami Code. After entering the Konami Code, 
an extra surprise will come up for the TA's pleasure. We hope you enjoy. Thanks for reading!




