using System.Collections.Generic;

namespace JRPC_HMS.Models
{
    public class FeedbackDBViewModel
    {
        public string Metrics { get; set; }
        public string FormName { get; set; }
        public int FormFeedbackCount { get; set; }
        public double Overall { get; set; }
        public Dictionary<string, double> QuestionsPerc { get; set; }
    }
}
