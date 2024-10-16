using System.ComponentModel.DataAnnotations;

namespace Bontescu_Maria_Lab2.Models
{
    public class Author
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        // Inițializează colecția pentru a evita eroarea NullReferenceException
        public ICollection<Book> Books { get; set; } = new List<Book>();

        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
    }
}
