using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Voting.Web.Models;
using Voting.Web.Persistance;

namespace Voting.Web.Controllers
{
	[Route("api/[controller]")]
	public class ProposalsController : Controller
	{
		private readonly VotingContext _context;

		public ProposalsController(VotingContext context) {
			_context = context;
		}

		[HttpGet]
		public IEnumerable<ProposalModel> Get() {
			return _context.ProposalModels();
		}

		[HttpGet]
		[Route("filter")]
		public IEnumerable<ProposalModel> Filter(string summary, int count, DateTime date) {
			IQueryable<ProposalModel> models = _context.ProposalModels(x =>
				(string.IsNullOrEmpty(summary) || x.Summary.Contains(summary))
				&& (date == default(DateTime) || x.DateTime <= date));
			return models.Where(x => x.VotesCount >= count);
		}

		[HttpGet("{id}", Name = "GetProposal")]
		public IActionResult GetById(int id) {
			ProposalModel proposal = _context.ProposalModels(x=> x.Id == id).FirstOrDefault();
			if (proposal == null) {
				return NotFound();
			}
			return new ObjectResult(proposal);
		}

		[HttpPost]
		public IActionResult Post([FromBody]Proposal item) {
			if (item == null) {
				return BadRequest();
			}

			User user = _context.Users.FirstOrDefault(x => x.Id == item.AuthorId);
			if (user == null) {
				return NotFound("User not found");
			}

			_context.Proposals.Add(item);
			_context.SaveChanges();

			return CreatedAtRoute("GetProposal", new { id = item.Id }, item);
		}

		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody]string value) {
			Proposal proposal = _context.Proposals.FirstOrDefault(x => x.Id == id);
			if (proposal == null) {
				return NotFound();
			}

			proposal.Summary = value;

			_context.Proposals.Update(proposal);
			_context.SaveChanges();
			return new NoContentResult();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id) {
			return BadRequest("Method is not allowed");
		}
	}
}
