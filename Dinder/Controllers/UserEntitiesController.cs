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

        [Route("updateName")]
        public void UpdateName([FromBody] UserEntity data)
        {
            var user = _uecontext.Users.First(u => u.Email == User.Identity.Name);
            user.Name = data.Name;

            _uecontext.SaveChanges();
        }

        [Route("updatePhone")]
        public void UpdatePhone([FromBody] UserEntity data)
        {
            var user = _uecontext.Users.First(u => u.Email == User.Identity.Name);
            user.Phone = data.Phone;

            _uecontext.SaveChanges();
        }

        [Route("updateBio")]
        public void UpdateBio([FromBody] UserEntity data)
        {
            var user = _uecontext.Users.First(u => u.Email == User.Identity.Name);
            user.Bio = data.Bio;

            _uecontext.SaveChanges();
        }

        [Route("postOnTimeline")]
        public void PostOnTimeline([FromBody] PostsEntity data)
        {
            using (var tran = _uecontext.Database.BeginTransaction())
            {
                try
                {
                    var user = _uecontext.Users.First(id => id.Email == User.Identity.Name);
            
                    var newPost = new PostsEntity
                    {
                        Author = user.Name,
                        Content = data.Content,
                        //Timeline = data.Timeline
                    };
                    _uecontext.Posts.Add(newPost);
                    _uecontext.SaveChanges(); 

                    _uecontext.UserPosts.Add(new UserPosts
                    {
                        UserID = user.UserID,
                        PostID = newPost.PostID
                    });
                    _uecontext.SaveChanges(); //has to be called once before, otherwise newPost.PostID is not created

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    Console.WriteLine("Rolled back. Error during post transaction: " + ex.Message);
                }
            }
        }

        [HttpPost]
        [Route("addFriend")]
        public void AddFriend([FromBody] Friendship data)
        {
            try
            {
#nullable enable
                Friendship? friend = _uecontext.Friendships.FirstOrDefault(f => (f.Friend1ID == data.Friend1ID && f.Friend2ID == data.Friend2ID) || (f.Friend1ID == data.Friend2ID && f.Friend2ID == data.Friend1ID));
                
                if (friend is null)
                {
                    _uecontext.Friendships.Add(new Friendship
                    {
                        FriendStatus = true,
                        Friend1ID = data.Friend1ID,
                        Friend2ID = data.Friend2ID
                    });
                }
                else
                {
                    friend.FriendStatus = true;
                }
                _uecontext.SaveChanges();
            }
            catch (Exception x)
            {
                Console.WriteLine("Friendship alive and thriving. Error: " + x.Message);
            }
#nullable disable
        }

        [HttpPost]
        [Route("removeFriend")]
        public void RemoveFriend([FromBody] Friendship data)
        {

            var exFriend = _uecontext.Friendships.First(f => (f.Friend1ID == data.Friend1ID && f.Friend2ID == data.Friend2ID) || (f.Friend1ID == data.Friend2ID && f.Friend2ID == data.Friend1ID));
            exFriend.FriendStatus = false;
            
            _uecontext.SaveChanges();
        }

        [Route("getProfile")]
        public List<Object> Profile([FromBody] UserEntity data)
        {
            List<Object> fullProfile = new List<Object>();
            var userModel = new List<UserEntity>();

            if (data.Email != "")
            {
                userModel = _uecontext.Users.Where(u => u.Email == data.Email).ToList();
            }
            else
            {
                userModel = _uecontext.Users.Where(u => u.UserID == data.UserID).ToList();
            }
            
            fullProfile.Add(userModel.ToList());
            
            return fullProfile;
        }

        public List<Friendship> Friends()
        {
            var userID = GetUserID();

            //var friends = _uecontext.Friendships.Where(f => f.Friend1 == userID);

            //return friends.ToList();
            return null;
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

        [Route("getUserID")]
        public int GetUserID()
        {
            return _uecontext.Users.First(id => id.Email == User.Identity.Name).UserID;
        }

        [Route("getUsername")]
        public string GetUsername()
        {
            return _uecontext.Users.First(id => id.Email == User.Identity.Name).Name;
        }
    }
}
