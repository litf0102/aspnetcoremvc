using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using aspnetcoremvc.Utils;

namespace aspnetcoremvc.Extensions
{
    public static class AuthorizationExtension
    {
        public static void AddAuthorizationExtension(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // ①権限付与
            // 使用例：[Authorize(Policy = "Admin")]
            services.AddAuthorization(options =>
            {
                options.AddPolicy("User", policy => policy.RequireRole("User").Build());
                options.AddPolicy("SystemOrAdmin", policy => policy.RequireRole("Admin", "System"));

            });

            // 設定ファイルの読み込み
            var symmetricKeyAsBase64 = AppsettingsHelper.app(new string[] { "AppSettings", "JwtSetting", "SecretKey" });
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var Issuer = AppsettingsHelper.app(new string[] { "AppSettings", "JwtSetting", "Issuer" });
            var Audience = AppsettingsHelper.app(new string[] { "AppSettings", "JwtSetting", "Audience" });

            // トークン査証パラメータ
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = Issuer,//発行者
                ValidateAudience = true,
                ValidAudience = Audience,//受取者
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30),
                RequireExpirationTime = true,
            };

            // ②認証：.Net Core自身のJWT認証
            // Bearer認証起動
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
             // JwtBearerサービス
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         // トークン期限が切れた場合、ヘッダーに関連情報を設定
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     }
                 };
             });
        }
    }
}
