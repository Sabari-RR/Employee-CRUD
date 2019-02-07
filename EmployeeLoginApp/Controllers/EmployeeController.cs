using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeLoginApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLoginApp.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly AppDbContext _db;
        

        public EmployeeController(AppDbContext db)
        {
            _db = db;
        }

        //GET Employee
        public async Task<IActionResult> Index()
        {    
            //foreach (Employee e in mvm.emplist)
            //{
            //    mvm.employee = e; 
            //}
            return View(await _db.Employees.ToListAsync());
        }

        //GET Employee/id?
        public async Task<IActionResult> GetEmployee(int? id)
        {
            MainViewModels me = new MainViewModels();
            if (id == null)
            {
                return NotFound();
            }
            me.employee = await _db.Employees.SingleOrDefaultAsync(m => m.EmployeeId == id);
            if (me.employee == null)
            {
                return NotFound();
            }
            return View(me);
        }
        // Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {

            if (ModelState.IsValid)
            {
                if (EmployeeExists(employee.EmployeeId))
                {
                    ModelState.AddModelError("EmployeeId", "Already assigned please give proper ID");
                    return View(employee);
                }
                _db.Add(employee);
                await _db.SaveChangesAsync();
                LoginUser lusr = new LoginUser();
                lusr.UserID = employee.EmployeeId;
                _db.Add(lusr);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = await _db.Employees.SingleOrDefaultAsync(e => e.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(employee);
                    await _db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            return View(employee);
        }

        private bool EmployeeExists(int id)
        {
            return _db.Employees.Any(e => e.EmployeeId == id);
        }

        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if(id == null)
        //    {
        //        return NotFound();
        //    }

        //    var employee = await _db.Employees.SingleOrDefaultAsync(e => e.EmployeeId == id);
        //    if(employee == null)
        //    {
        //        return NotFound();
        //    }
        //    //return View(employee);
        //}


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = await _db.Employees.SingleOrDefaultAsync(e => e.EmployeeId == id);
            var user = await _db.LoginUsers.SingleOrDefaultAsync(u => u.UserID == id);
            _db.Employees.Remove(employee);
            if(user !=null)
            { 
               _db.LoginUsers.Remove(user);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser loguser)
        {
            int id = loguser.UserID;
            MainViewModels mg = new MainViewModels();
            mg.employee = await _db.Employees.SingleOrDefaultAsync(e => e.EmployeeId == id);
           
            if (ModelState.IsValid)
            {
                if(id == 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else if (EmployeeExists(id))
                {
                    loguser = await _db.LoginUsers.SingleOrDefaultAsync(u => u.UserID == id);
                    if(loguser!=null)
                    {
                        loguser.Logintime = DateTime.Now; loguser.Status = "Log_IN";
                        _db.LoginUsers.Update(loguser);
                        await _db.SaveChangesAsync();
                        mg.Login = loguser;
                    }
                    return View("WorkSpace",mg);
                }
                else
                {
                    ModelState.AddModelError("UserID", "UserID not Exists, Please check with administrator");
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                LoginUser lu = new LoginUser();
                lu = _db.LoginUsers.SingleOrDefault(e => e.UserID == id);
                lu.Logouttime = DateTime.Now;
                lu.Status = "Log_out";
                lu.TotalHours = Math.Round((lu.Logouttime - lu.Logintime).TotalHours,2);
                _db.Update(lu);
                await _db.SaveChangesAsync();
            }
            return View("Login");
        }


        public async Task<IActionResult> GetWorkingHours()
        {
            MainViewModels mvm = new MainViewModels();
            mvm.emplist = await _db.Employees.ToListAsync();
            mvm.loginlist = await _db.LoginUsers.ToListAsync();
            mvm.emplist = mvm.emplist.OrderBy(e => e.EmployeeId).ToList();
            mvm.loginlist = mvm.loginlist.OrderBy(u => u.UserID).ToList();
            foreach(Employee e in mvm.emplist)
            {
                mvm.employee = e;
            }
            foreach (LoginUser lu in mvm.loginlist)
            {
                mvm.Login = lu;
            }
            return View("WorkingHours", mvm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}