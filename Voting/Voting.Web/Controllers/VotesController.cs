using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Voting.Web.Controllers
{
	[Route("api/[controller]")]
	public class VotesController : Controller
	{

		// GET all votes for bill
		[HttpGet("/for-user/{userId}")]
		public string GetByUser(int userId) {
			//get all votes for user
			return "value";
		}

		[HttpGet("/for-user/{userId}")]
		public string GetByProposal(int userId) {
			//get all votes for user
			return "value";
		}

		// POST vote
		[HttpPost]
		public void Post([FromBody]string value) {
			//vote for something
			
		}

		// DELETE vote
		[HttpDelete("{id}")]
		public void Delete(int id) {
			//remove vote
		}
	}
}
