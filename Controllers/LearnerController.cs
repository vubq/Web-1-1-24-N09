using BT.Data;
using BT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace BT.Controllers
{
    [Route("Admin/Learner")]
    public class LearnerController : Controller
    {
        private SchoolContext db;

        public LearnerController(SchoolContext context)
        {
            db = context;
        }

        [Route("Filter")]
        public IActionResult LearnerFilter(int? page, int? pageSize, int? mid, string? keyword)
        {
            var learners = (IQueryable<Learner>)db.Learners;

            page = (int)(page == null || page <= 0 ? 1 : page);
            pageSize = (int)(pageSize == null || pageSize <= 0 ? 10 : pageSize);

            if (mid != null)
            {
                learners = learners.Where(l => l.MajorID == mid);
            }
            if (keyword != null)
            {
                learners = learners
                    .Where(
                    l => string.IsNullOrEmpty(keyword) ||
                    l.LastName.Contains(keyword) ||
                    l.FirstMidName.Contains(keyword) 
                    /*|| l.EnrollmentDate.Date.ToString("dd-MM-yyyy").Contains(keyword)*/
                );
            }

            var totalPage = (int)Math.Ceiling(learners.Count() / (float)pageSize);

            ViewBag.mid = mid;
            ViewBag.keyword = keyword;
            ViewBag.totalPage = totalPage;
            ViewBag.page = page;
            ViewBag.pageSize = pageSize;

            var result = learners
                .Skip((int)(pageSize * (page - 1)))
                .Take((int)pageSize)
                .Include(m => m.Major);
            return PartialView("LearnerTable", result);
        }

        [Route("List")]
        public IActionResult Index()
        {
            /*
            var learners = (IQueryable<Learner>)db.Learners;
            if (mid != null)
            {
                learners = learners.Where(l => l.MajorID == mid);
            }

            this.totalPage = (int)Math.Ceiling(learners.Count() / (float)this.pageSize);
            ViewBag.totalPage = this.totalPage;
            ViewBag.page = this.page;

            var results = learners
                .Take(this.pageSize)
                .Include(m => m.Major);
            */
            return View();
        }

        [Route("Add")]
        public IActionResult Create()
        {
            /*
            var majors = new List<SelectListItem>();
            foreach (var item in db.Majors)
            {
                majors.Add(new SelectListItem
                {
                    Text = item.MajorName,
                    Value = item.MajorID.ToString()
                });
            }
            ViewBag.MajorID = majors;
            */

            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        [Route("Add")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstMidName, LastName, MajorID, EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Learners.Add(learner);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        [Route("Edit")]
        public IActionResult Edit(int? id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }

            var learner = db.Learners.Find(id);
            if (learner == null)
            {
                return NotFound();
            }
            ViewBag.MajorId = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("LearnerID, FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (id != learner.LearnerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learner);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorId = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        private bool LearnerExists(int id)
        {
            return (db.Learners?.Any(e => e.LearnerID == id)).GetValueOrDefault();
        }

        [Route("Delete")]
        public IActionResult Delete(int? id)
        {
            if (id == null || db.Learners == null)
            {
                return NotFound();
            }

            var learner = db.Learners.Include(l => l.Major)
                .Include(e => e.Enrollments)
                .FirstOrDefault(m => m.LearnerID == id);
            if (learner == null)
            {
                return NotFound();
            }
            if (learner.Enrollments.Count() > 0)
            {
                return Content("This learner has some enrollments, can't delete!");
            }
            return View(learner);
        }

        [Route("Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (db.Learners == null)
            {
                return Problem("Entity set 'Learners' is null.");
            }
            var learner = db.Learners.Find(id);
            if (learner != null)
            {
                db.Learners.Remove(learner);
            }

            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Route("LearnerByMajorID")]
        public IActionResult LearnerByMajorID(int? mid)
        {
            IQueryable<Learner> learners;
            if (mid != null)
            {
                learners = (IQueryable<Learner>)db.Learners
                    .Where(l => l.MajorID == mid)
                    .Include(m => m.Major);
            }
            else
            {
                learners = db.Learners.Include(m => m.Major);
            }
            return PartialView("LearnerTable", learners);
        }
    }
}
