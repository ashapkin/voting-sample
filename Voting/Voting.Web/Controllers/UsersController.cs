using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Voting.Web.Persistance;

namespace Voting.Web.Controllers {
	[Route("api/[controller]")]
	public class UsersController : Controller {
		private readonly VotingContext _context;

		public UsersController(VotingContext context) {
			_context = context;
		}

		[HttpGet]
		public IEnumerable<User> GetAll() {
			return _context.Users.ToList();
		}

		[HttpGet("{id}", Name = "GetUser")]
		public IActionResult GetById(int id) {
			User user = _context.Users.FirstOrDefault(t => t.Id == id);
			if (user == null) {
				return NotFound();
			}
			return new ObjectResult(user);
		}

		[HttpPost]
		public IActionResult Create([FromBody] User item) {
			if (item == null) {
				return BadRequest();
			}

			_context.Users.Add(item);
			_context.SaveChanges();

			return CreatedAtRoute("GetUser", new { id = item.Id }, item);
		}

		[HttpPut("{id}")]
		public IActionResult Update(long id, [FromBody] User item) {
			if (item == null) {
				return BadRequest();
			}

			User user = _context.Users.FirstOrDefault(t => t.Id == id);
			if (user == null) {
				return NotFound();
			}

			user.Name = item.Name;

			_context.Users.Update(user);
			_context.SaveChanges();
			return new NoContentResult();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id) {
			User user = _context.Users.FirstOrDefault(t => t.Id == id);
			if (user == null) {
				return NotFound();
			}

			_context.Users.Remove(user);
			_context.SaveChanges();
			return new NoContentResult();
		}
	}
}
