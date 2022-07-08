using System.ComponentModel.DataAnnotations;

namespace BusBoard.Web.Models
{
    public class PostcodeSelection
    {
        [RegularExpression(@"^(?i)([A-Z]{1,2}\d[A-Z\d]? ?\d[A-Z]{2}|GIR ?0A{2})$",
            ErrorMessage = "Please enter a valid postcode.")]
        [Required(ErrorMessage = "You must enter a postcode.", AllowEmptyStrings = false)]
        public string Postcode { get; set; }

    }
}
