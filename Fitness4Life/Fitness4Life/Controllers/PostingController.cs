using System.Linq;
using System.Threading.Tasks;
using Fitness4Life.Data;
using Fitness4Life.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fitness4Life.Controllers
{
    public class PostingController : Controller
    {
        
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _um;

        public PostingController(ApplicationDbContext db, UserManager<ApplicationUser> um)
        {
            _db = db;
            _um = um;
        }
        // GET
        public IActionResult Index()
        {
            ForumCommentViewModel vm = new ForumCommentViewModel();
            vm.Forums = _db.Forums.OrderByDescending(a =>a.Id).Include(d =>d.ApplicationUser);
            vm.Comments = _db.Comments.OrderByDescending(a => a.CommentId).Include(d => d.ApplicationUser);
            return View(vm);
        }
        [Authorize]
        public async Task<ActionResult> CreateAdd(CommentViewModel obj)
        {
            {
                if (obj.Comment == null)
                {
                    return NotFound();
                }
                var fid = obj.FID;
                ApplicationUser user = _um.GetUserAsync(User).Result;
                var r = new Comment();
                r.Summary = obj.Comment;
                r.ForumId = obj.FID;
                r.ApplicationUserId = user.Id;
                _db.Add(r);
                await _db.SaveChangesAsync();
                return RedirectPermanent($"Next/{obj.FID}");
            }
        }
        [Authorize]
        public  IActionResult Next(int? id)
        {
            
            ForumCommentViewModel vm = new ForumCommentViewModel();
            vm.Forums = _db.Forums.OrderByDescending(a =>a.Id).Include(d =>d.ApplicationUser);
            vm.Comments = _db.Comments.OrderByDescending(a =>a.CommentId).Include(d =>d.ApplicationUser);
            ViewBag.item = id;
            return View(vm);
        }
         // GET: Comment/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            ApplicationUser user = _um.GetUserAsync(User).Result;
            var comment1 = await _db.Comments
                .Include(b => b.ApplicationUser)
                .Include(b =>b.Forum)
                .FirstOrDefaultAsync(m => m.CommentId == id);
          
            if (comment1.ApplicationUserId != user.Id)
            {

                return NotFound();
            }

            if (id == null)
            {
                return NotFound();
            }

            var comment = await _db.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserId"] = new SelectList(_db.Users, "Id", "Id", comment.ApplicationUserId);
            return View(comment1);
        }

        // POST: Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize ]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,Summary, ForumId ,Time")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(comment);
                    ApplicationUser user = _um.GetUserAsync(User).Result;
                    comment.ApplicationUser = user;
                    comment.ApplicationUserId = user.Id;
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Next", "Posting", new {id = comment.ForumId});
            }
            ViewData["ApplicationUserId"] = new SelectList(_db.Users, "Id", "Id", comment.ApplicationUserId);
            return RedirectToAction("Next", "Posting", new {id = comment.ForumId});
        }

        // GET: Comment/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ApplicationUser user = _um.GetUserAsync(User).Result;
            
            if (id == null)
            {
                return NotFound();
            } 
            
            var comment = await _db.Comments
                .Include(b => b.ApplicationUser)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            if (comment.ApplicationUserId != user.Id)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _db.Comments.FindAsync(id);
            _db.Comments.Remove(comment);
            _db.Comments.Include(b => b.Forum);
            await _db.SaveChangesAsync();
            return RedirectToAction("Next", "Posting", new {id = comment.ForumId});
        }
        private bool CommentExists(int id)
        {
            return _db.Comments.Any(e => e.CommentId == id);
        }
    }
}