using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DinderDL;
using DinderDL.Models;

namespace Dinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEntitiesController : ControllerBase
    {
        private readonly UserEntityContext _uecontext;

        public UserEntitiesController(UserEntityContext context)
        {
            _uecontext = context;
        }

        [Route("insertEmail")]
        public void InsertEmail([FromBody] UserEntity data)
        {
            _uecontext.Users.Add(new DinderDL.Models.UserEntity
            {
                Email = data.Email
            });
            _uecontext.SaveChanges();
        }

        [Route("insertName")]
        public void InsertName([FromBody] UserEntity data)
        {
            var user = _uecontext.Users.First(u => u.Email == User.Identity.Name);
            user.Name = data.Name;

            _uecontext.SaveChanges();
        }

        [Route("insertPhone")]
        public void InsertPhone([FromBody] UserEntity data)
        {
            var user = _uecontext.Users.First(u => u.Email == User.Identity.Name);
            user.Phone = data.Phone;

            _uecontext.SaveChanges();
        }

        [Route("getProfile")]
        public List<Object> Profile(UserEntity data)
        {
            Console.WriteLine(data);
            List<Object> fullProfile = new List<Object>();
            var userModel = _uecontext.Users.ToList();

            fullProfile.Add(userModel.ToList());
            fullProfile.Add(Posts());
            return fullProfile;
        }

        public Object Posts()
        {
            var userID = GetUserID();

            var posts = from up in _uecontext.UserPosts
                        join p in _uecontext.Posts
                        on up.PostID equals p.PostID
                        where up.UserID.Equals(userID)
                        select new { 
                            PostID = up.PostID,
                            AuthorID = up.UserID,
                            Author = p.Author,
                            Title = p.Title,
                            Content = p.Content
                        };
            return posts.ToList();
        }

        // GET: api/UserEntities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserEntity>>> GetUsers()
        {
            return await _uecontext.Users.ToListAsync();
        }

        // GET: api/UserEntities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserEntity>> GetUserEntity(int id)
        {
            var userEntity = await _uecontext.Users.FindAsync(id);

            if (userEntity == null)
            {
                return NotFound();
            }

            return userEntity;
        }

        // PUT: api/UserEntities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserEntity(int id, UserEntity userEntity)
        {
            if (id != userEntity.UserID)
            {
                return BadRequest();
            }

            _uecontext.Entry(userEntity).State = EntityState.Modified;

            try
            {
                await _uecontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserEntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserEntities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserEntity>> PostUserEntity(UserEntity userEntity)
        {
            _uecontext.Users.Add(userEntity);
            await _uecontext.SaveChangesAsync();

            return CreatedAtAction("GetUserEntity", new { id = userEntity.UserID }, userEntity);
        }

        // DELETE: api/UserEntities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserEntity(int id)
        {
            var userEntity = await _uecontext.Users.FindAsync(id);
            if (userEntity == null)
            {
                return NotFound();
            }

            _uecontext.Users.Remove(userEntity);
            await _uecontext.SaveChangesAsync();

            return NoContent();
        }

        private bool UserEntityExists(int id)
        {
            return _uecontext.Users.Any(e => e.UserID == id);
        }

        public int GetUserID()
        {
            return _uecontext.Users.First(id => id.Email == User.Identity.Name).UserID;
        }
    }
}
