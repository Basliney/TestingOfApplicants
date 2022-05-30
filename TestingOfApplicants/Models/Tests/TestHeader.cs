using System.ComponentModel.DataAnnotations;

namespace TestingOfApplicants.Models.Tests
{
    public class TestHeader
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description{ get; set; }

        public int Subjectid { get; set; }
    }
}
