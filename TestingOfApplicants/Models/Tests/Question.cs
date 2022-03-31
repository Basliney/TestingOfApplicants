using System.ComponentModel.DataAnnotations;

namespace TestingOfApplicants.Models.Tests
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        public int HeaderId { get; set; }

        public string Ask { get; set; }

        public string Answer { get; set; }

        public string FakeAnswer1 { get; set; }
        public string FakeAnswer2 { get; set; }
        public string FakeAnswer3 { get; set; }
    }
}
