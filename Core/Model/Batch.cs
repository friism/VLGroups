using System;
using System.Collections.Generic;

namespace Core.Model
{
	public class Batch
	{
		public int BatchId { get; set; }
		public DateTime FetchedAt { get; set; }
		
		public virtual ICollection<Member> Members { get; set; }
	}
}
