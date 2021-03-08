using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DinderDL;
using DinderDL.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Net.Http.Headers;

namespace Dinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserEntitiesController : ControllerBase
    {
        private readonly UserEntityContext _uecontext;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserEntitiesController(UserEntityContext context, IWebHostEnvironment hostEnvironment)
        {
            _uecontext = context;
            _hostEnvironment = hostEnvironment;
        }

        [Route("insertEmail")]
        public void InsertEmail([FromBody] UserEntity data)
        {
            _uecontext.Users.Add(new UserEntity
            {
                Email = data.Email
            });
            _uecontext.SaveChanges();
        }

        [Route("UpsertProfilePic")]
        [HttpPost]
        public async Task UpsertProfilePicAsync([FromForm] Dinder.Models.FileViewModel data)
        {
                string filename = ContentDispositionHeaderValue.Parse(data.Image.ContentDisposition).FileName.Trim('"');

                filename = CorrectFilename(filename);
                var fullPath = Path.Combine(_hostEnvironment + "/assets/", filename);
                
                using (FileStream newFile = new FileStream(fullPath, FileMode.Create))
                {
                    await data.Image.CopyToAsync(newFile);
                }

                if (_uecontext.Files.Any(u => u.UserID == data.ProfileID))
                {
                    var dbfile = _uecontext.Files.FirstOrDefault(u => u.UserID == data.ProfileID);
                    dbfile.FilePath = fullPath;
                    dbfile.Filename = filename;
                }
                else
                {
                    _uecontext.Add(new FilesEntity
                    {
                        FilePath = fullPath,
                        Filename = filename,
                        UserID = data.ProfileID
                    });
                }
                _uecontext.SaveChanges();
        }

        [Route("updateName")]
        public void UpdateName([FromBody] UserEntity data)
        {
            var user = new UserEntity();
            if(data.Name != null)
            {
                user = _uecontext.Users.First(u => u.Email == data.Email);
            }
            user.Name = data.Name;

            _uecontext.SaveChanges();
        }

        [Route("updatePhone")]
        public void UpdatePhone([FromBody] UserEntity data)
        {
            var user = new UserEntity();
            if (data.Phone != null)
            {
                user = _uecontext.Users.First(u => u.Email == data.Email);
            }
            user.Phone = data.Phone;

            _uecontext.SaveChanges();
        }

        [Route("updateBio")]
        public void UpdateBio([FromBody] UserEntity data)
        {
            var user = new UserEntity();
            if (data.Bio != null)
            {
                user = _uecontext.Users.First(u => u.Email == data.Email);
            }
            user.Bio = data.Bio;

            _uecontext.SaveChanges();
        }

        [Route("updateEmail")]
        public void UpdateEmail([FromBody] UserEntity data)
        {
            var user = new UserEntity();
            //data.Name is a new email. I borrowed a property from UserEntity to avoid making a new model just for this function.
            if (data.Name != null)
            {
                user = _uecontext.Users.First(u => u.Email == data.Email);
            }
            user.Email = data.Name;

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
                        PosterID = user.UserID,
                        ReceiverID = data.ReceiverID,
                        Content = data.Content
                    };
                    _uecontext.Posts.Add(newPost);
                    _uecontext.SaveChanges(); 

                    //_uecontext.UserPosts.Add(new UserPosts
                    //{
                    //    UserID = user.UserID,
                    //    PostID = newPost.PostID
                    //});
                    //_uecontext.SaveChanges(); //has to be called once before, otherwise newPost.PostID is not created

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
                        Friend1ID = data.Friend1ID,
                        Friend2ID = data.Friend2ID
                    });
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
            try
            {
                var exFriend = _uecontext.Friendships.First(f => (f.Friend1ID == data.Friend1ID && f.Friend2ID == data.Friend2ID) || (f.Friend1ID == data.Friend2ID && f.Friend2ID == data.Friend1ID));

                _uecontext.Entry(exFriend).State = EntityState.Deleted;
                _uecontext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine("Probably already deleted somehow: " + e);
            }
        }

        [Route("acceptFriendRequest/{requesterID}")]
        [HttpPost]
        public void AcceptFriendRequest(int requesterID)
        {
            var friendship = new DinderDL.Models.Friendship();
            var userID = _uecontext.Users.First(u => u.Email == User.Identity.Name).UserID;
            friendship = _uecontext.Friendships.First(f => f.Friend1ID == requesterID && f.Friend2ID == userID);
            friendship.FriendStatus = true;

            _uecontext.SaveChanges();
        }

        [Route("declineFriendRequest/{requesterID}")]
        [HttpPost]
        public void DeclineFriendRequest(int requesterID)
        {
            var declinee = new DinderDL.Models.Friendship();
            var userID = _uecontext.Users.First(u => u.Email == User.Identity.Name).UserID;
            declinee = _uecontext.Friendships.First(f => f.Friend1ID == requesterID && f.Friend2ID == userID);

            _uecontext.Entry(declinee).State = EntityState.Deleted;
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
            try
            {
                return _uecontext.Users.First(id => id.Email == User.Identity.Name).UserID;

            }
            catch
            {
                return 0;
            }
        }

        [Route("getUsername")]
        public string GetUsername()
        {
            try
            {
                return _uecontext.Users.First(id => id.Email == User.Identity.Name).Name;
            }
            catch
            {
                return "";
            }
        }

        private string CorrectFilename(string filename)
        {
            if (filename.Contains("\\")) {
                filename = filename.Substring(filename.LastIndexOf("\\") + 1); 
            }
            return filename;
        }
    }
}
