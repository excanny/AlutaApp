using System.Runtime.Serialization;

namespace AlutaApp.DTO
{

	public class Month
    {
        public int January { get; set; }
        public int February { get; set; }
        public int March { get; set; }
        public int April { get; set; }
        public int May { get; set; }
        public int June { get; set; }
        public int July { get; set; }
        public int August { get; set; }
        public int September { get; set; }
        public int October { get; set; }
        public int November { get; set; }
        public int December { get; set; }
    }
	public class ChartFilter
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
    }
	public class ChartDataPoint
    {
        public HashSet<string> Date { get; set; }
		public List<int> Value { get; set; }
    }

	public class UserCount
	{
		public int Count { get; set; }
	}
	[DataContract]

	public class DataPoint
	{

		public DataPoint(string label, double y)
		{
			this.Label = label;
			this.Y = y;
		}

		public DataPoint(string label, bool isCumulativeSum, string indexLabel)
		{
			this.Label = label;
			this.IsCumulativeSum = isCumulativeSum;
			this.IndexLabel = indexLabel;
		}

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "label")]
		public string Label = "";

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "y")]
		public Nullable<double> Y = null;

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "isCumulativeSum")]
		public bool IsCumulativeSum = false;

		//Explicitly setting the name to be used while serializing to JSON.
		[DataMember(Name = "indexLabel")]
		public string? IndexLabel = null;
	}
}
