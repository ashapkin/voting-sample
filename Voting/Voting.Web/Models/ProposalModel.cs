using System;

namespace Voting.Web.Models {
	public class ProposalModel
	{
		public int Id { get; set; }
		public DateTime DateTime { get; set; }
		public string Summary { get; set; }
		public string Author { get; set; }
		public int VotesCount { get; set; }
	}
}
