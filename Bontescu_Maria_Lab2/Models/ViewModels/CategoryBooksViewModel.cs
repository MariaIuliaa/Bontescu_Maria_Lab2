using Bontescu_Maria_Lab2.Models;
namespace Bontescu_Maria_Lab2.ViewModels
{
    public class CategoryBooksViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Book> Books { get; set; } = new List<Book>();
    }
}
