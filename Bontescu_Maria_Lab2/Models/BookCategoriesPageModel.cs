using Microsoft.AspNetCore.Mvc.RazorPages;
using Bontescu_Maria_Lab2.Data; // Adaugă acest namespace pentru a găsi Bontescu_Maria_Lab2Context
namespace Bontescu_Maria_Lab2.Models
{
    public class BookCategoriesPageModel : PageModel
    {
        // Inițializăm AssignedCategoryDataList în constructor
        public List<AssignedCategoryData> AssignedCategoryDataList { get; set; } = new List<AssignedCategoryData>();




        public void PopulateAssignedCategoryData(Bontescu_Maria_Lab2Context context, Book book)
        {
            var allCategories = context.Category.ToList(); // Folosește ToList() pentru a forța execuția query-ului
            var bookCategories = new HashSet<int>(book.BookCategories.Select(c => c.CategoryID));
            AssignedCategoryDataList = new List<AssignedCategoryData>();

            foreach (var cat in allCategories)
            {
                AssignedCategoryDataList.Add(new AssignedCategoryData
                {
                    CategoryID = cat.ID,
                    Name = cat.CategoryName,
                    Assigned = bookCategories.Contains(cat.ID)
                });
            }
        }



        public void UpdateBookCategories(Bontescu_Maria_Lab2Context context, string[] selectedCategories, Book bookToUpdate)
        {
            if (selectedCategories == null)
            {
                bookToUpdate.BookCategories = new List<BookCategory>();
                return;
            }
            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var bookCategories = new HashSet<int>(bookToUpdate.BookCategories.Select(c => c.Category.ID));
            foreach (var cat in context.Category)
            {
                if (selectedCategoriesHS.Contains(cat.ID.ToString()))
                {
                    if (!bookCategories.Contains(cat.ID))
                    {
                        bookToUpdate.BookCategories.Add(
                        new BookCategory
                        {
                            BookID = bookToUpdate.ID,
                            CategoryID = cat.ID
                        });
                    }
                }
                else
                {
                    if (bookCategories.Contains(cat.ID))
                    {
                        BookCategory bookToRemove = bookToUpdate
                        .BookCategories
                        .SingleOrDefault(i => i.CategoryID == cat.ID);
                        context.Remove(bookToRemove);
                    }
                }
            }
        }
    }
}

