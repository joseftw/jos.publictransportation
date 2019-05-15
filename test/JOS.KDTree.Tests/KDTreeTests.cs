using System;
using Shouldly;
using Xunit;

namespace JOS.KDTree.Tests
{
    public class KDTreeTests
    {
        private double[][] _points;
        private string[] _nodes;

        public KDTreeTests()
        {
            _points = Array.Empty<double[]>();
            _nodes = Array.Empty<string>();
        }

        [Fact]
        public void GivenPosition_WhenNearestNeighbors_ThenReturnsCorrectClosestLocations()
        {
            _points = new[]
            {
                new []{54.12345678, 19.12345678},
                new []{55.12345678, 20.12345678},
                new []{56.12345678, 21.12345678},
            };
            _nodes = new[] {"Location 1", "Location 2", "Location 3"};
            var sut = new KdTree<double, string>(2, _points, _nodes, DistanceHelpers.L2Norm);

            var result = sut.NearestNeighbors(new[] {54.1234, 19.123}, 1);

            result.Count.ShouldBe(1);
            result.ShouldContain(x => x.Node.Equals("Location 1"));
            result.ShouldContain(x => x.Dimensions[0] == _points[0][0] && x.Dimensions[1] == _points[0][1]);
        }
    }
}
