using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Lab1
{
    class Book
    {
        public string Name { get; set; }
        public string Autor { get; set; }
        public int DateOfWriting { get; set; }
    }
    class Library
    {
        private List<Book> books = new List<Book>();
        public void addBook()
        {
            Console.Clear();
            Console.WriteLine("Enter name, autor and date of writing of book:");

            Book newBook = new Book
            {
                Name = Console.ReadLine(),
                Autor = Console.ReadLine(),
                DateOfWriting = Int32.Parse(Console.ReadLine())
            };
            books.Add(newBook);
        }
        public void printFullLibrary()
        {
            Console.Clear();
            foreach (var item in books)
            {
                Console.WriteLine("Name: {0}\nAutor: {1}\nDate of writing: {2}\n", item.Name, item.Autor, item.DateOfWriting);
            }
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }
        private void printResult(List<Book> ans)
        {
            foreach (var item in ans)
            {
                Console.WriteLine("Name: {0}\nAutor: {1}\nDate of writing: {2}\n", item.Name, item.Autor, item.DateOfWriting);
            }
            Console.WriteLine("Press any key to go back");
            Console.ReadKey();
        }
        public void findBooks()
        {
            Console.Clear();
            Console.Write("1. By name\n2. By date of writing\n3. By autor\n");
            int variant = Int32.Parse(Console.ReadLine());
            Console.Clear();

            switch (variant)
            {
                case 1:
                    findBooksByName();
                    break;
                case 2:
                    findBooksByDate();
                    break;
                case 3:
                    findBooksByAutor();
                    break;
                default:
                    Console.WriteLine("There is no such a parametr");
                    Console.WriteLine("Press any key to go back");
                    Console.ReadKey();
                    return;
            }
        }
        private void findBooksByName()
        {
            Console.Write("Enter name of the book: ");
            List<Book> ans = new List<Book>();
            string param = Console.ReadLine();
            Console.Clear();

            ans = books.FindAll(book => book.Name == param);
            if (ans.Count != 0) printResult(ans);
            else
            {
                Console.WriteLine("No books found");
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
            }
        }
        private void findBooksByDate()
        {
            Console.Write("Enter date of writing: ");
            List<Book> ans = new List<Book>();
            string param = Console.ReadLine();
            Console.Clear();

            ans = books.FindAll(book => book.DateOfWriting == Int32.Parse(param));
            if (ans.Count != 0) printResult(ans);
            else
            {
                Console.WriteLine("No books found");
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
            }
        }
        private void findBooksByAutor()
        {
            Console.Write("Enter autor of the book: ");
            List<Book> ans = new List<Book>();
            string param = Console.ReadLine();
            Console.Clear();

            ans = books.FindAll(book => book.Autor == param);
            if (ans.Count != 0) printResult(ans);
            else
            {
                Console.WriteLine("No books found");
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
            }
        }
        public void sortBooks()
        {
            Console.Clear();
            Console.Write("1. By name\n2. By date of writing\n3. By autor\n");
            int variant = Int32.Parse(Console.ReadLine());
            Console.Clear();

            switch (variant)
            {
                case 1:
                    books.Sort((x, y) => x.Name.CompareTo(y.Name));
                    printFullLibrary();
                    break;
                case 2:
                    books.Sort((x, y) => x.DateOfWriting.CompareTo(y.DateOfWriting));
                    printFullLibrary();
                    break;
                case 3:
                    books.Sort((x, y) => x.Autor.CompareTo(y.Autor));
                    printFullLibrary();
                    break;
                default:
                    Console.WriteLine("There is no such a parametr!");
                    Console.WriteLine("Press any key to go back");
                    Console.ReadKey();
                    return;
            }
        }
        public void deleteBook()
        {
            Console.Clear();
            Console.WriteLine("Enter name, autor and date of writing of book:");
            string nameOfBook = Console.ReadLine(), autorOfBook = Console.ReadLine();
            int dateOfWriting = Int32.Parse(Console.ReadLine());

            bool smthDeleted = Convert.ToBoolean(books.RemoveAll(x => (x.Name == nameOfBook && x.Autor == autorOfBook && x.DateOfWriting == dateOfWriting)));
            if (smthDeleted)
            {
                Console.WriteLine("Completed!");
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
            } 
            else
            {
                Console.WriteLine("Nothing deleted");
                Console.WriteLine("Press any key to go back");
                Console.ReadKey();
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Library myLib = new Library();
            
            while (true)
            {
                Console.Clear();
                Console.Write("What do you want to do?\n1. Add book\n2. Delete book\n3. Print library\n4. Find books\n5. Sort books\n6. Exit");
                int action = Int32.Parse(Console.ReadLine());

                switch (action)
                {
                    case 1:
                        myLib.addBook();
                        break;
                    case 2:
                        myLib.deleteBook();
                        break;
                    case 3:
                        myLib.printFullLibrary();
                        break;
                    case 4:
                        myLib.findBooks();
                        break;
                    case 5:
                        myLib.sortBooks();
                        break;
                    case 6:
                        return;
                    default:
                        Console.WriteLine("There is no such action!");
                        Thread.Sleep(100);
                        break;
                }
            }
        }
    }
}
