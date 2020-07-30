using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspnetcoremvc.Models;
using aspnetcoremvc.Services.UserApp;
using Microsoft.AspNetCore.Http;
using aspnetcoremvc.Utils;

namespace aspnetcoremvc.Controllers
{
    public class LoginController : Controller
    {
        private IUserAppService _userAppService;
        public LoginController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //ユーザチェック
                var user = _userAppService.CheckUser(model.UserName, model.Password);
                if (user != null)
                {
                    //記録Session
                    HttpContext.Session.SetString("CurrentUserId", user.Id.ToString());
                    HttpContext.Session.Set("CurrentUser", ByteConvertHelper.Object2Bytes(user));
                    //メイン画面へ遷移
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorInfo = "ユーザ名またはパスワードが不正です。";
                return View();
            }
            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return View(model);
        }
    }
}