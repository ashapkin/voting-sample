
using System;
using System.Collections.Generic;

namespace Voting.Web.Persistance {
	public class Proposal
	{
		public int Id { get; set; }
		public string Summary { get; set; }
		public int AuthorId { get; set; }
		public DateTime DateTime { get; set; }

		public User Author { get; set; }
		public ICollection<Vote> Votes { get; set; }
	}
}
