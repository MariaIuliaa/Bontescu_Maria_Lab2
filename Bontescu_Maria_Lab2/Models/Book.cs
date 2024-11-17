using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bontescu_Maria_Lab2.Models
{
    public class Book
    {
        public int ID { get; set; }

        [Display(Name = "Book Title")]
        [Required(ErrorMessage = "The title of the book is required.")] // Face titlul obligatoriu
        [StringLength(150, MinimumLength = 3, ErrorMessage = "The title must be between 3 and 150 characters.")] // Setează lungimea maximă și minimă
        public string Title { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        [Range(0.01, 500, ErrorMessage = "The price must be between $0.01 and $500.")]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishingDate { get; set; }

        public int? AuthorID { get; set; }
        public Author? Author { get; set; }

        public int? PublisherID { get; set; }
        public Publisher? Publisher { get; set; }

        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
        public ICollection<Borrowing>? Borrowings { get; set; }
    }
}
