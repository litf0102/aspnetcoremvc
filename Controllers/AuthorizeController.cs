using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using aspnetcoremvc.Entities;
using aspnetcoremvc.Log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using aspnetcoremvc.Models;
using aspnetcoremvc.Utils;
using aspnetcoremvc.Services.UserApp;
using aspnetcoremvc.Services.RoleApp;

namespace aspnetcoremvc.Controllers
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly ILoggerHelper _logger;
        private IUserAppService _userAppService;
        private IRoleAppService _roleAppService;

        public AuthorizeController(ILoggerHelper loggerHelper, IUserAppService userAppService, IRoleAppService roleAppService)
        {
            _logger = loggerHelper;
            _userAppService = userAppService;
            _roleAppService = roleAppService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.Info(typeof(AuthorizeController), "Get API");
            return Ok("Hello");
        }

        [HttpGet]
        public IActionResult Login(String userid,String password)
        {
            _logger.Info(typeof(AuthorizeController), "Login API");
            string jwtStr = string.Empty;
            bool suc = false;

            // 入力パラメータチェック
            if (userid == null || userid.Length == 0)
            {
                return Ok(new
                {
                    success = suc,
                    token = jwtStr
                });
            }
            if (password == null || password.Length == 0)
            {
                return Ok(new
                {
                    success = suc,
                    token = jwtStr
                });
            }

            // ユーザチェック
            var user = _userAppService.CheckUser(userid, password);
            if (user == null || user.IsDeleted != 0)
            {
                return Ok(new
                {
                    success = suc,
                    token = jwtStr
                });
            }

            // ロール取得
            var userDto = _userAppService.Get(user.Id);
            var userRole = userDto.UserRoles.ElementAt(0);
            var role = _roleAppService.Get(userRole.RoleId);

            // トークン生成
            // ユーザIDとロール名を設定
            TokenModel tokenModel = new TokenModel { Uid = userid, Role = role.Code };
            // トークン生成
            jwtStr = JwtHelper.IssueJwt(tokenModel);
            suc = true;

            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }

        /// <summary>
        /// 例外スロー
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Throw()
        {
            throw new System.IO.IOException();
        }

        /// <summary>
        /// token獲得
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult TestToken(string uid, string role)
        {
            string jwtStr = string.Empty;
            bool suc = false;

            if (role != null)
            {
                // ユーザIDとロール名を設定
                TokenModel tokenModel = new TokenModel { Uid = uid, Role = role };
                // トークン生成
                jwtStr = JwtHelper.IssueJwt(tokenModel);
                suc = true;
            }
            else
            {
                jwtStr = "login fail!!!";
            }

            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }

        /// <summary>
        /// Token解析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult ParseToken()
        {
            //Bearer 
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var user = JwtHelper.SerializeJwt(tokenHeader);
            return Ok(user);

        }

        /// <summary>
        /// Admin権限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAdmin()
        {
            return Ok("hello admin");
        }

        /// <summary>
        /// User権限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Role1")]
        public IActionResult TestUser()
        {
            return Ok("hello User");
        }
    }
}