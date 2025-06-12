using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Model.Feedback
{
    public class FeedbackStatisticsVm
    {
        public int TotalFeedbacks { get; set; }
        public double AverageRating { get; set; }
        public int Rating1Count { get; set; }
        public int Rating2Count { get; set; }
        public int Rating3Count { get; set; }
        public int Rating4Count { get; set; }
        public int Rating5Count { get; set; }
    }
}
