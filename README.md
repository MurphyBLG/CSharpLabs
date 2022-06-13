# C# Labs (3rd semester of study)

## Lab 1
Describe the "Home Library" class.
Implement the following functionality:
- The possibility of working with any number of books 
- Search for a book by any attribute (e.g. author or year of publication)
- Add books to the library, remove books from it, sort books by different fields
- The program should contain a menu that allows all methods to be checked

## Lab 2
Describe a char[] based "string" class to perform basic operations - compare, add character to end of string, get character by index. All of the operations are to be implemented as an overloading of operators. The program should contain a menu which allows you to
to check all methods.

## Lab 3
Based on one of the ready-to-use generalized (template) .NET object collections, create a "Management Company" class that includes a list of real estate objects (buildings). Buildings classes must form a hierarchy with the base class. There are two types of real estate objects: residential and non-residential. Describe in the base class an abstract method for calculating an approximate average number of occupants/employees of the building. For residential buildings, the average number of occupants is the number of flats by the number of rooms in the flat (flat type) by 1.3; for non-residential buildings, the average number of employees is proportional to the area with a factor of 0.2. Implement the following functionality in the form of a menu program.
1. Arrange the entire sequence of properties in descending order of the average
number of occupants/employees. If the value is the same - order the data by type
Buildings (residential, non-residential) and then alphabetically by building address. Output the building type, building address
the building, the average number of occupants/employees for all entries in the list.
2. Output the first 3 items from the list obtained in item 1.
3. Output the last 4 addresses of the building from the list in point 1.
4. In real time (while filling the list of buildings) calculate and keep up to date
the average number of occupants/employees of the property for the
company as a whole, save the value as a field of the class "management company".
5. Organize writing and reading of all data to/from the file. Support of the file format
XML.
6. Organise processing of incorrect input file format.

## Lab 4
Develop a class to analyse the HTML content of web pages on a given web resource. The analysis should be performed on all pages whose URI includes the base URI of the resource. Add to the class an event for defining the search target, with information about link name leading to the page, page URI, nesting level and the search target itself (URI of external resource and link name) to its handler. The event handler should output Output this information to console (or window) and to CSV file.

## Lab 5
Develop a programm for a social network (VK). The program should be a multi-window application (at least 2 windows). Interact with the social network via REST API. Provide at least 3 different types of requests to the social network via REST API.
