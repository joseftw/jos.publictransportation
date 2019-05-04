using System;

namespace JOS.TrafikLab.Client.RealTimeDepartures
{
    public class RealTimeDeparturesV4ResponseDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int ExecutionTime { get; set; }
        public RealTimeDeparturesV4ResponseData ResponseData { get; set; }
    }

    public class RealTimeDeparturesV4ResponseData
    {
        public DateTime? LatestUpdate { get; set; }
        public int DataAge { get; set; }
    }
}
