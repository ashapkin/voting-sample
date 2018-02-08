using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Voting.Web.Persistance {
	public class DbInitializer
	{
		private class ProposalData {
			public string Summary { get; set; }
			public string Author { get; set; }
			public string DateTime { get; set; }
			public string[] Votes { get; set; } = Array.Empty<string>();
		}

		private static ProposalData[] GetProposalsData() {
			return new[] {
				new ProposalData {
					Summary = "Revert bill about vehicles 'SHIPY' sign",
					Author = "Alex",
					DateTime = "2018/01/02",
					Votes = new[] {"Alex", "Bob", "Jhon"}
				},
				new ProposalData {
					Summary = "Refactor all project to .NET core 2.0!",
					Author = "Alex",
					DateTime = "2017/12/10",
					Votes = new[] {"Alex", "Bob"}
				},
				new ProposalData {
					Summary = "We need to work hardly",
					Author = "Clara",
					DateTime = "2017/05/30",
					Votes = new[] {"Clara"}
				},
				new ProposalData {
					Summary = "No one hears me",
					Author = "Clara",
					DateTime = "2017/05/30"
				},
				new ProposalData {
					Summary = "Lets get some beer",
					Author = "Bob",
					DateTime = "2017/12/30",
					Votes = new[] {"Clara"}
				},
				new ProposalData {
					Summary = "Happy NY!",
					Author = "Clara",
					DateTime = "2018/01/01",
					Votes = new[] {"Alex", "Bob", "Jhon", "Clara"}
				},
				new ProposalData {
					Summary = "Let's make an client server application sample!",
					Author = "Alex",
					DateTime = "2018/02/02",
					Votes = new[] {"Alex", "Bob", "Jhon", "Clara"}
				},
				new ProposalData {
					Summary = "We are ready to use server application sample!",
					Author = "Alex",
					DateTime = "2018/02/03",
					Votes = new[] {"Alex", "Jhon", "Clara"}
				}
			};
		}

		public static void Init(VotingContext context) {

			context.Database.EnsureCreated();
			if (context.Users.Any()) {
				return;
			}

			ProposalData[] data = GetProposalsData();


			Dictionary<string, User> users = data
				.SelectMany(x => x.Votes.Append(x.Author))
				.Distinct()
				.Select(x=> context.Add(new User {Name = x}))
				.ToDictionary(x => x.Entity.Name, x => x.Entity);
			context.SaveChanges();

			foreach (ProposalData proposalData in data) {
				var proposal = new Proposal {
					DateTime = DateTime.Parse(proposalData.DateTime),
					Summary = proposalData.Summary,
					Author = users[proposalData.Author],
				};
				context.Proposals.Add(proposal);
				foreach (string username in proposalData.Votes) {
					var vote = new Vote {
						User = users[username],
						Proposal = proposal
					};
					context.VoteItems.Add(vote);
				}
			}
			context.SaveChanges();
		}
	}
}
