using Bontescu_Maria_Lab2.Models;
namespace Bontescu_Maria_Lab2.ViewModels
{
    public class PublisherIndexData
    {

        public IEnumerable<Publisher> Publishers { get; set; }
        public IEnumerable<Book> Books { get; set; }

    }
}