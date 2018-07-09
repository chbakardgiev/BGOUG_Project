using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;

namespace WebApplication5.Controllers
{
    public class MembersController : Controller
    {
        private BGOUGEntities db = new BGOUGEntities();

        // GET: Members
        public ActionResult Index()
        {
            var members = db.members.Include(m => m.company).Include(m => m.interest).Include(m => m.members_events).Include(m => m.platform);
            return View(members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }
        //
        public ActionResult PaidMembership()
        {
            DbRawSqlQuery<IsPaid> data = db.Database.SqlQuery<IsPaid>
                ("Select name from members where isPaidMembership = 1");
            return View(data);
        }
        public ActionResult CompaniesRanking()
        {

            IEnumerable<Ranking> data = db.Database.SqlQuery<Ranking>
                ("SELECT companies.name,COUNT(members.name) as Count " +
                "FROM members LEFT JOIN companies ON companies.ID = members.currentCompany " +
                "GROUP BY companies.name; ");
            data = data.OrderByDescending(x => x.Count);
            return View(data);
        }
        public ActionResult EventsRanking()
        {
            IEnumerable<EventRanking> data = db.Database.SqlQuery<EventRanking>
                ("SELECT [events].name as EventName,[events].date as Date,companies.name AS CompanyName,Count(members.name) as CompanyEmployees " +
                "FROM members,[members-events],[events], companies " +
                "where[events].ID = [members-events].IDEvents and " +
                "[members-events].IDMembers = members.ID and " +
                "members.currentCompany = companies.ID " +
                "group by[events].name,[events].date, companies.name, members.name " +
                "order by[events].date, CompanyEmployees desc");
            return View(data);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            ViewBag.currentCompany = new SelectList(db.companies, "ID", "name");
            ViewBag.interestFK = new SelectList(db.interests, "ID", "interest1");
            ViewBag.ID = new SelectList(db.members_events, "IDMembers", "IDMembers");
            ViewBag.platformFK = new SelectList(db.platforms, "ID", "platformType");
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,name,position,address,telephone,typeMemebrship,isPaidMembership,discountAllowance,currentCompany,interestFK,platformFK,referencedBy")] member member)
        {
            if (ModelState.IsValid)
            {
                db.members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.currentCompany = new SelectList(db.companies, "ID", "name", member.currentCompany);
            ViewBag.interestFK = new SelectList(db.interests, "ID", "interest1", member.interestFK);
            ViewBag.ID = new SelectList(db.members_events, "IDMembers", "IDMembers", member.ID);
            ViewBag.platformFK = new SelectList(db.platforms, "ID", "platformType", member.platformFK);
            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.currentCompany = new SelectList(db.companies, "ID", "name", member.currentCompany);
            ViewBag.interestFK = new SelectList(db.interests, "ID", "interest1", member.interestFK);
            ViewBag.ID = new SelectList(db.members_events, "IDMembers", "IDMembers", member.ID);
            ViewBag.platformFK = new SelectList(db.platforms, "ID", "platformType", member.platformFK);
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,name,position,address,telephone,typeMemebrship,isPaidMembership,discountAllowance,currentCompany,interestFK,platformFK,referencedBy")] member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.currentCompany = new SelectList(db.companies, "ID", "name", member.currentCompany);
            ViewBag.interestFK = new SelectList(db.interests, "ID", "interest1", member.interestFK);
            ViewBag.ID = new SelectList(db.members_events, "IDMembers", "IDMembers", member.ID);
            ViewBag.platformFK = new SelectList(db.platforms, "ID", "platformType", member.platformFK);
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            member member = db.members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            member member = db.members.Find(id);
            db.members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
