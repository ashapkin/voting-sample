
namespace Voting.Web.Persistance {
	public class Vote {
		public int ProposalId { get; set; }
		public int UserId { get; set; }


		public Proposal Proposal { get; set; }
		public User User { get; set; }
	}
}
