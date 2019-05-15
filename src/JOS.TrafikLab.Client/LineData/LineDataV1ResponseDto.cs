using System;
using System.Collections.Generic;

namespace JOS.TrafikLab.Client.LineData
{
    public class LineDataV1ResponseDto : TrafikLabApiResponseDto
    {
        public LineDataV1ResponseDataDto ResponseData { get; set; }
    }

    public class LineDataV1ResponseDataDto
    {
        public DateTime Version { get; set; }
        public string Type { get; set; }
        public IEnumerable<StopPointDto> Result { get; set; }
    }

    public class StopPointDto
    {
        public int StopAreaNumber { get; set; }
        public int StopPointNumber { get; set; }
        public string StopPointName { get; set; }
        public double LocationNorthingCoordinate { get; set; }
        public double LocationEastingCoordinate { get; set; }
        public DateTime ExistsFromDate { get; set; }
        public string StopAreaTypeCode { get; set; }
    }
}
