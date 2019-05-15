using System;

namespace JOS.TrafikLab.Client.RealTimeDepartures
{
    public class RealTimeDeparturesV4ResponseDto : TrafikLabApiResponseDto
    {
        public RealTimeDeparturesV4ResponseDto ResponseData { get; set; }
    }

    public class RealTimeDeparturesV4ResponseData
    {
        public DateTime? LatestUpdate { get; set; }
        public int DataAge { get; set; }
    }
}
