using System.ComponentModel.DataAnnotations;

namespace BooksAPI.Models
{
    public class UpdateBook
    {
        [Required(ErrorMessage = "The title name can not be empty")]
        public string TitleName { get; set; }
        public int Price { get; set; }

        [Required(ErrorMessage = "The author name can not be empty")]
        public string AuthorName { get; set; }
    }
}
