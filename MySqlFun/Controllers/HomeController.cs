using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using MySqlFun.Models;


namespace MySqlFun.Controllers
{
    public class HomeController : Controller
    {
        private BowlerDbContext _context { get; set; }

        public HomeController(BowlerDbContext temp)
        {
            _context = temp;
        }

        public IActionResult Index()
        {
            
            ViewBag.Teams = _context.Teams.ToList();
            var blah = _context.Bowlers
                .Include(x => x.BowlerTeam)
                .ToList();
            return View(blah);
        }

        public IActionResult Team(int teamid)
        {
            ViewBag.Team = _context.Teams.Single(x => x.TeamID == teamid);
            ViewBag.Teams = _context.Teams.ToList();
            var teamPlayers = _context.Bowlers
                .Where(x => x.TeamID == teamid)
                .ToList();
            return View(teamPlayers);
        }

        [HttpGet]
        public IActionResult Form()
        {
            return View(new Bowler());
        }

        [HttpPost]
        public IActionResult Form(Bowler bowler)
        {
            if (ModelState.IsValid)
            {
                int id = _context.Bowlers
                    .Max(x => x.BowlerID);
                bowler.BowlerID = (id + 1);
                _context.Bowlers.Add(bowler);
                _context.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int bowlerid)
        {

            var bowler = _context.Bowlers
                .Include(x => x.BowlerTeam)
                .Single(x => x.BowlerID == bowlerid);
            return View("Form", bowler);
        }

        [HttpPost]
        public IActionResult Edit(Bowler bowler)
        {
            _context.Update(bowler);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int bowlerid)
        {

            var bowler = _context.Bowlers
                //.Include(x => x.BowlerTeam)
                .Single(x => x.BowlerID == bowlerid);
           
            _context.Remove(bowler);
            _context.SaveChanges();
            

            return RedirectToAction("Index");
        }
    }
}
