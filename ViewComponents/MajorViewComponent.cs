﻿using BT.Data;
using BT.Models;
using Microsoft.AspNetCore.Mvc;

namespace BT.ViewComponents
{
    public class MajorViewComponent : ViewComponent

    {
        SchoolContext db;
        List<Major> majors;

        public MajorViewComponent(SchoolContext _context)
        {
            db = _context;
            majors = db.Majors.ToList();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderMajor", majors);
        }
    }
}
