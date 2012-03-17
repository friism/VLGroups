namespace Core.Model
{
	public class Member
	{
		public int MemberId { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Title { get; set; }
		public string Company { get; set; }
		public bool ProbablyGender { get; set; }
		public int Group { get; set; }
		public int BatchId { get; set; }

		public virtual Batch Batch { get; set; }
	}
}
