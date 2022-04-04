using System.Collections.Generic;
using TestingOfApplicants.Models.Tests;

namespace TestingOfApplicants.Models
{
    public static class StaticData
    {
        public static User Me { get; set; }
        public static int ChoosedHeader { get; set; }

        public static List<CompletedTestDto> completedTestsDto { get; set; }

        public static TestHeader header { get; set; }
    }
}
