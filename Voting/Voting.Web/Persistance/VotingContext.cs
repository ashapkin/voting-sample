using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Voting.Web.Models;

namespace Voting.Web.Persistance {
	public class VotingContext : DbContext {
		public VotingContext(DbContextOptions<VotingContext> options)
			: base(options) { }


		public DbSet<User> Users { get; set; }
		public DbSet<Proposal> Proposals { get; set; }
		public DbSet<Vote> VoteItems { get; set; }

		public IQueryable<ProposalModel> ProposalModels(Expression<Func<Proposal, bool>> predicate = null) {
			IQueryable<Proposal> source = predicate != null ? Proposals.Where(predicate) : Proposals;
			return source.Select(x => new ProposalModel {
				Id = x.Id,
				DateTime = x.DateTime,
				Summary = x.Summary,
				Author = x.Author.Name,
				VotesCount = VoteItems.Count(y => y.ProposalId == x.Id)
			});
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<User>().ToTable("User").HasKey(x => x.Id);
			modelBuilder.Entity<Proposal>().ToTable("Proposal").HasKey(x => x.Id);
			modelBuilder.Entity<Proposal>().Property(x => x.AuthorId).IsRequired();
			modelBuilder.Entity<Vote>().ToTable("Vote").HasKey(x => new {x.ProposalId, x.UserId});
			modelBuilder.Entity<Vote>().HasOne(x => x.Proposal).WithMany(y => y.Votes).IsRequired().OnDelete(DeleteBehavior.Restrict);

		}
	}
}
