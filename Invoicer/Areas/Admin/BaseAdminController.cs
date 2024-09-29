using System;
using Microsoft.AspNetCore.Mvc;

namespace Invoicer.Areas.Admin;

[Area("Admin")]
public abstract class BaseAdminController : Controller
{
    protected BaseAdminController() {}
}
