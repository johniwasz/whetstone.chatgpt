using System.ComponentModel.DataAnnotations;

namespace Whetstone.ChatGPT.Blazor.App.Models
{
    public class TableRequest
    {
        [Required]
        public string Category { get; set; } = string.Empty;

        [Range(5, 30)]
        public int MaxItems { get; set; } = 10;

        public List<AttribueItem> Attributes { get; set; } = new()
            { new AttribueItem() { IsFixed = true, Name = "Number", IsNumeric = true },
              new AttribueItem() { IsFixed = true, Name = "Category", IsNumeric = false } };
    }

    public class AttribueItem
    {
        public bool IsFixed { get; set; }

        public string Name { get; set; } = string.Empty;


        public bool IsNumeric { get; set; }

    }
}
