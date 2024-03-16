using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookRadarApi.Controlers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserAccountController : ControllerBase
    {
        /*** Milos ;)
           - we have the below stored procedures ready to go (nothing is implmeneted here)
         * Get all users - inactive are inlcuded, we can filter them out in the gui
         * get User by ID - self explanatory
         * get user by email address - only active. Allowing for users to have an account, let it get closed, 
                        then make another with the same email address. old is archived. We can work this out when we are tackling actual registrations, or have a chat now
         * Get all by Type - Select all active by UserAccess type
         * POST: Create useraccount - this will hanlde a NULL accountType and use the default
         * PUT: Archive User account - this is our DELETE (which is a PUT, can't remember what we said about DELETE vs PUT, I am leaning towards DELETE)
         */


        // GET: api/<UserAccountController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/<UserAccountController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            throw new NotImplementedException();
        }

        // POST api/<UserAccountController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserAccountController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserAccountController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
